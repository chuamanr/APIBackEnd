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
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class HojaController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public HojaController(IRepositoryGeneral ir)//inyecto el objeto
        //{
        //    _ir = ir;
        //}

        /// <summary>
        /// actualizo los datos personales y laborales del usuario
        /// </summary>
        /// <param name="dato"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Hoja/UpdateDatosPersonales")]
        public async Task<IHttpActionResult> DatosPersonales(DatosPersonalesViewModels dato)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                dato.idUsuario = JwtManager.getIdUserSession();
                var encode = new FuncionesViewModels();
                var hoja = await _ir.GetFirst<HojaVidaDatosPersonales>(z => z.idUsuario == dato.idUsuario);//buscamos los datos del usuario para actualizar
                if (hoja != null)
                {
                    hoja.dicCiudadResidencia = dato.dicCiudadResidencia;
                    hoja.dicDepartamento = dato.dicDepartamento;
                    hoja.dicTipoDocumento = dato.dicTipoDocumento;
                    hoja.nombres = dato.nombres;
                    hoja.apellidoPaterno = dato.apellidoPaterno;
                    hoja.apellidoMaterno = dato.apellidoMaterno;
                    hoja.rutaFoto = dato.rutaFoto;
                    hoja.correoElectronico = dato.correoElectronico;
                    hoja.telefonoCelular = dato.telefonoCelular;
                    hoja.direccionResidencia = dato.direccionResidencia;
                    if (!String.IsNullOrEmpty(dato.anosExperiencia)) hoja.anosExperiencia = encode.Base64_Encode(dato.anosExperiencia);
                    hoja.dicAspiracionSalarial = dato.dicAspiracionSalarial;
                    hoja.paisesTrabajar = dato.paisesTrabajar;
                    hoja.redesSociales = dato.redesSociales;
                    hoja.movilidadLaboral = dato.movilidadLaboral;
                    hoja.fechaNacimiento = Convert.ToDateTime(dato.fechaNacimiento);
                    hoja.genero = dato.genero;
                    if (!String.IsNullOrEmpty(dato.dicProfesion)) hoja.dicProfesion = encode.Base64_Encode(dato.dicProfesion);
                    if (!String.IsNullOrEmpty(dato.dicAreaTrabajo)) hoja.dicAreaTrabajo = encode.Base64_Encode(dato.dicAreaTrabajo);
                    hoja.habilidades = dato.habilidades;
                    if (!String.IsNullOrEmpty(dato.idiomaNivel)) hoja.idiomaNivel = encode.Base64_Encode(dato.idiomaNivel);
                    hoja.descripcionPerfilProfesional = dato.descripcionPerfilProfesional;
                    hoja.fechaActualizacion = DateTime.Now;

                    await _ir.Update(hoja, hoja.idDatosPersonales);
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Datos actualizados exitosamente";
                    respuesta.data = "";
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Usuario no registrado en la base";
                    return Ok(respuesta);
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }

        }
        /// <summary>
        /// inhabilito los estudios
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Hoja/NoVigenteEstudio/{id}")]
        public async Task<IHttpActionResult> DeshabilitarEstudio(int id)
        {
            try
            {
                entRespuesta respuesta = new entRespuesta();
                var idUser = JwtManager.getIdUserSession();
                var entidad = await _ir.Find<HojaVidaInformacionEducativa>(id);//obtener el estudio correspondiente al id
                if (entidad != null && (entidad.idUsuario == idUser))//en caso que no sea null dejamos como no vigente el estudio
                {
                    entidad.vigente = false;
                    await _ir.Update(entidad, entidad.idInformacionEducativa);
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Estudio Eliminado";
                    respuesta.data = "";
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Estudio no encontrado";
                }
                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// inserto los estudios principales
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Hoja/InsertaEstudioPrincipal")]
        public async Task<IHttpActionResult> InsertarEstudioPrincipal(DatosEducacionViewModels registro)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                registro.idUsuario = JwtManager.getIdUserSession();
                var hoja = Mapper.Map<DatosEducacionViewModels, HojaVidaInformacionEducativa>(registro);//mapeamos el objeto
                var encode = new FuncionesViewModels();
                hoja.vigente = true;
                hoja.fechaCreacion = DateTime.Now;
                hoja.esComplementario = false;
                hoja.fechaInicio = Convert.ToDateTime(registro.fechaInicio);
                if (registro.estadoEstudio == "En proceso")
                {
                    hoja.fechaFin = null;
                }
                else
                {
                    hoja.fechaFin = Convert.ToDateTime(registro.fechaFin);
                }
                if (!String.IsNullOrEmpty(registro.dicAreaEstudio.ToString())) hoja.dicAreaEstudio = encode.Base64_Encode(registro.dicAreaEstudio.ToString());
                if (!String.IsNullOrEmpty(registro.dicCiudadEstudio.ToString())) hoja.dicCiudadEstudio = encode.Base64_Encode(registro.dicCiudadEstudio.ToString());
                if (!String.IsNullOrEmpty(registro.dicNivelEducativo.ToString())) hoja.dicNivelEducativo = encode.Base64_Encode(registro.dicNivelEducativo.ToString());
                if (!String.IsNullOrEmpty(registro.dicTipoEstudio.ToString())) hoja.dicTipoEstudio = encode.Base64_Encode(registro.dicTipoEstudio.ToString());
                if (!String.IsNullOrEmpty(registro.dicPaisExtranjero.ToString())) hoja.dicPaisExtranjero = encode.Base64_Encode(registro.dicPaisExtranjero.ToString());
                if (!String.IsNullOrEmpty(registro.institucionEducativa)) hoja.institucionEducativa = encode.Base64_Encode(registro.institucionEducativa);
                if (!String.IsNullOrEmpty(registro.tituloObtenido)) hoja.tituloObtenido = encode.Base64_Encode(registro.tituloObtenido);
                if (!String.IsNullOrEmpty(registro.estadoEstudio)) hoja.estadoEstudio = encode.Base64_Encode(registro.estadoEstudio);
                if (!String.IsNullOrEmpty(registro.descripcionEducacion)) hoja.descripcionEducacion = encode.Base64_Encode(registro.descripcionEducacion);
                await _ir.Add(hoja);//insertamosel estudio
                if (!String.IsNullOrEmpty(hoja.dicAreaEstudio)) hoja.dicAreaEstudio = encode.Base64_Decode(hoja.dicAreaEstudio);
                if (!String.IsNullOrEmpty(hoja.dicCiudadEstudio)) hoja.dicCiudadEstudio = encode.Base64_Decode(hoja.dicCiudadEstudio);
                if (!String.IsNullOrEmpty(hoja.dicNivelEducativo)) hoja.dicNivelEducativo = encode.Base64_Decode(hoja.dicNivelEducativo);
                if (!String.IsNullOrEmpty(hoja.dicTipoEstudio)) hoja.dicTipoEstudio = encode.Base64_Decode(hoja.dicTipoEstudio);
                if (!String.IsNullOrEmpty(hoja.dicPaisExtranjero)) hoja.dicPaisExtranjero = encode.Base64_Decode(hoja.dicPaisExtranjero);
                if (!String.IsNullOrEmpty(hoja.institucionEducativa)) hoja.institucionEducativa = encode.Base64_Decode(hoja.institucionEducativa);
                if (!String.IsNullOrEmpty(hoja.tituloObtenido)) hoja.tituloObtenido = encode.Base64_Decode(hoja.tituloObtenido);
                if (!String.IsNullOrEmpty(hoja.estadoEstudio)) hoja.estadoEstudio = encode.Base64_Decode(hoja.estadoEstudio);
                if (!String.IsNullOrEmpty(hoja.descripcionEducacion)) hoja.descripcionEducacion = encode.Base64_Decode(hoja.descripcionEducacion);
                var entidad = Mapper.Map<HojaVidaInformacionEducativa, DatosEducacionViewModels>(hoja);//mapeamos el objeto para devolverlo por json
                respuesta.codigo = 0;
                respuesta.mensaje = "Estudio principal agregado";
                respuesta.data = entidad;
                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        /// <summary>
        /// inserto en la tabla los rstudios complementarios
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/hoja/InsertaEstudioComplementario")]
        public async Task<IHttpActionResult> InsertaEstudioComplementario(DatosEducacionComplementariosViewModels registro)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                registro.idUsuario = JwtManager.getIdUserSession();
                var encode = new FuncionesViewModels();
                var hoja = Mapper.Map<DatosEducacionComplementariosViewModels, HojaVidaInformacionEducativa>(registro);//mapeamos el objeto
                hoja.fechaInicio = Convert.ToDateTime(registro.fechaInicio);
                if (registro.estadoEstudio == "En proceso")
                {
                    hoja.fechaFin = null;
                }
                else
                {
                    hoja.fechaFin = Convert.ToDateTime(registro.fechaFin);
                }

                hoja.vigente = true;
                hoja.fechaCreacion = DateTime.Now;
                hoja.esComplementario = true;
                if (!String.IsNullOrEmpty(registro.dicAreaEstudio.ToString())) hoja.dicAreaEstudio = encode.Base64_Encode(registro.dicAreaEstudio.ToString());
                if (!String.IsNullOrEmpty(registro.dicCiudadEstudio.ToString())) hoja.dicCiudadEstudio = encode.Base64_Encode(registro.dicCiudadEstudio.ToString());
                if (!String.IsNullOrEmpty(registro.dicIntensidadEstudio.ToString())) hoja.dicIntensidadEstudio = encode.Base64_Encode(registro.dicIntensidadEstudio.ToString());
                if (!String.IsNullOrEmpty(registro.dicTipoEstudio.ToString())) hoja.dicTipoEstudio = encode.Base64_Encode(registro.dicTipoEstudio.ToString());
                if (!String.IsNullOrEmpty(registro.dicPaisExtranjero.ToString())) hoja.dicPaisExtranjero = encode.Base64_Encode(registro.dicPaisExtranjero.ToString());
                if (!String.IsNullOrEmpty(registro.institucionEducativa)) hoja.institucionEducativa = encode.Base64_Encode(registro.institucionEducativa);
                if (!String.IsNullOrEmpty(registro.tituloObtenido)) hoja.tituloObtenido = encode.Base64_Encode(registro.tituloObtenido);
                if (!String.IsNullOrEmpty(registro.estadoEstudio)) hoja.estadoEstudio = encode.Base64_Encode(registro.estadoEstudio);
                if (!String.IsNullOrEmpty(registro.otroEstudio)) hoja.otroEstudio = encode.Base64_Encode(registro.otroEstudio);
                if (!String.IsNullOrEmpty(registro.descripcionEducacion)) hoja.descripcionEducacion = encode.Base64_Encode(registro.descripcionEducacion);
                await _ir.Add(hoja);//insertamos el estudio complementario
                if (!String.IsNullOrEmpty(hoja.dicAreaEstudio)) hoja.dicAreaEstudio = encode.Base64_Decode(hoja.dicAreaEstudio);
                if (!String.IsNullOrEmpty(hoja.dicCiudadEstudio)) hoja.dicCiudadEstudio = encode.Base64_Decode(hoja.dicCiudadEstudio);
                if (!String.IsNullOrEmpty(hoja.dicIntensidadEstudio)) hoja.dicIntensidadEstudio = encode.Base64_Decode(hoja.dicIntensidadEstudio);
                if (!String.IsNullOrEmpty(hoja.dicTipoEstudio)) hoja.dicTipoEstudio = encode.Base64_Decode(hoja.dicTipoEstudio);
                if (!String.IsNullOrEmpty(hoja.dicPaisExtranjero)) hoja.dicPaisExtranjero = encode.Base64_Decode(hoja.dicPaisExtranjero);
                if (!String.IsNullOrEmpty(hoja.institucionEducativa)) hoja.institucionEducativa = encode.Base64_Decode(hoja.institucionEducativa);
                if (!String.IsNullOrEmpty(hoja.tituloObtenido)) hoja.tituloObtenido = encode.Base64_Decode(hoja.tituloObtenido);
                if (!String.IsNullOrEmpty(hoja.estadoEstudio)) hoja.estadoEstudio = encode.Base64_Decode(hoja.estadoEstudio);
                if (!String.IsNullOrEmpty(hoja.otroEstudio)) hoja.otroEstudio = encode.Base64_Decode(hoja.otroEstudio);
                if (!String.IsNullOrEmpty(hoja.descripcionEducacion)) hoja.descripcionEducacion = encode.Base64_Decode(hoja.descripcionEducacion);
                var entidad = Mapper.Map<HojaVidaInformacionEducativa, DatosEducacionComplementariosViewModels>(hoja);//mapeamos el objeto para devolverlo por json
                respuesta.codigo = 0;
                respuesta.mensaje = "Estudio complementario agregado";
                respuesta.data = entidad;
                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// edito los estudios complementarios
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/hoja/EditarEstudioComplementario")]
        public async Task<IHttpActionResult> EditarEstudioComplementario(DatosEducacionComplementariosViewModels registro)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                registro.idUsuario = JwtManager.getIdUserSession();
                var encode = new FuncionesViewModels();
                var entidad = await _ir.Find<HojaVidaInformacionEducativa>(registro.idInformacionEducativa);//buscamos el id de estudio complementario
                if (entidad != null)
                {
                    entidad.fechaActualizacion = DateTime.Now;
                    entidad.idDatosPersonales = registro.idDatosPersonales;
                    entidad.idUsuario = registro.idUsuario;
                    entidad.dicAreaEstudio = !String.IsNullOrEmpty(registro.dicAreaEstudio.ToString()) ? encode.Base64_Encode(registro.dicAreaEstudio.ToString()) : "";
                    entidad.dicCiudadEstudio = !String.IsNullOrEmpty(registro.dicCiudadEstudio.ToString()) ? encode.Base64_Encode(registro.dicCiudadEstudio.ToString()) : "";
                    entidad.dicIntensidadEstudio = !String.IsNullOrEmpty(registro.dicIntensidadEstudio.ToString()) ? encode.Base64_Encode(registro.dicIntensidadEstudio.ToString()) : "";
                    entidad.dicTipoEstudio = !String.IsNullOrEmpty(registro.dicTipoEstudio.ToString()) ? encode.Base64_Encode(registro.dicTipoEstudio.ToString()) : "";
                    entidad.dicPaisExtranjero = !String.IsNullOrEmpty(registro.dicPaisExtranjero.ToString()) ? encode.Base64_Encode(registro.dicPaisExtranjero.ToString()) : "";
                    entidad.institucionEducativa = !String.IsNullOrEmpty(registro.institucionEducativa) ? encode.Base64_Encode(registro.institucionEducativa) : "";
                    entidad.tituloObtenido = !String.IsNullOrEmpty(registro.tituloObtenido) ? encode.Base64_Encode(registro.tituloObtenido) : "";
                    entidad.estadoEstudio = !String.IsNullOrEmpty(registro.estadoEstudio) ? encode.Base64_Encode(registro.estadoEstudio) : "";
                    entidad.otroEstudio = !String.IsNullOrEmpty(registro.otroEstudio) ? encode.Base64_Encode(registro.otroEstudio) : "";
                    entidad.descripcionEducacion = !String.IsNullOrEmpty(registro.descripcionEducacion) ? encode.Base64_Encode(registro.descripcionEducacion) : "";
                    entidad.fechaInicio = Convert.ToDateTime(registro.fechaInicio);
                    if (registro.estadoEstudio == "En proceso")
                    {
                        entidad.fechaFin = null;
                    }
                    else
                    {
                        entidad.fechaFin = Convert.ToDateTime(registro.fechaFin);
                    }

                    await _ir.Update(entidad, entidad.idInformacionEducativa);//actualizamos el registro
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Estudio complementario editado";
                    if (!String.IsNullOrEmpty(entidad.dicAreaEstudio)) entidad.dicAreaEstudio = encode.Base64_Decode(entidad.dicAreaEstudio);
                    if (!String.IsNullOrEmpty(entidad.dicCiudadEstudio)) entidad.dicCiudadEstudio = encode.Base64_Decode(entidad.dicCiudadEstudio);
                    if (!String.IsNullOrEmpty(entidad.dicIntensidadEstudio)) entidad.dicIntensidadEstudio = encode.Base64_Decode(entidad.dicIntensidadEstudio);
                    if (!String.IsNullOrEmpty(entidad.dicTipoEstudio)) entidad.dicTipoEstudio = encode.Base64_Decode(entidad.dicTipoEstudio);
                    if (!String.IsNullOrEmpty(entidad.dicPaisExtranjero)) { entidad.dicPaisExtranjero = encode.Base64_Decode(entidad.dicPaisExtranjero); } else { entidad.dicPaisExtranjero = null; }
                    if (!String.IsNullOrEmpty(entidad.institucionEducativa)) entidad.institucionEducativa = encode.Base64_Decode(entidad.institucionEducativa);
                    if (!String.IsNullOrEmpty(entidad.tituloObtenido)) entidad.tituloObtenido = encode.Base64_Decode(entidad.tituloObtenido);
                    if (!String.IsNullOrEmpty(entidad.estadoEstudio)) entidad.estadoEstudio = encode.Base64_Decode(entidad.estadoEstudio);
                    if (!String.IsNullOrEmpty(entidad.otroEstudio)) entidad.otroEstudio = encode.Base64_Decode(entidad.otroEstudio);
                    if (!String.IsNullOrEmpty(entidad.descripcionEducacion)) entidad.descripcionEducacion = encode.Base64_Decode(entidad.descripcionEducacion);
                    var entidadViewModels = Mapper.Map<HojaVidaInformacionEducativa, DatosEducacionComplementariosViewModels>(entidad);//mapeamos el objeto para devolverlo por json
                    respuesta.data = entidadViewModels;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.mensaje = "Id no corresponde a estudio principal";
                    respuesta.codigo = 1;
                    return Ok(respuesta);
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// edito los estudios principales
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/hoja/EditarEstudioPrincipal")]
        public async Task<IHttpActionResult> EditarEstudioPrincipal(DatosEducacionViewModels registro)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                registro.idUsuario = JwtManager.getIdUserSession();
                var encode = new FuncionesViewModels();
                var entidad = await _ir.Find<HojaVidaInformacionEducativa>(registro.idInformacionEducativa);//buscamos el id delestudio principal
                if (entidad != null)
                {
                    entidad.idDatosPersonales = registro.idDatosPersonales;
                    entidad.idUsuario = registro.idUsuario;
                    if (!String.IsNullOrEmpty(registro.dicAreaEstudio.ToString())) entidad.dicAreaEstudio = encode.Base64_Encode(registro.dicAreaEstudio.ToString());
                    if (!String.IsNullOrEmpty(registro.dicCiudadEstudio.ToString())) entidad.dicCiudadEstudio = encode.Base64_Encode(registro.dicCiudadEstudio.ToString());
                    if (!String.IsNullOrEmpty(registro.dicNivelEducativo.ToString())) entidad.dicNivelEducativo = encode.Base64_Encode(registro.dicNivelEducativo.ToString());
                    if (!String.IsNullOrEmpty(registro.dicTipoEstudio.ToString())) entidad.dicTipoEstudio = encode.Base64_Encode(registro.dicTipoEstudio.ToString());
                    if (!String.IsNullOrEmpty(registro.dicPaisExtranjero.ToString())) entidad.dicPaisExtranjero = encode.Base64_Encode(registro.dicPaisExtranjero.ToString());
                    if (!String.IsNullOrEmpty(registro.institucionEducativa)) entidad.institucionEducativa = encode.Base64_Encode(registro.institucionEducativa);
                    if (!String.IsNullOrEmpty(registro.tituloObtenido)) entidad.tituloObtenido = encode.Base64_Encode(registro.tituloObtenido);
                    if (!String.IsNullOrEmpty(registro.estadoEstudio)) entidad.estadoEstudio = encode.Base64_Encode(registro.estadoEstudio);
                    if (!String.IsNullOrEmpty(registro.descripcionEducacion)) entidad.descripcionEducacion = encode.Base64_Encode(registro.descripcionEducacion);
                    entidad.fechaInicio = Convert.ToDateTime(registro.fechaInicio);
                    if (registro.estadoEstudio == "En proceso")
                    {
                        entidad.fechaFin = null;
                    }
                    else
                    {
                        entidad.fechaFin = Convert.ToDateTime(registro.fechaFin);
                    }
                    entidad.fechaActualizacion = DateTime.Now;
                    await _ir.Update(entidad, entidad.idInformacionEducativa);//actualizamos el registro
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Estudio principal editado";
                    if (!String.IsNullOrEmpty(entidad.dicAreaEstudio)) entidad.dicAreaEstudio = encode.Base64_Decode(entidad.dicAreaEstudio);
                    if (!String.IsNullOrEmpty(entidad.dicCiudadEstudio)) entidad.dicCiudadEstudio = encode.Base64_Decode(entidad.dicCiudadEstudio);
                    if (!String.IsNullOrEmpty(entidad.dicNivelEducativo)) entidad.dicNivelEducativo = encode.Base64_Decode(entidad.dicNivelEducativo);
                    if (!String.IsNullOrEmpty(entidad.dicTipoEstudio)) entidad.dicTipoEstudio = encode.Base64_Decode(entidad.dicTipoEstudio);
                    if (!String.IsNullOrEmpty(entidad.dicPaisExtranjero)) entidad.dicPaisExtranjero = encode.Base64_Decode(entidad.dicPaisExtranjero);
                    if (!String.IsNullOrEmpty(entidad.institucionEducativa)) entidad.institucionEducativa = encode.Base64_Decode(entidad.institucionEducativa);
                    if (!String.IsNullOrEmpty(entidad.tituloObtenido)) entidad.tituloObtenido = encode.Base64_Decode(entidad.tituloObtenido);
                    if (!String.IsNullOrEmpty(entidad.estadoEstudio)) entidad.estadoEstudio = encode.Base64_Decode(entidad.estadoEstudio);
                    if (!String.IsNullOrEmpty(entidad.descripcionEducacion)) entidad.descripcionEducacion = encode.Base64_Decode(entidad.descripcionEducacion);
                    var entidadViewModels = Mapper.Map<HojaVidaInformacionEducativa, DatosEducacionViewModels>(entidad);//mapeamos el objeto
                    respuesta.data = entidadViewModels;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.mensaje = "Id no corresponde a estudio principal";
                    respuesta.codigo = 1;
                    return Ok(respuesta);
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// registro el idioma
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idioma"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/hoja/GuardaIdioma/{id}")]
        public async Task<IHttpActionResult> GuardaIdioma(int id, [FromBody] string idioma)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var idUser = JwtManager.getIdUserSession();
                var encode = new FuncionesViewModels();
                var entidad = await _ir.Find<HojaVidaDatosPersonales>(id);//buscamos el id y insertamos el idioma
                if (entidad != null && (entidad.idUsuario == idUser))
                {
                    entidad.idiomaNivel = encode.Base64_Encode(idioma);
                    entidad.fechaActualizacion = DateTime.Now;
                    await _ir.Update(entidad, entidad.idDatosPersonales);
                }

                respuesta.codigo = 0;
                respuesta.mensaje = "Idioma agregado exitosamente";

                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// insertamos la experiencia laboral
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/hoja/InsertaExperienciaLaboral")]
        public async Task<IHttpActionResult> InsertaExperienciaLaboral(DatosLaboralesViewModels registro)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var idUser = JwtManager.getIdUserSession();
                var entidadHV = await _ir.Find<HojaVidaDatosPersonales>(registro.idDatosPersonales);//buscamos el id usuario para validar
                if (entidadHV != null && (entidadHV.idUsuario != idUser))
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Error al agregar experiencia laboral";
                }
                var encode = new FuncionesViewModels();
                var hoja = Mapper.Map<DatosLaboralesViewModels, HojaVidaExperienciaLaboral>(registro);
                if (!String.IsNullOrEmpty(registro.empresa)) hoja.empresa = encode.Base64_Encode(registro.empresa);
                if (!String.IsNullOrEmpty(registro.cargo)) hoja.cargo = encode.Base64_Encode(registro.cargo);
                if (!String.IsNullOrEmpty(registro.funcionesLogros)) hoja.funcionesLogros = encode.Base64_Encode(registro.funcionesLogros);
                if (!String.IsNullOrEmpty(registro.trabajoActualmente)) hoja.trabajoActualmente = encode.Base64_Encode(registro.trabajoActualmente);
                hoja.fechaCreacion = DateTime.Now;
                hoja.vigente = true;
                hoja.fechaIngreso = Convert.ToDateTime(registro.fechaIngreso);
                hoja.fechaSalida = Convert.ToDateTime(registro.fechaSalida);
                await _ir.Add(hoja);//actualizamos los datos en hoja de vida personales
                var recibido = _ir.GetLast<HojaVidaExperienciaLaboral>();//obtenemos el ultimo registro insertado
                if (!String.IsNullOrEmpty(recibido.empresa)) recibido.empresa = encode.Base64_Decode(recibido.empresa);
                if (!String.IsNullOrEmpty(recibido.cargo)) recibido.cargo = encode.Base64_Decode(recibido.cargo);
                if (!String.IsNullOrEmpty(recibido.funcionesLogros)) recibido.funcionesLogros = encode.Base64_Decode(recibido.funcionesLogros);
                if (!String.IsNullOrEmpty(recibido.trabajoActualmente)) recibido.trabajoActualmente = encode.Base64_Decode(recibido.trabajoActualmente);
                var entidad = Mapper.Map<HojaVidaExperienciaLaboral, DatosLaboralesViewModels>(recibido);
                respuesta.codigo = 0;
                respuesta.mensaje = "Experiencia laboral agregada";
                respuesta.data = entidad;

                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// editamos la experiencia laboral
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/hoja/EditaExperienciaLaboral")]
        public async Task<IHttpActionResult> EditaExperienciaLaboral(DatosLaboralesViewModels registro)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var encode = new FuncionesViewModels();
                var idUser = JwtManager.getIdUserSession();
                var entidad = await _ir.Find<HojaVidaExperienciaLaboral>(registro.idExperienciaLaboral);//buscamos los datos de hoja de vida por el id
                if (entidad != null)
                {
                    var entidadHV = await _ir.Find<HojaVidaDatosPersonales>(entidad.idDatosPersonales);//buscamos el id usuario para validar
                    if (entidadHV.idUsuario != idUser)
                    {
                        respuesta.codigo = 1;
                        respuesta.mensaje = "Experiencia laboral no encontrada";
                        return Ok(respuesta);
                    }
                    entidad.idDatosPersonales = registro.idDatosPersonales;
                    if (!String.IsNullOrEmpty(registro.empresa)) entidad.empresa = encode.Base64_Encode(registro.empresa);
                    if (!String.IsNullOrEmpty(registro.cargo)) entidad.cargo = encode.Base64_Encode(registro.cargo);
                    if (!String.IsNullOrEmpty(registro.funcionesLogros)) entidad.funcionesLogros = encode.Base64_Encode(registro.funcionesLogros);
                    if (!String.IsNullOrEmpty(registro.trabajoActualmente)) entidad.trabajoActualmente = encode.Base64_Encode(registro.trabajoActualmente);
                    entidad.fechaIngreso = Convert.ToDateTime(registro.fechaIngreso);
                    entidad.fechaSalida = Convert.ToDateTime(registro.fechaSalida);
                    entidad.fechaActualizacion = DateTime.Now;
                    await _ir.Update(entidad, entidad.idExperienciaLaboral);//actualizamos el registro
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Experiencia laboral modificada";
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Experiencia laboral no encontrada";
                    return Ok(respuesta);
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// desahabilitar la experiencia laboral
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/hoja/NoVigenteLaboral/{id}")]
        public async Task<IHttpActionResult> DeshabilitarLaboral(int id)
        {
            try
            {
                entRespuesta respuesta = new entRespuesta();
                var idUser = JwtManager.getIdUserSession();
                var entidad = await _ir.Find<HojaVidaExperienciaLaboral>(id);//buscamos los datos de experiencia laboral para inhabilitar
                if (entidad != null)
                {
                    var entidadHV = await _ir.Find<HojaVidaDatosPersonales>(entidad.idDatosPersonales);//buscamos el id usuario para validar
                    if (entidadHV.idUsuario != idUser)
                    {
                        respuesta.codigo = 1;
                        respuesta.mensaje = "Experiencia laboral no encontrada";
                        return Ok(respuesta);
                    }
                    entidad.vigente = false;
                    await _ir.Update(entidad, entidad.idExperienciaLaboral);//procedemos a actualizar el estado
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
        /// traigo los datos del usuario y su hoja de vida por el id de datos personales
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hoja/TraeDatosPersonalesId/{id}")]
        public async Task<IHttpActionResult> GetDatosPersonalesxId(int id)
        {
            entRespuesta respuesta = new entRespuesta();

            try
            {
                id = JwtManager.getIdUserSession();
                var decode = new FuncionesViewModels();
                var entidad = await _ir.FindEspecial<HojaVidaDatosPersonales>(id);//obtener los datos de hoja de vida datos personales por el idHojaDatospersonales
                if (!String.IsNullOrEmpty(entidad.anosExperiencia)) entidad.anosExperiencia = decode.Base64_Decode(entidad.anosExperiencia);
                if (!String.IsNullOrEmpty(entidad.dicProfesion)) entidad.dicProfesion = decode.Base64_Decode(entidad.dicProfesion);
                if (!String.IsNullOrEmpty(entidad.dicAreaTrabajo)) entidad.dicAreaTrabajo = decode.Base64_Decode(entidad.dicAreaTrabajo);
                if (!String.IsNullOrEmpty(entidad.idiomaNivel)) entidad.idiomaNivel = decode.Base64_Decode(entidad.idiomaNivel);
                var x = new entHojaVida();
                x = Mapper.Map<HojaVidaDatosPersonales, entHojaVida>(entidad);//obtener los datos de educacion
                foreach (var y in entidad.HojaVidaInformacionEducativa.ToList())
                {
                    if (!String.IsNullOrEmpty(y.dicAreaEstudio)) y.dicAreaEstudio = decode.Base64_Decode(y.dicAreaEstudio);
                    if (!String.IsNullOrEmpty(y.dicCiudadEstudio)) y.dicCiudadEstudio = decode.Base64_Decode(y.dicCiudadEstudio);
                    if (!String.IsNullOrEmpty(y.dicIntensidadEstudio)) y.dicIntensidadEstudio = decode.Base64_Decode(y.dicIntensidadEstudio);
                    if (!String.IsNullOrEmpty(y.dicNivelEducativo)) y.dicNivelEducativo = decode.Base64_Decode(y.dicNivelEducativo);
                    if (!String.IsNullOrEmpty(y.dicTipoEstudio)) y.dicTipoEstudio = decode.Base64_Decode(y.dicTipoEstudio);
                    if (!String.IsNullOrEmpty(y.dicPaisExtranjero)) y.dicPaisExtranjero = decode.Base64_Decode(y.dicPaisExtranjero);
                    if (!String.IsNullOrEmpty(y.institucionEducativa)) y.institucionEducativa = decode.Base64_Decode(y.institucionEducativa);
                    if (!String.IsNullOrEmpty(y.tituloObtenido)) y.tituloObtenido = decode.Base64_Decode(y.tituloObtenido);
                    if (!String.IsNullOrEmpty(y.estadoEstudio)) y.estadoEstudio = decode.Base64_Decode(y.estadoEstudio);
                    if (!String.IsNullOrEmpty(y.otroEstudio)) y.otroEstudio = decode.Base64_Decode(y.otroEstudio);
                    if (!String.IsNullOrEmpty(y.descripcionEducacion)) y.descripcionEducacion = decode.Base64_Decode(y.descripcionEducacion);
                }
                var listaEducativa = Mapper.Map<List<HojaVidaInformacionEducativa>, List<entHojaVidaInformacionEducativa>>(entidad.HojaVidaInformacionEducativa.ToList());
                listaEducativa = listaEducativa.Where(c => c.vigente == true).ToList();//filtramos los que esten vigentes
                x.EntHojaVidaInformacionEducativa = listaEducativa;
                //obtener los datos de experiencia laboral
                foreach (var z in entidad.HojaVidaExperienciaLaboral.ToList())
                {
                    if (!String.IsNullOrEmpty(z.empresa)) z.empresa = decode.Base64_Decode(z.empresa);
                    if (!String.IsNullOrEmpty(z.cargo)) z.cargo = decode.Base64_Decode(z.cargo);
                    if (!String.IsNullOrEmpty(z.funcionesLogros)) z.funcionesLogros = decode.Base64_Decode(z.funcionesLogros);
                    if (!String.IsNullOrEmpty(z.trabajoActualmente)) z.trabajoActualmente = decode.Base64_Decode(z.trabajoActualmente);
                }
                var listaLaboral = Mapper.Map<List<HojaVidaExperienciaLaboral>, List<entExperienciaLaboral>>(entidad.HojaVidaExperienciaLaboral.ToList());
                listaLaboral = listaLaboral.Where(v => v.vigente == true).ToList();//filtramos los que esten vigentes
                x.EntExperienciaLaboral = listaLaboral;
                if (entidad != null)
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "OK";
                    respuesta.data = x;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Usuario no existente en la base de datos";
                }

                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// traigo los datos del usuario y su hoja de vida por el id de usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hoja/TraeDatosPersonalesIdUser/{id}")]
        public async Task<IHttpActionResult> GetDatosPersonalesxIdUser(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                id = JwtManager.getIdUserSession();
                var decode = new FuncionesViewModels();
                var entidad = await _ir.GetFirstEspecial<HojaVidaDatosPersonales>(z => z.idUsuario == id);//buscamos los datos de hoja de vida por id de usuario
                if (!String.IsNullOrEmpty(entidad.anosExperiencia)) entidad.anosExperiencia = decode.Base64_Decode(entidad.anosExperiencia);
                if (!String.IsNullOrEmpty(entidad.dicProfesion)) entidad.dicProfesion = decode.Base64_Decode(entidad.dicProfesion);
                if (!String.IsNullOrEmpty(entidad.dicAreaTrabajo)) entidad.dicAreaTrabajo = decode.Base64_Decode(entidad.dicAreaTrabajo);
                if (!String.IsNullOrEmpty(entidad.idiomaNivel)) entidad.idiomaNivel = decode.Base64_Decode(entidad.idiomaNivel);
                var x = new entHojaVida();
                x = Mapper.Map<HojaVidaDatosPersonales, entHojaVida>(entidad);
                foreach (var y in entidad.HojaVidaInformacionEducativa.ToList())
                {
                    if (!String.IsNullOrEmpty(y.dicAreaEstudio)) y.dicAreaEstudio = decode.Base64_Decode(y.dicAreaEstudio);
                    if (!String.IsNullOrEmpty(y.dicCiudadEstudio)) y.dicCiudadEstudio = decode.Base64_Decode(y.dicCiudadEstudio);
                    if (!String.IsNullOrEmpty(y.dicIntensidadEstudio)) y.dicIntensidadEstudio = decode.Base64_Decode(y.dicIntensidadEstudio);
                    if (!String.IsNullOrEmpty(y.dicNivelEducativo)) y.dicNivelEducativo = decode.Base64_Decode(y.dicNivelEducativo);
                    if (!String.IsNullOrEmpty(y.dicTipoEstudio)) y.dicTipoEstudio = decode.Base64_Decode(y.dicTipoEstudio);
                    if (!String.IsNullOrWhiteSpace(y.dicPaisExtranjero)) { y.dicPaisExtranjero = decode.Base64_Decode(y.dicPaisExtranjero); } else { y.dicPaisExtranjero = null; }
                    if (!String.IsNullOrEmpty(y.institucionEducativa)) y.institucionEducativa = decode.Base64_Decode(y.institucionEducativa);
                    if (!String.IsNullOrEmpty(y.tituloObtenido)) y.tituloObtenido = decode.Base64_Decode(y.tituloObtenido);
                    if (!String.IsNullOrEmpty(y.estadoEstudio)) y.estadoEstudio = decode.Base64_Decode(y.estadoEstudio);
                    if (!String.IsNullOrEmpty(y.otroEstudio)) y.otroEstudio = decode.Base64_Decode(y.otroEstudio);
                    if (!String.IsNullOrEmpty(y.descripcionEducacion)) y.descripcionEducacion = decode.Base64_Decode(y.descripcionEducacion);
                }
                var listaEducativa = Mapper.Map<List<HojaVidaInformacionEducativa>, List<entHojaVidaInformacionEducativa>>(entidad.HojaVidaInformacionEducativa.ToList());
                listaEducativa = listaEducativa.Where(c => c.vigente == true).ToList();//filtramos los que esten vigentes
                x.EntHojaVidaInformacionEducativa = listaEducativa;
                foreach (var z in entidad.HojaVidaExperienciaLaboral.ToList())
                {
                    if (!String.IsNullOrEmpty(z.empresa)) z.empresa = decode.Base64_Decode(z.empresa);
                    if (!String.IsNullOrEmpty(z.cargo)) z.cargo = decode.Base64_Decode(z.cargo);
                    if (!String.IsNullOrEmpty(z.funcionesLogros)) z.funcionesLogros = decode.Base64_Decode(z.funcionesLogros);
                    if (!String.IsNullOrEmpty(z.trabajoActualmente)) z.trabajoActualmente = decode.Base64_Decode(z.trabajoActualmente);
                }
                var listaLaboral = Mapper.Map<List<HojaVidaExperienciaLaboral>, List<entExperienciaLaboral>>(entidad.HojaVidaExperienciaLaboral.ToList());
                listaLaboral = listaLaboral.Where(v => v.vigente == true).ToList();//filtramos los que esten vigentes
                x.EntExperienciaLaboral = listaLaboral;

                if (entidad != null)
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "OK";
                    respuesta.data = x;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Usuario no existente en la base de datos";
                }

                //var objeto = new EntidadesViewModels();
                //objeto.educativa = entidad.HojaVidaInformacionEducativa;
                if (entidad != null)
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "OK";
                    respuesta.data = x;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Usuario no existente en la base de datos";
                }

                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// traigo los datos del usuario y su hoja de vida por la identificacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hoja/TraeDatosPersonalesxIdentificacion/{id}")]
        public async Task<IHttpActionResult> GetDatosPersonalesxIdentificacion(string id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                id = JwtManager.getIdUserSession().ToString();
                var decode = new FuncionesViewModels();
                var entidad = await _ir.GetFirstEspecial<HojaVidaDatosPersonales>(z => z.identificacion == id);//buscamos los datos de hoja de vida por la identificacion
                if (!String.IsNullOrEmpty(entidad.anosExperiencia)) entidad.anosExperiencia = decode.Base64_Decode(entidad.anosExperiencia);
                if (!String.IsNullOrEmpty(entidad.dicProfesion)) entidad.dicProfesion = decode.Base64_Decode(entidad.dicProfesion);
                if (!String.IsNullOrEmpty(entidad.dicAreaTrabajo)) entidad.dicAreaTrabajo = decode.Base64_Decode(entidad.dicAreaTrabajo);
                if (!String.IsNullOrEmpty(entidad.idiomaNivel)) entidad.idiomaNivel = decode.Base64_Decode(entidad.idiomaNivel);
                var x = new entHojaVida();
                x = Mapper.Map<HojaVidaDatosPersonales, entHojaVida>(entidad);
                foreach (var y in entidad.HojaVidaInformacionEducativa.ToList())
                {
                    if (!String.IsNullOrEmpty(y.dicAreaEstudio)) y.dicAreaEstudio = decode.Base64_Decode(y.dicAreaEstudio);
                    if (!String.IsNullOrEmpty(y.dicCiudadEstudio)) y.dicCiudadEstudio = decode.Base64_Decode(y.dicCiudadEstudio);
                    if (!String.IsNullOrEmpty(y.dicIntensidadEstudio)) y.dicIntensidadEstudio = decode.Base64_Decode(y.dicIntensidadEstudio);
                    if (!String.IsNullOrEmpty(y.dicNivelEducativo)) y.dicNivelEducativo = decode.Base64_Decode(y.dicNivelEducativo);
                    if (!String.IsNullOrEmpty(y.dicTipoEstudio)) y.dicTipoEstudio = decode.Base64_Decode(y.dicTipoEstudio);
                    if (!String.IsNullOrEmpty(y.dicPaisExtranjero)) y.dicPaisExtranjero = decode.Base64_Decode(y.dicPaisExtranjero);
                    if (!String.IsNullOrEmpty(y.institucionEducativa)) y.institucionEducativa = decode.Base64_Decode(y.institucionEducativa);
                    if (!String.IsNullOrEmpty(y.tituloObtenido)) y.tituloObtenido = decode.Base64_Decode(y.tituloObtenido);
                    if (!String.IsNullOrEmpty(y.estadoEstudio)) y.estadoEstudio = decode.Base64_Decode(y.estadoEstudio);
                    if (!String.IsNullOrEmpty(y.otroEstudio)) y.otroEstudio = decode.Base64_Decode(y.otroEstudio);
                    if (!String.IsNullOrEmpty(y.descripcionEducacion)) y.descripcionEducacion = decode.Base64_Decode(y.descripcionEducacion);
                }
                var listaEducativa = Mapper.Map<List<HojaVidaInformacionEducativa>, List<entHojaVidaInformacionEducativa>>(entidad.HojaVidaInformacionEducativa.ToList());
                listaEducativa = listaEducativa.Where(c => c.vigente == true).ToList();//filtramos los que esten vigentes
                x.EntHojaVidaInformacionEducativa = listaEducativa;
                foreach (var z in entidad.HojaVidaExperienciaLaboral.ToList())
                {
                    if (!String.IsNullOrEmpty(z.empresa)) z.empresa = decode.Base64_Decode(z.empresa);
                    if (!String.IsNullOrEmpty(z.cargo)) z.cargo = decode.Base64_Decode(z.cargo);
                    if (!String.IsNullOrEmpty(z.funcionesLogros)) z.funcionesLogros = decode.Base64_Decode(z.funcionesLogros);
                    if (!String.IsNullOrEmpty(z.trabajoActualmente)) z.trabajoActualmente = decode.Base64_Decode(z.trabajoActualmente);
                }
                var listaLaboral = Mapper.Map<List<HojaVidaExperienciaLaboral>, List<entExperienciaLaboral>>(entidad.HojaVidaExperienciaLaboral.ToList());
                listaLaboral = listaLaboral.Where(v => v.vigente == true).ToList();//filtramos los que esten vigentes
                x.EntExperienciaLaboral = listaLaboral;
                if (entidad != null)
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "OK";
                    respuesta.data = x;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Usuario no existente en la base de datos";
                }

                return Ok(respuesta);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        /// <summary>
        /// descargar pdf
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hoja/DescargaPdf/{id}")]
        public async Task<HttpResponseMessage> DescargarPdf(int id)
        {
            try
            {
                id = JwtManager.getIdUserSession();
                var user = await _ir.GetFirst<HojaVidaDatosPersonales>(z => z.idUsuario == id);//buscar los datos de hoja de vida por id
                if (user != null && user.pdfHojaDeVida != null)
                {
                    var bytes = user.pdfHojaDeVida;//convertimos el archivo a bytes
                    MemoryStream pdfStream = new MemoryStream(bytes);

                    pdfStream.Position = 0;

                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    result.Content = new StreamContent(pdfStream);
                    //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    //result.Content.Headers.ContentDisposition.FileName = "Hola.pdf";
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    result.Content.Headers.ContentLength = pdfStream.Length;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// obtengo el curriculum y lo muestro en formato pdf
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hoja/Pdf/{id}")]
        public async Task<HttpResponseMessage> ObtenerPdf(int id)
        {
            id = JwtManager.getIdUserSession();
            var decode = new FuncionesViewModels();
            var user = await _ir.FindEspecial<Usuario>(id);//obtener los datos del usuario
            var obtenerDatosPersonales = await _ir.GetFirstEspecial<HojaVidaDatosPersonales>(z => z.idUsuario == id);//obtener los datos de hoja de vida personales por el id
            if (obtenerDatosPersonales != null && obtenerDatosPersonales.pdfHojaDeVida != null)//si el usuario tiene un curriculum ya cargado lo mostramos
            {
                if (!String.IsNullOrEmpty(obtenerDatosPersonales.anosExperiencia)) obtenerDatosPersonales.anosExperiencia = decode.Base64_Decode(obtenerDatosPersonales.anosExperiencia);
                if (!String.IsNullOrEmpty(obtenerDatosPersonales.dicProfesion)) obtenerDatosPersonales.dicProfesion = decode.Base64_Decode(obtenerDatosPersonales.dicProfesion);
                if (!String.IsNullOrEmpty(obtenerDatosPersonales.dicAreaTrabajo)) obtenerDatosPersonales.dicAreaTrabajo = decode.Base64_Decode(obtenerDatosPersonales.dicAreaTrabajo);
                if (!String.IsNullOrEmpty(obtenerDatosPersonales.idiomaNivel)) obtenerDatosPersonales.idiomaNivel = decode.Base64_Decode(obtenerDatosPersonales.idiomaNivel);
                foreach (var y in obtenerDatosPersonales.HojaVidaInformacionEducativa.ToList())
                {
                    if (!String.IsNullOrEmpty(y.dicAreaEstudio)) y.dicAreaEstudio = decode.Base64_Decode(y.dicAreaEstudio);
                    if (!String.IsNullOrEmpty(y.dicCiudadEstudio)) y.dicCiudadEstudio = decode.Base64_Decode(y.dicCiudadEstudio);
                    if (!String.IsNullOrEmpty(y.dicIntensidadEstudio)) y.dicIntensidadEstudio = decode.Base64_Decode(y.dicIntensidadEstudio);
                    if (!String.IsNullOrEmpty(y.dicNivelEducativo)) y.dicNivelEducativo = decode.Base64_Decode(y.dicNivelEducativo);
                    if (!String.IsNullOrEmpty(y.dicTipoEstudio)) y.dicTipoEstudio = decode.Base64_Decode(y.dicTipoEstudio);
                    if (!String.IsNullOrEmpty(y.dicPaisExtranjero)) y.dicPaisExtranjero = decode.Base64_Decode(y.dicPaisExtranjero);
                    if (!String.IsNullOrEmpty(y.institucionEducativa)) y.institucionEducativa = decode.Base64_Decode(y.institucionEducativa);
                    if (!String.IsNullOrEmpty(y.tituloObtenido)) y.tituloObtenido = decode.Base64_Decode(y.tituloObtenido);
                    if (!String.IsNullOrEmpty(y.estadoEstudio)) y.estadoEstudio = decode.Base64_Decode(y.estadoEstudio);
                    if (!String.IsNullOrEmpty(y.otroEstudio)) y.otroEstudio = decode.Base64_Decode(y.otroEstudio);
                    if (!String.IsNullOrEmpty(y.descripcionEducacion)) y.descripcionEducacion = decode.Base64_Decode(y.descripcionEducacion);
                }
                foreach (var z in obtenerDatosPersonales.HojaVidaExperienciaLaboral.ToList())
                {
                    if (!String.IsNullOrEmpty(z.empresa)) z.empresa = decode.Base64_Decode(z.empresa);
                    if (!String.IsNullOrEmpty(z.cargo)) z.cargo = decode.Base64_Decode(z.cargo);
                    if (!String.IsNullOrEmpty(z.funcionesLogros)) z.funcionesLogros = decode.Base64_Decode(z.funcionesLogros);
                    if (!String.IsNullOrEmpty(z.trabajoActualmente)) z.trabajoActualmente = decode.Base64_Decode(z.trabajoActualmente);
                }
                var dataStream = new MemoryStream(obtenerDatosPersonales.pdfHojaDeVida);//cargamos en memoria el objeto
                HttpResponseMessage resultMensaje = new HttpResponseMessage(HttpStatusCode.OK);
                resultMensaje.Content = new StreamContent(dataStream);
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //result.Content.Headers.ContentDisposition.FileName = "Hola.pdf";
                resultMensaje.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                resultMensaje.Content.Headers.ContentLength = dataStream.Length;
                return resultMensaje;
            }

            var hoja = user.HojaVidaDatosPersonales.FirstOrDefault();//obtenemos el primer registro
            string html = "";
            if (hoja != null)
            {
                if (!String.IsNullOrEmpty(hoja.anosExperiencia)) hoja.anosExperiencia = decode.Base64_Decode(hoja.anosExperiencia);
                if (!String.IsNullOrEmpty(hoja.dicProfesion)) hoja.dicProfesion = decode.Base64_Decode(hoja.dicProfesion);
                if (!String.IsNullOrEmpty(hoja.dicAreaTrabajo)) hoja.dicAreaTrabajo = decode.Base64_Decode(hoja.dicAreaTrabajo);
                if (!String.IsNullOrEmpty(hoja.idiomaNivel)) hoja.idiomaNivel = decode.Base64_Decode(hoja.idiomaNivel);
                foreach (var y in hoja.HojaVidaInformacionEducativa.ToList())
                {
                    if (!String.IsNullOrEmpty(y.dicAreaEstudio)) y.dicAreaEstudio = decode.Base64_Decode(y.dicAreaEstudio);
                    if (!String.IsNullOrEmpty(y.dicCiudadEstudio)) y.dicCiudadEstudio = decode.Base64_Decode(y.dicCiudadEstudio);
                    if (!String.IsNullOrEmpty(y.dicIntensidadEstudio)) y.dicIntensidadEstudio = decode.Base64_Decode(y.dicIntensidadEstudio);
                    if (!String.IsNullOrEmpty(y.dicNivelEducativo)) y.dicNivelEducativo = decode.Base64_Decode(y.dicNivelEducativo);
                    if (!String.IsNullOrEmpty(y.dicTipoEstudio)) y.dicTipoEstudio = decode.Base64_Decode(y.dicTipoEstudio);
                    if (!String.IsNullOrEmpty(y.dicPaisExtranjero)) y.dicPaisExtranjero = decode.Base64_Decode(y.dicPaisExtranjero);
                    if (!String.IsNullOrEmpty(y.institucionEducativa)) y.institucionEducativa = decode.Base64_Decode(y.institucionEducativa);
                    if (!String.IsNullOrEmpty(y.tituloObtenido)) y.tituloObtenido = decode.Base64_Decode(y.tituloObtenido);
                    if (!String.IsNullOrEmpty(y.estadoEstudio)) y.estadoEstudio = decode.Base64_Decode(y.estadoEstudio);
                    if (!String.IsNullOrEmpty(y.otroEstudio)) y.otroEstudio = decode.Base64_Decode(y.otroEstudio);
                    if (!String.IsNullOrEmpty(y.descripcionEducacion)) y.descripcionEducacion = decode.Base64_Decode(y.descripcionEducacion);
                }
                foreach (var z in hoja.HojaVidaExperienciaLaboral.ToList())
                {
                    if (!String.IsNullOrEmpty(z.empresa)) z.empresa = decode.Base64_Decode(z.empresa);
                    if (!String.IsNullOrEmpty(z.cargo)) z.cargo = decode.Base64_Decode(z.cargo);
                    if (!String.IsNullOrEmpty(z.funcionesLogros)) z.funcionesLogros = decode.Base64_Decode(z.funcionesLogros);
                    if (!String.IsNullOrEmpty(z.trabajoActualmente)) z.trabajoActualmente = decode.Base64_Decode(z.trabajoActualmente);
                }
                var entidad = await _ir.Find<PlantillaHojaDeVida>(hoja.idPlantillaAplicada);//buscamos la planilla de hoja de vida generica
                html = entidad.htmlPlantilla;//obtenemos la plantilla
                html = html.Replace("#Nombre", user.nombres + " " + user.apellidoPaterno + " " + user.apellidoMaterno);//reemplazamos los nombres y los apellidos.
                if (hoja.fotoPerfil != null)//si el usuario tiene foto de perfil la agregamos a la plantilla
                {
                    string str = Convert.ToBase64String(hoja.fotoPerfil);
                    string link = "data:Image/png;base64," + str;

                    html = html.Replace("#imagen", link);
                }
                var x = await _ir.Find<DiccionarioDatos>(hoja.dicTipoDocumento);//obtenemos los datos del tipo de documento desde la base para reeemplazar en la plantilla 
                //por los valores correspondientes
                if (x != null && x.nombreDiccionario == "Cédula de Ciudadanía")
                {
                    html = html.Replace("#Pais", "Colombiana");
                }
                else
                {
                    html = html.Replace("#Pais", "Extranjera");
                }
                //reeemplazamos distintos valores en la planilla
                html = html.Replace("#identificacion", hoja.Usuario.identificacion);
                html = html.Replace("#nacimiento", hoja.fechaNacimiento.GetValueOrDefault().ToShortDateString());
                html = html.Replace("#direccion", hoja.direccionResidencia);
                html = html.Replace("#fono", hoja.telefonoCelular);
                html = html.Replace("#email", hoja.correoElectronico);
                html = html.Replace("#descripcion", hoja.descripcionPerfilProfesional);
                html = html.Replace("#tipoTextoHV", hoja.tipoTextoHV);
                html = html.Replace("#colorHV", hoja.colorHV);
                if (hoja.fechaNacimiento != null && hoja.fechaNacimiento.GetValueOrDefault().Year > 1850)
                {
                    DateTime nacimiento = hoja.fechaNacimiento.GetValueOrDefault(); //obtenemos la edad y la reemplazamos en la plantilla
                    int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                    html = html.Replace("#Edad", Convert.ToString(edad));
                }
                //buscamos todos los estudios vigentes
                var listaeEducativa = hoja.HojaVidaInformacionEducativa.Where(z => z.esComplementario == false && z.vigente == true).ToList();
                var cadenaEducativa = "";
                foreach (var item in listaeEducativa)//para cada item vamos reemplazando valores e irlos agregando a la plantilla
                {
                    string tagPrincipales = "";
                    tagPrincipales = entidad.tagEstudiosPrincipales;
                    string profesion = "";
                    string ciudad = "";
                    if (item.dicNivelEducativo != null)
                    {
                        var diccionarioEducacion = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicNivelEducativo));//obtenemos el nivel educativo
                        if (diccionarioEducacion != null)
                        {
                            profesion = diccionarioEducacion.nombreDiccionario;
                        }
                    }
                    if (item.dicCiudadEstudio != null)
                    {
                        var diccionarioCiudad = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicCiudadEstudio));//obtenemos la ciudad de estudio
                        if (diccionarioCiudad != null)
                        {
                            ciudad = diccionarioCiudad.nombreDiccionario;
                        }
                    }
                    tagPrincipales = tagPrincipales.Replace("#profesion", profesion);
                    tagPrincipales = tagPrincipales.Replace("#titulo", item.tituloObtenido);
                    tagPrincipales = tagPrincipales.Replace("#institucion", item.institucionEducativa);
                    tagPrincipales = tagPrincipales.Replace("#inicio", item.fechaInicio.GetValueOrDefault().ToShortDateString());
                    if (item.fechaFin != null)
                    {
                        tagPrincipales = tagPrincipales.Replace("#termino", item.fechaFin.GetValueOrDefault().ToShortDateString());
                    }
                    else
                    {
                        tagPrincipales = tagPrincipales.Replace("#termino", "En proceso");
                    }
                    tagPrincipales = tagPrincipales.Replace("#ciudad", ciudad);
                    cadenaEducativa = cadenaEducativa + tagPrincipales;
                }
                html = html.Replace("#estudioprincipal", cadenaEducativa);

                //obtenemos los datos de los estudios complementarios que esten vigentes
                var listaComplementario = hoja.HojaVidaInformacionEducativa.Where(z => z.esComplementario == true && z.vigente == true).ToList();
                var cadenaComplementario = "Sin estudios complementarios";
                foreach (var item in listaComplementario)//para cada item vamos reemplazando valores e irlos agregando a la plantilla
                {
                    //cadenaComplementario = ""; PABLO OTAYZA 23-04-2020
                    string tagComplementarios = "";
                    tagComplementarios = entidad.tagEstudiosComplementarios;
                    string profesion = "";
                    string ciudad = "";
                    string intensidadEstudio = "";
                    string tipoEstudio = "";
                    if (item.dicNivelEducativo != null)
                    {
                        var diccionarioComplementario = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicNivelEducativo));//obtenemos el nivel educativo
                        if (diccionarioComplementario != null)
                        {
                            profesion = diccionarioComplementario.nombreDiccionario;
                        }
                    }
                    if (item.dicCiudadEstudio != null)
                    {
                        var diccionarioCiudad = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicCiudadEstudio));//obtenemos la ciudad de estudio
                        if (diccionarioCiudad != null)
                        {
                            ciudad = diccionarioCiudad.nombreDiccionario;
                        }
                    }
                    if (item.dicIntensidadEstudio != null)
                    {
                        var diccionarioIntensidad = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicIntensidadEstudio));
                        if (diccionarioIntensidad != null)
                        {
                            intensidadEstudio = diccionarioIntensidad.nombreDiccionario;
                        }
                    }
                    if (item.dicTipoEstudio != null)
                    {
                        var diccionarioTipoEstudio = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicTipoEstudio));
                        if (diccionarioTipoEstudio != null)
                        {
                            tipoEstudio = diccionarioTipoEstudio.nombreDiccionario;
                        }
                    }
                    tagComplementarios = tagComplementarios.Replace("#profesion", profesion);
                    tagComplementarios = tagComplementarios.Replace("#titulo", item.tituloObtenido);
                    tagComplementarios = tagComplementarios.Replace("#institucion", item.institucionEducativa);
                    tagComplementarios = tagComplementarios.Replace("#inicio", item.fechaInicio.GetValueOrDefault().ToShortDateString());
                    if (item.fechaFin != null)
                    {
                        tagComplementarios = tagComplementarios.Replace("#termino", item.fechaFin.GetValueOrDefault().ToShortDateString());
                    }
                    else
                    {
                        tagComplementarios = tagComplementarios.Replace("#termino", "En proceso");
                    }
                    tagComplementarios = tagComplementarios.Replace("#ciudad", ciudad);
                    tagComplementarios = tagComplementarios.Replace("#intensidad", intensidadEstudio + " horas");
                    tagComplementarios = tagComplementarios.Replace("#tipoEstudio", tipoEstudio);

                    cadenaComplementario = cadenaComplementario + tagComplementarios;

                    //cadenaComplementario = cadenaComplementario + "<p>" + profesion + "/" + item.tituloObtenido + "/" + item.institucionEducativa + "<br/>" + item.fechaInicio.GetValueOrDefault().ToShortDateString()
                    //    + "-" + item.fechaFin.GetValueOrDefault().ToShortDateString() + ciudad + "</p>";
                }
                html = html.Replace("#estudiocomplementario", cadenaComplementario);
                //obtenemos la experiencia laboral
                var listaLaboral = hoja.HojaVidaExperienciaLaboral.Where(z => z.vigente == true).ToList();
                var cadenaLaboral = "";
                foreach (var item in listaLaboral)
                {
                    var tagLaboral = "";
                    tagLaboral = entidad.tagExperienciaLaboral;

                    tagLaboral = tagLaboral.Replace("#empresa", item.empresa);
                    tagLaboral = tagLaboral.Replace("#cargo", item.cargo);
                    tagLaboral = tagLaboral.Replace("#inicio", item.fechaIngreso.GetValueOrDefault().ToShortDateString());
                    tagLaboral = tagLaboral.Replace("#termino", item.fechaSalida.GetValueOrDefault().ToShortDateString());

                    cadenaLaboral = cadenaLaboral + tagLaboral;
                }
                html = html.Replace("#laboral", cadenaLaboral);

                var listaidioma = hoja.idiomaNivel;
                var cadenaIdioma = "Sin idiomas a ingresar";
                if (listaidioma != null && listaidioma.Length > 7)//si el valor no es null obtenemos los idiomas
                {
                    cadenaIdioma = "";
                    var tagIdioma = "";
                    tagIdioma = entidad.tagIdioma;
                    dynamic datos = JsonConvert.DeserializeObject(listaidioma);
                    foreach (var item in datos)
                    {
                        DiccionarioDatos compruebaIdioma = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.id));//obtenemos el nivel del idioma
                        string nivel = "";
                        if (item.nivel == "1")
                        {
                            nivel = "Básico";
                        }
                        else if (item.nivel == "2")
                        {
                            nivel = "Intermedio";
                        }
                        else if (item.nivel == "3")
                        {
                            nivel = "Avanzado";
                        }
                        cadenaIdioma = cadenaIdioma + tagIdioma.Replace("#nivel", nivel).Replace("#idiomaTag", compruebaIdioma.nombreDiccionario);
                        //cadenaIdioma = cadenaIdioma + "<p>" + compruebaIdioma.nombreDiccionario + "" + compruebaNivel.nombreDiccionario + "</p>";
                    }
                }
                html = html.Replace("#idioma", cadenaIdioma);


                string red = "Sin redes sociales ingresadas";
                string iconoLink = "< svg xmlns = 'http://www.w3.org/2000/svg' xmlns: xlink = 'http://www.w3.org/1999/xlink' width = '34' height = '34' viewBox = '0 0 24 24' > < defs > < path id = 'a' d = 'M0 1.18c0 .69.502 1.18 1.183 1.18.7 0 1.238-.454 1.238-1.18C2.42.473 1.9 0 1.22 0 .538 0 0 .472 0 1.18zM5.81 4.36h-.035l-.107-1.073H3.677c.07 2.543.053 5.067.053 7.608h2.296V6.52c0-.2 0-.4.072-.6.179-.508.645-.89 1.183-.89.951 0 1.165.855 1.165 1.653v4.212h2.314V6.41c0-1.743-.753-3.287-2.654-3.287-.915 0-1.811.436-2.296 1.236zM.054 10.894H2.35V3.286H.054v7.608z' /> </ defs > < g fill = 'none' fill - rule = 'evenodd' > < circle cx = '12' cy = '12' r = '12' fill = '#0C79BC' /> < path fill = '#FFF' d = 'M15.23 17.677v-4.212c0-.8-.216-1.653-1.166-1.653-.539 0-1.005.382-1.184.89-.072.2-.072.4-.072.6v4.375h-2.295c0-2.542.018-5.066-.054-7.608h1.99l.108 1.071h.036c.484-.798 1.381-1.235 2.296-1.235 1.9 0 2.654 1.544 2.654 3.287v4.485h-2.314zm-8.393 0h2.295V10.07H6.837v7.608zM8.002 6.783c.682 0 1.202.471 1.202 1.18 0 .726-.539 1.18-1.238 1.18-.681 0-1.183-.49-1.183-1.18 0-.709.538-1.18 1.22-1.18z' /> < g transform = 'translate(6.783 6.783)' > < mask id = 'b' fill = '#fff' > < use xlink: href = '#a' /> </ mask > < path fill = '#FFF' d = '' mask = 'url(#b)' /> </ g > </ g ></ svg >";
                string iconoFace = "<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='34' height='34' viewBox='0 0 24 24'> <defs> <path id='a' d='M1.071 2.902v1.052H0V6.04h1.071v5.758h2.174V6.04h2.158V3.954H3.245v-1.24c0-.517.91-.784 1.33-.784.341 0 .714.063 1.04.157L5.937.235C5.338.032 4.235 0 3.618 0 1.817 0 1.071 1.318 1.071 2.902z'/> </defs> <g fill='none' fill-rule='evenodd'> <ellipse cx='12' cy='11.773' fill='#2E77B1' rx='12' ry='11.773'/> <path fill='#FFF' d='M15.005 8.229a3.865 3.865 0 0 0-1.038-.157c-.422 0-1.33.267-1.33.784v1.24h2.158v2.086h-2.159v5.758h-2.174v-5.758h-1.07v-2.086h1.07V9.045c0-1.585.747-2.903 2.548-2.903.616 0 1.72.032 2.32.235l-.325 1.852z'/> <g transform='translate(9.391 6.142)'> <mask id='b' fill='#fff'> <use xlink:href='#a'/> </mask> <path fill='#FFF' d='M-84.522 2670.327h712.696V-222.655H-84.522z' mask='url(#b)'/> </g> </g></svg>";
                string iconoOtro = "<svg xmlns='http://www.w3.org/2000/svg' width='34' height='34' viewBox='0 0 24 24'> <g fill='none' fill-rule='evenodd'> <circle cx='12' cy='12' r='12' fill='#66D17E'/> <path fill='#FFF' d='M15.43 11.775c-.017-.687-.1-1.358-.234-2.012h2.112c.285.62.453 1.291.503 2.012h-2.38zm1.945 2.532l-1.962-1.408c.017-.151.017-.302.017-.453h2.381a6.151 6.151 0 0 1-.436 1.86zm-.453 1.173l-1.005.05.318.504.805 1.224a.346.346 0 0 1-.118.503c-.033.017-.05.033-.083.033h-.017c-.017 0-.05.017-.067.017h-.017c-.033 0-.05 0-.084-.017h-.016c-.018 0-.034-.016-.05-.016 0 0-.017 0-.017-.017-.017-.017-.034-.017-.05-.034l-.017-.017a.22.22 0 0 1-.05-.067l-.37-.553-.788-1.173-.386.972c-.016.033-.033.067-.05.084l-.017.016c-.016.017-.033.034-.05.034s-.017.017-.033.017c-.017 0-.034.017-.05.017h-.168c-.017 0-.034-.017-.05-.034 0 0-.017 0-.017-.017-.017-.017-.05-.033-.067-.067a6.955 6.955 0 0 1-.05-.1l-.286-1.04-.067-.235-.687-2.716c0-.033-.017-.05 0-.084v-.016c0-.017 0-.034.016-.05v-.017c.017-.017.017-.034.034-.068l.05-.05s.017 0 .017-.017c.017-.016.033-.016.05-.016h.017c.017 0 .05-.017.067-.017h.1c.034.017.05.017.085.033l.939.671.62.453 1.106.788.654.47c.235.167.134.536-.15.552zm-3.94 2.264c-.285.05-.57.067-.871.067-.336 0-.654-.033-.973-.084-.553-.754-.973-1.643-1.257-2.598h3.32l.419 1.643c-.185.335-.403.67-.638.972zm-5.7-2.615h1.91c.236.838.571 1.626.99 2.347a5.661 5.661 0 0 1-2.9-2.347zm-.856-2.683h2.381c.017.687.101 1.358.235 2.012H6.913a5.716 5.716 0 0 1-.487-2.012zm.487-2.683h2.13a11.378 11.378 0 0 0-.252 2.012H6.426a5.716 5.716 0 0 1 .487-2.012zm3.336-3.035A10.332 10.332 0 0 0 9.21 9.093H7.282a5.68 5.68 0 0 1 2.967-2.365zm1.862-.318c.335 0 .654.033.972.083.553.755.972 1.644 1.258 2.6H9.914a8.365 8.365 0 0 1 1.324-2.616c.286-.05.57-.067.873-.067zm-2.65 5.365c.017-.687.118-1.358.268-2.012h4.78c.15.654.217 1.325.25 2.012H9.462zm3.22 1.241l.369 1.442H9.713c-.15-.654-.218-1.325-.252-2.012h3.236a.943.943 0 0 0-.017.57zm4.258-3.923h-1.894a10.067 10.067 0 0 0-.99-2.348 5.69 5.69 0 0 1 2.884 2.348zm1.543 3.018a6.369 6.369 0 0 0-6.371-6.372 6.369 6.369 0 0 0-6.372 6.372c0 3.52 2.85 6.37 6.372 6.37a6.298 6.298 0 0 0 3.42-1.005l.368.537c.185.284.52.452.856.452a1 1 0 0 0 .537-.15c.234-.152.402-.387.47-.655.066-.268 0-.553-.152-.788l-.503-.754a.922.922 0 0 0 .721-.67 1.04 1.04 0 0 0 0-.554c.42-.839.654-1.778.654-2.783z'/> </g></svg>";
                if (!String.IsNullOrEmpty(hoja.redesSociales))//obtenemos las redes sociales del usuario
                {
                    red = "";
                    dynamic datos = JsonConvert.DeserializeObject(hoja.redesSociales);
                    foreach (var item in datos)
                    {
                        var tagRedes = "";
                        tagRedes = entidad.tagRedes;
                        string s = Convert.ToString(item);
                        string ic = iconoOtro;
                        s = s.Replace('"', ' ').Trim();
                        if (s.Split(':')[0] == "facebook")
                        {
                            ic = iconoFace;
                        }
                        if (s.Split(':')[0] == "linkedin")
                        {
                            ic = iconoFace;
                        }
                        tagRedes = tagRedes.Replace("#red", s.Split(':')[1]).Replace("#icono", ic);
                        red = red + tagRedes;
                    }
                }
                html = html.Replace("#red", red);

                string logro = "Sin habilidades ingresadas";
                if (!String.IsNullOrEmpty(hoja.habilidades))//obtenemos las redes sociales del usuario
                {
                    dynamic datos = JsonConvert.DeserializeObject(hoja.habilidades);
                    foreach (var item in datos)
                    {
                        var tagLogros = "";
                        tagLogros = entidad.tagFuncionesLogros;
                        DiccionarioDatos diccionarioHabilidades = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item));//obtenemos la aptitud y habilidades
                        tagLogros = tagLogros.Replace("#logro", diccionarioHabilidades.nombreDiccionario);
                        logro = logro + tagLogros;
                    }
                }
                html = html.Replace("#aptitud", logro);

                string movilidad = "Sin diponibilidad";
                if (!String.IsNullOrEmpty(hoja.paisesTrabajar))//obtenemos las redes sociales del usuario
                {
                    movilidad = "";
                    dynamic datos = JsonConvert.DeserializeObject(hoja.paisesTrabajar);
                    foreach (var item in datos)
                    {
                        DiccionarioDatos compruebaPais = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item));//obtenemos el nivel del idioma
                        string tagMovilidad = "";
                        tagMovilidad = entidad.tagMovilidadLaboral;
                        tagMovilidad = tagMovilidad.Replace("#pais", compruebaPais.nombreDiccionario);
                        movilidad = movilidad + tagMovilidad;
                    }
                }
                html = html.Replace("#movilidad", movilidad);
            }

            SelectPdf.HtmlToPdf A = new SelectPdf.HtmlToPdf();
            SelectPdf.PdfDocument pdf = A.ConvertHtmlString(html);//convertimos le objeto a pdf 
            A.Options.KeepTextsTogether = true;
            A.Options.MarginTop = 35;
            A.Options.MarginBottom = 35;
            // create memory stream to save PDF
            MemoryStream pdfStream = new MemoryStream();

            // save pdf document into a MemoryStream
            pdf.Save(pdfStream);
            pdfStream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(pdfStream);
            //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //result.Content.Headers.ContentDisposition.FileName = "Hola.pdf";
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentLength = pdfStream.Length;
            return result;//enviamos el pdf para ser visualizado
        }
        /// <summary>
        /// obtengo el curriculum pdf y lo envio en formato json para que pueda ser consumido
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hoja/PdfJson/{id}")]
        public async Task<IHttpActionResult> ObtenerPdfJson(int id)
        {
            try
            {
                id = JwtManager.getIdUserSession();
                var user = await _ir.FindEspecial<Usuario>(id);//obtener los datos del usuario
                var decode = new FuncionesViewModels();
                //if (obtenerDatosPersonales != null && obtenerDatosPersonales.pdfHojaDeVida != null)//lo transformamos a string para enviarlo por json y pueda ser consumido
                //{
                //    if (!String.IsNullOrEmpty(obtenerDatosPersonales.anosExperiencia)) obtenerDatosPersonales.anosExperiencia = decode.Base64_Decode(obtenerDatosPersonales.anosExperiencia);
                //    if (!String.IsNullOrEmpty(obtenerDatosPersonales.dicProfesion)) obtenerDatosPersonales.dicProfesion = decode.Base64_Decode(obtenerDatosPersonales.dicProfesion);
                //    if (!String.IsNullOrEmpty(obtenerDatosPersonales.dicAreaTrabajo)) obtenerDatosPersonales.dicAreaTrabajo = decode.Base64_Decode(obtenerDatosPersonales.dicAreaTrabajo);
                //    if (!String.IsNullOrEmpty(obtenerDatosPersonales.idiomaNivel)) obtenerDatosPersonales.idiomaNivel = decode.Base64_Decode(obtenerDatosPersonales.idiomaNivel);
                //    foreach (var y in obtenerDatosPersonales.HojaVidaInformacionEducativa.ToList())
                //    {
                //        if (!String.IsNullOrEmpty(y.dicAreaEstudio)) y.dicAreaEstudio = decode.Base64_Decode(y.dicAreaEstudio);
                //        if (!String.IsNullOrEmpty(y.dicCiudadEstudio)) y.dicCiudadEstudio = decode.Base64_Decode(y.dicCiudadEstudio);
                //        if (!String.IsNullOrEmpty(y.dicIntensidadEstudio)) y.dicIntensidadEstudio = decode.Base64_Decode(y.dicIntensidadEstudio);
                //        if (!String.IsNullOrEmpty(y.dicNivelEducativo)) y.dicNivelEducativo = decode.Base64_Decode(y.dicNivelEducativo);
                //        if (!String.IsNullOrEmpty(y.dicTipoEstudio)) y.dicTipoEstudio = decode.Base64_Decode(y.dicTipoEstudio);
                //        if (!String.IsNullOrEmpty(y.dicPaisExtranjero)) y.dicPaisExtranjero = decode.Base64_Decode(y.dicPaisExtranjero);
                //        if (!String.IsNullOrEmpty(y.institucionEducativa)) y.institucionEducativa = decode.Base64_Decode(y.institucionEducativa);
                //        if (!String.IsNullOrEmpty(y.tituloObtenido)) y.tituloObtenido = decode.Base64_Decode(y.tituloObtenido);
                //        if (!String.IsNullOrEmpty(y.estadoEstudio)) y.estadoEstudio = decode.Base64_Decode(y.estadoEstudio);
                //        if (!String.IsNullOrEmpty(y.otroEstudio)) y.otroEstudio = decode.Base64_Decode(y.otroEstudio);
                //        if (!String.IsNullOrEmpty(y.descripcionEducacion)) y.descripcionEducacion = decode.Base64_Decode(y.descripcionEducacion);
                //    }
                //    foreach (var z in obtenerDatosPersonales.HojaVidaExperienciaLaboral.ToList())
                //    {
                //        if (!String.IsNullOrEmpty(z.empresa)) z.empresa = decode.Base64_Decode(z.empresa);
                //        if (!String.IsNullOrEmpty(z.cargo)) z.cargo = decode.Base64_Decode(z.cargo);
                //        if (!String.IsNullOrEmpty(z.funcionesLogros)) z.funcionesLogros = decode.Base64_Decode(z.funcionesLogros);
                //        if (!String.IsNullOrEmpty(z.trabajoActualmente)) z.trabajoActualmente = decode.Base64_Decode(z.trabajoActualmente);
                //    }
                //    string strgs = Convert.ToBase64String(obtenerDatosPersonales.pdfHojaDeVida);
                //    return Ok(strgs);
                //}

                var hoja = user.HojaVidaDatosPersonales.FirstOrDefault();//obtenemos el primer registro
                string html = "";
                if (hoja != null)
                {
                    if (!String.IsNullOrEmpty(hoja.anosExperiencia)) hoja.anosExperiencia = decode.Base64_Decode(hoja.anosExperiencia);
                    if (!String.IsNullOrEmpty(hoja.dicProfesion)) hoja.dicProfesion = decode.Base64_Decode(hoja.dicProfesion);
                    if (!String.IsNullOrEmpty(hoja.dicAreaTrabajo)) hoja.dicAreaTrabajo = decode.Base64_Decode(hoja.dicAreaTrabajo);
                    if (!String.IsNullOrEmpty(hoja.idiomaNivel)) hoja.idiomaNivel = decode.Base64_Decode(hoja.idiomaNivel);
                    foreach (var y in hoja.HojaVidaInformacionEducativa.ToList())
                    {
                        if (!String.IsNullOrEmpty(y.dicAreaEstudio)) y.dicAreaEstudio = decode.Base64_Decode(y.dicAreaEstudio);
                        if (!String.IsNullOrEmpty(y.dicCiudadEstudio)) y.dicCiudadEstudio = decode.Base64_Decode(y.dicCiudadEstudio);
                        if (!String.IsNullOrEmpty(y.dicIntensidadEstudio)) y.dicIntensidadEstudio = decode.Base64_Decode(y.dicIntensidadEstudio);
                        if (!String.IsNullOrEmpty(y.dicNivelEducativo)) y.dicNivelEducativo = decode.Base64_Decode(y.dicNivelEducativo);
                        if (!String.IsNullOrEmpty(y.dicTipoEstudio)) y.dicTipoEstudio = decode.Base64_Decode(y.dicTipoEstudio);
                        if (!String.IsNullOrEmpty(y.dicPaisExtranjero)) y.dicPaisExtranjero = decode.Base64_Decode(y.dicPaisExtranjero);
                        if (!String.IsNullOrEmpty(y.institucionEducativa)) y.institucionEducativa = decode.Base64_Decode(y.institucionEducativa);
                        if (!String.IsNullOrEmpty(y.tituloObtenido)) y.tituloObtenido = decode.Base64_Decode(y.tituloObtenido);
                        if (!String.IsNullOrEmpty(y.estadoEstudio)) y.estadoEstudio = decode.Base64_Decode(y.estadoEstudio);
                        if (!String.IsNullOrEmpty(y.otroEstudio)) y.otroEstudio = decode.Base64_Decode(y.otroEstudio);
                        if (!String.IsNullOrEmpty(y.descripcionEducacion)) y.descripcionEducacion = decode.Base64_Decode(y.descripcionEducacion);
                    }
                    foreach (var z in hoja.HojaVidaExperienciaLaboral.ToList())
                    {
                        if (!String.IsNullOrEmpty(z.empresa)) z.empresa = decode.Base64_Decode(z.empresa);
                        if (!String.IsNullOrEmpty(z.cargo)) z.cargo = decode.Base64_Decode(z.cargo);
                        if (!String.IsNullOrEmpty(z.funcionesLogros)) z.funcionesLogros = decode.Base64_Decode(z.funcionesLogros);
                        if (!String.IsNullOrEmpty(z.trabajoActualmente)) z.trabajoActualmente = decode.Base64_Decode(z.trabajoActualmente);
                    }
                    var entidad = await _ir.Find<PlantillaHojaDeVida>(hoja.idPlantillaAplicada);//buscamos la planilla de hoja de vida generica
                    html = entidad.htmlPlantilla;//obtenemos la plantilla
                    html = html.Replace("#Nombre", hoja.nombres + " " + hoja.apellidoPaterno + " " + hoja.apellidoMaterno);//reemplazamos los nombres y los apellidos.
                    if (hoja.fotoPerfil != null)//si el usuario tiene foto de perfil la agregamos a la plantilla
                    {
                        string str = Convert.ToBase64String(hoja.fotoPerfil);
                        string link = "data:Image/png;base64," + str;

                        html = html.Replace("#imagen", link);
                    }
                    var x = await _ir.Find<DiccionarioDatos>(hoja.dicTipoDocumento);//obtenemos los datos del tipo de documento desde la base para reeemplazar en la plantilla 
                                                                                    //por los valores correspondientes
                    if (x != null && x.nombreDiccionario == "Cédula de Ciudadanía")
                    {
                        html = html.Replace("#Pais", "Colombiana");
                    }
                    else
                    {
                        html = html.Replace("#Pais", "Extranjera");
                    }
                    //reeemplazamos distintos valores en la planilla
                    html = html.Replace("#identificacion", hoja.identificacion);
                    html = html.Replace("#nacimiento", hoja.fechaNacimiento.GetValueOrDefault().ToShortDateString());
                    html = html.Replace("#direccion", hoja.direccionResidencia);
                    html = html.Replace("#fono", hoja.telefonoCelular);
                    html = html.Replace("#email", hoja.correoElectronico);
                    html = html.Replace("#descripcion", hoja.descripcionPerfilProfesional);
                    html = html.Replace("#tipoTextoHV", hoja.tipoTextoHV);
                    html = html.Replace("#colorHV", hoja.colorHV);
                    if (hoja.fechaNacimiento != null && hoja.fechaNacimiento.GetValueOrDefault().Year > 1850)
                    {
                        DateTime nacimiento = hoja.fechaNacimiento.GetValueOrDefault(); //obtenemos la edad y la reemplazamos en la plantilla
                        int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                        html = html.Replace("#Edad", Convert.ToString(edad));
                    }
                    //buscamos todos los estudios vigentes
                    var listaeEducativa = hoja.HojaVidaInformacionEducativa.Where(z => z.esComplementario == false && z.vigente == true).ToList();
                    var cadenaEducativa = "";
                    foreach (var item in listaeEducativa)//para cada item vamos reemplazando valores e irlos agregando a la plantilla
                    {
                        string tagPrincipales = "";
                        tagPrincipales = entidad.tagEstudiosPrincipales;
                        string profesion = "";
                        string ciudad = "";
                        if (item.dicNivelEducativo != null)
                        {
                            var diccionarioEducacion = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicNivelEducativo));//obtenemos el nivel educativo
                            if (diccionarioEducacion != null)
                            {
                                profesion = diccionarioEducacion.nombreDiccionario;
                            }
                        }
                        if (item.dicCiudadEstudio != null)
                        {
                            var diccionarioCiudad = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicCiudadEstudio));//obtenemos la ciudad de estudio
                            if (diccionarioCiudad != null)
                            {
                                ciudad = diccionarioCiudad.nombreDiccionario;
                            }
                        }
                        tagPrincipales = tagPrincipales.Replace("#profesion", profesion);
                        tagPrincipales = tagPrincipales.Replace("#titulo", item.tituloObtenido);
                        tagPrincipales = tagPrincipales.Replace("#institucion", item.institucionEducativa);
                        tagPrincipales = tagPrincipales.Replace("#inicio", item.fechaInicio.GetValueOrDefault().ToShortDateString());
                        if (item.fechaFin != null)
                        {
                            tagPrincipales = tagPrincipales.Replace("#termino", item.fechaFin.GetValueOrDefault().ToShortDateString());
                        }
                        else
                        {
                            tagPrincipales = tagPrincipales.Replace("#termino", "Actualidad");
                        }
                        tagPrincipales = tagPrincipales.Replace("#ciudad", ciudad);
                        cadenaEducativa = cadenaEducativa + tagPrincipales;
                    }
                    html = html.Replace("#estudioprincipal", cadenaEducativa);

                    //obtenemos los datos de los estudios complementarios que esten vigentes
                    var listaComplementario = hoja.HojaVidaInformacionEducativa.Where(z => z.esComplementario == true && z.vigente == true).ToList();
                    var cadenaComplementario = "";
                    foreach (var item in listaComplementario)//para cada item vamos reemplazando valores e irlos agregando a la plantilla
                    {
                        string tagComplementarios = "";
                        tagComplementarios = entidad.tagEstudiosComplementarios;
                        string profesion = "";
                        string ciudad = "";
                        string intensidadEstudio = "";
                        string tipoEstudio = "";
                        if (item.dicNivelEducativo != null)
                        {
                            var diccionarioComplementario = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicNivelEducativo));//obtenemos el nivel educativo
                            if (diccionarioComplementario != null)
                            {
                                profesion = diccionarioComplementario.nombreDiccionario;
                            }
                        }
                        if (item.dicCiudadEstudio != null)
                        {
                            var diccionarioCiudad = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicCiudadEstudio));//obtenemos la ciudad de estudio
                            if (diccionarioCiudad != null)
                            {
                                ciudad = diccionarioCiudad.nombreDiccionario;
                            }
                        }
                        if (item.dicIntensidadEstudio != null)
                        {
                            var diccionarioIntensidad = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicIntensidadEstudio));
                            if (diccionarioIntensidad != null)
                            {
                                intensidadEstudio = "/" + diccionarioIntensidad.nombreDiccionario;
                            }
                        }
                        if (item.dicTipoEstudio != null)
                        {
                            var diccionarioTipoEstudio = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.dicTipoEstudio));
                            if (diccionarioTipoEstudio != null)
                            {
                                tipoEstudio = diccionarioTipoEstudio.nombreDiccionario;
                            }
                        }
                        tagComplementarios = tagComplementarios.Replace("#profesion", profesion);
                        tagComplementarios = tagComplementarios.Replace("#titulo", item.tituloObtenido);
                        tagComplementarios = tagComplementarios.Replace("#institucion", item.institucionEducativa);
                        tagComplementarios = tagComplementarios.Replace("#inicio", item.fechaInicio.GetValueOrDefault().ToShortDateString());
                        if (item.fechaFin != null)
                        {
                            tagComplementarios = tagComplementarios.Replace("#termino", item.fechaFin.GetValueOrDefault().ToShortDateString());
                        }
                        else
                        {
                            tagComplementarios = tagComplementarios.Replace("#termino", "Actualidad");
                        }
                        tagComplementarios = tagComplementarios.Replace("#ciudad", ciudad);
                        tagComplementarios = tagComplementarios.Replace("#intensidad", intensidadEstudio + " horas");
                        tagComplementarios = tagComplementarios.Replace("#tipoEstudio", tipoEstudio);
                        tagComplementarios = tagComplementarios.Replace("#descripcion", item.descripcionEducacion);
                        cadenaComplementario = cadenaComplementario + tagComplementarios;

                        //cadenaComplementario = cadenaComplementario + "<p>" + profesion + "/" + item.tituloObtenido + "/" + item.institucionEducativa + "<br/>" + item.fechaInicio.GetValueOrDefault().ToShortDateString()
                        //    + "-" + item.fechaFin.GetValueOrDefault().ToShortDateString() + ciudad + "</p>";
                    }
                    if (String.IsNullOrWhiteSpace(cadenaComplementario))
                    {
                        cadenaComplementario = "Sin estudios complementarios";
                    }
                    html = html.Replace("#estudiocomplementario", cadenaComplementario);
                    //obtenemos la experiencia laboral
                    var listaLaboral = hoja.HojaVidaExperienciaLaboral.Where(z => z.vigente == true).ToList();
                    var cadenaLaboral = "";
                    foreach (var item in listaLaboral)
                    {
                        var tagLaboral = "";
                        tagLaboral = entidad.tagExperienciaLaboral;

                        tagLaboral = tagLaboral.Replace("#empresa", item.empresa);
                        tagLaboral = tagLaboral.Replace("#cargo", item.cargo);
                        tagLaboral = tagLaboral.Replace("#inicio", item.fechaIngreso.GetValueOrDefault().ToShortDateString());
                        if (item.trabajoActualmente == "S")
                        {
                            tagLaboral = tagLaboral.Replace("#termino", "Actualidad");
                        }
                        else
                        {
                            tagLaboral = tagLaboral.Replace("#termino", item.fechaSalida.GetValueOrDefault().ToShortDateString());
                        }

                        cadenaLaboral = cadenaLaboral + tagLaboral;
                    }
                    html = html.Replace("#laboral", cadenaLaboral);

                    var listaidioma = hoja.idiomaNivel;
                    var cadenaIdioma = "Sin idiomas a ingresar";
                    if (listaidioma != null && listaidioma.Length > 7)//si el valor no es null obtenemos los idiomas
                    {
                        cadenaIdioma = "";
                        var tagIdioma = "";
                        tagIdioma = entidad.tagIdioma;
                        dynamic datos = JsonConvert.DeserializeObject(listaidioma);
                        foreach (var item in datos)
                        {
                            DiccionarioDatos compruebaIdioma = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item.id));//obtenemos el nivel del idioma
                            string nivel = "";
                            if (item.nivel == "1")
                            {
                                nivel = "Básico";
                            }
                            else if (item.nivel == "2")
                            {
                                nivel = "Intermedio";
                            }
                            else if (item.nivel == "3")
                            {
                                nivel = "Avanzado";
                            }
                            cadenaIdioma = cadenaIdioma + tagIdioma.Replace("#nivel", nivel).Replace("#idiomaTag", compruebaIdioma.nombreDiccionario);
                            //cadenaIdioma = cadenaIdioma + "<p>" + compruebaIdioma.nombreDiccionario + "" + compruebaNivel.nombreDiccionario + "</p>";
                        }
                    }
                    html = html.Replace("#idioma", cadenaIdioma);

                    string red = "Sin redes sociales ingresadas";
                    string iconoLink = "<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='32' height='32' viewBox='0 0 24 24'> <defs> <path id='a' d='M0 1.18c0 .69.502 1.18 1.183 1.18.7 0 1.238-.454 1.238-1.18C2.42.473 1.9 0 1.22 0 .538 0 0 .472 0 1.18zM5.81 4.36h-.035l-.107-1.073H3.677c.07 2.543.053 5.067.053 7.608h2.296V6.52c0-.2 0-.4.072-.6.179-.508.645-.89 1.183-.89.951 0 1.165.855 1.165 1.653v4.212h2.314V6.41c0-1.743-.753-3.287-2.654-3.287-.915 0-1.811.436-2.296 1.236zM.054 10.894H2.35V3.286H.054v7.608z'/> </defs> <g fill='none' fill-rule='evenodd'> <circle cx='12' cy='12' r='12' fill='#0C79BC'/> <path fill='#FFF' d='M15.23 17.677v-4.212c0-.8-.216-1.653-1.166-1.653-.539 0-1.005.382-1.184.89-.072.2-.072.4-.072.6v4.375h-2.295c0-2.542.018-5.066-.054-7.608h1.99l.108 1.071h.036c.484-.798 1.381-1.235 2.296-1.235 1.9 0 2.654 1.544 2.654 3.287v4.485h-2.314zm-8.393 0h2.295V10.07H6.837v7.608zM8.002 6.783c.682 0 1.202.471 1.202 1.18 0 .726-.539 1.18-1.238 1.18-.681 0-1.183-.49-1.183-1.18 0-.709.538-1.18 1.22-1.18z'/> <g transform='translate(6.783 6.783)'> <mask id='b' fill='#fff'> <use xlink:href='#a'/> </mask> <path fill='#FFF' d='' mask='url(#b)'/> </g> </g></svg>";
                    string iconoFace = "<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='32' height='32' viewBox='0 0 24 24'> <defs> <path id='a' d='M1.071 2.902v1.052H0V6.04h1.071v5.758h2.174V6.04h2.158V3.954H3.245v-1.24c0-.517.91-.784 1.33-.784.341 0 .714.063 1.04.157L5.937.235C5.338.032 4.235 0 3.618 0 1.817 0 1.071 1.318 1.071 2.902z'/> </defs> <g fill='none' fill-rule='evenodd'> <ellipse cx='12' cy='11.773' fill='#2E77B1' rx='12' ry='11.773'/> <path fill='#FFF' d='M15.005 8.229a3.865 3.865 0 0 0-1.038-.157c-.422 0-1.33.267-1.33.784v1.24h2.158v2.086h-2.159v5.758h-2.174v-5.758h-1.07v-2.086h1.07V9.045c0-1.585.747-2.903 2.548-2.903.616 0 1.72.032 2.32.235l-.325 1.852z'/> <g transform='translate(9.391 6.142)'> <mask id='b' fill='#fff'> <use xlink:href='#a'/> </mask> <path fill='#FFF' d='' mask='url(#b)'/> </g> </g></svg>";
                    string iconoOtro = "<svg xmlns='http://www.w3.org/2000/svg' width='32' height='32' viewBox='0 0 24 24'> <g fill='none' fill-rule='evenodd'> <circle cx='12' cy='12' r='12' fill='#66D17E'/> <path fill='#FFF' d='M15.43 11.775c-.017-.687-.1-1.358-.234-2.012h2.112c.285.62.453 1.291.503 2.012h-2.38zm1.945 2.532l-1.962-1.408c.017-.151.017-.302.017-.453h2.381a6.151 6.151 0 0 1-.436 1.86zm-.453 1.173l-1.005.05.318.504.805 1.224a.346.346 0 0 1-.118.503c-.033.017-.05.033-.083.033h-.017c-.017 0-.05.017-.067.017h-.017c-.033 0-.05 0-.084-.017h-.016c-.018 0-.034-.016-.05-.016 0 0-.017 0-.017-.017-.017-.017-.034-.017-.05-.034l-.017-.017a.22.22 0 0 1-.05-.067l-.37-.553-.788-1.173-.386.972c-.016.033-.033.067-.05.084l-.017.016c-.016.017-.033.034-.05.034s-.017.017-.033.017c-.017 0-.034.017-.05.017h-.168c-.017 0-.034-.017-.05-.034 0 0-.017 0-.017-.017-.017-.017-.05-.033-.067-.067a6.955 6.955 0 0 1-.05-.1l-.286-1.04-.067-.235-.687-2.716c0-.033-.017-.05 0-.084v-.016c0-.017 0-.034.016-.05v-.017c.017-.017.017-.034.034-.068l.05-.05s.017 0 .017-.017c.017-.016.033-.016.05-.016h.017c.017 0 .05-.017.067-.017h.1c.034.017.05.017.085.033l.939.671.62.453 1.106.788.654.47c.235.167.134.536-.15.552zm-3.94 2.264c-.285.05-.57.067-.871.067-.336 0-.654-.033-.973-.084-.553-.754-.973-1.643-1.257-2.598h3.32l.419 1.643c-.185.335-.403.67-.638.972zm-5.7-2.615h1.91c.236.838.571 1.626.99 2.347a5.661 5.661 0 0 1-2.9-2.347zm-.856-2.683h2.381c.017.687.101 1.358.235 2.012H6.913a5.716 5.716 0 0 1-.487-2.012zm.487-2.683h2.13a11.378 11.378 0 0 0-.252 2.012H6.426a5.716 5.716 0 0 1 .487-2.012zm3.336-3.035A10.332 10.332 0 0 0 9.21 9.093H7.282a5.68 5.68 0 0 1 2.967-2.365zm1.862-.318c.335 0 .654.033.972.083.553.755.972 1.644 1.258 2.6H9.914a8.365 8.365 0 0 1 1.324-2.616c.286-.05.57-.067.873-.067zm-2.65 5.365c.017-.687.118-1.358.268-2.012h4.78c.15.654.217 1.325.25 2.012H9.462zm3.22 1.241l.369 1.442H9.713c-.15-.654-.218-1.325-.252-2.012h3.236a.943.943 0 0 0-.017.57zm4.258-3.923h-1.894a10.067 10.067 0 0 0-.99-2.348 5.69 5.69 0 0 1 2.884 2.348zm1.543 3.018a6.369 6.369 0 0 0-6.371-6.372 6.369 6.369 0 0 0-6.372 6.372c0 3.52 2.85 6.37 6.372 6.37a6.298 6.298 0 0 0 3.42-1.005l.368.537c.185.284.52.452.856.452a1 1 0 0 0 .537-.15c.234-.152.402-.387.47-.655.066-.268 0-.553-.152-.788l-.503-.754a.922.922 0 0 0 .721-.67 1.04 1.04 0 0 0 0-.554c.42-.839.654-1.778.654-2.783z'/> </g></svg>";
                    if (!String.IsNullOrEmpty(hoja.redesSociales))//obtenemos las redes sociales del usuario
                    {
                        red = "";
                        dynamic datos = JsonConvert.DeserializeObject(hoja.redesSociales);
                        foreach (var item in datos)
                        {
                            var tagRedes = "";
                            tagRedes = entidad.tagRedes;
                            string s = Convert.ToString(item);
                            string ic = iconoOtro;
                            s = s.Replace('"', ' ').Trim();
                            if (s.Split(':')[0].Trim() == "facebook")
                            {
                                ic = iconoFace;
                            }
                            if (s.Split(':')[0].Trim() == "linkedin")
                            {
                                ic = iconoLink;
                            }
                            tagRedes = tagRedes.Replace("#red", s.Split(':')[1]).Replace("#icono", ic);
                            red = red + tagRedes;
                        }
                    }
                    html = html.Replace("#red", red);

                    string logro = "Sin habilidades ingresadas";
                    if (!String.IsNullOrEmpty(hoja.habilidades))//obtenemos las redes sociales del usuario
                    {
                        logro = "";
                        dynamic datos = JsonConvert.DeserializeObject(hoja.habilidades);
                        foreach (var item in datos)
                        {
                            var tagLogros = "";
                            tagLogros = entidad.tagFuncionesLogros;
                            DiccionarioDatos diccionarioHabilidades = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item));//obtenemos la aptitud y habilidades
                            tagLogros = tagLogros.Replace("#logro", diccionarioHabilidades.nombreDiccionario);
                            logro = logro + tagLogros;
                        }
                    }
                    html = html.Replace("#aptitud", logro);

                    string movilidad = "Sin diponibilidad";
                    if (!String.IsNullOrEmpty(hoja.paisesTrabajar))//obtenemos las redes sociales del usuario
                    {
                        movilidad = "";
                        dynamic datos = JsonConvert.DeserializeObject(hoja.paisesTrabajar);
                        foreach (var item in datos)
                        {
                            DiccionarioDatos compruebaPais = await _ir.Find<DiccionarioDatos>(Convert.ToInt32(item));//obtenemos el nivel del idioma
                            string tagMovilidad = "";
                            tagMovilidad = entidad.tagMovilidadLaboral;
                            tagMovilidad = tagMovilidad.Replace("#pais", compruebaPais.nombreDiccionario);
                            movilidad = movilidad + tagMovilidad;
                        }
                    }
                    html = html.Replace("#movilidad", movilidad);
                }

                SelectPdf.HtmlToPdf A = new SelectPdf.HtmlToPdf();
                SelectPdf.PdfDocument pdf = A.ConvertHtmlString(html);//convertimos le objeto a pdf
                A.Options.KeepTextsTogether = true;
                A.Options.MarginTop = 35;
                A.Options.MarginBottom = 35;// ESTAS TRES LINEAS SON PARA RENDERIZAR MEJOR EL HTML DEL PDF EN LA HOJA DE VIDA
                MemoryStream pdfStream = new MemoryStream();

                // save pdf document into a MemoryStream
                pdf.Save(pdfStream);
                pdfStream.Position = 0;


                byte[] buffer = new byte[pdfStream.Length];
                for (int totalBytesCopied = 0; totalBytesCopied < pdfStream.Length;)
                    totalBytesCopied += pdfStream.Read(buffer, totalBytesCopied, Convert.ToInt32(pdfStream.Length) - totalBytesCopied);

                string strg = Convert.ToBase64String(buffer);//convertimos el objeto a string para enviarlo por json
                var obtenerDatosPersonales = await _ir.GetFirstEspecial<HojaVidaDatosPersonales>(z => z.idUsuario == id);
                if (!String.IsNullOrEmpty(obtenerDatosPersonales.anosExperiencia)) obtenerDatosPersonales.anosExperiencia = decode.Base64_Encode(obtenerDatosPersonales.anosExperiencia);
                if (!String.IsNullOrEmpty(obtenerDatosPersonales.dicProfesion)) obtenerDatosPersonales.dicProfesion = decode.Base64_Encode(obtenerDatosPersonales.dicProfesion);
                if (!String.IsNullOrEmpty(obtenerDatosPersonales.dicAreaTrabajo)) obtenerDatosPersonales.dicAreaTrabajo = decode.Base64_Encode(obtenerDatosPersonales.dicAreaTrabajo);
                if (!String.IsNullOrEmpty(obtenerDatosPersonales.idiomaNivel)) obtenerDatosPersonales.idiomaNivel = decode.Base64_Encode(obtenerDatosPersonales.idiomaNivel);
                obtenerDatosPersonales.pdfHojaDeVida = buffer;
                await _ir.Update<HojaVidaDatosPersonales>(obtenerDatosPersonales, obtenerDatosPersonales.idDatosPersonales);
                
                return Ok(strg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Se actualiza la contraseña del usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Profile/ActualizarPassword")]
        public async Task<IHttpActionResult> ActualizarPassword(CambiarClaveViewModels user)
        {
            try
            {
                user.idUsuario = JwtManager.getIdUserSession().ToString();
                var xc = new FuncionesViewModels();
                var clave64 = user.clave;
                entRespuesta respuesta = new entRespuesta();
                var entidad = await _ir.Find<Usuario>(Convert.ToInt32(user.idUsuario));//buscamos el id de estudio complementario
                user.clave = xc.encriptar256(clave64);
                if (entidad != null)
                {
                    if (xc.encriptar256(user.claveAnterior) == entidad.clave)
                    {
                        entidad.clave = user.clave;
                        await _ir.Update(entidad, Convert.ToInt32(user.idUsuario));//actualizamos el registro
                        respuesta.codigo = 0;
                        respuesta.mensaje = "Contraseña actualizada correctamente";
                        respuesta.data = "";
                        return Ok(respuesta);
                    }
                    else
                    {
                        respuesta.mensaje = "La contraseña actual no coincide con nuestros registros.";
                        respuesta.codigo = 1;
                        return Ok(respuesta);
                    }
                }
                else
                {
                    respuesta.mensaje = "Id no corresponde a un usuario válido";
                    respuesta.codigo = 1;
                    return Ok(respuesta);
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        /// <summary>
        /// Se actualizan los datos personales del usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Profile/ActualizarDatosPersonales")]
        public async Task<IHttpActionResult> ActualizarDatospersonales(UsuarioViewModels user)
        {
            try
            {
                FuncionesViewModels encode = new FuncionesViewModels();
                entRespuesta respuesta = new entRespuesta();
                //int idUser = int.Parse(user.idUsuario);
                int idUser = JwtManager.getIdUserSession();
                var entidad = await _ir.Find<Usuario>(idUser);//buscamos el id de estudio complementario
                if (entidad != null)
                {
                    List<string>coberturas = await ListaCoberturas(entidad.identificacion, entidad.dicTipoDocumento.Value);
                    entidad.nombres = user.nombres;
                    entidad.apellidoPaterno = user.apellidoPaterno;
                    entidad.apellidoMaterno = user.apellidoMaterno;
                    entidad.correoElectronico = user.correoElectronico;
                    entidad.numeroCelular = user.numeroCelular;
                    await _ir.Update(entidad, idUser);//actualizamos el registro
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Datos personales actualizados";
                    UsuarioViewModels respuestaViewModels = new UsuarioViewModels();
                    respuestaViewModels = Mapper.Map<Usuario, UsuarioViewModels>(entidad);
                    respuestaViewModels.idUsuario = encode.Base64_Encode(respuestaViewModels.idUsuario);
                    respuestaViewModels.clave = "";
                    respuestaViewModels.perfiles = coberturas;
                    var dataToken = new { token = JwtManager.GenerarTokenJwt<UsuarioViewModels>(WebConfigurationManager.AppSettings["sitioUsuario"].ToString(), "Usuario", respuestaViewModels) };
                    respuesta.data = dataToken;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.mensaje = "Id no corresponde a un usuario válido";
                    respuesta.codigo = 1;
                    return Ok(respuesta);
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        public async Task<List<string>> ListaCoberturas(string identificacion, int tipoDoc)
        {
            FuncionesViewModels validacionSms = new FuncionesViewModels();
            entRespuesta respuesta = new entRespuesta();
            Rootobject ent = new Rootobject();
            var respuestaDevuelta = new List<ContingenciaPrimaria>();
            respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == identificacion);
            var registros = new DataTable();
            List<String> coberturas = new List<String>();
            var direccionamientoMetodo = await _ir.Find<Parametros>(10);
            string Tipo = "";
            if (tipoDoc == 5)//
            {
                Tipo = "CC";
            }
            if (direccionamientoMetodo != null && direccionamientoMetodo.parametroJSON == "1")
            {
                ent = validacionSms.ValidaUser(Tipo, identificacion);//obtrenemos los datos del usuario desde la api de cardif
            }
            var respuestaViewModels = new UsuarioViewModels();
            var listaSubSocios = new List<string>();

            if (ent != null && (ent.clienteAsegurado != null && ent.clienteAsegurado.statusResponse.status == "200"))
            {
                var rs = ent.clienteAsegurado.customerInformation.policy.ToList().FirstOrDefault();
                foreach (Policy policy in ent.clienteAsegurado.customerInformation.policy)
                {
                    if (policy.product != null)
                    {
                        foreach (Product product in policy.product)
                        {
                            foreach (Plan plan in product.plan)
                            {
                                if (plan.coverage != null)
                                {
                                    foreach (Coverage coverage in plan.coverage)
                                    {
                                        if (coverage.coverageId == 861 || coverage.coverageId == 160)
                                        {
                                            if (!coberturas.Contains("Desempleo"))
                                            {
                                                coberturas.Add("Desempleo");
                                            }
                                        }
                                        else if (coverage.coverageId == 865 || coverage.coverageId == 700)
                                        {
                                            if (!coberturas.Contains("Fraude"))
                                            {
                                                coberturas.Add("Fraude");
                                            }
                                            else if (coverage.coverageName == "Servicio Extendido para Discapacidad Temporal Total" ||
                                                     coverage.coverageName == "Incapacidad Total y Permanente Total por Accidente Aereo" ||
                                                     coverage.coverageName == "Incapacidad Permanente Total")
                                            {
                                                if (!coberturas.Contains("Desempleo"))
                                                {
                                                    coberturas.Add("Itt");
                                                }
                                            }
                                        }
                                        else if (coverage.coverageId == 862)
                                        {
                                            if (!coberturas.Contains("Itt"))
                                            {
                                                coberturas.Add("Itt");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == identificacion);

                // Consultamos los productos en contingenciaPrimaria del usuario
                var productos = new List<UsuarioProducto>();
                productos = await _ir.GetList<UsuarioProducto>(z => z.IdentificacionUsuario == identificacion);

                foreach (UsuarioProducto producto in productos)
                {
                    SqlParameter[] parametros =
                    {
                                            new SqlParameter("@IdProducto", producto.IdProducto)
                                        };

                    var query = "dbo.pa_ObtenerCoberturas";
                    registros = await _ir.dataOutPlacement(query, parametros);

                    var tabla = registros.AsEnumerable().Select(z => new { Id = z[0].ToString(), Codigo = z[1].ToString(), Descripcion = z[2].ToString() });
                    foreach (var row in tabla)
                    {
                        if (row.Descripcion != null && !coberturas.Contains(row.Descripcion))
                        {
                            coberturas.Add(row.Descripcion);
                        }
                    }
                }
                if (respuestaDevuelta.Count > 0 && (respuestaDevuelta[0].cobertura3 == "R"))
                {
                    coberturas.Clear();
                    coberturas.Add("retencion");
                }

            }
            return coberturas;
        }

        /// <summary>
        /// guardar archivo en la base de datos para prueba
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Hoja/subirArchivo")]
        public async Task<IHttpActionResult> SubirArchivo(ArchivoViewModels archivo)
        {
            try
            {
                archivo.idUsuario = JwtManager.getIdUserSession();
                var ccc = HostingEnvironment.MapPath("~/Imagenes/Branding");
                var log = new logFoto();
                log.texto = JsonConvert.SerializeObject(archivo);
                await _ir.Add(log);
                var registro = await _ir.GetFirst<HojaVidaDatosPersonales>(z => z.idUsuario == archivo.idUsuario);
                archivo.imagenBase64 = archivo.imagenBase64.Replace("data:image/jpeg;base64,", "");

                if (registro != null)
                {
                    var bytes = Convert.FromBase64String(archivo.imagenBase64);//convertimos el archivo a bytes
                    if (!String.IsNullOrEmpty(archivo.tipoArchivo) && archivo.tipoArchivo == "imagen")
                    {
                        registro.fotoPerfil = bytes;
                        registro.idImagen = archivo.idImagen;
                        registro.pdfHojaDeVida = null;
                        await _ir.Update(registro, registro.idDatosPersonales);
                    }
                    else if (!String.IsNullOrEmpty(archivo.tipoArchivo) && archivo.tipoArchivo != "imagen")
                    {
                        registro.pdfHojaDeVida = bytes;
                        registro.fotoPerfil = null;
                        registro.idImagen = null;
                        await _ir.Update(registro, registro.idDatosPersonales);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// obtengo la base para enviarla por json
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hoja/mostrarImagen/{id}")]
        public async Task<IHttpActionResult> MuestraImagen(int id)
        {
            try
            {
                id = JwtManager.getIdUserSession();
                var registro = await _ir.GetFirst<HojaVidaDatosPersonales>(z => z.idUsuario == id);
                if (registro != null && registro.fotoPerfil != null)
                {
                    string formato = "";
                    if (!String.IsNullOrEmpty(registro.idImagen))
                    {
                        formato = registro.idImagen;
                    }
                    string str = formato + Convert.ToBase64String(registro.fotoPerfil);
                    return Ok(str);
                }
                else
                {
                    return Ok("No tiene foto disponible");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("api/Hoja/actualizarPlantilla/{id}")]
        public async Task<IHttpActionResult> ActualizarPlantilla(int id, int user, string tipoTexto, string colorHv)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                user = JwtManager.getIdUserSession();
                var registro = await _ir.GetFirst<HojaVidaDatosPersonales>(z => z.idUsuario == user);
                if (registro != null)
                {
                    registro.idPlantillaAplicada = id;
                    registro.tipoTextoHV = tipoTexto;
                    registro.colorHV = colorHv;
                    await _ir.Update(registro, registro.idDatosPersonales);
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Plantilla modificada";
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Registro no actualizado";
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
        /// lista las plantillas html
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hoja/plantillas")]
        public async Task<IHttpActionResult> Plantillas()
        {
            try
            {
                var plantillas = await _ir.GetAll<PlantillaHojaDeVida>();
                var plantillasViewModels = Mapper.Map<List<PlantillaHojaDeVida>, List<PlantillaHojaVidaViewModels>>(plantillas);
                return Ok(plantillasViewModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Hoja/editarIdioma")]
        public async Task<IHttpActionResult> editarIdioma(ActualizarIdiomaViewModel idiomas)
        {
            entRespuesta ent = new entRespuesta();
            try
            {
                idiomas.idUsuario = JwtManager.getIdUserSession();
                var registro = await _ir.GetFirst<HojaVidaDatosPersonales>(z => z.idUsuario == idiomas.idUsuario);
                var encode = new FuncionesViewModels();
                if (registro != null)
                {
                    registro.idiomaNivel = encode.Base64_Encode(idiomas.idiomas);
                    await _ir.Update(registro, registro.idDatosPersonales);
                    ent.codigo = 0;
                    ent.mensaje = "Idioma actualizado correctamente.";
                }
                else
                {
                    ent.codigo = 1;
                    ent.mensaje = "No existe el usuario para actualizar el idioma.";
                }
                return Ok(ent);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/Hoja/Tipos/{id}")]
        public async Task<IHttpActionResult> MenusTipos(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                respuesta.codigo = 0;
                respuesta.mensaje = "OK";

                var menus = await _ir.GetList<DiccionarioDatos>(z => z.tipoDiccionario == id && z.vigente == true);
                var lista = new List<DiccionarioViewModels>();


                foreach (var item in menus)
                {
                    lista.Add(new DiccionarioViewModels { id = item.IdDiccionarioDatos, text = item.nombreDiccionario, habilitarOtro = item.habilitarOtro });
                }
                respuesta.data = lista.OrderBy(n => n.text);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Hoja/guardarRedSocial")]
        public async Task<IHttpActionResult> guardarRedSocial(HojaVidaDatosPersonales hv)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                hv.idUsuario = JwtManager.getIdUserSession();
                var hoja = await _ir.Find<HojaVidaDatosPersonales>(hv.idDatosPersonales);
                if (hoja != null)
                {
                    hoja.redesSociales = hv.redesSociales;
                    await _ir.Update<HojaVidaDatosPersonales>(hoja, hoja.idDatosPersonales);
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Registro guardado correctamente";
                    return Ok(respuesta);
                }
                respuesta.codigo = 1;
                respuesta.mensaje = "No se encontraron datos";
                return Ok(respuesta);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/Hoja/obtenerPassword")]
        public async Task<IHttpActionResult> obtenerPassword()
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var idUsuario = JwtManager.getIdUserSession();
                var usuario = await _ir.Find<Usuario>(idUsuario);
                if (usuario != null)
                {
                    respuesta.codigo = 0;
                    respuesta.data = usuario.clave;
                    return Ok(respuesta);
                }
                respuesta.codigo = 1;
                respuesta.mensaje = "No se encontraron datos";
                return Ok(respuesta);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
