using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using static EcoOplacementApi.ViewModels.HtmlExperianViewModels;

namespace EcoOplacementApi.ViewModels
{
    public class FuncionesViewModels
    {
        private readonly IRepositoryGeneral _ir = new RepositoryGeneral();

        /// <summary>
        /// envio de sms
        /// </summary>
        /// <param name="telefono"></param>
        /// <param name="mensaje"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> EnvioSms(string telefono, string mensaje)
        {
            string url = WebConfigurationManager.AppSettings["SMS_Server_Wavy"].ToString();
            string user = WebConfigurationManager.AppSettings["SMS_User_Wavy"].ToString();
            string key = WebConfigurationManager.AppSettings["SMS_Key_Wavy"].ToString();

            try
            {
                HttpWebRequest request;
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Headers["authenticationtoken"] = key;
                request.Headers["username"] = user;
                //request.Host = "api-messaging.movile.com";
                request.Host = WebConfigurationManager.AppSettings["HostingSms"].ToString();
                request.Method = "POST";

                //string json = "{\"messages\":[{ \"destination\":" + telefono + ",\"messageText\":\"" + mensaje + "\"}]}";
                string json = mensaje;
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = request.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                var objText = tReader.ReadToEnd();//respuesta que nos envia la api
                            }
                        }
                    }
                }
                return "";
            }
            catch (WebException wex)
            {
                ErrorLog logFoto = new ErrorLog();
                logFoto.errorMensaje = wex.Message;
                await _ir.Add(logFoto);
                string error = "Problema con el envio sms";
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }
                return error;
            }
        }


        //public async Task<string> prueba(CourseraViewModels coursera)
        //{
        //    try
        //    {

        //        var client = new RestClient(WebConfigurationManager.AppSettings["urlCourseraT"].ToString());
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("content-type", "application/json");
        //        request.AddParameter("application/json", "{\"client_id\":\"gJCZVGqU8qtSeqe5OKpkaa6HI1AOlJXJ\",\"client_secret\":\"YAMUevre-WT5QDsVymUWhGEzOZ-hEEXOYNmhkQr2K8s6xrPgcyBtuTdHotmO90bx\",\"audience\":\"https://dev-vqdsauwl.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
        //        IRestResponse response = client.Execute(request);

        //        token obj = JsonConvert.DeserializeObject<token>(response.Content);
        //        var user = await _ir.Find<Usuario>(coursera.id);

        //        if (user != null)
        //        {
        //            var client2 = new RestClient(WebConfigurationManager.AppSettings["urlCourseraM"].ToString() + user.idCoursera);
        //            var request2 = new RestRequest(Method.POST);
        //            request2.AddHeader("Authorization", "Bearer " + obj.access_token);
        //            request2.AddHeader("Content-Type", "application/json");
        //            request2.AddParameter("application/json", ""{\"client_id\": \"AUYjreEPUsXTB60ET0EixLIav1HlVLak\", \"email\": \"" + coursera.email + "\",\"email_verified\": \"" + true + "\",\"password\": \"" + coursera.password +
        //            "\",\"connection\": \"CourseraDB\",\"name\": \"" + coursera.name + "\",\"user_metadata\": {\"color\": \"red\"}}"", ParameterType.RequestBody);
        //            IRestResponse response2 = client2.Execute(request2);
        //            return Ok("Contraeña modificada");
        //        }
        //        else
        //        {
        //            return BadRequest("Usuario no encontrado");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}


        /// <summary>
        /// registramos en la data de coursera
        /// </summary>
        /// <param name="coursera"></param>
        /// <returns></returns>
        public async Task<string> RegistroCousera(CourseraViewModels coursera)
        {
            try
            {
                //var client = new RestClient(WebConfigurationManager.AppSettings["urlCoursera"].ToString());
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("content-type", "application/json");
                //request.AddParameter("application/json", "{\"client_id\": \"AUYjreEPUsXTB60ET0EixLIav1HlVLak\", \"email\": \"" + coursera.email + "\",\"email_verified\":true,\"password\": \"" + coursera.password +
                //    "\",\"connection\": \"CourseraDB\",\"name\": \"" + coursera.name + "\",\"user_metadata\": {\"color\": \"red\"}}", ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);
                //var content = response.Content;

                //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                //dynamic j = jsonSerializer.Deserialize<dynamic>(content);//deserializamos el objeto
                //var id = j["_id"].ToString();

                //var user = await _ir.Find<Usuario>(coursera.id);

                //if (user != null)
                //{
                //    user.idCoursera = id;
                //    user.dataCoursera = content;
                //    await _ir.Update<Usuario>(user, user.idUsuario);
                //    ActualizaEstadoCousera(id);
                //}
                return "Usuario registrado exitosamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        private void ActualizaEstadoCousera(string courseraId)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["urlCourseraT"].ToString());
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\"client_id\":\"gJCZVGqU8qtSeqe5OKpkaa6HI1AOlJXJ\",\"client_secret\":\"YAMUevre-WT5QDsVymUWhGEzOZ-hEEXOYNmhkQr2K8s6xrPgcyBtuTdHotmO90bx\",\"email_verified\":true,\"audience\":\"https://dev-vqdsauwl.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                token obj = JsonConvert.DeserializeObject<token>(response.Content);


                var client2 = new RestClient(WebConfigurationManager.AppSettings["urlCourseraM"].ToString() + courseraId);
                var request2 = new RestRequest(Method.PATCH);
                request2.AddHeader("Authorization", "Bearer " + obj.access_token);
                request2.AddHeader("Content-Type", "application/json");
                request2.AddParameter("application/json", "{\"email_verified\":true,\"connection\": \"CourseraDB\"}", ParameterType.RequestBody);
                IRestResponse response2 = client2.Execute(request2);

            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }

        public object ClienteDataExperian(ExperianViewModels experian, string x)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/creditinfo");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "bearer " + x);
                JavaScriptSerializer java = new JavaScriptSerializer();
                request.AddJsonBody(
                new
                {
                    documentType = experian.documentType,
                    document = experian.document,
                    lastName = experian.lastName
                });

                var response = client.Post(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject(response.Content);

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// devuelve el token pra conectarse
        /// </summary>
        /// <param name="experian"></param>
        /// <returns></returns>
        public RootObject HtmlDataExperian(ExperianViewModels experian, string x)
        {
            RootObject ent = new RootObject();
            ent = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/creditinfohtml");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "bearer " + x);
                JavaScriptSerializer java = new JavaScriptSerializer();
                request.AddJsonBody(
                new
                {
                    document = experian.document,
                    documentType = experian.documentType,
                    lastName = experian.lastName
                });

                var response = client.Post(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //var entX= JsonConvert.SerializeObject(response.Content);
                    ent = JsonConvert.DeserializeObject<RootObject>(response.Content);

                    return ent;
                }
                return null;
            }
            catch (Exception ex)
            {
                return ent;
            }
        }

        /// <summary>
        /// devuelve el token pra conectarse
        /// </summary>
        /// <param name="experian"></param>
        /// <returns></returns>
        public string TokenDataExperian(ExperianViewModels experian)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/oauth/token");
                client.Authenticator = new HttpBasicAuthenticator("cardif_auth_prd", "vGr9wdf;YnT<eX");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                ServicePointManager.CheckCertificateRevocationList = false;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                JavaScriptSerializer java = new JavaScriptSerializer();

                request.AddParameter("password", experian.password, ParameterType.GetOrPost);
                request.AddParameter("username", experian.username, ParameterType.GetOrPost);
                request.AddParameter("grant_type", experian.grant_type, ParameterType.GetOrPost);

                var response = client.Post(request);

                var cadena = response.Content;
                var cadenaDeserializada = java.Deserialize<dynamic>(cadena);
                var token = cadenaDeserializada["access_token"].ToString();
                return token;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        /// devuelve la data de experian
        /// </summary>
        /// <param name="experian"></param>
        /// <returns></returns>
        public object IdentificacionExperian(string identificacion, string x)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/getUserNotifications");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "bearer " + x);
                JavaScriptSerializer java = new JavaScriptSerializer();
                request.AddJsonBody(
                new
                {
                    identification = identificacion
                });

                var response = client.Post(request);

                var result = JsonConvert.DeserializeObject(response.Content);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ingreso"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public object InsertaExperian(IngresoDataExperianViewModels ingreso, string x)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/saveUserNotifications");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "bearer " + x);
                JavaScriptSerializer java = new JavaScriptSerializer();

                string jsonToSend = java.Serialize(ingreso);

                request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                var response = client.Post(request);

                if (response.StatusCode.ToString() == "OK")
                {
                    var result = JsonConvert.DeserializeObject(response.Content);
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                return null;
            }
        }

        public async Task EnvioEmailBienvenida(int idUser)
        {
            try
            {
                var query = "[dbo].[pa_EnvioCorreoEcosistemas]";
                SqlParameter[] param = {
                    new SqlParameter("@idUsuario",idUser),
                    new SqlParameter("@TipoPlantilla",Convert.ToInt32(WebConfigurationManager.AppSettings["PlantillaBienvenida"].ToString())),
                };
                var tabla = await _ir.data(query, param);
                var registro = tabla.AsEnumerable().Select(z => new { Codigo = z[0].ToString(), Mensaje = z[1].ToString() }).FirstOrDefault();
                if (registro.Codigo == "1")
                {
                    var logError = new ErrorLog();
                    logError.errorMensaje = registro.Mensaje;
                    logError.evento = "Envio email bienvenida";
                    await _ir.Add(logError);
                }
            }
            catch (Exception ex)
            {
                var logError = new ErrorLog();
                logError.errorMensaje = ex.Message;
                logError.evento = "Envio email bienvenida";
                await _ir.Add(logError);
            }
        }

        public async Task EnvioEmail(int idUser, string html, string evento, int tipo, string fecha, string hora, int idAgendaTipo, string contacto)
        {
            try
            {
                var query = "[dbo].[pa_EnvioCorreoGenericoAgendamiento]";
                SqlParameter[] param = {new SqlParameter("@idUsuario",idUser),
                new SqlParameter("@plantilla",html),
                new SqlParameter("@esTipo ",tipo),
                new SqlParameter("@fecha",fecha),
                new SqlParameter("@hora",hora),
                new SqlParameter("@idAgendaTipo",idAgendaTipo),
                new SqlParameter("@contacto",contacto),
                        };
                var tabla = await _ir.data(query, param);
                var registro = tabla.AsEnumerable().Select(z => new { Codigo = z[0].ToString(), Mensaje = z[1].ToString() }).FirstOrDefault();
                if (registro.Codigo == "1")
                {
                    var logError = new ErrorLog();
                    logError.errorMensaje = registro.Mensaje;
                    logError.evento = evento + " envio email";
                    await _ir.Add(logError);
                }
            }
            catch (Exception ex)
            {
                var logError = new ErrorLog();
                logError.errorMensaje = ex.Message;
                logError.evento = evento + " envio email";
                await _ir.Add(logError);
            }
        }
        /// <summary>
        /// crear al usuario en instafit
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object CrearUserInstafit(UserInstafitViewModels user)
        {
            try
            {
                //var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/creditinfo");
                var client = new RestClient(WebConfigurationManager.AppSettings["Link_instafit"].ToString() + "v3.3/user");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", WebConfigurationManager.AppSettings["token_instafit"].ToString());
                request.AddHeader("Content-Type", "application/json");
                JavaScriptSerializer java = new JavaScriptSerializer();

                string jsonToSend = java.Serialize(user);

                request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                var response = client.Post(request);

                if (response.StatusCode.ToString() == "Created")
                {
                    var result = JsonConvert.DeserializeObject(response.Content);
                    return result;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject(response.Content);

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// crear al usuario en instafit
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string ValidarUserInstafit(UserInstafitViewModels user)
        {
            try
            {
                //VARIABLES DE WEBCONFIG
                var Bearer = WebConfigurationManager.AppSettings["token_instafit_validacion"].ToString();
                var link_instafit_base = WebConfigurationManager.AppSettings["Link_instafit_base"].ToString();
                var Cupon = WebConfigurationManager.AppSettings["Instafit_cupon"].ToString();

                //VARIABLES PARA EL API
                var finalToken = "";
                var uid = "";
                var tokenCupon = "";
                //var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/creditinfo");
                var client = new RestClient(link_instafit_base + "/api/v1/members/profile");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Bearer", Bearer);
                request.AddHeader("Content-Type", "application/json");
                JavaScriptSerializer java = new JavaScriptSerializer();
                //Create my object
                var email = new
                {
                    email = user.email
                };
                var member = new
                {
                    member = email
                };

                //Tranform it to Json object
                string jsonToSend = JsonConvert.SerializeObject(member);
                request.AddParameter("application/json;", jsonToSend, ParameterType.RequestBody);
                var response = client.Post(request);
                dynamic data = JsonConvert.DeserializeObject(response.Content);
                if (data["member"]["data"] != null)
                {
                    finalToken = data["member"]["data"]["attributes"]["authentication_token"];
                    uid = data["member"]["data"]["attributes"]["id"];
                }
                else
                {
                    finalToken = "";
                }

                if (finalToken == "")
                {
                    //var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/creditinfo");
                    client = new RestClient(link_instafit_base + "/api/v3.3/user");
                    request = new RestRequest(Method.POST);
                    request.AddHeader("Bearer", Bearer);
                    request.AddHeader("Content-Type", "application/json");
                    java = new JavaScriptSerializer();

                    jsonToSend = java.Serialize(user);

                    request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                    response = client.Post(request);

                    if (response.StatusCode.ToString() == "Created")
                    {
                        dynamic result = JsonConvert.DeserializeObject(response.Content);
                        uid = result["data"]["id"];
                        finalToken = result["data"]["authentication_token"];
                    }
                    else
                    {
                        finalToken = "";
                    }
                }
                if (finalToken != "")
                {
                    //GENERAR CUPON
                    client = new RestClient(link_instafit_base + "/api/v4.0/company/coupon/request");
                    request = new RestRequest(Method.POST);
                    request.AddHeader("Bearer", Bearer);
                    request.AddHeader("Content-Type", "application/json");
                    java = new JavaScriptSerializer();
                    var cuponcito = new
                    {
                        uid = uid,
                        coupon = Cupon,
                    };
                    jsonToSend = java.Serialize(cuponcito);
                    request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                    response = client.Post(request);
                    if (response.StatusCode.ToString() == "Created")
                    {
                        dynamic result = JsonConvert.DeserializeObject(response.Content);
                        tokenCupon = result["token"];
                    }
                    else
                    {
                        tokenCupon = "";
                    }
                }
                else
                {
                    tokenCupon = "";
                }
                if (tokenCupon != "" && finalToken != "")
                {
                    client = new RestClient(link_instafit_base + "/api/v3.3/user/subscription/coupon");
                    request = new RestRequest(Method.POST);
                    request.AddHeader("Bearer", Bearer);
                    request.AddHeader("uid", uid);
                    request.AddHeader("token", finalToken);
                    request.AddHeader("Content-Type", "application/json");
                    java = new JavaScriptSerializer();
                    var cuponcito = new
                    {
                        coupon_code = tokenCupon,
                        source = "WEB",
                    };
                    jsonToSend = java.Serialize(cuponcito);
                    request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                    response = client.Post(request);
                }
                else
                {
                    return "";
                }
                return link_instafit_base + "/user/week_plans?session_token=" + finalToken;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// Solicitud de token a instafit
        /// </summary>
        /// <param name="requestI"></param>
        /// <returns></returns>
        public object RequestInstafit(RequestInstafitViewModels requestI)
        {
            try
            {
                //var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/creditinfo");
                var client = new RestClient(WebConfigurationManager.AppSettings["Link_instafit"].ToString() + "v4.0/company/coupon/request");
                var request = new RestRequest(Method.POST);
                request.AddHeader("bearer", WebConfigurationManager.AppSettings["token_instafit"].ToString());
                request.AddHeader("Content-Type", "application/json");
                JavaScriptSerializer java = new JavaScriptSerializer();

                string jsonToSend = java.Serialize(requestI);

                request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                var response = client.Post(request);

                if (response.StatusCode.ToString() == "Created")
                {
                    var result = JsonConvert.DeserializeObject(response.Content);
                    return result;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject(response.Content);

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        public object SuscripcionInstafit(CouponInstafitViewModels co)
        {
            try
            {
                //var client = new RestClient(WebConfigurationManager.AppSettings["urlExperian"].ToString() + "/credithapigee-ws/creditinfo");
                var client = new RestClient(WebConfigurationManager.AppSettings["Link_instafit"].ToString() + "v3.3/user/subscription/coupon");
                var request = new RestRequest(Method.POST);
                request.AddHeader("bearer", WebConfigurationManager.AppSettings["token_instafit"].ToString());
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("uid", co.uid);
                request.AddHeader("token", co.token);
                JavaScriptSerializer java = new JavaScriptSerializer();

                request.AddJsonBody(
                new
                {
                    coupon_code = co.coupon_code,
                    source = co.source
                });

                var response = client.Post(request);

                if (response != null)
                {
                    var result = JsonConvert.DeserializeObject(response.Content);
                    return result;
                }
                //if (response.StatusCode = HttpStatusCode.OK)
                //{
                //    var result = JsonConvert.DeserializeObject(response.Content);

                //    return result;
                //}
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static bool AllwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
        /// <summary>
        /// validar al usuario en la api de cardif
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="documento"></param>
        /// <returns></returns>
        public Rootobject ValidaUser(string tipo, string documento)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls |
                                       SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                Rootobject myojb = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                //servicepointmanager.servercertificatevalidationcallback += new remotecertificatevalidationcallback(allwaysgoodcertificate);
                //servicepointManager.CheckCertificateRevocationList = false;

                var url = WebConfigurationManager.AppSettings["repositoryAddress"].ToString();
                var restolink = "/customers/v2/basic_info/" + WebConfigurationManager.AppSettings["IdCodigo"].ToString() + "?customerDocumentType=" + tipo + "&customerDocumentNumber=" + documento;
                WebRequest tRequest = WebRequest.Create(url + restolink);
                tRequest.Method = "get";



                //tRequest.Headers["serviceId"] = "BNPC0083";
                //tRequest.Headers["userId"] = "M1511";
                tRequest.Headers["serviceId"] = WebConfigurationManager.AppSettings["serviceId"].ToString();
                tRequest.Headers["userId"] = WebConfigurationManager.AppSettings["userId"].ToString();
                tRequest.Headers["Accept-Encoding"] = "gzip,deflate";

                using (var response = tRequest.GetResponse())

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();
                    myojb = (Rootobject)serializer.Deserialize(result, typeof(Rootobject));//capturamos la data desde la api de cardiff
                }
                return myojb;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Rootobject2 ValidaUserSpecial(string tipo, string documento)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls |
                                       SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                Rootobject2 myojb = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                //servicepointmanager.servercertificatevalidationcallback += new remotecertificatevalidationcallback(allwaysgoodcertificate);
                //servicepointManager.CheckCertificateRevocationList = false;

                var url = WebConfigurationManager.AppSettings["repositoryAddress"].ToString();
                var restolink = "/customers/v2/all_info/" + WebConfigurationManager.AppSettings["IdCodigo"].ToString() + "?customerDocumentType=" + tipo + "&customerDocumentNumber=" + documento;
                WebRequest tRequest = WebRequest.Create(url + restolink);
                tRequest.Method = "get";



                //tRequest.Headers["serviceId"] = "BNPC0083";
                //tRequest.Headers["userId"] = "M1511";
                tRequest.Headers["serviceId"] = WebConfigurationManager.AppSettings["serviceId"].ToString();
                tRequest.Headers["userId"] = WebConfigurationManager.AppSettings["userId"].ToString();
                tRequest.Headers["Accept-Encoding"] = "gzip,deflate";

                using (var response = tRequest.GetResponse())

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    //var result = "{ \"customerInformation\":    { \"customerData\":       { \"customerRole\": \"A\", \"customerPersonalInformation\":          { \"customerDocumentType\": \"CC\", \"customerDocumentNumber\": \"123456789\", \"customerFullName\": \"MARIA FERNANDA CARDENAS\", \"customerFirstName\": \"MARIA\", \"customerMiddleName\": \"FERNANDA\", \"customerLastName\": \"CARDENAS\", \"customerGender\": F\", \"customerBirthDate\": \"1988-01-10\", \"customerBirthPlaceName\": \"CO\", \"customerDeathDate\": \"\", \"customerNationalityOrigin\": \"\", \"customerChildren\": \"\", \"customerPets\": \"\", \"customerShareInformation\": \"\", \"customerContactCellphone\": \"X\", \"customerContactMail\": \"X\", \"customerIncome\": \"\", \"customerEconomicActivity\": \"\", \"customerMaritalStatus\": \"S\" }, \"customerContactData\":          { \"address\":             { \"customerAddress\": \"CL 1 1 A 2 \", \"customerAddressCityId\": \"001\", \"customerAddressCity\": \"BOGOTA, D.C.\", \"customerAddressDeparmentId\": \"11\", \"customerAddressDeparment\": \"BOGOTA\", \"customerAddressCountryId\": \"\", \"customerAddressCountry\": \"\" }, \"email\": {\"customerEmail\": \"PRUEBA123@HOTMAIL.COM\"}, \"phone\":             { \"customerMobile1\": \"\", \"customerMobile2\": \"571234567894\", \"customerPhoneCode1\": \"5\", \"customerPhoneNumber1\": \"1234567\", \"customerExtension1\": \"\", \"customerPhoneCode2\": \"\", \"customerPhoneNumber2\": \"\", \"customerExtension2\": \"\" } } }, \"policy\":       [ { \"cardifPolicyNumber\": \"122222222222222222\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-25\", \"cardifPolicyEndDate\": \"2020-05-25\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 213, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 24785, \"cardifPolicyGrossPremiumAmnt\": 24785, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6606\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6606, \"cardifProductName\": \"6606-Scotiabank Colpatria S.A.-MigraciÃ³n-Accidentes Personales-Anual-Multiprima-\", \"cardifProductComercialName\": \"Accidentes Personales\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"202\", \"coverageName\": \"HospitalizaciÃ³n Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 99800, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"163\", \"coverageName\": \"Incapacidad Total y Permanente por Accidente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"716\", \"coverageName\": \"Muerte accidental y desmembramiento (DesmembraciÃ³n)\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 7, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-05-25\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 7, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-25\", \"lastBillsDate\": \"2020-05-25\", \"efectiveBills\": 7, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"11111111111111111111\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-08\", \"cardifPolicyEndDate\": \"2020-07-08\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 274, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 34566, \"cardifPolicyGrossPremiumAmnt\": 34566, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6603\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6603, \"cardifProductName\": \"6603-Scotiabank Colpatria S.A.-MigraciÃ³n-Vida-Anual-Multiprima-Hall\", \"cardifProductComercialName\": \"Vida\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"160\", \"coverageName\": \"Incapacidad Total Permanente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"157\", \"coverageName\": \"Muerte\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"716\", \"coverageName\": \"Muerte accidental y desmembramiento (DesmembraciÃ³n)\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"277\", \"coverageName\": \"Gastos Funerarios\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 5000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"367\", \"coverageName\": \"Muerte Accidental en Transporte PÃºblico\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 9, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-07-08\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 9, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-08\", \"lastBillsDate\": \"2020-07-08\", \"efectiveBills\": 9, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"33333333333333333\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-28\", \"cardifPolicyEndDate\": \"2020-05-28\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 213, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 33946, \"cardifPolicyGrossPremiumAmnt\": 33946, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6603\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6603, \"cardifProductName\": \"6603-Scotiabank Colpatria S.A.-MigraciÃ³n-Vida-Anual-Multiprima-Hall\", \"cardifProductComercialName\": \"Vida\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"160\", \"coverageName\": \"Incapacidad Total Permanente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"157\", \"coverageName\": \"Muerte\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"716\", \"coverageName\": \"Muerte accidental y desmembramiento (DesmembraciÃ³n)\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"277\", \"coverageName\": \"Gastos Funerarios\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 5000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"367\", \"coverageName\": \"Muerte Accidental en Transporte PÃºblico\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 7, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-05-28\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 7, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-28\", \"lastBillsDate\": \"2020-05-28\", \"efectiveBills\": 7, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"67777777777777777\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-10\", \"cardifPolicyEndDate\": \"2020-05-09\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 212, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 6610, \"cardifPolicyGrossPremiumAmnt\": 6610, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6605\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6605, \"cardifProductName\": \"6605-Scotiabank Colpatria S.A.-MigraciÃ³n-Vida-Anual-Multiprima-Hall\", \"cardifProductComercialName\": \"Vida\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"157\", \"coverageName\": \"Muerte\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 30000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"160\", \"coverageName\": \"Incapacidad Total Permanente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 30000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"367\", \"coverageName\": \"Muerte Accidental en Transporte PÃºblico\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 30000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 6, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-05-09\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 7, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-10\", \"lastBillsDate\": \"2020-05-09\", \"efectiveBills\": 6, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"155555555555555555\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-07\", \"cardifPolicyEndDate\": \"2020-12-07\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 427, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 22015, \"cardifPolicyGrossPremiumAmnt\": 22015, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6606\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6606, \"cardifProductName\": \"6606-Scotiabank Colpatria S.A.-MigraciÃ³n-Accidentes Personales-Anual-Multiprima-\", \"cardifProductComercialName\": \"Accidentes Personales\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"202\", \"coverageName\": \"HospitalizaciÃ³n Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 99800, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"163\", \"coverageName\": \"Incapacidad Total y Permanente por Accidente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"716\", \"coverageName\": \"Muerte accidental y desmembramiento (DesmembraciÃ³n)\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 14, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-12-07\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 14, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-07\", \"lastBillsDate\": \"2020-12-07\", \"efectiveBills\": 14, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"1544444444444444444\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-27\", \"cardifPolicyEndDate\": \"2020-05-26\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 212, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 25186, \"cardifPolicyGrossPremiumAmnt\": 25186, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6606\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6606, \"cardifProductName\": \"6606-Scotiabank Colpatria S.A.-MigraciÃ³n-Accidentes Personales-Anual-Multiprima-\", \"cardifProductComercialName\": \"Accidentes Personales\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"202\", \"coverageName\": \"HospitalizaciÃ³n Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 99800, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"163\", \"coverageName\": \"Incapacidad Total y Permanente por Accidente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"716\", \"coverageName\": \"Muerte accidental y desmembramiento (DesmembraciÃ³n)\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 6, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-05-26\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 7, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-27\", \"lastBillsDate\": \"2020-05-26\", \"efectiveBills\": 6, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"15547777777777777\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-09\", \"cardifPolicyEndDate\": \"2020-05-08\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 212, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 21994, \"cardifPolicyGrossPremiumAmnt\": 21994, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6606\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6606, \"cardifProductName\": \"6606-Scotiabank Colpatria S.A.-MigraciÃ³n-Accidentes Personales-Anual-Multiprima-\", \"cardifProductComercialName\": \"Accidentes Personales\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"202\", \"coverageName\": \"HospitalizaciÃ³n Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 99800, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"163\", \"coverageName\": \"Incapacidad Total y Permanente por Accidente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"716\", \"coverageName\": \"Muerte accidental y desmembramiento (DesmembraciÃ³n)\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 6, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-05-08\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 7, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-09\", \"lastBillsDate\": \"2020-05-08\", \"efectiveBills\": 6, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"1550000000000000000\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-07\", \"cardifPolicyEndDate\": \"2020-12-07\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 427, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 22015, \"cardifPolicyGrossPremiumAmnt\": 22015, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6606\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6606, \"cardifProductName\": \"6606-Scotiabank Colpatria S.A.-MigraciÃ³n-Accidentes Personales-Anual-Multiprima-\", \"cardifProductComercialName\": \"Accidentes Personales\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"202\", \"coverageName\": \"HospitalizaciÃ³n Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 99800, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"163\", \"coverageName\": \"Incapacidad Total y Permanente por Accidente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"716\", \"coverageName\": \"Muerte accidental y desmembramiento (DesmembraciÃ³n)\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 14, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-12-07\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 14, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-07\", \"lastBillsDate\": \"2020-12-07\", \"efectiveBills\": 14, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"155488888888888888\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-09\", \"cardifPolicyEndDate\": \"2020-05-08\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 212, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 21994, \"cardifPolicyGrossPremiumAmnt\": 21994, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6606\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6606, \"cardifProductName\": \"6606-Scotiabank Colpatria S.A.-MigraciÃ³n-Accidentes Personales-Anual-Multiprima-\", \"cardifProductComercialName\": \"Accidentes Personales\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"202\", \"coverageName\": \"HospitalizaciÃ³n Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 99800, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"163\", \"coverageName\": \"Incapacidad Total y Permanente por Accidente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"716\", \"coverageName\": \"Muerte accidental y desmembramiento (DesmembraciÃ³n)\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 150000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 6, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-05-08\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 7, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-09\", \"lastBillsDate\": \"2020-05-08\", \"efectiveBills\": 6, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"13999999999999999\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-11\", \"cardifPolicyEndDate\": \"2020-08-11\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 305, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 31232, \"cardifPolicyGrossPremiumAmnt\": 31232, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6603\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6603, \"cardifProductName\": \"6603-Scotiabank Colpatria S.A.-MigraciÃ³n-Vida-Anual-Multiprima-Hall\", \"cardifProductComercialName\": \"Vida\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"160\", \"coverageName\": \"Incapacidad Total Permanente\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"157\", \"coverageName\": \"Muerte\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"716\", \"coverageName\": \"Muerte accidental y desmembramiento (DesmembraciÃ³n)\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"277\", \"coverageName\": \"Gastos Funerarios\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 5000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"367\", \"coverageName\": \"Muerte Accidental en Transporte PÃºblico\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"34\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 12, \"coverageMaxAge\": 74, \"insuredValue\": 0, \"maxInsuredValue\": 50000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 10, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-08-11\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 10, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2020-04-11\", \"lastBillsDate\": \"2020-08-11\", \"efectiveBills\": 10, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"73888888888888888\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-10-05\", \"cardifPolicyEndDate\": \"2020-09-05\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 336, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"2\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 9847, \"cardifPolicyGrossPremiumAmnt\": 9847, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6619\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6619, \"cardifProductName\": \"6619-Scotiabank Colpatria S.A.-MigraciÃ³n-Accidentes Personales-Anual-Multiprima-\", \"cardifProductComercialName\": \"Accidentes Personales\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 1825, \"prescriptionYears\": 5, \"coverageMinAge\": 18, \"coverageMaxAge\": 71, \"insuredValue\": 0, \"maxInsuredValue\": 1970000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"162\", \"coverageName\": \"Desempleo\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 71, \"insuredValue\": 0, \"maxInsuredValue\": 1970000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"197\", \"coverageName\": \"Incapacidad Total Temporal\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 71, \"insuredValue\": 0, \"maxInsuredValue\": 1970000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 11, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-09-05\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 11, \"quoteValue\": 0, \"billingPeriodicity\": null, \"firstBillsDate\": \"2020-04-05\", \"lastBillsDate\": \"2020-09-05\", \"efectiveBills\": 11, \"ipc\": 0, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } }, { \"cardifPolicyNumber\": \"949822222222222222\", \"cardifPolicyCountryISO\": \"COP\", \"cardifPolicyCountryName\": \"\", \"cardifpolicyStartDate\": \"2019-09-22\", \"cardifPolicyEndDate\": \"2020-09-22\", \"cardifPolicySubscriptionDate\": \"2019-11-04\", \"cardifPolicyEffectiveCancellationDate\": \"\", \"cardifPolicyApplyCancellationDate\": \"\", \"cardifPolicyRequestCancellationDate\": \"\", \"cardifPolicyCancellationReason\": \"\", \"cardifPolicyDurationDays\": 366, \"policyStatusCode\": \"V\", \"vigilanceDesc\": \"\", \"migrationProductDate\": \"\", \"source\": \"COBRA\", \"idUpload\": 0, \"vigilanceList\": \"\", \"fileUploadName\": \"\", \"policyBusinessLineCodeCardif\": \"\", \"loanAmount\": 0, \"loanAmountCurrencyISO\": \"COP\", \"cardifPolicyNetPremiumAmnt\": 369341, \"cardifPolicyGrossPremiumAmnt\": 434476, \"cardifPolicyPremiumCurrencyISO\": \"COP\", \"partnerPolicyInfo\":             { \"partnerPolicyNumber\": \"\", \"partnerPolicyProdCode\": \"6628\", \"partnerTransactionNumber\": \"\", \"partnerApprovalNumber\": \"\", \"partnerContractNumber\": \"\", \"partnerRate\": \"\", \"partnerInsuranceType\": \"\", \"partnerPolicyMovement\": \"\", \"partnerPremiumType\": \"\", \"cardifPolicySubscriptionDate\": null, \"cardifPolicyTransationTime\": \"00:00:00\", \"cardifPolicyTransationAmount\": \"\", \"cardifPolicyTransactionCurrencyISO\": \"COP\" }, \"mortgageInfo\":             { \"mortgageCountryCode\": \"COP\", \"mortgageCountryName\": \"\", \"mortgageStateCode\": \"\", \"mortgageStateName\": \"\", \"mortgageCityCode\": \"\", \"mortgageCityName\": \"\", \"mortgageCityZone\": \"\", \"mortgageZoneType\": \"\", \"mortgageAddress\": \"\" }, \"product\":             { \"cardifProductId\": 6628, \"cardifProductName\": \"6628-Scotiabank Colpatria S.A.-MigraciÃ³n-Fraude-Anual-Multiprima-Hall\", \"cardifProductComercialName\": \"Fraude\", \"productDeudorIndic\": null, \"branch\": null, \"plan\": [               { \"cardifPlanId\": 1, \"cardifPlanName\": \"Plan 1\", \"coverage\":                   [ { \"coverageId\": \"199\", \"coverageName\": \"Uso Fraudulento\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 1029000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"200\", \"coverageName\": \"Bolso Protegido\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 3, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 80, \"insuredValue\": 0, \"maxInsuredValue\": 550000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"337\", \"coverageName\": \"Robo\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 3, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 80, \"insuredValue\": 0, \"maxInsuredValue\": 1000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"400\", \"coverageName\": \"Fraude Internet\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 1000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"159\", \"coverageName\": \"Muerte Accidental\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 51420000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"397\", \"coverageName\": \"UtilizaciÃ³n Forzada de las Tarjetas DÃ©bito y/o Chequera\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 1029000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"398\", \"coverageName\": \"FalsificaciÃ³n\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 1029000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"860\", \"coverageName\": \"Compra Protegida DaÃ±o\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 515000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"517\", \"coverageName\": \"GarantÃ­a Extendida\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 1000000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"360\", \"coverageName\": \"Gastos MÃ©dicos\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"31\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 5142000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"497\", \"coverageName\": \"ReposiciÃ³n de Documentos \", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 515000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"639\", \"coverageName\": \"Hurto por Avance Theftby\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 1029000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"717\", \"coverageName\": \"Hurto en Cajero ElectrÃ³nico\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 1029000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"718\", \"coverageName\": \"ReposiciÃ³n de Llaves \", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 515000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true }, { \"coverageId\": \"824\", \"coverageName\": \"ImpresiÃ³n de Vales MÃºltiples\", \"coverageType\": \"0\", \"coverageBeginDate\": \"\", \"coverageBusinessLineCodeCardif\": \"9\", \"coverageMaxIncapacity\": 0, \"coverageMinIncapacity\": 0, \"coverageTimeBlocking\": 0, \"coverageTime\": 0, \"coverageTimeClaim\": 0, \"coverageLoanInstallmentAmount\": 0, \"coverageMaxClaimInstallment\": 0, \"coverageMaxClaimByYear\": 0, \"waitingPeriod\": \"0\", \"lackPeriod\": \"0\", \"prescriptionDays\": 730, \"prescriptionYears\": 2, \"coverageMinAge\": 18, \"coverageMaxAge\": 99, \"insuredValue\": 0, \"maxInsuredValue\": 1029000, \"insuredValueCurrencyISO\": \"COP\", \"continuityJob\": \"\", \"continuousWorkingDays\": 0, \"workingPermanencyDays\": 0, \"eventLimitFlag\": true, \"gender\": \"Male\", \"smokerFlag\": true, \"rentFlag\": true } ] }] }, \"policyDetails\":             { \"generalDetails\":                { \"statusDue\": \"\", \"monthPastDue\": 12, \"policyCondicionalVersion\": \"0 \", \"policyLastPremBillingDate\": \"2020-09-22\", \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionStatus\": \"\" }, \"creditCard\":                { \"creditCardSoldDate\": \"\", \"creditCardSoldAmnt\": 0, \"policyTypePremPartnrType\": \"\" } }, \"billing\":             { \"elapsedTimeOnMonths\": 12, \"quoteValue\": 0, \"billingPeriodicity\": \"0\", \"firstBillsDate\": \"2019-09-22\", \"lastBillsDate\": \"2020-09-22\", \"efectiveBills\": 12, \"ipc\": 65135, \"payerFullName\": \"MARIA FERNANDA CARDENAS\", \"billingCurrencyISO\": \"COP\", \"payment\":                { \"lastCollectionEffectiveDate\": \"\", \"lastCollectionAttemptDate\": \"\", \"lastCollectionEndDate\": \"\", \"lastCollectionState\": null }, \"financial\":                { \"companyCardType\": \"\", \"cardType\": \"\", \"cardNumber\": \"\" }, \"reversion\": [               { \"reverseBills\": 0, \"calcultdRefundRefDateField\": \"\", \"reasonDescField\": \"\", \"sentReversionDate\": \"\", \"confirmReversionDate\": \"\", \"referenceReversion\": \"\", \"refundEffectiveDate\": \"\" }] }, \"assesor\":             { \"assessorId\": \"\", \"assessorName\": \"\", \"assessorLastName\": \"\", \"assessorDocumentType\": \"\", \"assessorDocumentNumber\": \"\" }, \"partnerInfo\":             { \"partnerId\": 80, \"partnerName\": \"ScotiaBank\", \"partnerDocumentType\": \"\", \"partnerDocumentNumber\": \"\", \"subPartnerId\": \"1\", \"subPartnerName\": \"Scotiabank\", \"channel\": \"2\", \"salesChannelId\": \"\", \"partnerSubsidiaryId\": \"\", \"partnerSubsidiaryName\": \"\", \"partnerOfficeId\": null, \"partnerOfficeName\": \"\", \"partnerRed\": \"\", \"partnerOfficeZone\": \"\" } } ] }, \"statusResponse\":    { \"status\": \"200\", \"description\": \"Success\" } }";
                    var result = reader.ReadToEnd();
                    result = "{\"clienteAsegurado\": " + result.ToString() + "}";

                    myojb = (Rootobject2)serializer.Deserialize(result, typeof(Rootobject2));//capturamos la data desde la api de cardiff
                }
                return myojb;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// codificar en base 64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Base64_Encode(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        public string encriptar256(string frase)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(frase));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string encryptDecrypt(string input, string keyPass)
        {
                byte[] password = Convert.FromBase64String(input);
                keyPass = keyPass.Substring(keyPass.Length - 10, 10); //
                char[] key = keyPass.ToCharArray(); //
                char[] output = new char[password.Length];

                for (int i = 0; i < password.Length; i++)
                {
                    output[i] = (char)(password[i] ^ key[i % key.Length]);
                }
                return new string(output);
            
        }

        /// <summary>
        /// decodificar base 64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Base64_Decode(string str)
        {
            try
            {
                byte[] decbuff = Convert.FromBase64String(str);
                return System.Text.Encoding.UTF8.GetString(decbuff);
            }
            catch
            {
                return "";//si se envia una cadena si codificación base64, mandamos vacio
            }
        }

        /// <summary>
        /// metodo para conectarse a insertar en mocksys y trae la ot
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="idTipoAgenda"></param>
        /// <returns></returns>
        public async Task<DataTable> TraeMocksys(int idUser, int idTipoAgenda, DateTime fechaAgenda,int idContrato, string observacion)
        {
            var user = await _ir.Find<Usuario>(idUser);//obtener el usuario
            var tipoAgenda = await _ir.Find<AgendaTipo>(idTipoAgenda);//obtener el tipo de agenda
            if (user != null && tipoAgenda != null)
            {
                var query = "[ecosistema].[pa_CrearCasoEcosistema]";
                SqlParameter[] parameters =
                             {
                new SqlParameter("@idPais", 3),
                new SqlParameter("@rut", user.identificacion),
                new SqlParameter("@nombreCliente", user.nombres),
                new SqlParameter("@apellidoPaterno", user.apellidoPaterno),
                new SqlParameter("@apellidoMaterno", user.apellidoMaterno),
                new SqlParameter("@idRequerimiento", tipoAgenda.idRequerimientoMoksys),
                new SqlParameter("@idContrato", idContrato),
                new SqlParameter("@idServicio",tipoAgenda.idServicioMoksys),
                new SqlParameter("@idTipoServicio", tipoAgenda.idTipoServicioMoksys),
                new SqlParameter("@observacion", observacion),
                new SqlParameter("@fonoContacto", user.numeroCelular),
                new SqlParameter("@correoContacto", user.correoElectronico),
                new SqlParameter("@fechaAgendamiento", fechaAgenda)
                     };
                var dt = await _ir.MocksysGuardar(query, parameters);//obtenemos el datatable
                return dt;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// anula servicio de una ot de mocksys
        /// </summary>
        /// <param name="ot"></param>
        /// <returns></returns>
        public async Task<string> EliminaMocksys(int ot)
        {
            try
            {
                var query = "[ecosistema].[pa_AnularCasoEcosistema]";
                SqlParameter[] parameters =
                             {
                new SqlParameter("@idOT", ot)
                     };
                var dt = await _ir.MocksysGuardar(query, parameters);//obtenemos el datatable
                var xs = dt.Rows[0].ItemArray[0].ToString() + "-" + dt.Rows[0].ItemArray[1].ToString();
                return xs;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// metodo que prepara el envio del sms
        /// </summary>
        /// <param name="dicAgenda"></param>
        /// <param name="discriminador"></param>
        /// <param name="contacto"></param>
        /// <param name="codigoSms"></param>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <returns></returns>
        public async Task<string> EnvioSmsAnterior(string dicAgenda, string discriminador, string contacto, int codigoSms, string fecha, string hora)
        {
            //1 confirmacion de agendamiento, 2 eliminacionde agenda 
            string cadena = "";
            int idSms = 0;
            //confirmacion de agendamiento
            if (dicAgenda == "HojaVida" && codigoSms == 1)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaRevision"].ToString());
            }
            else if (dicAgenda == "Entrevista" && codigoSms == 1)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaEntrenamiento"].ToString());
            }
            else if (dicAgenda == "Legal" && codigoSms == 1)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaAsesoria"].ToString());
            }
            else if (dicAgenda == "Legal ITT" && codigoSms == 1)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaLegalItt"].ToString());
            }
            else if (dicAgenda == "Coworking" && codigoSms == 1)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaCoworking"].ToString());
            }
            else if (dicAgenda == "Branding" && codigoSms == 1)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaBranding"].ToString());
            }else if (dicAgenda == "Ayuda" && codigoSms == 1)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdAgregaAyuda"].ToString());
            }

            //eliminacion de agendamiento
            else if (dicAgenda == "HojaVida" && codigoSms == 2)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaRevision"].ToString());
            }
            else if (dicAgenda == "Entrevista" && codigoSms == 2)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaEntrenamiento"].ToString());
            }
            else if (dicAgenda == "Legal" && codigoSms == 2)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaAsesoria"].ToString());
            }
            else if (dicAgenda == "Legal ITT" && codigoSms == 2)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaLegalItt"].ToString());
            }
            else if (dicAgenda == "Coworking" && codigoSms == 2)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaCoworking"].ToString());
            }
            else if (dicAgenda == "Branding" && codigoSms == 2)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaBranding"].ToString());
            }
            else if (dicAgenda == "Ayuda" && codigoSms == 2)
            {
                idSms = Convert.ToInt32(WebConfigurationManager.AppSettings["IdEliminaAyuda"].ToString());
            }
            var mensaje = await _ir.Find<Mensaje>(idSms);//obtener el cuerpo del mensaje
            var parametrosTelefono = await _ir.Find<Parametros>(8);//obtener el codigo del pais
            var codigoPais = parametrosTelefono.parametroJSON;

            if (mensaje != null)
            {
                //0 y 1 telefono, 2 email para discriminador
                if (discriminador != "2" && codigoSms == 2)
                {
                    mensaje.textoMensaje = mensaje.textoMensaje.Replace("@link", WebConfigurationManager.AppSettings["LinkAgendaEliminar"].ToString());
                    var telefonocortado = "";
                    if (contacto.Length > 10)
                    {
                        telefonocortado = contacto.Substring(contacto.Length - 10, 10);
                    }
                    else
                    {
                        telefonocortado = contacto;
                    }
                    string telefono = codigoPais + telefonocortado;
                    var cadenatexto = "{\"messages\":[{ \"destination\":" + telefono + ",\"messageText\":\"" + mensaje.textoMensaje + "\"}]}";
                    cadena = await EnvioSms(telefono, cadenatexto);
                }
                else if (discriminador != "2" && codigoSms == 1)
                {
                    var telefonocortado = "";
                    if (contacto.Length > 10)
                    {
                        telefonocortado = contacto.Substring(contacto.Length - 10, 10);
                    }
                    else
                    {
                        telefonocortado = contacto;
                    }
                    var mensajeFinal = mensaje.textoMensaje.Replace("@fecha", fecha).Replace("@hora", hora);
                    string telefono = codigoPais + telefonocortado;//juntamos lod¿s datos del codigo de pais y del telefono
                    var cadenatexto = "{\"messages\":[{ \"destination\":" + telefono + ",\"messageText\":\"" + mensajeFinal + "\"}]}";//datos que se envia en el post 
                                                                                                                                      //del sms
                    cadena = await EnvioSms(telefono, cadenatexto);
                }
            }
            return cadena;
        }


        public object SentinelDataEvalInfo(DataSentinelViewModels dataSentinel)
        {
            try
            {
                var client = new RestClient("https://www2.sentinelperu.com/wsrest/rest/RWS_EvalInfoDet");
                //client.Authenticator = new HttpBasicAuthenticator("cliente", "password");
                //const string sendResource = "/oauth/token";
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                JavaScriptSerializer java = new JavaScriptSerializer();

                request.AddJsonBody(
                new
                {
                    Gx_UsuEnc = dataSentinel.Gx_UsuEnc,
                    Gx_PasEnc = dataSentinel.Gx_PasEnc,
                    Gx_Key = dataSentinel.Gx_Key,
                    TipoDoc = dataSentinel.TipoDoc,
                    NroDoc = dataSentinel.NroDoc
                });

                //string jsonToSend = java.Serialize(experian);
                //request.AddParameter("documentType", experian.documentType, ParameterType.RequestBody);
                //request.AddParameter("document", experian.document, ParameterType.RequestBody);
                //request.AddParameter("lastName", experian.lastName, ParameterType.RequestBody);

                //request.AddParameter("application/x-www-form-urlencoded", experian, ParameterType.RequestBody);
                //request.RequestFormat = DataFormat.Xml;
                //request.RequestFormat = DataFormat.Json;
                var response = client.Post(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject(response.Content);
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
    public class token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }

}
