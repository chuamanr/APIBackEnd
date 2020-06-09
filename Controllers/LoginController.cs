using AutoMapper;
using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace EcoOplacementApi.Controllers
{
    /// <summary>
    /// Clase para Login
    /// </summary>
    //[System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public LoginController(IRepositoryGeneral ir)//inyectamos el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// Metodo para obtener el Token de forma simple para el registro usuario.
        /// </summary>
        /// <param name="login">Recibe el login.</param>
        /// <returns>Retorna el Token.</returns>
        [HttpPost]
        [Route("api/Login/ObtenerToken")]
        public IHttpActionResult postObtenerToken([FromBody] RequestLoginJWT login)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                if (login == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                //TODO: Validate credentials Correctly, this code is only for demo !!
                bool isCredentialValid = (login.sitioPassword == WebConfigurationManager.AppSettings["sitioPassword"].ToString() &&
                    login.sitioUsuario == WebConfigurationManager.AppSettings["sitioUsuario"].ToString());
                if (isCredentialValid)
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "OK";
                    respuesta.data = new { token = JwtManager.GenerarTokenJwt(login.sitioUsuario) };
                    return Ok(respuesta);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Login/Logout/{id}")]
        public async Task<IHttpActionResult> Logout(int id)
        {
            try
            {
                //var user = _ir.ObtenerRegistro<UsuarioSesion>(z => z.idUsuario == id && z.vigente == true);
                //if (user != null)
                //{
                //    user.vigente = false;
                //    await _ir.Update(user, user.idUsuarioSesion);
                //    return Ok("Se ha cerrado la sesión");
                //}
                //else
                //{
                //    return Ok("Usuario no existente");
                //}
                var queryUserSesion = "[dbo].[pa_UpdateaSesion] @id"; //cerramos las sesiones de usuario
                SqlParameter[] param = { new SqlParameter("@id", id) };
                await _ir.Guardar(queryUserSesion, param);
                return Ok("Se ha cerrado la sesión");


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool IsReCaptchaValid(String token)
        {
            var result = false;
            var secretKey = ConfigurationManager.AppSettings["CaptchaKey"];
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, token);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

        /// <summary>
        /// Metodo para Iniciar Session, Recibe el codigo y el login.
        /// </summary>
        /// <param name="id">Recibe el id.</param>
        /// <param name="login">Recibe el login.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Login/IniciarSession")]
        public async Task<IHttpActionResult> postIniciarSession([FromBody] RequestLoginJWT login)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var decode = new FuncionesViewModels();
                login.sitioUsuario = WebConfigurationManager.AppSettings["sitioUsuario"].ToString(); 
                login.sitioPassword = WebConfigurationManager.AppSettings["sitioPassword"].ToString();
                login.usuario = decode.Base64_Decode(login.usuario);
                if (login == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                var captcha = IsReCaptchaValid(login.tokenCaptcha);
                if (captcha == false)
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se validó el captcha";
                    return Ok(respuesta);
                }
                var password = decode.encryptDecrypt(login.password, login.tokenCaptcha);
                login.password = password;
                //TODO: Validate credentials Correctly, this code is only for demo !!
                bool isCredentialValid = (login.sitioPassword == WebConfigurationManager.AppSettings["sitioPassword"].ToString() &&
                    login.sitioUsuario == WebConfigurationManager.AppSettings["sitioUsuario"].ToString());
                if (isCredentialValid)
                {
                    var validacionSms = new FuncionesViewModels();
                    Usuario usuario = await _ir.GetFirst<Usuario>(x => x.identificacion == login.usuario);//buscamos los datos del usuario por la identificacion
                    if (usuario != null)
                    {
                        if (usuario.vigente.Value)//si el musuario esta vigente
                        {
                            //var clave64 = login.password;
                            var clave64 = decode.encriptar256(login.password);//decodificamos la clave de la base para comparar la password ingresada por el usuario
                            if (clave64 == usuario.clave && (usuario.usuarioBloqueado == false || usuario.usuarioBloqueado == null))//comparamos que la clave sea igual y que el usuario no este bloqueado
                            {
                                string Tipo = "";
                                if (usuario.dicTipoDocumento == 5)//
                                {
                                    Tipo = "CC";
                                }
                                string nombre = "";
                                string ip = "";

                                var usuarioVerificado = _ir.ObtenerRegistro<UsuarioSesion>(z => z.idUsuario == usuario.idUsuario && z.vigente == true);
                                //Se obtiene parametro para verificar si se tiene que validar la ip.
                                var sesionIP = _ir.ObtenerRegistro<Parametros>(z => z.idParametro == 13);
                                //var identity = (ClaimsIdentity)User.Identity;
                                //if (identity != null && identity.Claims.Count() > 0)
                                //{
                                //    nombre = identity.FindFirstValue(ClaimTypes.Name).ToString();
                                //    ip = identity.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
                                //}
                                if (sesionIP != null && sesionIP.parametroJSON == "1")
                                {
                                    if (usuarioVerificado != null)
                                    {
                                        if (usuarioVerificado.ip != login.ip && usuarioVerificado.fechaSesion.GetValueOrDefault().ToShortDateString() == DateTime.Now.ToShortDateString())
                                        {
                                            respuesta.codigo = 1;
                                            respuesta.mensaje = "No puede ingresar desde un segundo dispositivo";
                                            return Ok(respuesta);
                                        }
                                    }
                                }
                                //var claims = new List<Claim>();
                                //claims.Add(new Claim(ClaimTypes.Name, login.usuario));
                                //claims.Add(new Claim(ClaimTypes.NameIdentifier, login.ip));

                                //var idt = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                                //var ctx = Request.GetOwinContext();
                                //var authenticationManager = ctx.Authentication;
                                //authenticationManager.SignIn(new AuthenticationProperties()
                                //{
                                //    AllowRefresh = true,
                                //    ExpiresUtc = DateTime.UtcNow.AddMinutes(30) //le asigne 30 minutos puede cambiar                                
                                //}, idt);
                                //if (login.usuario != "71678391")
                                //{
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                Rootobject ent = new Rootobject();
                                var respuestaDevuelta = new List<ContingenciaPrimaria>();
                                respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == login.usuario);
                                var registros = new DataTable();
                                List<String> coberturas = new List<String>();

                                var direccionamientoMetodo = await _ir.Find<Parametros>(10);

                                if (direccionamientoMetodo != null && direccionamientoMetodo.parametroJSON == "0")
                                {
                                    respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == login.usuario);

                                    // Consultamos los productos en contingenciaPrimaria del usuario
                                    var productos = new List<UsuarioProducto>();
                                    productos = await _ir.GetList<UsuarioProducto>(z => z.IdentificacionUsuario == login.usuario);

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
                                }
                                else
                                {
                                    ent = validacionSms.ValidaUser(Tipo, login.usuario);//obtrenemos los datos del usuario desde la api de cardif
                                }


                                //Rootobject ent = (Rootobject)serializer.Deserialize(usuario.responseJSON, typeof(Rootobject));
                                //if (ent == null)//si no existe usuario valido, se muestra mensaje de error.
                                //{
                                //    //respuesta.codigo = 1;                        en caso que pase a produccion la api descomentar estas 3 lineas y comentar la ultima
                                //    //respuesta.mensaje = "Aun no cuentas con seguro activo";
                                //    //return Ok(respuesta);
                                //    respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == login.usuario);
                                //}
                                if (ent == null && respuestaDevuelta.Count < 1)
                                {
                                    respuesta.codigo = 1;
                                    respuesta.mensaje = "No cuentas con seguro activo";
                                    return Ok(respuesta);
                                }

                                usuario.responseJSON = JsonConvert.SerializeObject(ent);//
                                usuario.cantidadIntentos = 0;
                                //}

                                await _ir.Update(usuario, usuario.idUsuario);//actualizamos los datos del usuario
                                //EJECUTAR REST DE BANCO BOGOTA
                                var respuestaViewModels = new UsuarioViewModels();
                                var countProduct = 0;
                                var countCoverage = 0;
                                var listaSubSocios = new List<string>();

                                if (ent != null && (ent.clienteAsegurado != null && ent.clienteAsegurado.statusResponse.status == "200"))
                                {
                                    if (ent.clienteAsegurado.customerInformation.policy == null)
                                    {
                                        respuesta.codigo = 1;
                                        respuesta.mensaje = "No cuentas con seguro activo";
                                        return Ok(respuesta);
                                    }

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
                                                    else
                                                    {
                                                        countCoverage++;
                                                    }
                                                    if (countCoverage == product.plan.Length && countCoverage == ent.clienteAsegurado.customerInformation.policy.Length && plan.coverage != null)
                                                    {
                                                        respuesta.codigo = 1;
                                                        respuesta.mensaje = "El usuario no cuenta con coberturas";
                                                        return Ok(respuesta);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            countProduct++;
                                        }
                                        if (countProduct == ent.clienteAsegurado.customerInformation.policy.Length)
                                        {
                                            respuesta.codigo = 1;
                                            respuesta.mensaje = "El usuario no cuenta con un producto asociado";
                                            return Ok(respuesta);
                                        }
                                    }
                                }
                                else
                                {
                                    respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == login.usuario);

                                    // Consultamos los productos en contingenciaPrimaria del usuario
                                    var productos = new List<UsuarioProducto>();
                                    productos = await _ir.GetList<UsuarioProducto>(z => z.IdentificacionUsuario == login.usuario);

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

                                var fun = new FuncionesViewModels();
                                respuestaViewModels = Mapper.Map<Usuario, UsuarioViewModels>(usuario);

                                respuestaViewModels.idUsuario = fun.Base64_Encode(respuestaViewModels.idUsuario);

                                respuestaViewModels.clave = login.password;
                                respuestaViewModels.perfiles = coberturas;

                                // Socios
                                respuestaViewModels.socios = listaSubSocios;

                                respuesta.codigo = 0;
                                respuesta.mensaje = "OK";

                                //var queryUserSesion = "[dbo].[pa_UpdateaSesion] @id"; //ucerramos las sesiones de usuario
                                //SqlParameter[] param = { new SqlParameter("@id", usuario.idUsuario) };
                                //await _ir.Guardar(queryUserSesion, param);

                                //generamos el token
                                respuestaViewModels.clave = "";
                                var dataToken = new { token = JwtManager.GenerarTokenJwt<UsuarioViewModels>(login.sitioUsuario, "Usuario", respuestaViewModels) };
                                
                                var base64clave = StringCipher.Encrypt(dataToken.token, "grupomok");
                                respuesta.data = dataToken;
                                var idTransformado = fun.Base64_Decode(respuestaViewModels.idUsuario);
                                var userT = new UsuarioSesion();
                                userT.idUsuario = Convert.ToInt32(idTransformado);
                                userT.token = dataToken.token;
                                userT.fechaSesion = DateTime.Now;
                                userT.ip = login.ip;
                                userT.vigente = true;
                                usuario.instafitUrl = respuestaViewModels.instafitUrl;
                                if (sesionIP != null && sesionIP.parametroJSON == "1")
                                {
                                    userT.vigente = true;
                                }
                                else
                                {
                                    userT.vigente = false;
                                }
                                if (usuarioVerificado == null)
                                {
                                    await _ir.Add(userT);
                                }
                                else if (usuarioVerificado != null && usuarioVerificado.ip != login.ip)
                                {
                                    await _ir.Add(userT);
                                }

                                return Ok(respuesta);
                            }
                            else
                            {
                                if (usuario.usuarioBloqueado == true)//si el usuario esta bloqueado se envia un mensaje de aviso
                                {
                                    respuesta.codigo = -1;
                                    respuesta.mensaje = "Su cuenta ha sido bloqueada";
                                    return Ok(respuesta);
                                }
                                usuario.cantidadIntentos = usuario.cantidadIntentos + 1;
                                //obtenermos los parametros para el envio del mensaje sms
                                var parametros = await _ir.Find<Parametros>(Convert.ToInt32(WebConfigurationManager.AppSettings["IdIntentos"].ToString()));
                                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                                //desearializamos el objeto json
                                dynamic j = jsonSerializer.Deserialize<dynamic>(parametros.parametroJSON);
                                //obtenemos la cantidad de intentos
                                int intento = Convert.ToInt32(j["cantidad"].ToString());
                                if (usuario.cantidadIntentos > intento)
                                {
                                    usuario.usuarioBloqueado = true;
                                    respuesta.codigo = -1;
                                    respuesta.mensaje = "Su cuenta ha sido bloqueada";
                                }
                                else
                                {
                                    respuesta.mensaje = "Tu número de documento y contraseña no coinciden, Verifica tus datos e intenta nuevamente.";
                                    respuesta.codigo = 1;
                                }
                            }
                            await _ir.Update(usuario, usuario.idUsuario);//actualizamos los datos del usuario
                            return Ok(respuesta);
                        }
                        else
                        {
                            respuesta.codigo = 1;
                            respuesta.mensaje = "El usuario no esta vigente.";

                            return Ok(respuesta);
                        }
                    }
                    else
                    {
                        respuesta.codigo = 1;
                        respuesta.mensaje = "Tu número de documento y contraseña no coinciden, Verifica tus datos e intenta nuevamente.";

                        return Ok(respuesta);
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("api/Login/IniciarSessionEspecial")]
        public async Task<IHttpActionResult> postIniciarSessionEspecial([FromBody] RequestLoginJWT login)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var decode = new FuncionesViewModels();
                login.sitioUsuario = WebConfigurationManager.AppSettings["sitioUsuario"].ToString();
                login.sitioPassword = WebConfigurationManager.AppSettings["sitioPassword"].ToString();
                login.usuario = decode.Base64_Decode(login.usuario);
                if (login == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                var captcha = IsReCaptchaValid(login.tokenCaptcha);
                if (captcha == false)
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se validó el captcha";
                    return Ok(respuesta);
                }
                var password = decode.encryptDecrypt(login.password, login.tokenCaptcha);
                login.password = password;
                //TODO: Validate credentials Correctly, this code is only for demo !!
                bool isCredentialValid = (login.sitioPassword == WebConfigurationManager.AppSettings["sitioPassword"].ToString() &&
                    login.sitioUsuario == WebConfigurationManager.AppSettings["sitioUsuario"].ToString());
                if (isCredentialValid)
                {
                    var validacionSms = new FuncionesViewModels();
                    Usuario usuario = await _ir.GetFirst<Usuario>(x => x.identificacion == login.usuario);//buscamos los datos del usuario por la identificacion
                    if (usuario != null)
                    {
                        if (usuario.vigente.Value)//si el musuario esta vigente
                        {
                            //var clave64 = login.password;
                            var clave64 = decode.encriptar256(login.password);//decodificamos la clave de la base para comparar la password ingresada por el usuario
                            if (clave64 == usuario.clave && (usuario.usuarioBloqueado == false || usuario.usuarioBloqueado == null))//comparamos que la clave sea igual y que el usuario no este bloqueado
                            {
                                string Tipo = "";
                                if (usuario.dicTipoDocumento == 5)//
                                {
                                    Tipo = "CC";
                                }
                                string nombre = "";
                                string ip = "";

                                var usuarioVerificado = _ir.ObtenerRegistro<UsuarioSesion>(z => z.idUsuario == usuario.idUsuario && z.vigente == true);
                                //Se obtiene parametro para verificar si se tiene que validar la ip.
                                var sesionIP = _ir.ObtenerRegistro<Parametros>(z => z.idParametro == 13);

                                if (sesionIP != null && sesionIP.parametroJSON == "1")
                                {
                                    if (usuarioVerificado != null)
                                    {
                                        if (usuarioVerificado.ip != login.ip && usuarioVerificado.fechaSesion.GetValueOrDefault().ToShortDateString() == DateTime.Now.ToShortDateString())
                                        {
                                            respuesta.codigo = 1;
                                            respuesta.mensaje = "No puede ingresar desde un segundo dispositivo";
                                            return Ok(respuesta);
                                        }
                                    }
                                }

                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                Rootobject2 ent = new Rootobject2();
                                var respuestaDevuelta = new List<ContingenciaPrimaria>();
                                respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == login.usuario);
                                var registros = new DataTable();
                                List<String> coberturas = new List<String>();

                                var direccionamientoMetodo = await _ir.Find<Parametros>(10);

                                if (direccionamientoMetodo != null && direccionamientoMetodo.parametroJSON == "0")
                                {
                                    respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == login.usuario);

                                    // Consultamos los productos en contingenciaPrimaria del usuario
                                    var productos = new List<UsuarioProducto>();
                                    productos = await _ir.GetList<UsuarioProducto>(z => z.IdentificacionUsuario == login.usuario);

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
                                }
                                else
                                {
                                    ent = validacionSms.ValidaUserSpecial(Tipo, login.usuario);//obtrenemos los datos del usuario desde la api de cardif
                                }

                                if (ent == null && respuestaDevuelta.Count < 1)
                                {
                                    respuesta.codigo = 1;
                                    respuesta.mensaje = "No cuentas con seguro activo";
                                    return Ok(respuesta);
                                }

                                usuario.responseJSON = JsonConvert.SerializeObject(ent);//
                                usuario.cantidadIntentos = 0;
                                //}

                                await _ir.Update(usuario, usuario.idUsuario);//actualizamos los datos del usuario
                                //EJECUTAR REST DE BANCO BOGOTA
                                var respuestaViewModels = new UsuarioViewModels();
                                var countPolicy = 0;
                                var listaSubSocios = new List<string>();

                                if (ent != null && (ent.clienteAsegurado != null && ent.clienteAsegurado.statusResponse.status == "200"))
                                {
                                    if (ent.clienteAsegurado.customerInformation.policy == null)
                                    {
                                        respuesta.codigo = 1;
                                        respuesta.mensaje = "No cuentas con seguro activo";
                                        return Ok(respuesta);
                                    }

                                    var rs = ent.clienteAsegurado.customerInformation.policy.ToList().FirstOrDefault();
                                    foreach (Policy2 policy in ent.clienteAsegurado.customerInformation.policy)
                                    {
                                        if (policy.product != null)
                                        {
                                            if(policy.policyStatusCode.Equals("V"))
                                            {
                                                countPolicy++;
                                            }

                                        }
                                    }
                                    if (countPolicy <= 0)
                                    {
                                        respuesta.codigo = 1;
                                        respuesta.mensaje = "El usuario no cuenta con un producto asociado";
                                        return Ok(respuesta);
                                    }
                                }
                                else
                                {
                                    respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == login.usuario);

                                    // Consultamos los productos en contingenciaPrimaria del usuario
                                    var productos = new List<UsuarioProducto>();
                                    productos = await _ir.GetList<UsuarioProducto>(z => z.IdentificacionUsuario == login.usuario);

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

                                var fun = new FuncionesViewModels();
                                respuestaViewModels = Mapper.Map<Usuario, UsuarioViewModels>(usuario);

                                respuestaViewModels.idUsuario = fun.Base64_Encode(respuestaViewModels.idUsuario);

                                respuestaViewModels.clave = login.password;
                                respuestaViewModels.perfiles = coberturas;

                                // Socios
                                respuestaViewModels.socios = listaSubSocios;

                                respuesta.codigo = 0;
                                respuesta.mensaje = "OK";

                                //generamos el token
                                var dataToken = new { token = JwtManager.GenerarTokenJwt<UsuarioViewModels>(login.sitioUsuario, "Usuario", respuestaViewModels) };

                                var base64clave = StringCipher.Encrypt(dataToken.token, "grupomok");
                                respuesta.data = dataToken;
                                var idTransformado = fun.Base64_Decode(respuestaViewModels.idUsuario);
                                var userT = new UsuarioSesion();
                                userT.idUsuario = Convert.ToInt32(idTransformado);
                                userT.token = dataToken.token;
                                userT.fechaSesion = DateTime.Now;
                                userT.ip = login.ip;
                                userT.vigente = true;
                                usuario.instafitUrl = respuestaViewModels.instafitUrl;
                                if (sesionIP != null && sesionIP.parametroJSON == "1")
                                {
                                    userT.vigente = true;
                                }
                                else
                                {
                                    userT.vigente = false;
                                }
                                if (usuarioVerificado == null)
                                {
                                    await _ir.Add(userT);
                                }
                                else if (usuarioVerificado != null && usuarioVerificado.ip != login.ip)
                                {
                                    await _ir.Add(userT);
                                }

                                return Ok(respuesta);
                            }
                            else
                            {
                                if (usuario.usuarioBloqueado == true)//si el usuario esta bloqueado se envia un mensaje de aviso
                                {
                                    respuesta.codigo = -1;
                                    respuesta.mensaje = "Su cuenta ha sido bloqueada";
                                    return Ok(respuesta);
                                }
                                usuario.cantidadIntentos = usuario.cantidadIntentos + 1;
                                //obtenermos los parametros para el envio del mensaje sms
                                var parametros = await _ir.Find<Parametros>(Convert.ToInt32(WebConfigurationManager.AppSettings["IdIntentos"].ToString()));
                                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                                //desearializamos el objeto json
                                dynamic j = jsonSerializer.Deserialize<dynamic>(parametros.parametroJSON);
                                //obtenemos la cantidad de intentos
                                int intento = Convert.ToInt32(j["cantidad"].ToString());
                                if (usuario.cantidadIntentos > intento)
                                {
                                    usuario.usuarioBloqueado = true;
                                    respuesta.codigo = -1;
                                    respuesta.mensaje = "Su cuenta ha sido bloqueada";
                                }
                                else
                                {
                                    respuesta.mensaje = "Tu número de documento y contraseña no coinciden, Verifica tus datos e intenta nuevamente.";
                                    respuesta.codigo = 1;
                                }
                            }
                            await _ir.Update(usuario, usuario.idUsuario);//actualizamos los datos del usuario
                            return Ok(respuesta);
                        }
                        else
                        {
                            respuesta.codigo = 1;
                            respuesta.mensaje = "El usuario no esta vigente.";

                            return Ok(respuesta);
                        }
                    }
                    else
                    {
                        respuesta.codigo = 1;
                        respuesta.mensaje = "Tu número de documento y contraseña no coinciden, Verifica tus datos e intenta nuevamente.";

                        return Ok(respuesta);
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// registramos el usuario
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Login/RegistrarUserEspecial")]
        public async Task<RespuestaViewModels> PostUserEspecial(RegisterViewModels register)
        {           
            Usuario user = new Usuario();
            var respuesta = new RespuestaViewModels();
            try
            {
                var captcha = IsReCaptchaValid(register.tokenCaptcha);
                if (captcha == false)
                {
                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "No se validó el captcha";
                    return respuesta;
                }
                Rootobject2 ent = new Rootobject2();
                var respuestaDevuelta = new List<ContingenciaPrimaria>();
                respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == register.identificacion);
                var validacionSms = new FuncionesViewModels();
                var registros = new DataTable();
                List<String> coberturas = new List<String>();
                string Tipo = "";
                if (register.tipoIdentificacion == 5)
                {
                    Tipo = "CC";
                }

                SqlParameter[] parametros2 =
                        {
                             new SqlParameter("@IdProducto", 0)
                        };

                var query2 = "dbo.pa_ObtenerCoberturas";
                var registros2 = await _ir.dataOutPlacement(query2, parametros2);
                var tabla2 = registros2.AsEnumerable().Select(z => new { Id = z[0].ToString(), Codigo = z[1].ToString(), Descripcion = z[2].ToString() });
                var contadorCob = 0;

                var direccionamientoMetodo = await _ir.Find<Parametros>(10);
                if (direccionamientoMetodo != null && direccionamientoMetodo.parametroJSON == "0")
                {
                    respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == register.identificacion);
                    var productos = new List<UsuarioProducto>();
                    productos = await _ir.GetList<UsuarioProducto>(z => z.IdentificacionUsuario == register.identificacion);
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
                    foreach (var t2 in tabla2)
                    {
                        if (coberturas.Contains(t2.Descripcion))
                        {
                            contadorCob++;
                        }
                    }
                    if (contadorCob == 0)
                    {
                        respuesta.Codigo = 1;
                        respuesta.Mensaje = "No cuentas con seguro activo";
                        return respuesta;
                    }
                }
                else
                {
                    ent = validacionSms.ValidaUserSpecial(Tipo, register.identificacion);//obtrenemos los datos del usuario desde la api de cardif

                    // Evaluacion de productos segun los codigos de cbertuas
                    var countPolicy = 0;
                    if (ent != null && (ent.clienteAsegurado != null && ent.clienteAsegurado.statusResponse.status == "200"))
                    {
                        if (ent.clienteAsegurado.customerInformation.policy == null)
                        {
                            respuesta.Codigo = 1;
                            respuesta.Mensaje = "No cuentas con seguro activo";
                            return respuesta;
                        }

                        var rs = ent.clienteAsegurado.customerInformation.policy.ToList().FirstOrDefault();
                        foreach (Policy2 policy in ent.clienteAsegurado.customerInformation.policy)
                        {
                            if (policy.product != null)
                            {
                                if (policy.policyStatusCode.Equals("V"))
                                {
                                    countPolicy++;
                                }

                            }
                        }
                        if (countPolicy <= 0)
                        {
                            respuesta.Codigo = 1;
                            respuesta.Mensaje = "El usuario no cuenta con un producto asociado";
                            return respuesta;
                        }
                    }
                    else if (respuestaDevuelta.Count == 0 && ent != null && (ent.clienteAsegurado != null && ent.clienteAsegurado.statusResponse.status == "404"))
                    {
                        respuesta.Codigo = 1;
                        respuesta.Mensaje = "No cuentas con un seguro activo";
                        return respuesta;
                    }
                    else
                    {
                        ent = new Rootobject2();
                        respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == register.identificacion);

                        // Consultamos los productos en contingenciaPrimaria del usuario
                        var productos = new List<UsuarioProducto>();
                        productos = await _ir.GetList<UsuarioProducto>(z => z.IdentificacionUsuario == register.identificacion);

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
                }
                //if (ent.customerInformation == null)//si no es valido se envia mensaje de error
                //{
                //    respuesta.Codigo = 1;
                //    respuesta.Mensaje = "Aun no cuentas con seguro activo";
                //    return respuesta;
                //}



                //var userViewModels = await TraeUser(register.identificacion, register.tipoIdentificacion);//verificamos en ws cardiff   

                var registro = await _ir.GetFirst<Usuario>(z => z.identificacion == register.identificacion);//si el usuario es null

                if (ent != null && (ent.clienteAsegurado != null && (ent.clienteAsegurado.customerInformation != null)))//si los datos son validos procedemos a insertar los datos del usuario
                {
                    var nombre2 = "";
                    var nombre1 = "";

                    if (registro == null)
                    {

                        if (ent.clienteAsegurado.customerInformation.policy == null)
                        {
                            respuesta.Codigo = 1;
                            respuesta.Mensaje = "Aún no cuentas con seguro activo";
                            return respuesta;
                        }

                        user.identificacion = register.identificacion;

                        user.apellidoPaterno = ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerLastName;
                        user.apellidoMaterno = null;

                        user.correoElectronico = ent.clienteAsegurado.customerInformation.customerData.customerContactData.email.customerEmail;


                        if (!String.IsNullOrEmpty(ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerMiddleName))
                        {
                            nombre2 = ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerMiddleName;
                        }
                        if (!String.IsNullOrEmpty(ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerFirstName))
                        {
                            nombre1 = ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerFirstName;
                        }
                        user.nombres = nombre1 + " " + nombre2;

                        user.dicTipoDocumento = register.tipoIdentificacion;
                        user.responseJSON = JsonConvert.SerializeObject(ent);

                    }
                    else if (registro != null && (registro.clave == "" && registro.numeroCelular == null))
                    {
                        var datoTipoD = await _ir.GetFirst<DiccionarioDatos>(z => z.IdDiccionarioDatos == register.tipoIdentificacion);
                        var dataUser = new DataRespuestaViewModels();
                        dataUser.email = registro.correoElectronico;
                        dataUser.fono = registro.numeroCelular;
                        dataUser.identificacion = registro.identificacion;
                        dataUser.idusuario = registro.idUsuario;
                        dataUser.materno = registro.apellidoMaterno;
                        dataUser.nombre = registro.nombres;
                        dataUser.paterno = registro.apellidoPaterno;
                        dataUser.idTipoIdentificacion = datoTipoD.IdDiccionarioDatos;
                        dataUser.nombreTipoIdentificacion = datoTipoD.nombreDiccionario;
                        respuesta.user = dataUser;
                        respuesta.Codigo = 0;
                        respuesta.Mensaje = "Usuario registrado exitosamente";
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "Ya existe ese usuario con esa documentación, por favor inicie sesión";
                        return respuesta;
                    }
                }else if (registro != null && (registro.clave == "" && registro.numeroCelular == null))
                {
                    var datoTipoD = await _ir.GetFirst<DiccionarioDatos>(z => z.IdDiccionarioDatos == register.tipoIdentificacion);
                    var dataUser = new DataRespuestaViewModels();
                    dataUser.email = registro.correoElectronico;
                    dataUser.fono = registro.numeroCelular;
                    dataUser.identificacion = registro.identificacion;
                    dataUser.idusuario = registro.idUsuario;
                    dataUser.materno = registro.apellidoMaterno;
                    dataUser.nombre = registro.nombres;
                    dataUser.paterno = user.apellidoPaterno;
                    dataUser.idTipoIdentificacion = datoTipoD.IdDiccionarioDatos;
                    dataUser.nombreTipoIdentificacion = datoTipoD.nombreDiccionario;
                    respuesta.user = dataUser;
                    respuesta.Codigo = 0;
                    respuesta.Mensaje = "Usuario registrado exitosamente";
                    return respuesta;
                }
                else if (registro != null)
                {
                    respuesta.Codigo = 2;
                    respuesta.Mensaje = "Ya existe ese usuario con esa documentación, por favor inicie sesión";
                    return respuesta;
                }
                else if (ent == null || ((ent.clienteAsegurado == null && respuestaDevuelta.Count == 0 && registro == null)))
                {
                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Aún no cuentas con seguro activo";
                    return respuesta;
                }
                else if (ent == null || ((ent.clienteAsegurado == null && respuestaDevuelta.Count >= 1 && registro == null) || (ent.clienteAsegurado.customerInformation == null && respuestaDevuelta.Count >= 1 && registro == null)))
                {
                    // Invocamos la busqueda a contingencia cuando no obtenemos datos del WS
                    respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == register.identificacion);

                    user.identificacion = register.identificacion;

                    user.apellidoPaterno = "";
                    user.apellidoMaterno = "";

                    user.correoElectronico = respuestaDevuelta.FirstOrDefault().correoElectronico;

                    user.nombres = respuestaDevuelta.FirstOrDefault().nombreUsuario;

                    user.dicTipoDocumento = register.tipoIdentificacion;
                    user.responseJSON = "";
                }
                else if (registro != null && String.IsNullOrEmpty(registro.clave))
                {
                    var datoTipoR = await _ir.GetFirst<DiccionarioDatos>(z => z.IdDiccionarioDatos == register.tipoIdentificacion);//traemos los datos del iddiccionario
                    var dataR = new DataRespuestaViewModels();
                    dataR.email = registro.correoElectronico;
                    dataR.fono = registro.numeroCelular;
                    dataR.identificacion = registro.identificacion;
                    dataR.idusuario = registro.idUsuario;
                    dataR.materno = registro.apellidoMaterno;
                    dataR.nombre = registro.nombres;
                    dataR.paterno = registro.apellidoPaterno;
                    dataR.idTipoIdentificacion = datoTipoR.IdDiccionarioDatos;
                    dataR.nombreTipoIdentificacion = datoTipoR.nombreDiccionario;

                    respuesta.user = dataR;
                    respuesta.Codigo = 0;
                    respuesta.Mensaje = "Usuario existente";
                    return respuesta;
                }
                else
                {
                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Aún no cuentas con seguro activo";
                    return respuesta;
                }
                user.fechaCreacion = DateTime.Now;
                user.usuarioBloqueado = false;
                user.numeroCelular = null;
                user.tipoEstado = 1;
                user.ultimaFechaCambioClave = null;
                user.validacionData = true;
                user.vigente = true;
                user.diasRestantesCambioClave = 30;
                user.estadoUsuario = 1;
                user.fechaActualizacion = null;
                user.fechaCambioClave = null;
                user.fechaUltimoLogin = null;
                user.cantidadIntentos = 0;
                user.clave = "";
                await _ir.Add<Usuario>(user);//insertamos el usuario

                var registerUser = await _ir.GetFirst<Usuario>(z => z.identificacion == user.identificacion);
                user.idUsuario = registerUser.idUsuario;//traigo los datos del usuario recien registrado para obtener el id

                respuesta.Codigo = 0;
                respuesta.Mensaje = "Usuario registrado exitosamente";

                var datoTipo = await _ir.GetFirst<DiccionarioDatos>(z => z.IdDiccionarioDatos == register.tipoIdentificacion);//traemos los datos del iddiccionario

                var data = new DataRespuestaViewModels();
                data.email = user.correoElectronico;
                data.fono = user.numeroCelular;
                data.identificacion = user.identificacion;
                data.idusuario = user.idUsuario;
                data.materno = user.apellidoMaterno;
                data.nombre = user.nombres;
                data.paterno = user.apellidoPaterno;
                data.idTipoIdentificacion = datoTipo.IdDiccionarioDatos;
                data.nombreTipoIdentificacion = datoTipo.nombreDiccionario;
                respuesta.user = data;

                var hojadatos = new HojaVidaDatosPersonales();//creo al usuario en la hoja de vida
                hojadatos.idUsuario = registerUser.idUsuario;
                hojadatos.fechaCreacion = DateTime.Now;
                hojadatos.vigente = true;
                hojadatos.identificacion = user.identificacion;
                hojadatos.nombres = user.nombres;
                hojadatos.apellidoPaterno = user.apellidoPaterno;
                hojadatos.correoElectronico = user.apellidoMaterno;
                hojadatos.identificacion = register.identificacion;
                hojadatos.dicTipoDocumento = register.tipoIdentificacion;

                await _ir.Add(hojadatos);//insserto al usuario en hoja de datos de vida personales

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
        }

        /// <summary>
        /// registramos el usuario
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Login/RegistrarUser")]
        public async Task<RespuestaViewModels> PostUser(RegisterViewModels register)
        {
            Usuario user = new Usuario();
            var respuesta = new RespuestaViewModels();
            try
            {
                var captcha = IsReCaptchaValid(register.tokenCaptcha);
                if (captcha == false)
                {
                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "No se validó el captcha";
                    return respuesta;
                }
                Rootobject ent = new Rootobject();
                var respuestaDevuelta = new List<ContingenciaPrimaria>();
                respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == register.identificacion);
                var validacionSms = new FuncionesViewModels();
                var registros = new DataTable();
                List<String> coberturas = new List<String>();
                string Tipo = "";
                if (register.tipoIdentificacion == 5)
                {
                    Tipo = "CC";
                }

                SqlParameter[] parametros2 =
                        {
                             new SqlParameter("@IdProducto", 0)
                        };

                var query2 = "dbo.pa_ObtenerCoberturas";
                var registros2 = await _ir.dataOutPlacement(query2, parametros2);
                var tabla2 = registros2.AsEnumerable().Select(z => new { Id = z[0].ToString(), Codigo = z[1].ToString(), Descripcion = z[2].ToString() });
                var contadorCob = 0;

                var direccionamientoMetodo = await _ir.Find<Parametros>(10);
                if (direccionamientoMetodo != null && direccionamientoMetodo.parametroJSON == "0")
                {
                    respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == register.identificacion);
                    var productos = new List<UsuarioProducto>();
                    productos = await _ir.GetList<UsuarioProducto>(z => z.IdentificacionUsuario == register.identificacion);
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
                    foreach (var t2 in tabla2)
                    {
                        if (coberturas.Contains(t2.Descripcion))
                        {
                            contadorCob++;
                        }
                    }
                    if (contadorCob == 0)
                    {
                        respuesta.Codigo = 1;
                        respuesta.Mensaje = "No cuentas con seguro activo";
                        return respuesta;
                    }
                }
                else
                {
                    ent = validacionSms.ValidaUser(Tipo, register.identificacion);//obtrenemos los datos del usuario desde la api de cardif

                    // Evaluacion de productos segun los codigos de cbertuas
                    var countProduct = 0;
                    var countCoverage = 0;
                    if (ent != null && (ent.clienteAsegurado != null && ent.clienteAsegurado.statusResponse.status == "200"))
                    {
                        if (ent.clienteAsegurado.customerInformation.policy == null)
                        {
                            respuesta.Codigo = 1;
                            respuesta.Mensaje = "No cuentas con seguro activo";
                            return respuesta;
                        }

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
                                        else
                                        {
                                            countCoverage++;
                                        }
                                        if (countCoverage == product.plan.Length && countCoverage == ent.clienteAsegurado.customerInformation.policy.Length && plan.coverage != null)
                                        {
                                            respuesta.Codigo = 1;
                                            respuesta.Mensaje = "El usuario no cuenta con coberturas";
                                            return respuesta;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                countProduct++;
                            }
                            if (countProduct == ent.clienteAsegurado.customerInformation.policy.Length)
                            {
                                respuesta.Codigo = 1;
                                respuesta.Mensaje = "No cuentas con un seguro activo";
                                return respuesta;
                            }
                        }
                    }
                    else if (respuestaDevuelta.Count == 0 && ent != null && (ent.clienteAsegurado != null && ent.clienteAsegurado.statusResponse.status == "404"))
                    {
                        respuesta.Codigo = 1;
                        respuesta.Mensaje = "No cuentas con un seguro activo";
                        return respuesta;
                    }
                    else
                    {
                        ent = new Rootobject();
                        respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == register.identificacion);

                        // Consultamos los productos en contingenciaPrimaria del usuario
                        var productos = new List<UsuarioProducto>();
                        productos = await _ir.GetList<UsuarioProducto>(z => z.IdentificacionUsuario == register.identificacion);

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
                    foreach (var t2 in tabla2)
                    {
                        foreach (var cob in coberturas)
                        {
                            if (t2.Descripcion.ToLower().Contains(cob.ToLower()))
                            {
                                contadorCob++;
                            }
                            else if (cob == "retencion")
                            {
                                contadorCob++;
                            }
                        }
                    }
                    if (contadorCob == 0)
                    {
                        respuesta.Codigo = 1;
                        respuesta.Mensaje = "Aún no cuentas con seguro activo";
                        return respuesta;
                    }
                }
                //if (ent.customerInformation == null)//si no es valido se envia mensaje de error
                //{
                //    respuesta.Codigo = 1;
                //    respuesta.Mensaje = "Aun no cuentas con seguro activo";
                //    return respuesta;
                //}



                //var userViewModels = await TraeUser(register.identificacion, register.tipoIdentificacion);//verificamos en ws cardiff   

                var registro = await _ir.GetFirst<Usuario>(z => z.identificacion == register.identificacion);//si el usuario es null

                if (ent != null && (ent.clienteAsegurado != null && (ent.clienteAsegurado.customerInformation != null)))//si los datos son validos procedemos a insertar los datos del usuario
                {
                    var nombre2 = "";
                    var nombre1 = "";

                    if (registro == null)
                    {

                        if (ent.clienteAsegurado.customerInformation.policy == null)
                        {
                            respuesta.Codigo = 1;
                            respuesta.Mensaje = "Aún no cuentas con seguro activo";
                            return respuesta;
                        }

                        user.identificacion = register.identificacion;

                        user.apellidoPaterno = ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerLastName;
                        user.apellidoMaterno = null;

                        user.correoElectronico = ent.clienteAsegurado.customerInformation.customerData.customerContactData.email.customerEmail;


                        if (!String.IsNullOrEmpty(ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerMiddleName))
                        {
                            nombre2 = ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerMiddleName;
                        }
                        if (!String.IsNullOrEmpty(ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerFirstName))
                        {
                            nombre1 = ent.clienteAsegurado.customerInformation.customerData.customerPersonalInformation.customerFirstName;
                        }
                        user.nombres = nombre1 + " " + nombre2;

                        user.dicTipoDocumento = register.tipoIdentificacion;
                        user.responseJSON = JsonConvert.SerializeObject(ent);

                    }
                    else if (registro != null && (registro.clave == "" && registro.numeroCelular == null))
                    {
                        var datoTipoD = await _ir.GetFirst<DiccionarioDatos>(z => z.IdDiccionarioDatos == register.tipoIdentificacion);
                        var dataUser = new DataRespuestaViewModels();
                        dataUser.email = registro.correoElectronico;
                        dataUser.fono = registro.numeroCelular;
                        dataUser.identificacion = registro.identificacion;
                        dataUser.idusuario = registro.idUsuario;
                        dataUser.materno = registro.apellidoMaterno;
                        dataUser.nombre = registro.nombres;
                        dataUser.paterno = user.apellidoPaterno;
                        dataUser.idTipoIdentificacion = datoTipoD.IdDiccionarioDatos;
                        dataUser.nombreTipoIdentificacion = datoTipoD.nombreDiccionario;
                        respuesta.user = dataUser;
                        respuesta.Codigo = 0;
                        respuesta.Mensaje = "Usuario registrado exitosamente";
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "Ya existe ese usuario con esa documentación, por favor inicie sesión";
                        return respuesta;
                    }
                }
                else if (registro != null && (registro.clave == "" && registro.numeroCelular == null))
                {
                    var datoTipoD = await _ir.GetFirst<DiccionarioDatos>(z => z.IdDiccionarioDatos == register.tipoIdentificacion);
                    var dataUser = new DataRespuestaViewModels();
                    dataUser.email = registro.correoElectronico;
                    dataUser.fono = registro.numeroCelular;
                    dataUser.identificacion = registro.identificacion;
                    dataUser.idusuario = registro.idUsuario;
                    dataUser.materno = registro.apellidoMaterno;
                    dataUser.nombre = registro.nombres;
                    dataUser.paterno = user.apellidoPaterno;
                    dataUser.idTipoIdentificacion = datoTipoD.IdDiccionarioDatos;
                    dataUser.nombreTipoIdentificacion = datoTipoD.nombreDiccionario;
                    respuesta.user = dataUser;
                    respuesta.Codigo = 0;
                    respuesta.Mensaje = "Usuario registrado exitosamente";
                    return respuesta;
                }
                else if (registro != null)
                {
                    respuesta.Codigo = 2;
                    respuesta.Mensaje = "Ya existe ese usuario con esa documentación, por favor inicie sesión";
                    return respuesta;
                }
                else if (ent == null || ((ent.clienteAsegurado == null && respuestaDevuelta.Count == 0 && registro == null)))
                {
                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Aún no cuentas con seguro activo";
                    return respuesta;
                }
                else if (ent == null || ((ent.clienteAsegurado == null && respuestaDevuelta.Count >= 1 && registro == null) || (ent.clienteAsegurado.customerInformation == null && respuestaDevuelta.Count >= 1 && registro == null)))
                {
                    // Invocamos la busqueda a contingencia cuando no obtenemos datos del WS
                    respuestaDevuelta = await _ir.GetList<ContingenciaPrimaria>(z => z.numeroIdentificacion == register.identificacion);

                    user.identificacion = register.identificacion;

                    user.apellidoPaterno = "";
                    user.apellidoMaterno = "";

                    user.correoElectronico = respuestaDevuelta.FirstOrDefault().correoElectronico;

                    user.nombres = respuestaDevuelta.FirstOrDefault().nombreUsuario;

                    user.dicTipoDocumento = register.tipoIdentificacion;
                    user.responseJSON = "";
                }
                else if (registro != null && String.IsNullOrEmpty(registro.clave))
                {
                    var datoTipoR = await _ir.GetFirst<DiccionarioDatos>(z => z.IdDiccionarioDatos == register.tipoIdentificacion);//traemos los datos del iddiccionario
                    var dataR = new DataRespuestaViewModels();
                    dataR.email = registro.correoElectronico;
                    dataR.fono = registro.numeroCelular;
                    dataR.identificacion = registro.identificacion;
                    dataR.idusuario = registro.idUsuario;
                    dataR.materno = registro.apellidoMaterno;
                    dataR.nombre = registro.nombres;
                    dataR.paterno = registro.apellidoPaterno;
                    dataR.idTipoIdentificacion = datoTipoR.IdDiccionarioDatos;
                    dataR.nombreTipoIdentificacion = datoTipoR.nombreDiccionario;

                    respuesta.user = dataR;
                    respuesta.Codigo = 0;
                    respuesta.Mensaje = "Usuario existente";
                    return respuesta;
                }
                else
                {
                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Aún no cuentas con seguro activo";
                    return respuesta;
                }
                user.fechaCreacion = DateTime.Now;
                user.usuarioBloqueado = false;
                user.numeroCelular = null;
                user.tipoEstado = 1;
                user.ultimaFechaCambioClave = null;
                user.validacionData = true;
                user.vigente = true;
                user.diasRestantesCambioClave = 30;
                user.estadoUsuario = 1;
                user.fechaActualizacion = null;
                user.fechaCambioClave = null;
                user.fechaUltimoLogin = null;
                user.cantidadIntentos = 0;
                user.clave = "";
                await _ir.Add<Usuario>(user);//insertamos el usuario

                var registerUser = await _ir.GetFirst<Usuario>(z => z.identificacion == user.identificacion);
                user.idUsuario = registerUser.idUsuario;//traigo los datos del usuario recien registrado para obtener el id

                respuesta.Codigo = 0;
                respuesta.Mensaje = "Usuario registrado exitosamente";

                var datoTipo = await _ir.GetFirst<DiccionarioDatos>(z => z.IdDiccionarioDatos == register.tipoIdentificacion);//traemos los datos del iddiccionario

                var data = new DataRespuestaViewModels();
                data.email = user.correoElectronico;
                data.fono = user.numeroCelular;
                data.identificacion = user.identificacion;
                data.idusuario = user.idUsuario;
                data.materno = user.apellidoMaterno;
                data.nombre = user.nombres;
                data.paterno = user.apellidoPaterno;
                data.idTipoIdentificacion = datoTipo.IdDiccionarioDatos;
                data.nombreTipoIdentificacion = datoTipo.nombreDiccionario;
                respuesta.user = data;

                var hojadatos = new HojaVidaDatosPersonales();//creo al usuario en la hoja de vida
                hojadatos.idUsuario = registerUser.idUsuario;
                hojadatos.fechaCreacion = DateTime.Now;
                hojadatos.vigente = true;
                hojadatos.identificacion = user.identificacion;
                hojadatos.nombres = user.nombres;
                hojadatos.apellidoPaterno = user.apellidoPaterno;
                hojadatos.correoElectronico = user.apellidoMaterno;
                hojadatos.identificacion = register.identificacion;
                hojadatos.dicTipoDocumento = register.tipoIdentificacion;

                await _ir.Add(hojadatos);//insserto al usuario en hoja de datos de vida personales

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
        }
        /// <summary>
        /// actualizar los datos del usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Login/UpdateUser")]
        public async Task<RespuestaViewModels> UpdateUser(UpdateViewModels model)
        {
            var respuesta = new RespuestaViewModels();
            try
            {
                var xc = new FuncionesViewModels();
                var captcha = IsReCaptchaValid(model.tokenCaptcha);
                if (captcha == false)
                {
                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "No se validó el captcha";
                    return respuesta;
                }
                var password = xc.encryptDecrypt(model.clave, model.tokenCaptcha);
                model.clave = password;
                var entidad = await _ir.Find<Usuario>(model.idusuario);//verifico si existe en la base y procedo a actualizar la clave y otros datos
                if (entidad != null)
                {
                    var clave64 = model.clave;
                    entidad.clave = xc.encriptar256(clave64);
                    entidad.correoElectronico = model.email;
                    entidad.numeroCelular = model.fono;
                    entidad.idUsuario = model.idusuario;

                    await _ir.Update(entidad, entidad.idUsuario);//actualizamos los datos

                    var hoja = await _ir.GetFirst<HojaVidaDatosPersonales>(z => z.idUsuario == model.idusuario);
                    hoja.telefonoCelular = model.fono;
                    if (!String.IsNullOrEmpty(model.email))//actualizamos el registro en hoja de vida de datos personales
                    {
                        hoja.correoElectronico = model.email;
                    }
                    await _ir.Update(hoja, hoja.idDatosPersonales);

                    var mem = new FuncionesViewModels();
                    var xr = new CourseraViewModels();
                    xr.email = entidad.correoElectronico;
                    xr.id = entidad.idUsuario;
                    xr.name = entidad.nombres;
                    xr.password = model.clave;

                    var mensajeCoursera = await mem.RegistroCousera(xr);
                    if (mensajeCoursera != "Usuario registrado exitosamente")
                    {
                        respuesta.Mensaje = mensajeCoursera;
                    }
                    else
                    {
                        respuesta.Mensaje = "Haz finalizado tu registro exitosamente";
                    }
                    respuesta.Codigo = 0;
                    var funcion = new FuncionesViewModels();
                    if (!(String.IsNullOrEmpty(entidad.numeroCelular)))//si el numero de celular es valido procedemos a enviar un sms
                    {
                        var indicativo = await _ir.Find<Parametros>(8);
                        var mensaje = await _ir.Find<Mensaje>(Convert.ToInt32(WebConfigurationManager.AppSettings["idBienvenidaRegistro"].ToString()));
                        string telefono = indicativo.parametroJSON + entidad.numeroCelular;
                        var cadenatexto = "{\"messages\":[{ \"destination\":" + telefono + ",\"messageText\":\"" + mensaje.textoMensaje + "\"}]}";
                        var cadena = await funcion.EnvioSms(telefono, cadenatexto);
                        //var insercionlog = new LogRecuperacionContrasenaSMS();
                        //insercionlog.celular = entidad.numeroCelular.ToString();
                        //insercionlog.identificacion = entidad.identificacion;
                        //insercionlog.mensajeSMS = ";";
                        //insercionlog.response = cadena;
                        //await _ir.Add(insercionlog);
                    }
                    if (!String.IsNullOrWhiteSpace(entidad.correoElectronico))
                    {
                        await funcion.EnvioEmailBienvenida(entidad.idUsuario);
                    }
                    var data = new DataRespuestaViewModels();//enviamos los datos del usuario registrado
                    data.email = entidad.correoElectronico;
                    data.fono = entidad.numeroCelular;
                    data.identificacion = entidad.identificacion;
                    data.idusuario = entidad.idUsuario;
                    data.materno = entidad.apellidoMaterno;
                    data.nombre = entidad.nombres;
                    data.paterno = entidad.apellidoPaterno;

                    respuesta.user = data;
                    return respuesta;
                }
                else
                {
                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Usuario no existe en la base de datos";
                    respuesta.user = null;
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = ex.Message;
                respuesta.user = null;
                return respuesta;
            }
        }


        /// <summary>
        /// obtengo los datos de la tabla diccionario
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("api/Login/Tipos/{id}")]
        public async Task<IHttpActionResult> MenusTipos(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                respuesta.codigo = 0;
                respuesta.mensaje = "OK";

                var menus = await _ir.GetList<DiccionarioDatos>(z => z.tipoDiccionario == 1 && z.vigente == true);
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
            finally
            {

            }
        }

        /// <summary>
        /// obtengo los datos de la tabla paramertros
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("api/Login/Parametros/{id}")]
        public async Task<IHttpActionResult> Parametros(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                respuesta.codigo = 0;
                respuesta.mensaje = "OK";

                var parametro = await _ir.GetFirst<Parametros>(z => z.idParametro == id);
                respuesta.data = parametro;

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

        /// <summary>
        /// obtengo los datos de la tabla diccionario
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("api/Login/ciudadesDepartamento/{id}")]
        public async Task<IHttpActionResult> CiudadDepartamento(int id)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                respuesta.codigo = 0;
                respuesta.mensaje = "OK";

                var menus = await _ir.GetList<DiccionarioDatos>(z => z.departamento == id && z.vigente == true);
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
            finally
            {

            }
        }

        /// <summary>
        /// Metodo para Refrescar el Token ya en session.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost, JwtAuthentication]
        [Route("api/Token/RefreshToken")]
        public async Task<IHttpActionResult> postRefreshToken([FromBody] RequestLoginJWT login)
        {
            try
            {
                var decode = new FuncionesViewModels();
                login.sitioUsuario = WebConfigurationManager.AppSettings["sitioUsuario"].ToString();
                login.sitioPassword = WebConfigurationManager.AppSettings["sitioPassword"].ToString();
                login.usuario = decode.Base64_Decode(login.usuario);
                if (login == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                bool isCredentialValid = (login.sitioPassword == WebConfigurationManager.AppSettings["sitioPassword"].ToString() &&
                    login.sitioUsuario == WebConfigurationManager.AppSettings["sitioUsuario"].ToString());
                if (isCredentialValid)
                {
                    UsuarioViewModels usuario = (UsuarioViewModels)JwtManager.obtenerSession<UsuarioViewModels>("Usuario");
                    usuario.clave = "";
                    string base64Encoded = usuario.idUsuario;
                    int idUsuario;
                    byte[] data = System.Convert.FromBase64String(base64Encoded);
                    idUsuario = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(data));
                    var userNew = await _ir.GetFirst<Usuario>(z => z.idUsuario == idUsuario);
                    var urlInstafit = userNew.instafitUrl;
                    if (usuario != null)
                    {
                        usuario.instafitUrl = urlInstafit;
                        var token = new { token = JwtManager.GenerarTokenJwt<UsuarioViewModels>(login.sitioUsuario, "Usuario", usuario) };

                        var session = await _ir.GetFirst<Parametros>(z => z.idParametro == 13);
                        if (session.parametroJSON == "1")
                        {
                            var userData = _ir.ObtenerRegistro<UsuarioSesion>(z => z.idUsuario == idUsuario && z.vigente == true);
                            userData.token = token.token;
                            userData.vigente = true;
                            await _ir.Update<UsuarioSesion>(userData, userData.idUsuarioSesion);
                            //var userToken = new UsuarioSesion { idUsuario = idUsuario, token = token.token, fechaSesion = DateTime.Now };
                            //await _ir.Add(userToken);
                        }


                        return Ok(token);
                    }
                    else
                    {
                        throw new Exception("Error al obtener la session actual del usuario.");
                    }
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Metodo para Obtener los datos de la sesion.
        /// </summary>
        /// <returns></returns>
        [HttpGet, JwtAuthentication]
        [Route("api/Login/obtenerSession")]
        public IHttpActionResult getResultado()
        {
            try
            {
                UsuarioViewModels Usuario = (UsuarioViewModels)JwtManager.obtenerSession<UsuarioViewModels>("Usuario");
                Usuario.clave = "";
                return Ok(Usuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Recuperar contraseña se envia el codigo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Login/RecuperaContrasena")]
        public async Task<IHttpActionResult> RecuperarContrasena(RecuperarContraseña recuperar)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var captcha = IsReCaptchaValid(recuperar.tokenCaptcha);
                if (captcha == false)
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se validó el captcha";
                    return Ok(respuesta);
                }
                //buscamos el registro en la tabla de usuarios por 
                var entidad = await _ir.GetFirst<Usuario>(z => z.identificacion == recuperar.identificacion);

                if (entidad != null)
                {//generamos un codigo aleatorio
                    Random rnd = new Random();
                    int aleatorio = rnd.Next(100000, 999999);
                    int documentoId = entidad.idUsuario;
                    entidad.codigo = aleatorio;
                    entidad.fechaCambioClave = DateTime.Now;
                    entidad.fechaExpiracionCodigo = DateTime.Now.AddMinutes(10);
                    await _ir.Update(entidad, entidad.idUsuario);//guardamos en la tabla de usuario el codigo, la fecha de cambio clave y fecha expiracion

                    var funcion = new FuncionesViewModels();//enviamos el sms e insertamos en la tabla log
                    //obtener el mensaje
                    var mensajetexto = await _ir.Find<Mensaje>(Convert.ToInt32(WebConfigurationManager.AppSettings["idMensajeRecuperacion"].ToString()));
                        if (entidad.numeroCelular != null)//si el numero de celular es valido
                        {
                            var indicativo = await _ir.Find<Parametros>(8);
                            string mensaje = mensajetexto.textoMensaje.Replace("@codigo", Convert.ToString(aleatorio));
                            string telefono = indicativo.parametroJSON + entidad.numeroCelular;
                            var cadenatexto = "{\"messages\":[{ \"destination\":" + telefono + ",\"messageText\":\"" + mensaje + "\"}]}";

                            var cadena = await funcion.EnvioSms(telefono, cadenatexto);
                            var insercionlog = new LogRecuperacionContrasenaSMS();
                            insercionlog.celular = entidad.numeroCelular.ToString();
                            insercionlog.identificacion = entidad.identificacion;
                            insercionlog.mensajeSMS = "Hola se le envia el siguiente código para que recupere la contraseña:" + aleatorio;
                            insercionlog.response = cadena;
                            await _ir.Add(insercionlog);//insertamos la log como respaldo de envio de sms
                        }
                        if (!String.IsNullOrEmpty(entidad.correoElectronico))//envio mensaje por correo
                        {
                            await EnvioEmailRecuperacion(entidad.idUsuario);
                        }
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Se ha enviado un codigo para recuperar la contraseña";
                    respuesta.data = documentoId;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Se ha enviado un codigo para recuperar la contraseña";
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// metodo para cambiar la contraseña
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Login/CambiarContrasena")]
        public async Task<IHttpActionResult> CambiarContrasena(ConfirmaViewModels confirma)
        {
            entRespuesta respuesta = new entRespuesta();
            var entidad = await _ir.Find<Usuario>(confirma.id);//buscamos los datos del usuario por el id
            if (entidad.codigo != null && entidad.codigo == confirma.code && entidad.fechaExpiracionCodigo >= DateTime.Now)
            {
                FuncionesViewModels codificar = new FuncionesViewModels();
                var captcha = IsReCaptchaValid(confirma.tokenCaptcha);
                if (captcha == false)
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se validó el captcha";
                    return Ok(respuesta);
                }
                var password = codificar.encryptDecrypt(confirma.password, confirma.tokenCaptcha);
                confirma.password = password;
                if (entidad != null)
                {
                    var _fun = new FuncionesViewModels();
                    var clave = confirma.password;
                    entidad.clave = codificar.encriptar256(clave);
                    entidad.usuarioBloqueado = false;
                    entidad.cantidadIntentos = 0;
                    entidad.codigo = null;
                    entidad.fechaExpiracionCodigo = null;
                    await _ir.Update(entidad, entidad.idUsuario);//actualizamos los datos del usuario
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Se ha cambiado la contraseña";
                    respuesta.data = null;
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Que pena por usted, debera iniciar nuevamente el proceso de recuperacion";
                }
            }
            else
            {
                respuesta.codigo = 1;
                respuesta.mensaje = "Código de verificación incorrecto";
            }

            return Ok(respuesta);
          
        }
        /// <summary>
        /// metodo para enviar el codigo ya sea por sms o correo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Login/EnvioCodigo")]
        public async Task<IHttpActionResult> EnvioCodigo(CodigoRespuestaViewModels codigo)
        {
            entRespuesta respuesta = new entRespuesta();
            try
            {
                var captcha = IsReCaptchaValid(codigo.tokenCaptcha);
                if (captcha == false)
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "No se validó el captcha";
                    return Ok(respuesta);
                }
                var entidad = await _ir.Find<Usuario>(codigo.id);//obtengo los datos de usuario por el id
                if (entidad != null && entidad.codigo == codigo.code && entidad.fechaExpiracionCodigo >= DateTime.Now)
                {
                    respuesta.codigo = 0;
                    respuesta.mensaje = "Se le han enviado los datos";
                }
                else
                {
                    respuesta.codigo = 1;
                    respuesta.mensaje = "Debera iniciar nuevamente el proceso de recuperación de la contraseña";
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// funcion para enviar el email de recuperacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<string> EnvioEmailRecuperacion(int id)
        {
            string mensaje = "";
            try
            {
                SqlParameter[] parametros =
            {
                new SqlParameter("@idUsuario",id)
            };
                string query = "[dbo].[pa_EnvioCorreoRecuperacionClave]";
                var tabla = await _ir.data(query, parametros);
                return mensaje = tabla.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                return mensaje = ex.Message;
            }
        }

        private async Task<Usuario> TraeUser(string identificacion, int tipo)
        {
            var registro = await _ir.Find<Parametros>(3);
            string data = registro.parametroJSON;
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //dynamic j = jsonSerializer.Deserialize<dynamic>(data);

            dynamic dynObj = JsonConvert.DeserializeObject(data);
            var lista = new List<Usuario>();
            var cadena1 = dynObj["clienteAsegurado"];
            var cadena2 = cadena1["customerInformation"];
            var cadena3 = cadena2["customerData"];
            var cadena4 = cadena3["customerPersonalInformation"];

            var cadenacorreo = cadena3["customerContactData"];
            var cadenacorreo1 = cadenacorreo["email"];

            string documento = cadena4["customerDocumentNumber"];

            var user = new Usuario();

            user = lista.FirstOrDefault(z => z.identificacion == identificacion && z.dicTipoDocumento == tipo);
            return user;
        }

       
    }
}
