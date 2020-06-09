using AutoMapper;
using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class AgendamientosController : ApiController
    {

        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public AgendamientosController(IRepositoryGeneral ir)//inyectamos el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// guardar el contacto
        /// </summary>
        /// <param name="contacto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/agendamientos/contacto")]
        public async Task<IHttpActionResult> Contacto(ContactoViewModels contacto)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var entidad = new Contacto();
                entidad.idUsuario = JwtManager.getIdUserSession();
                entidad.tipoContacto = contacto.tipoContacto;
                entidad.contacto1 = contacto.contacto;
                entidad.vigente = true;
                await _ir.Add(entidad);     //agregamos la entidad a la tabla de la base de datos
                respuesta.codigo = 0;
                respuesta.mensaje = "Registro insertado exitosamente";
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// listamos los contactos
        /// </summary>
        /// <param name="contacto"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("api/agendamientos/contactos")]
        //public async Task<IHttpActionResult> Contactos()
        //{
        //    entRespuesta respuesta = new entRespuesta();
        //    try
        //    {
        //        var contactos = await _ir.GetList<Contacto>(z=>z.vigente==true);
        //        respuesta.data = contactos;
        //        return Ok(respuesta);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        /// <summary>
        /// ver el contacto por el id de usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/agendamientos/verContactos/{id}")]
        public async Task<IHttpActionResult> VerContacto(int id)
        {
            try
            {
                id = JwtManager.getIdUserSession();
                var entidad = await _ir.GetList<Contacto>(z => z.idUsuario == id && z.vigente == true);
                return Ok(entidad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// eliminar un contacto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/agendamientos/eliminarContacto/{id}")]
        public async Task<IHttpActionResult> EliminarContacto(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var idUser = JwtManager.getIdUserSession();
                var entidad = await _ir.Find<Contacto>(id);
                if (entidad != null && (entidad.idUsuario == idUser))
                {
                    entidad.vigente = false;
                    await _ir.Update(entidad, entidad.id);
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Contacto eliminado exitosamente";
                }
                else
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Id de contacto no encontrado";
                }
                
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// traer todos los agendamientos del usuario 
        /// </summary>
        /// <param name="agenda"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/agendamientos/agendas/{id}")]
        public async Task<IHttpActionResult> DatosAgenda(int id)
        {
            try
            {
                id = JwtManager.getIdUserSession();
                entRespuesta respuesta = new entRespuesta();
                var agendamientos = await _ir.GetList<Agenda>(z => z.idUsuario == id && z.vigente == true);//buscamos los agendamientos por el id y que se encuentre vigente
                var registros = Mapper.Map<List<Agenda>, List<AgendamientoViewModels>>(agendamientos);//mapeamos el objeto

                if (agendamientos.Count > 0)
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "";
                    respuesta.data = registros;
                }
                else
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Usuario no tiene agendas válidas";
                }

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Obtener los tipos de agenda
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/agendamientos/tiposAgenda")]
        public async Task<IHttpActionResult> TiposAgenda()
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                respuesta.codigo = 0;
                respuesta.mensaje = "OK";

                var menus = await _ir.GetAll<AgendaTipo>();//obtener los tiposd de agenda y los pasamos para que los consuma un select list
                var lista = new List<DiccionarioViewModels>();
                foreach (var item in menus)
                {
                    lista.Add(new DiccionarioViewModels { id = item.idAgendaTipo, text = item.glosaAgendaTipo });
                }
                respuesta.data = lista.OrderBy(n => n.text);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// traigo los datos del agendamiento
        /// </summary>
        /// <param name="agenda"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/agendamientos/agenda/{id}")]
        public async Task<IHttpActionResult> DetalleAgenda(int id)
        {
            try
            {
                id = JwtManager.getIdUserSession();
                entRespuesta respuesta = new entRespuesta();
                var agendamiento = await _ir.GetFirst<Agenda>(z => z.idAgenda == id && z.vigente == true);//buscamos el agendamiento por el id y que se encuentre vigente
                if (agendamiento != null)
                {
                    var registro = Mapper.Map<Agenda, AgendamientoViewModels>(agendamiento);//mapeamos el objeto
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Datos de agendamiento enviados";
                    respuesta.data = registro;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se ha podido encontrar agendamiento";
                    respuesta.data = null;
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// agregar un agendamiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/agendamientos/AgregarAgenda")]
        public async Task<IHttpActionResult> AddAgenda(AgendamientosDataViewModels agendamientoViewModels)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                agendamientoViewModels.idUsuario = JwtManager.getIdUserSession();
                var registro = Mapper.Map<AgendamientosDataViewModels, Agenda>(agendamientoViewModels);//mapeamos el objeto
                registro.fechaAgenda = Convert.ToDateTime(agendamientoViewModels.fechaAgenda);
                registro.fechaCreacion = DateTime.Now;
                registro.vigente = true;
                await _ir.Add(registro);//guardamos

                var registroInsertado = _ir.GetLast<Agenda>();
                var registroViewmodels = Mapper.Map<Agenda, AgendamientosDataViewModels>(registroInsertado);

                respuesta.codigo = 0;
                respuesta.mensaje = "Agendamiento agregado";
                respuesta.data = registroViewmodels;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// verifica si hay una agendamiento vigente
        /// </summary>
        /// <param name="agendamiento"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/agendamientos/Verifica")]
        public async Task<IHttpActionResult> VerificaAgendamiento(RespuestaAgendaViewModels agendamiento)
        {
            try
            {   //verificamos si el usuario ya tiene un agendamiento para ease tipo
                agendamiento.idUser = JwtManager.getIdUserSession();
                var valida = await _ir.GetList<Agenda>(z => z.idUsuario == agendamiento.idUser && z.idAgendaTipo == agendamiento.idAgenda && z.vigente == true);
                if (valida.Count > 0)
                {
                    return Ok(1);
                }
                else
                {
                    return Ok(0);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// modificamos un agendamiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/agendamientos/UpdateAgenda")]
        public async Task<IHttpActionResult> UpdateAgenda(AgendamientosDataViewModels agendamientoViewModels)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {   //buscamos el agendamiento
                agendamientoViewModels.idUsuario = JwtManager.getIdUserSession();
                var agendamiento = await _ir.GetFirst<Agenda>(z => z.idAgenda == agendamientoViewModels.idAgenda && z.vigente == true);
                if (agendamiento != null)
                {
                    agendamiento.fechaActualizacion = DateTime.Now;
                    agendamiento.contacto = agendamientoViewModels.contacto;
                    agendamiento.fechaAgenda = Convert.ToDateTime(agendamientoViewModels.fechaAgenda);
                    agendamiento.horaAgenda = agendamientoViewModels.horaAgenda;
                    await _ir.Update(agendamiento, agendamiento.idAgenda);//actualizamos el agendamiento

                    respuesta.codigo = 0;
                    respuesta.mensaje = "Datos de agendamiento actualizados";
                    respuesta.data = agendamientoViewModels.idAgenda;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se ha podido encontrar agendamiento";
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// desahabilitar una agendamiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/agendamientos/DeleteAgenda/{id}")]
        public async Task<IHttpActionResult> Deletegenda(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var idUser = JwtManager.getIdUserSession();
                var agendamiento = await _ir.Find<Agenda>(id);//buscamos el agendamiento
                if (agendamiento != null && (agendamiento.idUsuario == idUser))//si no es null procedemos a dejarlo como no vigente y actualizamos
                {
                    agendamiento.vigente = false;
                    await _ir.Update(agendamiento, id);

                    var fun = new FuncionesViewModels();
                    string mensaje = "";
                    if (agendamiento.idCasoMoksys != null)
                    {
                        mensaje = await fun.EliminaMocksys(agendamiento.idCasoMoksys.GetValueOrDefault());
                    }

                    respuesta.mensajeExtra = mensaje;
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Agendamiento eliminado";
                    respuesta.data = id;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se ha podido encontrar agendamiento";
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// funcion para el envio del sms
        /// </summary>
        /// <param name="dicAgenda"></param>
        /// <returns></returns>
        //private async Task<string> EnvioSms(string dicAgenda, string discriminador, string contacto, int codigoSms, string fecha,string hora)
        //{
        //    //1 confirmacion de agendamiento, 2 eliminacionde agenda 
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
        //    else if (dicAgenda == "Legal ITT" && codigoSms == 1)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaLegalItt"].ToString());
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
        //    else if (dicAgenda == "Legal ITT" && codigoSms == 2)
        //    {
        //        idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaLegalItt"].ToString());
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
        //            if (contacto.Length >10)
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
        //            var mensajeFinal = mensaje.textoMensaje.Replace("@fecha",fecha).Replace("@hora", hora);
        //            string telefono = codigoPais + telefonocortado;//juntamos lod¿s datos del codigo de pais y del telefono
        //            var cadenatexto = "{\"messages\":[{ \"destination\":" + telefono + ",\"messageText\":\"" + mensajeFinal + "\"}]}";//datos que se envia en el post 
        //                                                                                                                                      //del sms
        //            cadena =await  funcion.EnvioSms(telefono, cadenatexto, parametros);
        //        }
        //    }
        //    return cadena;
        //}


        /// <summary>
        /// envio sms y correo e insercion en el servicio de mocksys
        /// </summary>
        /// <param name="id"></param>
        /// <param name="metodo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/agendamientos/MensajeAgendar/{id}")]
        public async Task<IHttpActionResult> EnvioMensajesAgendar(int id, string metodo)
        {
            entRespuesta respuesta = new entRespuesta();
            var _fun = new FuncionesViewModels();
            try
            {
                var idUser = JwtManager.getIdUserSession();
                var ultimaAgenda = await _ir.FindEspecial<Agenda>(id);//obtenemos el registro insertado
                if (ultimaAgenda != null && (ultimaAgenda.idUsuario == idUser))
                {
                    string fecha = "";
                    string hora = "";
                    string accion = "";
                    int codSms = 0;
                    int IdPlantilla = 0;
                    int idAgendaTipo = 0;
                    string contacto = "";

                    if (metodo == "Agendar")
                    {
                        codSms = 1;
                        IdPlantilla = Convert.ToInt32(WebConfigurationManager.AppSettings["PlantillaAgendaInsertar"].ToString());
                        fecha = Convert.ToDateTime(ultimaAgenda.fechaAgenda).ToString("dd-MM-yyyy");
                        hora = ultimaAgenda.horaAgenda.GetValueOrDefault().ToString();
                        idAgendaTipo = ultimaAgenda.AgendaTipo.idAgendaTipo;
                        contacto = ultimaAgenda.contacto;
                        accion = "Agendamiento realizado";
                    }
                    else if (metodo == "Eliminar")
                    {
                        codSms = 2;
                        IdPlantilla = Convert.ToInt32(WebConfigurationManager.AppSettings["PlantillaAgendaEliminar"].ToString());
                        fecha = Convert.ToDateTime(ultimaAgenda.fechaAgenda).ToString("dd-MM-yyyy");
                        hora = ultimaAgenda.horaAgenda.GetValueOrDefault().ToString();
                        accion = "Agendamiento eliminado";
                        idAgendaTipo = ultimaAgenda.AgendaTipo.idAgendaTipo;
                        contacto = ultimaAgenda.contacto;
                    }
                    else if (metodo == "Modificar")
                    {
                        codSms = 1;
                        IdPlantilla = Convert.ToInt32(WebConfigurationManager.AppSettings["PlantillaAgendaModificar"].ToString());
                        fecha = Convert.ToDateTime(ultimaAgenda.fechaAgenda).ToString("dd-MM-yyyy");
                        hora = ultimaAgenda.horaAgenda.GetValueOrDefault().ToString();
                        accion = "Agendamiento modificado";
                        idAgendaTipo = ultimaAgenda.AgendaTipo.idAgendaTipo;
                        contacto = ultimaAgenda.contacto;
                    }
                    string respuestaDevuelta = await _fun.EnvioSmsAnterior(ultimaAgenda.AgendaTipo.modulo, ultimaAgenda.discriminadorAgendamiento,
                            ultimaAgenda.contacto, codSms, fecha, hora);//enviamos el sms de agendamiento
                    if (!String.IsNullOrEmpty(respuestaDevuelta))
                    {
                        var logError = new ErrorLog();//en caso de error insertamos en la tabla de error 
                        logError.errorMensaje = respuestaDevuelta;
                        logError.evento = accion + "envio sms";
                        await _ir.Add(logError);
                    }
                    var plantilla = await _ir.Find<PlantillaCorreo>(IdPlantilla);//obtenemos la plantilla de correo
                    await _fun.EnvioEmail(ultimaAgenda.idUsuario.GetValueOrDefault(), plantilla.Plantilla, accion, codSms, fecha, hora,idAgendaTipo, contacto);//enviamos el email

                    if (codSms == 1)
                    {
                        string observacion = "";
                        switch (ultimaAgenda.discriminadorAgendamiento)
                        {
                            case "1":
                                observacion = "Caso generado desde ecosistemas. Contacto por WhatsApp: " + ultimaAgenda.contacto;
                                break;
                            case "2":
                                observacion = "Caso generado desde ecosistemas. Contacto por Skype: " + ultimaAgenda.contacto;
                                break;
                            case "0":
                                observacion = "Caso generado desde ecosistemas. Contacto por Teléfono: " + ultimaAgenda.contacto;
                                break;
                        }
                        var urlProveedores = "";
                        var idSocio = _fun.Base64_Encode(WebConfigurationManager.AppSettings["idSocio"].ToString());
                        var idAgenda = _fun.Base64_Encode(ultimaAgenda.idAgenda.ToString());

                        if (ultimaAgenda.AgendaTipo.modulo == "HojaVida")
                        {
                            urlProveedores = "https://statistics.grupomok.com.co/HojaDeVida/" + idSocio + "/" + idAgenda;
                        }
                        else if (ultimaAgenda.AgendaTipo.modulo == "Entrevista")
                        {
                            urlProveedores = "https://statistics.grupomok.com.co/PruebasPsicotecnicas/" + idSocio + "/" + idAgenda;
                        }

                        observacion = observacion + ". Motivo de consulta: " + ultimaAgenda.AgendaTipo.glosaAgendaTipo + ". " + urlProveedores;

                        var fechaAgenda = ultimaAgenda.fechaAgenda + ultimaAgenda.horaAgenda;
                        var datosMocksys = await _fun.TraeMocksys(ultimaAgenda.idUsuario.GetValueOrDefault(), ultimaAgenda.idAgendaTipo.GetValueOrDefault(), fechaAgenda.GetValueOrDefault(),ultimaAgenda.AgendaTipo.idContratoMoksys.GetValueOrDefault(), observacion);//insertamos la 
                                                                                                                                                                                              //data en el servicio de mocksys
                        ultimaAgenda.idCasoMoksys = Convert.ToInt32(datosMocksys.Rows[0].ItemArray[0].ToString());//obtenemos la ot de mocksys
                        await _ir.Update(ultimaAgenda, ultimaAgenda.idAgenda);//y la insertamos en el agendamiento
                    }
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Mensajes enviados";
                    return Ok(respuesta);
                }
                respuesta.codigo = 1;
                respuesta.mensaje = "Sin agendamiento";
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.codigo = 2;
                respuesta.mensaje = ex.Message;
                return Ok(respuesta);
            }
        }
        /// <summary>
        /// traigo los agendamientos por modulo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/agendamientos/agendasModulos/{id}")]
        public async Task<IHttpActionResult> AgendasxModulos(string id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                respuesta.codigo = 0;
                respuesta.mensaje = "OK";

                var menus = await _ir.GetList<AgendaTipo>(z => z.modulo == id && z.vigente == true);//obtener los tipos de agenda 
                respuesta.data = Mapper.Map<List<AgendaTipo>, List<AgendaTipoViewModels>>(menus);//mapeamos los objetos
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
    }
}