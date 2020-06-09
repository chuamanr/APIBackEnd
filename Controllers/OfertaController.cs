using EcoOplacementApi.Repository;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using EcoOplacementApi.Models;
using EcoOplacementApi.ViewModels;
using EcoOplacementApi.Filters.Security;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class OfertaController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public OfertaController(IRepositoryGeneral ir)//inyectamos el objeto
        //{
        //    _ir = ir;
        //}
        [HttpGet]
        [Route("api/oferta/sectores")]
        public async Task<IHttpActionResult> Sectores()
        {
            try
            {
                var query = "[dbo].[pa_ObtenerOfertasLaborales_sector]";
                var registros = await _ir.GetAllMocksysSinParametros(query, 2);
                var tabla = registros.AsEnumerable().Select(z => new { Sector = z[0].ToString() });
                return Ok(tabla);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/oferta/ciudades")]
        public async Task<IHttpActionResult> Ciudades()
        {
            try
            {
                var query = "[dbo].[pa_ObtenerOfertasLaborales_ciudad]";
                var registros = await _ir.GetAllMocksysSinParametros(query, 2);
                var tabla = registros.AsEnumerable().Select(z => new { Ciudad = z[0].ToString() });
                return Ok(tabla);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/oferta/salarios")]
        public async Task<IHttpActionResult> Salarios()
        {
            try
            {
                var query = "[dbo].[pa_ObtenerOfertasLaborales_salario]";
                var registros = await _ir.GetAllMocksysSinParametros(query, 2);
                var tabla = registros.AsEnumerable().Select(z => new { Salario = z[0].ToString() });
                return Ok(tabla);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/oferta/ofertas")]
        public async Task<IHttpActionResult> Ofertas(string sector, string ciudad, string salario, int cantidad, string busqueda)
        {
            try
            {
                var salarios = salario.Replace(",","-");
                var query = "[dbo].[pa_ObtenerOfertasLaborales]";
                SqlParameter[] param = {new SqlParameter("@sector",sector),
                new SqlParameter("@ciudad",ciudad),
                new SqlParameter("@salarios",salarios),
                new SqlParameter("@topCantidad",cantidad),
                new SqlParameter("@busqueda",busqueda)
                        };
                var registros = await _ir.dataOutPlacement(query, param);
                var tabla = registros.AsEnumerable().Select(z => new
                {
                    idOferta = z[0].ToString(),
                    idUsuarioModificador = z[1].ToString(),
                    idUsuario = z[2].ToString(),
                    tituloOferta = z[3].ToString(),
                    descripcionOferta = z[4].ToString(),
                    salarioOferta = z[5].ToString(),
                    ciudadOferta = z[6].ToString(),
                    fechaPublicacionOferta = z[7],
                    fechaCreacion = z[8].ToString(),
                    fechaActualizacion = z[9].ToString(),
                    estadoOferta = z[10].ToString(),
                    vigente = z[11].ToString(),
                    link = z[12].ToString(),
                    idOfertaProveedor = z[13].ToString(),
                    proveedorEmpleo = z[14].ToString(),
                    sector = z[15].ToString(),
                    tipoContrato = z[16].ToString(),
                    razonSocial = z[17].ToString(),
                    cantidadVacantes = z[18].ToString(),
                    requisitos = z[19].ToString(),
                    experienciaRequerida = z[20].ToString(),
                    fechaVencimiento = z[21].ToString(),
                    area = z[22].ToString()

                });
                return Ok(tabla);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/oferta/aplicarOferta")]
        public async Task<IHttpActionResult> aplicarOferta(AplicacionOfertasViewModel aplicacion)
        {
            aplicacion.idUsuario = JwtManager.getIdUserSession();
            entRespuesta res = new entRespuesta();
            var registro = new DataTable();
            try
            {
                var oferta = await _ir.GetFirst<OfertasEmpleo>(x => x.idOfertaPlacement == aplicacion.idOferta);
                if (oferta == null)
                {
                    SqlParameter[] parametros =
                    {
                        new SqlParameter("@id", aplicacion.idOferta)
                    };

                    var query = "dbo.pa_ObtenerOfertaAplicada";
                    registro = await _ir.dataOutPlacement(query, parametros);
                    var tabla = registro.Rows;

                    var descrip = (tabla[0].ItemArray[4]).ToString();
                    var descripcion = descrip.Replace("\n"," ");

                    #region Agregar Oferta
                    var ofertas = new OfertasEmpleo();
                    ofertas.idOfertaPlacement = Convert.ToInt32(tabla[0].ItemArray[0]);
                    ofertas.idUsuarioModificador = Convert.ToInt32(tabla[0].ItemArray[1]);
                    ofertas.idUsuario = Convert.ToInt32(tabla[0].ItemArray[2]);
                    ofertas.tituloOferta = (tabla[0].ItemArray[3]).ToString();
                    ofertas.descripcionOferta = descripcion;
                    ofertas.salarioOferta = (tabla[0].ItemArray[5]).ToString();
                    ofertas.ciudadOferta = (tabla[0].ItemArray[6]).ToString();
                    ofertas.fechaPublicacionOferta = Convert.ToDateTime(tabla[0].ItemArray[7]);
                    ofertas.fechaCreacion = Convert.ToDateTime(tabla[0].ItemArray[8]);                   
                    if (tabla[0].ItemArray[9].ToString() != "") ofertas.fechaActualizacion = Convert.ToDateTime(tabla[0].ItemArray[9]);
                    ofertas.estadoOferta = Convert.ToInt32(tabla[0].ItemArray[10]);
                    ofertas.vigente = Convert.ToBoolean(tabla[0].ItemArray[11]);
                    ofertas.link = (tabla[0].ItemArray[12]).ToString();
                    ofertas.idOfertaProveedor = (tabla[0].ItemArray[13]).ToString();
                    ofertas.proveedorEmpleo = (tabla[0].ItemArray[14]).ToString();
                    ofertas.sector = (tabla[0].ItemArray[15]).ToString();
                    ofertas.tipoContrato = (tabla[0].ItemArray[16]).ToString();
                    ofertas.razonSocial = (tabla[0].ItemArray[17]).ToString();
                    ofertas.cantidadVacantes = (tabla[0].ItemArray[18]).ToString();
                    ofertas.requisitos = (tabla[0].ItemArray[19]).ToString();
                    ofertas.experienciaRequerida = (tabla[0].ItemArray[20]).ToString();
                    if (tabla[0].ItemArray[21].ToString() != "") ofertas.fechaVencimiento = Convert.ToDateTime(tabla[0].ItemArray[21]);
                    ofertas.area = (tabla[0].ItemArray[22]).ToString();
                    await _ir.Add<OfertasEmpleo>(ofertas);
                    #endregion
                }
                var aplica = new AplicacionesOfertas();
                aplica.idOferta = aplicacion.idOferta;
                aplica.idUsuario = aplicacion.idUsuario;
                aplica.vigente = aplicacion.vigente;
                aplica.fechaCreacion = DateTime.Now;
                aplica.fechaAplicacion = DateTime.Now;
                await _ir.Add<AplicacionesOfertas>(aplica);

                res.codigo = 0;
                res.mensaje = "Oferta aplicada";
                return Ok(res);

            }
            catch (Exception e)
            {
                 res.codigo = 1;
                res.mensaje = e.Message;
                return Ok(res);
            }
        }
    }
}
