using AutoMapper;
using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class CoworkingController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public CoworkingController(IRepositoryGeneral ir)//inyecto el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// listamos los coworking
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/coworking/ListaCoworking/{id}")]
        public async Task<IHttpActionResult>Lista(int id)
        {
            try
            {
                id = JwtManager.getIdUserSession();
                var lista = await _ir.GetList<Coworking>(z => z.idUsuario == id && z.vigente == true);//listamos los coworking por el id de usuario
                var listaViewmodels = Mapper.Map<List<Coworking>, List<CoworkingViewModels>>(lista);
                return Ok(listaViewmodels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// agregamos un coworking
        /// </summary>
        /// <param name="coworking"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/coworking/AgregarCoworking")]
        public async Task<IHttpActionResult> AddCoworking(CoworkingViewModels coworking)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                coworking.idUsuario = JwtManager.getIdUserSession();
                var registro = Mapper.Map<CoworkingViewModels, Coworking>(coworking);//mapeamos el objeto
                registro.fechaCreacion = DateTime.Now;
                registro.vigente = true;
                await _ir.Add(registro);//guardamos el registro en la base

                var registroInsertado = _ir.GetLast<Coworking>();//obtenemos el ultimo registro del coworking
                var registroViewmodels = Mapper.Map<Coworking, CoworkingViewModels>(registroInsertado);

                //var fun = new FuncionesViewModels();
                //var datosMocksys = await fun.TraeMocksys(coworking.idUsuario, ultimaAgenda.idAgendaTipo.GetValueOrDefault());//insertamos la 
                //data en el servicio de mocksys
                respuesta.codigo = 0;
                respuesta.mensaje = "Coworking agregado";
                respuesta.data = registroViewmodels;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// inhabilitamos el agendamiento de coworking
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/coworking/DeleteCoworking/{id}")]
        public async Task<IHttpActionResult> DeleteCoworking(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var idUser = JwtManager.getIdUserSession();
                var agendamiento = await _ir.Find<Coworking>(id);//buscamos el agendamiento
                if (agendamiento != null && (agendamiento.idUsuario == idUser))//si no es null procedemos a dejarlo como no vigente y actualizamos
                {
                    agendamiento.vigente = false;
                    await _ir.Update(agendamiento, id);


                    respuesta.codigo = 0;
                    respuesta.mensaje = "Coworking eliminado";
                    respuesta.data = id;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se ha podido encontrar Coworking";
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
        //[HttpGet]
        //[Route("api/coworking/MensajeAgendar/{id}")]
        //public async Task EnvioMensajesCoworking(int id, string metodo)
        //{
        //    var ultimaAgenda = await _ir.Find<Coworking>(id);//obtenemos el registro insertado
        //    if (ultimaAgenda != null)
        //    {
        //        //string tipoAgenda = "";
        //        //var agendaTipo = await _ir.Find<AgendaTipo>(ultimaAgenda.idAgendaTipo);
        //        //if (agendaTipo != null)//obtenemos el modulo
        //        //{
        //        //    tipoAgenda = agendaTipo.modulo;
        //        //}
        //        string fecha = "";
        //        string hora = "";
        //        string accion = "";
        //        int codSms = 0;
        //        int IdPlantilla = 0;

        //        if (metodo == "Agendar")
        //        {
        //            codSms = 1;
        //            IdPlantilla = Convert.ToInt32(WebConfigurationManager.AppSettings["PlantillaAgendaInsertar"].ToString());
        //            fecha = Convert.ToDateTime(ultimaAgenda.fechaAgendamiento).ToString("dd-MM-yyyy");
        //            hora = ultimaAgenda.horario;
        //            accion = "Agendamiento realizado";
        //        }
        //        else if (metodo == "Eliminar")
        //        {
        //            codSms = 2;
        //            IdPlantilla = Convert.ToInt32(WebConfigurationManager.AppSettings["PlantillaAgendaEliminar"].ToString());
        //            fecha = "";
        //            hora = "";
        //            accion = "Agendamiento eliminado";
        //        }
        //        else if (metodo == "Modificar")
        //        {
        //            codSms = 1;
        //            IdPlantilla = Convert.ToInt32(WebConfigurationManager.AppSettings["PlantillaAgendaInsertar"].ToString());
        //            fecha = Convert.ToDateTime(ultimaAgenda.fechaAgendamiento).ToString("dd-MM-yyyy");
        //            hora = ultimaAgenda.horario;
        //            accion = "Agendamiento modificado";
        //        }
        //        string respuestaDevuelta = await EnvioSms(ultimaAgenda.AgendaTipo.modulo, ultimaAgenda.discriminadorAgendamiento,
        //                ultimaAgenda.contacto, codSms, fecha, hora);//enviamos el sms de agendamiento
        //        if (!String.IsNullOrEmpty(respuestaDevuelta))
        //        {
        //            var logError = new ErrorLog();//en caso de error insertamos en la tabla de error 
        //            logError.errorMensaje = respuestaDevuelta;
        //            logError.evento = accion + "envio sms";
        //            await _ir.Add(logError);
        //        }
        //        var plantilla = await _ir.Find<PlantillaCorreo>(IdPlantilla);//obtenemos la plantilla
        //                                                                     //de correo
        //        await EnvioEmail(ultimaAgenda.idUsuario.GetValueOrDefault(), plantilla.Plantilla, accion, codSms, fecha, hora);//enviamos el email

        //        if (codSms == 1)
        //        {
        //            var datosMocksys = await TraeMocksys(ultimaAgenda.idUsuario.GetValueOrDefault(), ultimaAgenda.idAgendaTipo.GetValueOrDefault());//insertamos la 
        //                                                                                                                                            //data en el servicio de mocksys
        //            ultimaAgenda.idCasoMoksys = Convert.ToInt32(datosMocksys.Rows[0].ItemArray[0].ToString());//obtenemos la ot de mocksys
        //            await _ir.Update(ultimaAgenda, ultimaAgenda.idAgenda);//y la insertamos en el agendamiento
        //        }

        //    }
        //}
        /// <summary>
        /// funcion para el envio del sms
        /// </summary>
        /// <param name="dicAgenda"></param>
        /// <returns></returns>
        //private async Task<string> EnvioSms(string dicAgenda, string discriminador, string contacto, int codigoSms, string fecha, string hora)
        //{
        //    //1 confirmacion de agendamiento, 2 eliminacionde Coworking 
        //    string cadena = "";
        //    int idSms = 0;
        //    //confirmacion de agendamiento
        //    if (dicAgenda == "HojaVida" && codigoSms == 1)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaRevision"].ToString());
        //    }
        //    else if (dicAgenda == "Entrevista" && codigoSms == 1)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaEntrenamiento"].ToString());
        //    }
        //    else if (dicAgenda == "Legal" && codigoSms == 1)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaAsesoria"].ToString());
        //    }
        //    else if (dicAgenda == "Coworking" && codigoSms == 1)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaCoworking"].ToString());
        //    }

        //    //eliminacion de agendamiento
        //    else if (dicAgenda == "HojaVida" && codigoSms == 2)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaRevision"].ToString());
        //    }
        //    else if (dicAgenda == "Entrevista" && codigoSms == 2)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaEntrenamiento"].ToString());
        //    }
        //    else if (dicAgenda == "Legal" && codigoSms == 2)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaAsesoria"].ToString());
        //    }
        //    else if (dicAgenda == "Coworking" && codigoSms == 2)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaCoworking"].ToString());
        //    }

        //    var funcion = new FuncionesViewModels();//enviamos el sms e insertamos en la tabla log
        //    var mensaje = await _ir.Find<Mensaje>(idSms);//obtener el cuerpo del mensaje
        //    var parametros = await _ir.Find<Parametros>(Convert.ToInt32(WebConfigurationManager.AppSettings["IdEnvioSms"].ToString()));//obtener los parametros de la base
        //                                                                                                                               //para el envio del sms
        //    var parametrosTelefono = await _ir.Find<Parametros>(Convert.ToInt32(WebConfigurationManager.AppSettings["IdCodigo"].ToString()));//obtener el codigo del pais
        //    var codigoPais = parametrosTelefono.parametroJSON;

        //    if (mensaje != null)
        //    {
        //        //0 y 1 telefono, 2 email para discriminador
        //        if (discriminador != "2" && codigoSms == 2)
        //        {
        //            mensaje.textoMensaje = mensaje.textoMensaje.Replace("@link", "http://seguroesparatibdb.com");
        //            var telefonocortado = "";
        //            if (contacto.Length > 10)
        //            {
        //                telefonocortado = contacto.Substring(contacto.Length - 10, 10);
        //            }
        //            else
        //            {
        //                telefonocortado = contacto;
        //            }
        //            string telefono = codigoPais + telefonocortado;
        //            var cadenatexto = "{\"messages\":[{ \"destination\":" + telefono + ",\"messageText\":\"" + mensaje.textoMensaje + "\"}]}";
        //            cadena = await funcion.EnvioSms(telefono, cadenatexto, parametros);
        //        }
        //        else if (discriminador != "2" && codigoSms == 1)
        //        {
        //            var telefonocortado = "";
        //            if (contacto.Length > 10)
        //            {
        //                telefonocortado = contacto.Substring(contacto.Length - 10, 10);
        //            }
        //            else
        //            {
        //                telefonocortado = contacto;
        //            }
        //            var mensajeFinal = mensaje.textoMensaje.Replace("@fecha", fecha).Replace("@hora", hora);
        //            string telefono = codigoPais + telefonocortado;//juntamos lod¿s datos del codigo de pais y del telefono
        //            var cadenatexto = "{\"messages\":[{ \"destination\":" + telefono + ",\"messageText\":\"" + mensajeFinal + "\"}]}";//datos que se envia en el post 
        //                                                                                                                              //del sms
        //            cadena = await funcion.EnvioSms(telefono, cadenatexto, parametros);
        //        }
        //    }
        //    return cadena;
        //}
     
    }
}
