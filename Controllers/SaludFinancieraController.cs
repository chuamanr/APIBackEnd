using AutoMapper;
using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class SaludFinancieraController : ApiController
    {
        //private IRepositoryGeneral _ir;
        //public SaludFinancieraController(IRepositoryGeneral ir)
        //{
        //    _ir = ir;
        //}
        private RepositoryGeneral _ir = new RepositoryGeneral();

        /// <summary>
        /// metodo que guarda las respuestas de los formularios de salud financiera
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idIndicador"></param>
        /// <param name="jsonRespuesta"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/saludfin/GuardarSaludFinanciera")]
        public async Task<IHttpActionResult> GuardarSaludFinanciera(SaludRespuesModels r)
        {   
            entRespuesta ent = new entRespuesta();
            try
            {
                var idUser = JwtManager.getIdUserSession();
                var query = "[dbo].[pa_GuardarRespuestasSaludFinanciera]";
                SqlParameter[] param = {new SqlParameter("@idIndicador",r.idIndicador),
                new SqlParameter("@jsonRespuestas",r.jsonRespuesta),
                new SqlParameter("@idUsuario",idUser)
                        };
                var dt = await _ir.data(query, param);//obtenemos el datatable de los eventos y si esta registrado un usuario a un evento

                var registros = dt.AsEnumerable().Select(z => new
                {
                    Coderror = z[0].ToString(),
                    Respuesta = z[1].ToString()
                });
                ent.data = "OK";
            }
            catch (Exception ex)
            {
                ent.mensaje = ex.Message;
            }
            return Ok(ent);
        }

        /// <summary>
        /// Obtiene los indicadores del usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/saludfin/obtenerIndicadores/{id}")]
        public async Task<IHttpActionResult> ObtenerIndicadores(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            id = JwtManager.getIdUserSession();
            try
            {
                respuesta.codigo = 0;
                respuesta.mensaje = "OK";

                var query = "[dbo].[pa_ObtenerIndicadoresUsuario]";
                SqlParameter[] param = { new SqlParameter("@idUsuario", id) };
                var dt = await _ir.data(query, param);

                var registros = dt.AsEnumerable().Select(z => new
                {
                    idSaludFinancieraTipo = Convert.ToInt32(z[0].ToString()),
                    nombreSaludFinancieraTipo = z[1].ToString(),
                    descripcion = z[2].ToString(),
                    vigente = Convert.ToBoolean(z[3]),
                    scoreUsuario = Convert.ToInt32(z[4]),
                    resolucionUsuario = z[5].ToString(),
                    ponderacion = z[6].ToString(),
                    sectionEcosistema = z[7].ToString(),
                    pjeUsuario = Convert.ToInt32(z[8]),
                    pjePrueba = Convert.ToInt32(z[9]),
                    ponderacionTotal = Convert.ToDouble(z[10]),
                    colorIndicador = z[11].ToString()
                });
                respuesta.data = registros;

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {

            }
        }

    }
}
