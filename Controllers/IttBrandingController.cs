using AutoMapper;
using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class IttBrandingController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public IttBrandingController(IRepositoryGeneral ir)//inyectamos el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// listamos los brandings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/itt/listaBranding")]
        public async Task<IHttpActionResult> ListaBranding()
        {
            entRespuesta ent = new entRespuesta();
            try
            {
                var lista = await _ir.GetAll<IttBrandingDigital>();//buscamos los brandings
                var listaViewModels = Mapper.Map<List<IttBrandingDigital>, List<IttBrandingDigitalViewModels>>(lista);//mapeamos el objeto
                ent.data = listaViewModels;
                return Ok(ent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        /// <summary>
        /// registramos el branding
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/itt/RegistrarBranding")]
        public async Task<IHttpActionResult> AddBranding(DataBrandingViewModels data)
        {
            entRespuesta ent = new entRespuesta();
            data.IdUsuario = JwtManager.getIdUserSession();
            try
            {
                var digital = new IttBrandingDigital();
                var empresa = new IttEmpresa();

                //data.logoEmpresa = data.logoEmpresa.Replace("data:image/jpeg;base64,", "");
                //var bytes = Convert.FromBase64String(data.logoEmpresa);//convertimos el archivo a bytes

                //empresa.logoEmpresa = bytes;

                //string extension = ".jpeg";
                var xImagen = data.logoEmpresa.Replace("data:image/jpeg;base64,", "");
                string path = Path.Combine(HttpRuntime.AppDomainAppPath, "Imagenes/Branding/" + data.nombreArchivo) ;//ruta donde se guardara la imagen
                File.WriteAllBytes(path, Convert.FromBase64String(xImagen));//guardamos la imagen en la ruta 

                empresa.descripcionEmpresa = data.descripcionEmpresa;
                empresa.fechaCreacion = DateTime.Now;
                empresa.idSector = data.idSector;
                empresa.mailEmpresarial = data.mailEmpresarial;
                empresa.nombreEmpresa = data.nombreEmpresa;
                empresa.paginaWeb = data.paginaWeb;
                empresa.telefonoEmpresarial = data.telefonoEmpresarial;
                empresa.vigente = true;
                empresa.idUsuario = data.IdUsuario;
                empresa.nombreEmpresa = data.nombreEmpresa;
                empresa.nombreSector = data.nombreSector;
                empresa.rutaImagen = WebConfigurationManager.AppSettings["rutaImagenBranding"].ToString() + data.nombreArchivo;
                await _ir.Add(empresa);//guardamos el registro en itt empresa

                var registroInsertado = _ir.GetLast<IttEmpresa>();//obtenemos el ultimo registro insertado

                digital.descripcionServicios = data.descripcionServicios;
                digital.estadoBranding = data.estadoBranding;
                digital.fechaCreacion = DateTime.Now;
                digital.idAgenda = null;
                digital.idEmpresa = registroInsertado.idEmpresa;
                digital.identidadDigital = data.identidadDigital;
                digital.IdUsuario = data.IdUsuario;
                digital.vigente = true;

                await _ir.Add(digital);//guardamos el registro en itt branding

                var brandingInsertado = _ir.GetLast<IttBrandingDigital>();//obtenemos el registro branding insertado
                var brandingViewmodels = Mapper.Map<IttBrandingDigital, IttBrandingDigitalViewModels>(brandingInsertado);

                ent.mensaje = "Registro insertado exitosamente";
                ent.data = brandingViewmodels;
                return Ok(ent);
            }
            catch (Exception ex)
            {
                ent.mensaje = ex.Message;
                return Ok(ent);
            }
        }

        /// <summary>
        /// guardar archivo en la base de datos
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/itt/SubirImagen")]
        public async Task<IHttpActionResult> SubirArchivo(ArchivoViewModels archivo)
        {
            archivo.idUsuario = JwtManager.getIdUserSession();
            try
            {
                var registro = await _ir.GetFirst<IttEmpresa>(z => z.idUsuario == archivo.idUsuario);
                var x = archivo.imagenBase64.Replace("data:image/jpeg;base64,", "");
                //var x = archivo.imagenBase64; 
                //string extension = ".jpeg";
                //if (registro != null)
                //{
                //var bytes = Convert.FromBase64String(archivo.imagenBase64);//convertimos el archivo a bytes
                //var filePath = HttpContext.Current.Server.MapPath("~/Imagenes/" + bytes+ extension);

                //var filename = "ccccc.jpeg";
                string path = Path.Combine(HttpRuntime.AppDomainAppPath, "Imagenes/ccccc.jpeg");
               // var path=HostingEnvironment.MapPath("~/Imagenes/ddddd.jpeg");
                File.WriteAllBytes(path, Convert.FromBase64String(x));

                    //if (!String.IsNullOrEmpty(archivo.tipoArchivo) && archivo.tipoArchivo == "imagen")
                    //{
                    //    registro.idLogo = archivo.idImagen;
                    //    registro.logoEmpresa = bytes;
                    //    await _ir.Update(registro, registro.idEmpresa);
                    //}
                //}
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// listamos todas las empressas para el branding
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/itt/listaEmpresas")]
        public async Task<IHttpActionResult> ListaEmpresas()
        {
            try
            {
                entRespuesta ent = new entRespuesta();
                var lista = await _ir.GetList<IttEmpresa>(z => z.vigente == true);//obtenemos las empresas
                var listaViewModels = Mapper.Map<List<IttEmpresa>, List<IttEmpresaViewModels>>(lista);//mapeamos el objeto
                ent.data = listaViewModels;
                return Ok(ent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        /// <summary>
        /// dejar in registro branding inhabilitado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/networking/desahabilitarBranding/{id}")]
        public async Task<IHttpActionResult> DesaHabilitarBranding(int id)
        {
            try
            {
                var idUser = JwtManager.getIdUserSession();
                entRespuesta respuesta = new entRespuesta();
                var entidad = await _ir.Find<IttBrandingDigital>(id);
                if (entidad != null && (entidad.IdUsuario == idUser))
                {
                    entidad.vigente = false;
                    await _ir.Update(entidad, entidad.idBranding);//aactualizamos el estado de vigencia
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Branding eliminado";
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Branding no encontrado";
                }
                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
    }
}