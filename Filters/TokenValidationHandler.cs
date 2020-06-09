using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace EcoOplacementApi.Filters
{

    public class TokenValidationHandler : DelegatingHandler
    {
        //private RepositoryGeneral _ir = new RepositoryGeneral();

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            RepositoryGeneral general = new RepositoryGeneral();
            var usaSesionUsuario = general.ObtenerRegistro<Parametros>(z => z.idParametro == 13);
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            if (usaSesionUsuario.parametroJSON == "0")
            {
                return true;
            }
            var jwt = token;
            var handler = new JwtSecurityTokenHandler();
            var tokenRead = handler.ReadJwtToken(jwt);
            var user = tokenRead.Claims.FirstOrDefault(z => z.Type == "Usuario");
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic j = jsonSerializer.Deserialize<dynamic>(user.Value);//deserializamos el objeto
            var fun = new FuncionesViewModels();
            var idTransformado = fun.Base64_Decode(j["idUsuario"].ToString());
            int id = Convert.ToInt32(idTransformado);

            var userData = general.ObtenerRegistro<UsuarioSesion>(z=>z.idUsuario==id && z.vigente == true);
            if (userData != null)
            {
                if (userData.token==token)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            } 
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;

            // determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                var secretKey = ConfigurationManager.AppSettings["JwtSecretKey"];
                var audienceToken = ConfigurationManager.AppSettings["JwtAudienceToken"];
                var issuerToken = ConfigurationManager.AppSettings["JwtIssuerToken"];
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));

                SecurityToken securityToken;
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                // Extract and assign Current Principal and user
                Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User  = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

    }
}