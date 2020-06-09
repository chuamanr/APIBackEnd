using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class CourseraController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public CourseraController(IRepositoryGeneral ir)
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// metodo para registrar en coursera
        /// </summary>
        /// <param name="coursera"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/coursera/registro")]
        public async Task<IHttpActionResult> registro(CourseraViewModels coursera)
        {
            coursera.id = JwtManager.getIdUserSession();
            var mem = new FuncionesViewModels();
            var mensaje = await mem.RegistroCousera(coursera);
            return Ok(mensaje);
        }

        /// <summary>
        /// metodo para obtener link de coursera
        /// </summary>
        /// <param name="coursera"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/coursera/obtenerLink")]
        public async Task<IHttpActionResult> generarLinkCoursera(int id) 
        {
            id = JwtManager.getIdUserSession();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var paramCoursera = await _ir.Find<Parametros>(18);//http://sso.seguroesparatibdb.com
                var paramCourseraProvider = await _ir.Find<Parametros>(17);//http://sso.seguroesparatibdb.com
                Usuario usuario = await _ir.GetFirst<Usuario>(x => x.idUsuario == id);//buscamos los datos del usuario por la identificacion
                string tokenString = JwtManager.generarTokenCoursera(usuario.correoElectronico, usuario.idUsuario.ToString(), usuario.nombres, usuario.apellidoPaterno, paramCourseraProvider.parametroJSON);
                //string tokenString = JwtManager.generarTokenCoursera("pablo.otayza2@grupomok.com", "123", "Pablo", "Otayza", "bancodebogota");
                string requestBodyString = tokenString;
                string contentType = "text/plain";
                string requestMethod = "POST";
                byte[] responseBody;
                byte[] requestBodyBytes = Encoding.UTF8.GetBytes(requestBodyString);
                var linkCoursera = paramCoursera.parametroJSON;
                //var linkCoursera = "http://sso.seguroesparatibdb.com";
                string requestUri = linkCoursera + "/api/sessions";
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = contentType;
                    responseBody = client.UploadData(requestUri, requestMethod, requestBodyBytes);
                }
                var respuestaJSON = System.Text.Encoding.UTF8.GetString(responseBody);
                return Ok(linkCoursera + "/integration/" + JObject.Parse(respuestaJSON)["token"]);
            }
            //return Ok(tokenString);

            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        public class token
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
        }
    }
}