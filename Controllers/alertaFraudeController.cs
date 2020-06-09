using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using static EcoOplacementApi.ViewModels.HtmlExperianViewModels;

namespace EcoOplacementApi.Controllers
{
    //[Authorize]
    ////[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class alertaFraudeController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public alertaFraudeController(IRepositoryGeneral ir)//inyecto el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// verificar si usuario ya tiene notificacion
        /// </summary>
        /// <param name="notificacion"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/alertaFraude/VerNotificacion")]
        public async Task<IHttpActionResult> VerNotificacion(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                id = JwtManager.getIdUserSession();
                var registro = await _ir.GetFirst<Usuario>(z => z.idUsuario == id && z.habilitarNotificacion == true);//verificamos si el usuario tiene acyivada las notificaciones
                if (registro != null)
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Usuario ya tiene notificación";
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Usuario no tiene notificación";
                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 2;
                respuesta.mensaje = ex.Message;
                return Ok(respuesta);
            }
        }
        /// <summary>
        /// registramos la notificaion del usuario
        /// </summary>
        /// <param name="notificacion"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/alertaFraude/Notificaciones")]
        public async Task<IHttpActionResult> GuardaNotificacion(NotificacionViewModels notificacion)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                notificacion.idUsuario = JwtManager.getIdUserSession();
                var registro = await _ir.Find<Usuario>(notificacion.idUsuario);//buscamos al usuario por la id para guardar los datos de la notificacion
                if (registro != null)
                {
                    registro.numeroCelular = notificacion.telefono;
                    registro.correoElectronico = notificacion.email;
                    registro.habilitarNotificacion = notificacion.habilitado;
                    await _ir.Update(registro, registro.idUsuario);
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Notificación realizada";
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Usuario no existente";
                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 2;
                respuesta.mensaje = ex.Message;
                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("api/alertaFraude/experian")]
        public async Task<IHttpActionResult> ObtenerDataCredito(ExperianViewModels experian)
        {
            try
            {
                var idUser = JwtManager.getIdUserSession();
                var Usuario = await _ir.Find<Usuario>(idUser);
                experian.password = WebConfigurationManager.AppSettings["Experian_Password"].ToString();
                experian.username = WebConfigurationManager.AppSettings["Experian_User"].ToString();
                experian.grant_type = WebConfigurationManager.AppSettings["Experian_Gran_Type"].ToString();
                experian.documentType = "1";
                experian.document = Usuario.identificacion;
                var user = await _ir.GetFirst<Usuario>(z => z.identificacion == experian.document);
                if (user != null && !String.IsNullOrEmpty(user.responseDataExperian))
                {
                    // Fecha Auxiliar
                    DateTime now = DateTime.Now;
                    // Sumamos 1 mes a la fecha guardada
                    DateTime date = user.fechaAccesoExperian.GetValueOrDefault().AddMonths(1);
                    // Comparamos la fecha modificada, si es menor significa que ya pasó un mes y hay 
                    // que hacer la petición si no retorna lo que está almacenado
                    if (date < now)
                    {
                        var fun = new FuncionesViewModels();
                        var token = fun.TokenDataExperian(experian);
                        var respuesta = fun.ClienteDataExperian(experian, token);
                        //object respuesta = null;
                        if (respuesta != null)
                        {
                            var t = await _ir.GetFirst<Usuario>(z => z.identificacion == experian.document);
                            if (t != null)
                            {
                                JavaScriptSerializer java = new JavaScriptSerializer();
                                t.responseDataExperian = Convert.ToString(respuesta);
                                t.fechaAccesoExperian = DateTime.Now;
                                await _ir.Update(t, t.idUsuario);
                            }
                            return Ok(respuesta);
                        }
                        else
                        {
                            return Ok("Sin datos para esta identificación");
                        }
                    }
                    else
                    {
                        var data = JsonConvert.DeserializeObject(user.responseDataExperian);
                        return Ok(data);
                    }
                }
                else
                {
                    var fun = new FuncionesViewModels();
                    var token = fun.TokenDataExperian(experian);
                    var respuesta = fun.ClienteDataExperian(experian, token);
                    //object respuesta = null;
                    if (respuesta != null)
                    {
                        var t = await _ir.GetFirst<Usuario>(z => z.identificacion == experian.document);
                        if (t != null)
                        {
                            JavaScriptSerializer java = new JavaScriptSerializer();
                            t.responseDataExperian = Convert.ToString(respuesta);
                            t.fechaAccesoExperian = DateTime.Now;
                            await _ir.Update(t, t.idUsuario);
                        }
                        return Ok(respuesta);
                    }
                    else
                    {
                        return Ok("Sin datos para esta identificación");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/alertaFraude/experian/html")]
        public async Task<IHttpActionResult> ObtenerHtml(ExperianViewModels experian)
        {
            try
            {
                experian.documentType = "1";
                experian.password = WebConfigurationManager.AppSettings["Experian_Password"].ToString();
                experian.username = WebConfigurationManager.AppSettings["Experian_User"].ToString();
                experian.grant_type = WebConfigurationManager.AppSettings["Experian_Gran_Type"].ToString();
                var idUser = JwtManager.getIdUserSession();
                var Usuario = await _ir.Find<Usuario>(idUser);
                var fun = new FuncionesViewModels();
                var token = "";
                //var respuesta = null;
                var user = await _ir.GetFirst<Usuario>(z => z.identificacion == Usuario.identificacion);
                if (user != null && !String.IsNullOrEmpty(user.responseDataExperian))
                {
                    // Fecha Auxiliar
                    DateTime now = DateTime.Now;
                    // Sumamos 1 mes a la fecha guardada
                    DateTime date = user.fechaAccesoExperian.GetValueOrDefault().AddMonths(1);
                    // Comparamos la fecha modificada, si es menor significa que ya pasó un mes y hay 
                    // que hacer la petición si no retorna lo que está almacenado
                    if (date < now)
                    {
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        string urlFuente = "";
                        string fuente = "";
                        string color = "";
                        string enlace = "";

                        var id = Convert.ToInt32(WebConfigurationManager.AppSettings["Id_Html"].ToString());
                        var htmlData = WebConfigurationManager.AppSettings["HtmlExperian"];
                        //var htmlData = await _ir.GetFirst<Parametros>(z => z.idParametro == id);
                        if (htmlData != null)
                        {
                            dynamic j = jsonSerializer.Deserialize<dynamic>(htmlData);//deserializamos el objeto
                            urlFuente = j["urlFuente"].ToString();
                            fuente = j["fuente"].ToString();
                            color = j["color"].ToString();
                            enlace = j["enlaceDiagnostico"].ToString();
                        }

                        token = fun.TokenDataExperian(experian);
                        var respuesta = fun.HtmlDataExperian(experian, token);
                        respuesta.scoreHTML = respuesta.scoreHTML.Replace("{{urlFuente}}", urlFuente).Replace("{{fuente}}", fuente).Replace("{{color}}", color)
                            .Replace("{{enlaceDiagnostico}}", enlace);
                        respuesta.diagnoseHTML = respuesta.diagnoseHTML.Replace("{{urlFuente}}", urlFuente).Replace("{{fuente}}", fuente).Replace("{{color}}", color)
                            .Replace("{{enlaceDiagnostico}}", enlace);
                        //object respuesta = null;

                        var t = await _ir.GetFirst<Usuario>(z => z.identificacion == experian.document);
                        if (t != null)
                        {
                            t.responseDataExperian = jsonSerializer.Serialize(respuesta);
                            t.fechaAccesoExperian = DateTime.Now;
                            await _ir.Update(t, t.idUsuario);
                        }
                        return Ok(respuesta);                        
                    }
                    else
                    {
                        var data = JsonConvert.DeserializeObject(user.responseDataExperian);
                        return Ok(data);
                    }
                }
                else
                {
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    string urlFuente = "";
                    string fuente = "";
                    string color = "";
                    string enlace = "";

                    var id = Convert.ToInt32(WebConfigurationManager.AppSettings["Id_Html"].ToString());
                    var htmlData = WebConfigurationManager.AppSettings["HtmlExperian"];
                    if (htmlData != null)
                    {
                        dynamic j = jsonSerializer.Deserialize<dynamic>(htmlData);//deserializamos el objeto
                        urlFuente = j["urlFuente"].ToString();
                        fuente = j["fuente"].ToString();
                        color = j["color"].ToString();
                        enlace = j["enlaceDiagnostico"].ToString();
                    }

                    token = fun.TokenDataExperian(experian);
                    var respuesta = fun.HtmlDataExperian(experian, token);
                    respuesta.scoreHTML = respuesta.scoreHTML.Replace("{{urlFuente}}", urlFuente).Replace("{{fuente}}", fuente).Replace("{{color}}", color)
                        .Replace("{{enlaceDiagnostico}}", enlace);
                    respuesta.diagnoseHTML = respuesta.diagnoseHTML.Replace("{{urlFuente}}", urlFuente).Replace("{{fuente}}", fuente).Replace("{{color}}", color)
                        .Replace("{{enlaceDiagnostico}}", enlace);
                    //object respuesta = null;

                    var t = await _ir.GetFirst<Usuario>(z => z.identificacion == experian.document);
                    if (t != null)
                    {
                        t.responseDataExperian = jsonSerializer.Serialize(respuesta);
                        t.fechaAccesoExperian = DateTime.Now;
                        await _ir.Update(t, t.idUsuario);
                    }
                    return Ok(respuesta);
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// evalua in
        /// </summary>
        /// <param name="dataSentinel"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("api/alertaFraude/sentinelEvalua")]
        //public IHttpActionResult DatasentinelEvalua(DataSentinelViewModels dataSentinel)
        //{
        //    var fun = new FuncionesViewModels();
        //    var respuesta = fun.SentinelDataEvalInfo(dataSentinel);

        //    return Ok(respuesta);
        //}

        /// <summary>
        /// obtener alertas
        /// </summary>
        /// <param name="identificacion"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("api/alertaFraude/obtieneAlerta")]
        //public IHttpActionResult ObtenerAlertaExperion(AlertaExperianViewModels alerta)
        //{
        //    try
        //    {
        //        var fun = new FuncionesViewModels();
        //        var experian = new ExperianViewModels();
        //        experian.grant_type = alerta.grant_type;
        //        experian.username = alerta.username;
        //        experian.password = alerta.password;
        //        var token = fun.TokenDataExperian(experian);
        //        var respuesta = fun.IdentificacionExperian(alerta.identificacion, token);

        //        return Ok(respuesta);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }


        //}

        /// <summary>
        /// guarda alertas
        /// </summary>
        /// <param name="ingreso"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("api/alertaFraude/insertaAlerta")]
        //public IHttpActionResult InsertaAlertaExperion(IngresoDataExperianViewModels ingreso)
        //{
        //    try
        //    {
        //        var fun = new FuncionesViewModels();
        //        var experian = new ExperianViewModels();
        //        experian.grant_type = ingreso.grant_type;
        //        experian.username = ingreso.username;
        //        experian.password = ingreso.password;
        //        var token = fun.TokenDataExperian(experian);
        //        var respuesta = fun.InsertaExperian(ingreso, token);
        //        return Ok(respuesta);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
