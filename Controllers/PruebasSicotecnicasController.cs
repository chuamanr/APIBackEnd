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
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class PruebasSicotecnicasController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public PruebasSicotecnicasController(IRepositoryGeneral ir)//inyectamos el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// obtengo los tipos de preguntas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/PruebasSicotecnicas/Tipos")]
        public async Task<IHttpActionResult> TraeTipos()
        {
            try
            {
                var lista = await _ir.GetAll<TipoPregunta>();//obtenemos los tipos de preguntaas
                entRespuesta respuesta = new entRespuesta();
                var tipos = Mapper.Map<List<TipoPregunta>, List<TipoPreguntaViewModels>>(lista);//mapeamos el objeto
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// obtener las preguntas para la prueba
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/PruebasSicotecnicas/Preguntas/prueba/{id}")]
        public async Task<IHttpActionResult> PreguntasPrueba(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var query = "[dbo].[pa_Obtener_Preguntas_Aleatorias_Prueba]";
                SqlParameter[] param = {new SqlParameter("@tipoPregunta",id)
                        };
                var registros = await _ir.data(query, param);
                var listaPreguntas = registros.AsEnumerable().Select(z => new
                {
                    IdPregunta = z[0].ToString(),
                    glosaPregunta = z[2].ToString(),
                    numeroPagina = z[6].ToString()
                });
                var TablaPreguntasRespuestas = await PreguntasRespuestas();
                var ListaPreguntasRespuestas = TablaPreguntasRespuestas.AsEnumerable().Select(z => new {
                    idPreguntaRespuesta = z[0].ToString(),
                    idPregunta = z[1].ToString(),
                    glosaPreguntaRespuesta = z[2].ToString(),
                    orden = z[3].ToString(),
                    esCorrecta = z[4].ToString()
                }).ToList();

                List<DataPreguntasViewModels> lista = new List<DataPreguntasViewModels>();
                foreach (var item in listaPreguntas)//generamos las preguntas
                {
                    var dataPregunta = new DataPreguntasViewModels();
                    var dataPreguntaRespuesta = new DataPreguntasRespuestasViewModels();
                    dataPregunta.IdPregunta = Convert.ToInt32(item.IdPregunta);
                    dataPregunta.glosaPregunta = item.glosaPregunta;
                    dataPregunta.numeroPagina = Convert.ToInt32(item.numeroPagina);

                    var listaRespuestas = ListaPreguntasRespuestas.Where(z => z.idPregunta == item.IdPregunta).ToList();
                    var listaX = new List<DataPreguntasRespuestasViewModels>();
                    foreach (var z in listaRespuestas)//para cada pregunta genaramos las sub preguntas
                    {
                        var x = new DataPreguntasRespuestasViewModels();
                        x.idPregunta = Convert.ToInt32(z.idPregunta);
                        x.esCorrecta = Convert.ToBoolean(z.esCorrecta);
                        x.glosaPreguntaRespuesta = z.glosaPreguntaRespuesta;
                        x.idPreguntaRespuesta = Convert.ToInt32(z.idPreguntaRespuesta);
                        x.orden = Convert.ToInt32(z.orden);

                        listaX.Add(x);
                    }
                    dataPregunta.Data = listaX.ToArray();
                    lista.Add(dataPregunta);
                }
                respuesta.mensaje = "Data recibida";
                respuesta.data = lista;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// obtener las preguntas para simulacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/PruebasSicotecnicas/Preguntas/simulacion/{id}")]
        public async Task<IHttpActionResult> PreguntasSimulacion(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var query = "[dbo].[pa_Obtener_Preguntas_Aleatorias_Simulacion_Nuevo]";
                SqlParameter[] param = {new SqlParameter("@tipoPregunta",id)
                        };
                var registros = await _ir.data(query, param);
                var listaPreguntas = registros.AsEnumerable().Select(z => new
                {
                    IdPregunta = z[0].ToString(),
                    glosaPregunta = z[2].ToString(),
                    numeroPagina= z[6].ToString()
                });
                var TablaPreguntasRespuestas = await PreguntasRespuestas();
                var ListaPreguntasRespuestas = TablaPreguntasRespuestas.AsEnumerable().Select(z => new {
                    idPreguntaRespuesta = z[0].ToString(),
                    idPregunta = z[1].ToString(),
                    glosaPreguntaRespuesta = z[2].ToString(),
                    orden = z[3].ToString(),
                    esCorrecta = z[4].ToString()
                }).ToList();

                List<DataPreguntasViewModels> lista = new List<DataPreguntasViewModels>();
                foreach (var item in listaPreguntas)//generamos las preguntas
                {
                    var dataPregunta = new DataPreguntasViewModels();
                    var dataPreguntaRespuesta = new DataPreguntasRespuestasViewModels();
                    dataPregunta.IdPregunta = Convert.ToInt32(item.IdPregunta);
                    dataPregunta.glosaPregunta = item.glosaPregunta;
                    dataPregunta.numeroPagina = Convert.ToInt32(item.numeroPagina);

                    var listaRespuestas = ListaPreguntasRespuestas.Where(z => z.idPregunta == item.IdPregunta).ToList();
                    var listaX = new List<DataPreguntasRespuestasViewModels>();
                    foreach (var z in listaRespuestas)//para cada pregunta genaramos las sub preguntas
                    {
                        var x = new DataPreguntasRespuestasViewModels();
                        x.idPregunta = Convert.ToInt32(z.idPregunta);
                        x.esCorrecta = Convert.ToBoolean(z.esCorrecta);
                        x.glosaPreguntaRespuesta = z.glosaPreguntaRespuesta;
                        x.idPreguntaRespuesta = Convert.ToInt32(z.idPreguntaRespuesta);
                        x.orden = Convert.ToInt32(z.orden);
                        
                        listaX.Add(x);
                    }
                    dataPregunta.Data = listaX.ToArray();
                    lista.Add(dataPregunta);
                }
                respuesta.mensaje = "Data recibida";
                respuesta.data = lista;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// registramos la prueba
        /// </summary>
        /// <param name="pruebaViewModels"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/PruebasSicotecnicas/RegistraPrueba")]
        public async Task<IHttpActionResult> RegistraPrueba(PruebaViewModels pruebaViewModels)
        {
            entRespuesta respuesta = new entRespuesta();
            pruebaViewModels.idUsuario = JwtManager.getIdUserSession();
            try
            {
                var prueba = Mapper.Map<PruebaViewModels,Prueba>(pruebaViewModels);//mapeamos el objeto
                prueba.completado = false;
                prueba.fechaInicioPrueba = DateTime.Now;
                prueba.vigente = true;

                await _ir.Add(prueba);//agregamos el registro
                var registro=_ir.GetLast<Prueba>();//obtenemos el registro insertado
                var dataRespuesta = Mapper.Map<Prueba, PruebaViewModels>(registro);//mapeamos el objeto de vuelta
                
                respuesta.codigo = 0;
                respuesta.mensaje = "";
                respuesta.data = dataRespuesta;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// registramos la pregunta
        /// </summary>
        /// <param name="preguntaViewModels"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/PruebasSicotecnicas/RegistraPregunta")]
        public async Task<IHttpActionResult> RegistraPregunta(PruebaPreguntaViewModels preguntaViewModels)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var prueba = Mapper.Map<PruebaPreguntaViewModels, PruebaRespuesta>(preguntaViewModels);//mapeamos el objeto
                //prueba.completado = false;
                //prueba.fechaInicioPrueba = DateTime.Now;
                prueba.vigente = true;

                await _ir.Add(prueba);//registramos en la base
                var registro = _ir.GetLast<PruebaRespuesta>();//obtenemos el ultimo registro insertado
                var dataRespuesta= Mapper.Map<PruebaRespuesta, PruebaPreguntaViewModels>(registro);//mapeamos el objeto para devolucion

                respuesta.codigo = 0;
                respuesta.mensaje = "";
                respuesta.data = dataRespuesta;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //obtenemos la resolucion de la prueba
        [HttpGet]
        [Route("api/PruebasSicotecnicas/resolucion/{id}")]
        public async Task<IHttpActionResult> Resolucion(int id)
        {
            id = JwtManager.getIdUserSession();
            try
            {
                var query = "[dbo].[pa_ObtenerResolucionPruebaPsicotecnica]";
                SqlParameter[] param = {new SqlParameter("@idUsuario",id)
                        };
                var registros = await _ir.data(query,param);
                var lista = registros.AsEnumerable().Select(z => new
                {
                    json1 = z[0].ToString(),
                    json2 = z[1].ToString(),
                    json3 = z[2].ToString(),
                    json4 = z[3].ToString(),
                    json5 = z[4].ToString(),
                });
                return Ok(lista);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return BadRequest(mensaje);
            }
        }
        /// <summary>
        /// verificamos si la prueba ha sido completada
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/PruebasSicotecnicas/verifica/{id}")]
        public async Task<IHttpActionResult> Verifica(int id, int idTipo)
        {
            entRespuesta respuesta = new entRespuesta();
            id = JwtManager.getIdUserSession();
            try
            {
                var lista = await _ir.GetList<Prueba>(x => x.idUsuario == id && x.idTipoPregunta == idTipo && x.completado == true && x.esSimulacion == false);
                var registro= lista.OrderByDescending(z => z.fechaInicioPrueba).FirstOrDefault();
                if (registro != null)
                {
                    var viewmodels = Mapper.Map<Prueba, PruebaViewModels>(registro);
                    respuesta.codigo = 1;
                    respuesta.mensaje = "true";
                    respuesta.data = registro;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "false";
                    respuesta.data = null;
                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// verfica si el usuario tiene laas prueba completas
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/PruebasSicotecnicas/verificaCompleta/{id}")]
        public async Task<IHttpActionResult> VerificaCompleta(int id, string tipo)
        {
            entRespuesta respuesta = new entRespuesta();
            id = JwtManager.getIdUserSession();
            try
            {
                bool validado;
                if (tipo=="prueba")
                {
                    validado = false;
                }
                else
                {
                    validado = true;
                }
                var tiposPreguntas = await _ir.GetAll<TipoPregunta>();//obtenemos todas las preguntas
                var lista = new List<PreguntasSicotecnicasViewModels>();
                foreach (var item in tiposPreguntas)
                {
                    var listaPreguntas = await _ir.GetList<Prueba>(x => x.idUsuario == id && x.idTipoPregunta == item.idTipoPregunta && x.completado == true 
                    && x.esSimulacion == validado);
                    var registro= listaPreguntas.OrderByDescending(c => c.fechaInicioPrueba).FirstOrDefault();

                    var z = new PreguntasSicotecnicasViewModels();
                    if (registro != null)
                    {
                        z.completado = registro.completado;
                        z.tipoPregunta = registro.idTipoPregunta;
                    }
                    else
                    {
                        z.completado = false;
                        z.tipoPregunta = item.idTipoPregunta;
                    }
                    lista.Add(z);
                }
                respuesta.data = lista;
            }
            catch (Exception ex)
            {
                respuesta.mensaje = ex.Message;
            }
            return Ok(respuesta);
        }
        /// <summary>
        /// finalizar prueba
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/PruebasSicotecnicas/finalizarPrueba/{id}")]
        public async Task<IHttpActionResult>FinalizarPrueba(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            var idUser = JwtManager.getIdUserSession();
            try
            {
                var prueba = await _ir.Find<Prueba>(id);
                if (prueba != null && (prueba.idUsuario == idUser))
                {
                    prueba.completado = true;
                    await _ir.Update(prueba,id);
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Prueba Finalizada exitosamente";
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Prueba no existente";
                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 2;
                respuesta.mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        /// <summary>
        /// traigo las preguntas respuestas de la pregunta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<DataTable> PreguntasRespuestas()
        {
            try
            {
                var query = "[dbo].[pa_Obtener_PreguntasRespuestas]";
                var registros = await _ir.dataSinP(query);
                return registros;
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return null;
            }
        }


    }
}
