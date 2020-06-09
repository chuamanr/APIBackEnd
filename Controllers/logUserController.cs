using EcoOplacementApi.ConexionMongo;
using EcoOplacementApi.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class logUserController : ApiController
    {
        private DataConexion _db;
        private IMongoCollection<MongoViewModels> mongoCollection;

        /// <summary>
        /// inyectamos el objeto para el constructor
        /// </summary>
        public logUserController()
        {
            _db = new DataConexion();
            mongoCollection = _db.database.GetCollection<MongoViewModels>("logUser");
        }
        /// <summary>
        /// listo todos los registros de la base de mongo
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("api/logUser/log")]
        //public IHttpActionResult Log()
        //{
        //    try
        //    {
        //        List<MongoViewModels> logs = mongoCollection.AsQueryable<MongoViewModels>().ToList();
        //        return Ok(logs);
        //    }
        //    catch (Exception ex)
        //    {
        //        string mensaje;
        //        mensaje = ex.Message;
        //        if (ex.InnerException != null) {
        //            mensaje = mensaje + ", " +  ex.InnerException.Message;
        //        }
        //        return BadRequest(mensaje);
        //    }
        //}
        /// <summary>
        /// inserta el log en la base de mongo db
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/logUser/inserta")]
        public IHttpActionResult InsertarLog(MongoViewModels log)
        {
            try
            {
                log.fechaServidor = DateTime.Now.ToString("dd-MM-yyyy");
                log.horaServidor = DateTime.Now.ToShortTimeString();
                mongoCollection.InsertOne(log);
                return Ok();
            }
            catch (Exception ex)
            {
                string mensaje;
                mensaje = ex.Message;
                if (ex.InnerException != null)
                {
                    mensaje = mensaje + ", " + ex.InnerException.Message;
                }
                return BadRequest(mensaje);
            }
            
        }
    }
}
