using AutoMapper;
using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class CursosController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public CursosController(IRepositoryGeneral ir)//inyecto el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// simular comparativo ahorro y deuda
        /// </summary>
        /// <param name="ahorroDeudaViewModels"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/cursos/simularAhorroDeuda")]
        public async Task<IHttpActionResult> SimularAhorroDeuda(AhorroDeudaViewModels ahorroDeudaViewModels)
        {
            try
            {
                var registro = Mapper.Map<AhorroDeudaViewModels, EjercicioAhorro>(ahorroDeudaViewModels);//mapeamos el objeto
                await _ir.Add(registro);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        /// <summary>
        /// simular ahorro mensual y anual
        /// </summary>
        /// <param name="ahorroMensualAnual"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/cursos/simularAhorroDeuda")]
        public async Task<IHttpActionResult> SimularMensualAnual(AhorroMensualAnual ahorroMensualAnual)
        {
            try
            {
                ahorroMensualAnual.idUsuario = JwtManager.getIdUserSession();
                var registro = Mapper.Map<AhorroMensualAnual, EjercicioAhorro>(ahorroMensualAnual);//mapear el objeto
                await _ir.Add(registro);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// simular ahorro anual
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/cursos/simularAhorroAnual")]
        public async Task<IHttpActionResult> SimularAnual(AhorroanualViewModels ahorroanualViewModels)
        {
            try
            {
                ahorroanualViewModels.idUsuario = JwtManager.getIdUserSession();
                var registro = Mapper.Map<AhorroanualViewModels, EjercicioAhorro>(ahorroanualViewModels);//mapeamos el objeto
                await _ir.Add(registro);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// simular inversion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/cursos/simularInversion")]
        public async Task<IHttpActionResult> SimularInversion(InversionViewModels inversionViewModels)
        {
            try
            {
                inversionViewModels.idUsuario = JwtManager.getIdUserSession();
                var registro = Mapper.Map<InversionViewModels, EjercicioAhorro>(inversionViewModels);//mapeamos el objeto
                await _ir.Add(registro);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// lista los cursos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cursos/listaCursos")]
        public async Task<IHttpActionResult> ListaCursos()
        {
            try
            {
                var lista = await _ir.GetAll<FraCurso>();//obtenemos los cursos
                var registros = Mapper.Map<List<FraCurso>, List<CursosViewModels>>(lista);//mapeamos la lista
                return Ok(registros);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// lista todos los videos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/cursos/videos")]
        public async Task<IHttpActionResult> Videos()
        {
            try
            {
                var videos = await _ir.GetAll<Videos>();
                return Ok(videos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
