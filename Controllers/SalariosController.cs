using EcoOplacementApi.Repository;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class SalariosController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public SalariosController(IRepositoryGeneral ir)
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// obtener las areas de trabajo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/salarios/area")]
        public async Task<IHttpActionResult> ObtenerAreas()
        {
            var query = "[dbo].[pa_Salario_ObtenerArea]";
            var registros = await _ir.dataSinP(query);
            var tabla = registros.AsEnumerable().Select(z => new { Id = z[0].ToString(), Text = z[1].ToString() });
            return Ok(tabla);
        }
        /// <summary>
        /// obtengo los niveles jerarquicos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/salarios/jerarquico/{id}")]
        public async Task<IHttpActionResult> ObtenerNivelJerarquico(string id)
        {
            SqlParameter[] param = {new SqlParameter("@AREA",id)
                        };
            var query = "[dbo].[pa_Salario_ObtenerNivelJerarquico]";
            var registros = await _ir.data(query, param);
            var tabla = registros.AsEnumerable().Select(z => new { Id = z[0].ToString(), Text = z[1].ToString() });
            return Ok(tabla);
        }
        /// <summary>
        /// obtengo los cargos
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sector"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/salarios/cargos")]
        public async Task<IHttpActionResult> ObtenerCargos(string area,string sector)
        {
            var query = "[dbo].[pa_Salario_ObtenerCargos]";
            SqlParameter[] param = {new SqlParameter("@AREA",area),
                new SqlParameter("@SECTOR",sector)
                        };
            var registros = await _ir.data(query, param);
            var tabla = registros.AsEnumerable().Select(z => new { Id = z[0].ToString(), Text = z[1].ToString() });
            return Ok(tabla);
        }
        /// <summary>
        /// obtener el tamaño de la empresa
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sector"></param>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/salarios/obtenerEmpresas")]
        public async Task<IHttpActionResult> ObtenerTamanoEmpresa(string area, string sector,string cargo)
        {
            var query = "[dbo].[pa_Salario_ObtenerTamanoEmpresa]";
            SqlParameter[] param = {new SqlParameter("@AREA",area),
                new SqlParameter("@SECTOR",sector),
                new SqlParameter("@CARGO",cargo)
                        };
            var registros = await _ir.data(query, param);
            var tabla = registros.AsEnumerable().Select(z => new { Id = z[0].ToString(), Text = z[1].ToString() });
            return Ok(tabla);
        }
        /// <summary>
        /// obtener las regiones
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sector"></param>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/salarios/obtenerRegion")]
        public async Task<IHttpActionResult> ObtenerRegion(string area, string sector, string cargo)
        {
            var query = "[dbo].[pa_Salario_ObtenerRegion]";
            SqlParameter[] param = {new SqlParameter("@AREA",area),
                new SqlParameter("@SECTOR",sector),
                new SqlParameter("@CARGO",cargo)
                        };
            var registros = await _ir.data(query,param);
            var tabla = registros.AsEnumerable().Select(z => new { Id = z[0].ToString(), Text = z[1].ToString() });
            return Ok(tabla);
        }
        /// <summary>
        /// obtener los graficos
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sector"></param>
        /// <param name="cargo"></param>
        /// <param name="region"></param>
        /// <param name="tamano"></param>
        /// <param name="salario"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/salarios/ObtenerGraficos")]
        public async Task<IHttpActionResult> ObtenerGraficos(string area, string sector, string cargo, string region,string tamano, float salario)
        {
            try
            {
                var query = "[dbo].[pa_ObtenerGraficosSalarios]";
                SqlParameter[] param = {new SqlParameter("@AREA",area),
                new SqlParameter("@SECTOR",sector),
                new SqlParameter("@CARGO",cargo),
                new SqlParameter("@REGION",region),
                new SqlParameter("@TAMANO",tamano),
                new SqlParameter("@SALARIO",salario)
                        };
                var registros = await _ir.data(query, param);
                var tabla = registros.AsEnumerable().Select(z => new { JsonGrafico1 = z[0].ToString(), JsonGrafico2 = z[1].ToString(), JsonGrafico3 = z[2].ToString() });
                return Ok(tabla);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
