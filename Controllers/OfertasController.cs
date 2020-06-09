using AutoMapper;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class OfertasController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public OfertasController(IRepositoryGeneral ir)//inyectamos el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// obtener las areas para las ofertas de empleo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ofertas/all")]
        public async Task<IHttpActionResult> ObtenerAreas()
        {
            var lista = await _ir.GetAll<OfertasEmpleo>();//obtenemos todas las ofertas de empleo
            var listaViewModels = Mapper.Map<List<OfertasEmpleo>, List<OfertaViewModels>>(lista);
            return Ok(listaViewModels);
        }
        /// <summary>
        /// obtengo los niveles jerarquicos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ofertas/filtro/{id}")]
        public async Task<IHttpActionResult> ObtenerNivelJerarquico(string id)
        {
            var lista = await _ir.GetList<OfertasEmpleo>(z=>z.proveedorEmpleo==id);//obtenemos las ofertas de empleo por el proveedor
            var listaViewModels = Mapper.Map<List<OfertasEmpleo>, List<OfertaViewModels>>(lista);
            return Ok(listaViewModels);
        }
    }
}