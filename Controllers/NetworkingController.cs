using AutoMapper;
using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class NetworkingController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public NetworkingController(IRepositoryGeneral ir)//inyectamos el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// listamos todas las inscripciones
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("api/networking/listaInscripciones")]
        //public async Task<IHttpActionResult> ListaInscripciones()
        //{
        //    entRespuesta ent = new entRespuesta();
        //    try
        //    {
        //        var lista = await _ir.GetAll<IttIncripcionEvento>();//obtener todas las inscripciones de los eventos
        //        var listaViewModels = Mapper.Map<List<IttIncripcionEvento>, List<IttIncripcionEventoViewModels>>(lista);
        //        ent.data = listaViewModels;
        //    }
        //    catch (Exception ex)
        //    {
        //        ent.mensaje = ex.Message;
        //    }
        //    return Ok(ent);
        //}
        /// <summary>
        /// listamos los eventos por id de usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/networking/listaEventos/{id}")]
        public async Task<IHttpActionResult> ListaEventos(int id)
        {
            entRespuesta ent = new entRespuesta();
            id = JwtManager.getIdUserSession();
            try
            {
                var query = "[dbo].[pa_Eventos_User]";
                SqlParameter[] parameters =
                             {
                new SqlParameter("@idUser", id)
                     };
                var dt = await _ir.data(query, parameters);//obtenemos el datatable de los eventos y si esta registrado un usuario a un evento
                var registros = dt.AsEnumerable().Select(z => new
                {
                    idEvento = z[0].ToString(),
                    idCiudad = z[1].ToString(),
                    idTipoEvento = z[2].ToString(),
                    rutaImagen = z[3].ToString(),
                    descripcionEvento = z[4].ToString(),
                    nombreConferencista = z[5].ToString(),
                    conferencista = z[6].ToString(),
                    fechaEvento = z[7].ToString(),
                    horaEvento = z[8].ToString(),
                    lugarEvento = z[9].ToString(),
                    fechaCreacion = z[10].ToString(),
                    vigente = Convert.ToBoolean(z[11].ToString()),
                    inscrito = Convert.ToBoolean(z[12].ToString())
                });
                ent.data = registros;

            }
            catch (Exception ex)
            {
                ent.mensaje = ex.Message;
            }
            return Ok(ent);

        }
        /// <summary>
        /// agregar evento
        /// </summary>
        /// <param name="eventosNetworkingViewModels"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/networking/AddEvento")]
        public async Task<IHttpActionResult> AddEvento(IttEventosNetworkingViewModels eventosNetworkingViewModels)
        {
            entRespuesta ent = new entRespuesta();
            try
            {
                string extension = ".jpeg";
                //var xImagen = eventosNetworkingViewModels.rutaImagenEvento.Replace(eventosNetworkingViewModels.rutaImagenEvento.Split(',')[0]+',', "");
                eventosNetworkingViewModels.rutaImagenEvento= eventosNetworkingViewModels.rutaImagenEvento.Replace(eventosNetworkingViewModels.rutaImagenEvento.Split(',')[0] + ',', "");
                var evento = Mapper.Map<IttEventosNetworkingViewModels, IttEventosNetworking>(eventosNetworkingViewModels);//mapeamos el objeto
                //evento.rutaImagenEvento = WebConfigurationManager.AppSettings["rutaImagenEventos"].ToString() + eventosNetworkingViewModels.nombreArchivo;//ruta como se va a guardar en la base
                evento.rutaImagenEvento = Convert.FromBase64String(eventosNetworkingViewModels.rutaImagenEvento); ;
                evento.vigente = true;
                evento.fechaEvento = Convert.ToDateTime(eventosNetworkingViewModels.fechaEvento);
                await _ir.Add(evento);

                var registroInsertado = _ir.GetLast<IttEventosNetworking>();//obtenemos el registro insertado

                var eventoViewmodels = Mapper.Map<IttEventosNetworking, IttEventosNetworkingViewModels>(registroInsertado);

                ent.mensaje = "Evento agregado";
                ent.data = eventoViewmodels;
            }
            catch (Exception ex)
            {
                ent.mensaje = ex.Message;
            }
            return Ok(ent);
        }
        /// <summary>
        /// registrar inscripcion a evento
        /// </summary>
        /// <param name="incripcionEventoViewModels"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/networking/AddInscripcion")]
        public async Task<IHttpActionResult> AddInscripcion(IttIncripcionEventoViewModels incripcionEventoViewModels)
        {
            entRespuesta ent = new entRespuesta();
            incripcionEventoViewModels.IdUsuario = JwtManager.getIdUserSession();
            try
            {
                var inscripcion = Mapper.Map<IttIncripcionEventoViewModels, IttIncripcionEvento>(incripcionEventoViewModels);
                inscripcion.vigente = true;
                await _ir.Add(inscripcion);//registramos la inscricion

                var registroInsertado = _ir.GetLast<IttIncripcionEvento>();//obtenemos el registro insertado

                var inscripcionViewmodels = Mapper.Map<IttIncripcionEvento, IttIncripcionEventoViewModels>(registroInsertado);

                ent.mensaje = "Inscripción agregada";
                ent.data = inscripcion;
            }
            catch (Exception ex)
            {
                ent.mensaje = ex.Message;
            }

            return Ok(ent);
        }
        /// <summary>
        /// desahabilitar inscripcion a evento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/networking/desahabilitarInscripcion/{id}")]
        public async Task<IHttpActionResult> DesaHabilitarInscripcion(int id)
        {
            try
            {
                var idUser = JwtManager.getIdUserSession();
                entRespuesta respuesta = new entRespuesta();
                var entidad = await _ir.Find<IttIncripcionEvento>(id);//buscamos el registro a inhabilitar y actualizamos el estado
                if (entidad != null && (entidad.IdUsuario == idUser))
                {
                    entidad.vigente = false;
                    await _ir.Update(entidad, entidad.idInscripcionEvento);
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Inscripción evento Eliminada";
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Inscripción evento no encontrada";
                }
                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// desahabilitar evento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/networking/desahabilitar/{id}")]
        public async Task<IHttpActionResult> DesaHabilitarEvento(int id)
        {
            try
            {
                entRespuesta respuesta = new entRespuesta();
                var entidad = await _ir.Find<IttEventosNetworking>(id);//buscamos el registro a inhabilitar y actualizamos el estado
                if (entidad != null)
                {
                    entidad.vigente = false;
                    await _ir.Update(entidad, entidad.idEvento);
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Experiencia laboral Eliminada";
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Experiencia laboral no encontrada";
                }
                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// visualizar el evento por la id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/networking/verNetworkingEvento/{id}")]
        public async Task<IHttpActionResult> verNetworkingEvento(int id)
        {
            try
            {
                entRespuesta respuesta = new entRespuesta();
                var agendamiento = await _ir.GetFirst<IttEventosNetworking>(z => z.idEvento == id && z.vigente == true);//buscamos el agendamiento por el id y que se encuentre vigente
                if (agendamiento != null)
                {
                    var registro = Mapper.Map<IttEventosNetworking, IttEventosNetworkingViewModels>(agendamiento);//mapeamos el objeto
                    registro.fechaEvento = Convert.ToDateTime(registro.fechaEvento).ToShortDateString();//pasamos la fecha a formato corto
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Datos de evento enviados";
                    respuesta.data = registro;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se ha podido encontrar evento";
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
        /// visualizar la inscricion por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/networking/verNetworkingInscripcion/{id}")]
        public async Task<IHttpActionResult> verNetworkingInscripcion(int id)
        {
            try
            {
                var idUser = JwtManager.getIdUserSession();
                entRespuesta respuesta = new entRespuesta();
                var agendamiento = await _ir.GetFirst<IttIncripcionEvento>(z => z.idInscripcionEvento == id && z.vigente == true);//buscamos el agendamiento por el id y que se encuentre vigente
                if (agendamiento != null && (agendamiento.IdUsuario == idUser))
                {
                    var registro = Mapper.Map<IttIncripcionEvento, IttIncripcionEventoViewModels>(agendamiento);//mapeamos el objeto
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Datos de inscripcion enviados";
                    respuesta.data = registro;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se ha podido encontrar inscripcion";
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
        /// listamos los eventos por id de usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/networking/mant_listaEventos")]
        public async Task<IHttpActionResult> mant_listaEventos()
        {
            entRespuesta ent = new entRespuesta();
            try
            {
                var query = "[dbo].[pa_mant_obtenerEventos]";
                var dt = await _ir.dataSinP(query);//obtenemos el datatable de los eventos y si esta registrado un usuario a un evento
                var registros = dt.AsEnumerable().Select(z => new
                {
                    idEvento = z[0].ToString(),
                    idCiudad = z[1].ToString(),
                    idTipoEvento = z[2].ToString(),

                    rutaImagen = z[3],
                    
                    descripcionEvento = z[4].ToString(),
                    nombreConferencista = z[5].ToString(),
                    conferencista = z[6].ToString(),
                    fechaEvento = z[7].ToString(),
                    horaEvento = z[8].ToString(),
                    lugarEvento = z[9].ToString(),
                    fechaCreacion = z[10].ToString(),
                    vigente = Convert.ToBoolean(z[11].ToString()),
                    glosaCiudad = z[12].ToString()
                });
                ent.data = registros;

            }
            catch (Exception ex)
            {
                ent.mensaje = ex.Message;
            }
            return Ok(ent);

        }
        /// <summary>
        /// Edita un evento de networking
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/networking/mant_editEvento")]
        public async Task<IHttpActionResult> mant_listaEventos(IttEventosNetworkingViewModels evento)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var ev = Mapper.Map<IttEventosNetworkingViewModels, IttEventosNetworking>(evento);//mapeamos el objeto
                if (ev.idEvento > 0)
                {
                    ev.rutaImagenEvento = Convert.FromBase64String(evento.rutaImagenEvento.Replace(evento.rutaImagenEvento.Split(',')[0] + ',', ""));
                    ev.fechaCreacion = DateTime.Now;
                    await _ir.Update(ev, ev.idEvento);//insertamosel estudio 
                    var recibido = _ir.Find<IttEventosNetworkingViewModels>(ev.idEvento);//obtenemos el estudio insertado
                    //var entidad = Mapper.Map<IttEventosNetworking, IttEventosNetworkingViewModels>(recibido);//mapeamos el objeto para devolverlo por json
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Evento actualizado correctamente";
                    respuesta.data = evento;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "IdEvento debe ser válido";
                    respuesta.data = "";
                    return Ok(respuesta);
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
    }
}
