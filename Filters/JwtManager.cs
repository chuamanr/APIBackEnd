using EcoOplacementApi.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace EcoOplacementApi.Filters.Security
{
    internal static class JwtManager
    {
        /// <summary>
        /// Metodo de Seguridad que Genera un Token.
        /// </summary>
        /// <param name="username">Recibe el nombre de usuario.</param>
        /// <returns>Retorna un Token</returns>
        public static string GenerarTokenJwt(string username)
        {
            try
            {
                // Configuracion para generar el token.
                var secretKey = ConfigurationManager.AppSettings["JwtSecretKey"];
                var audienceToken = ConfigurationManager.AppSettings["JwtAudienceToken"];
                var issuerToken = ConfigurationManager.AppSettings["JwtIssuerToken"];
                var expireTime = ConfigurationManager.AppSettings["JwtExpireMinutes"];

                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                // reclama una identidad.
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) });

                // Crea el token para el usuario.
                var token = new JwtSecurityToken(
                    audience: audienceToken,
                    issuer: issuerToken,
                    claims: claimsIdentity.Claims,
                    notBefore: DateTime.UtcNow,
                    //expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                    signingCredentials: signingCredentials);

                var tokenHandler = new JwtSecurityTokenHandler();

                var jwtTokenString = tokenHandler.WriteToken(token);
                return jwtTokenString;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Metodo de Seguridad que genera un codigo y recibe una entidad tipada para generar un payload con datos.
        /// </summary>
        /// <typeparam name="T">Recibe el tipo de datos de la entidad.</typeparam>
        /// <param name="username">Recibe el tipo de usuario.</param>
        /// <param name="entidad">Recibe la entidad.</param>
        /// <returns>Retorna un token.</returns>
        public static string GenerarTokenJwt<T>(string username, T entidad)
        {
            try
            {
                // Configuracion para generar el token.
                var secretKey = ConfigurationManager.AppSettings["JwtSecretKey"];
                var audienceToken = ConfigurationManager.AppSettings["JwtAudienceToken"];
                var issuerToken = ConfigurationManager.AppSettings["JwtIssuerToken"];
                var expireTime = ConfigurationManager.AppSettings["JwtExpireMinutes"];

                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                // reclama una identidad.
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) });

                // Crea el token para el usuario.
                var token = new JwtSecurityToken(
                    audience: audienceToken,
                    issuer: issuerToken,
                    claims: claimsIdentity.Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                    signingCredentials: signingCredentials);

                generarPayload<T>(entidad, ref token);

                var tokenHandler = new JwtSecurityTokenHandler();

                var jwtTokenString = tokenHandler.WriteToken(token);
                return jwtTokenString;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Metodo de Seguridad que genera un codigo y recibe una entidad tipada para generar un payload con datos.
        /// </summary>
        /// <typeparam name="T">Recibe el tipo de datos de la entidad.</typeparam>
        /// <param name="username">Recibe el tipo de usuario.</param>
        /// <param name="entidad">Recibe la entidad.</param>
        /// <returns>Retorna un token.</returns>
        public static string GenerarTokenJwt<T>(string username, string nombreClave, T entidad)
        {
            try
            {
                // Configuracion para generar el token.
                var secretKey = ConfigurationManager.AppSettings["JwtSecretKey"];
                var audienceToken = ConfigurationManager.AppSettings["JwtAudienceToken"];
                var issuerToken = ConfigurationManager.AppSettings["JwtIssuerToken"];
                var expireTime = ConfigurationManager.AppSettings["JwtExpireMinutes"];

                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                // reclama una identidad.
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) });

                // Crea el token para el usuario.
                var token = new JwtSecurityToken(
                    audience: audienceToken,
                    issuer: issuerToken,
                    claims: claimsIdentity.Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                    signingCredentials: signingCredentials);

                generarPayload<T>(entidad, nombreClave, ref token);

                var tokenHandler = new JwtSecurityTokenHandler();

                var jwtTokenString = tokenHandler.WriteToken(token);
                return jwtTokenString;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Metodo de seguridad que genera un payload en base a una entidad tipada.
        /// </summary>
        /// <typeparam name="T">Recibe el tipo de dato de la entidad.</typeparam>
        /// <param name="datos">Reciben los datos del payload.</param>
        /// <param name="token">Recibe el Token.</param>
        private static void generarPayload<T>(T datos, ref JwtSecurityToken token)
        {

            var props = (datos.GetType()).GetProperties();

            foreach (var prop in props)
            {
                token.Payload[prop.Name] = datos.GetType().GetProperty(prop.Name).GetValue(datos, null).ToString();
            }
        }

        /// <summary>
        /// Metodo de seguridad que genera un payload en base a una entidad tipada.
        /// </summary>
        /// <typeparam name="T">Recibe el tipo de dato de la entidad.</typeparam>
        /// <param name="datos">Reciben los datos del payload.</param>
        /// <param name="token">Recibe el Token.</param>
        private static void generarPayload<T>(T datos, string nombreClave, ref JwtSecurityToken token)
        {
            token.Payload[nombreClave] = Newtonsoft.Json.JsonConvert.SerializeObject(datos);
        }

        public static ClaimsPrincipal ObtenerPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var secretKey = ConfigurationManager.AppSettings["JwtSecretKey"];
                var audienceToken = ConfigurationManager.AppSettings["JwtAudienceToken"];
                var issuerToken = ConfigurationManager.AppSettings["JwtIssuerToken"];
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));

                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = bool.Parse(ConfigurationManager.AppSettings["JwtValidateLifetime"]),
                    ValidateIssuerSigningKey = bool.Parse(ConfigurationManager.AppSettings["JwtValidateIssuerSigningKey"]),
                    LifetimeValidator = LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para validar el tiempo de vida del Token.
        /// </summary>
        /// <param name="notBefore">(no antes de)</param>
        /// <param name="expires">(fecha expiracion)</param>
        /// <param name="securityToken">(token de seguridad)</param>
        /// <param name="validationParameters">(parametros de validacion del token)</param>
        /// <returns>Retorna verdadero o falso.</returns>
        private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo para obtener un valor de la session segun nombre y clave.
        /// </summary>
        /// <typeparam name="T">Recibe el tipo de dato.</typeparam>
        /// <param name="nombreClave">Recibe el nombre de la clave.</param>
        /// <returns>Retorna el valor.</returns>
        public static T obtenerSession<T>(string nombreClave)
        {
            try
            {
                ClaimsIdentity claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;

                var claims = claimsIdentity.Claims.Select(x => new { type = x.Type, value = x.Value }).Where((c) => c.type == nombreClave).SingleOrDefault().value;

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(claims);
                //return (T)Convert.ChangeType(claims, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static int getIdUserSession()
        {
            UsuarioViewModels usuario2 = (UsuarioViewModels)JwtManager.obtenerSession<UsuarioViewModels>("Usuario");
            FuncionesViewModels decode = new FuncionesViewModels();
            return int.Parse(decode.Base64_Decode(usuario2.idUsuario));
        }

        public static string generarTokenCoursera(string email, string id, string nombre, string apellido,string provider)
        {
            DateTime Expiry = DateTime.UtcNow.AddMinutes(2);

            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

            // Create Security key  using private key above:
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("YWJjITEyMyE0NTYh"));

            // length should be >256b
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Finally create a Token
            var header = new JwtHeader(credentials);

            //Zoom Required Payload
            var payload = new JwtPayload
           {
               { "emailAddress", email},
               { "eppn", id},
               { "firstName", nombre},
               { "lastName", apellido},
               { "provider", provider},
               { "exp", ts},
           };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            var tokenString = handler.WriteToken(secToken);

            return tokenString;
        }
    }

}
