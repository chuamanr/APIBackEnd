using EcoOplacementApi.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace EcoOplacementApi.Filters.Security
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        #region Propiedades
            public string Realm { get; set; }
        #endregion

        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("No se ha especificado el metodo de Autorizacion", request);
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Los parametros de Autorizacion no son validos", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticarJwtToken(token);

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("El Token de Autorizacion no es valido o esta caducado", request);
            else
                context.Principal = principal;
        }

        /// <summary>
        /// Metodo para validar el token.
        /// </summary>
        /// <param name="token">Recibe el token.</param>
        /// <param name="usuario">Recibe el nombre de usuario.</param>
        /// <returns>Retorna Verdadero o falso.</returns>
        private static ClaimsPrincipal ValidarToken(string token, out string usuario)
        {
            usuario = null;

            ClaimsPrincipal simplePrinciple = JwtManager.ObtenerPrincipal(token);

            if (simplePrinciple == null)
                return null;

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return null;

            if (!identity.IsAuthenticated)
                return null;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            usuario = usernameClaim?.Value;

            if (string.IsNullOrEmpty(usuario))
                return null;

            return simplePrinciple;
        }


        /// <summary>
        /// Metodo para Authenticar el Token de manera asincrona.
        /// </summary>
        /// <param name="token">Recibe el token.</param>
        /// <returns>Retorna una Interfaz Principal</returns>
        protected Task<IPrincipal> AuthenticarJwtToken(string token)
        {
            string usuario;

            var prin = ValidarToken(token, out usuario);

            if (prin != null)
            {
                // basado en el nombre de usuario para obtener más información de la base de datos para construir la identidad local.
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario)
                    // Se pueden añadir mas roles.
                };

                var identity = new ClaimsIdentity(prin.Claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}