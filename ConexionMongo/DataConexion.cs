using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace EcoOplacementApi.ConexionMongo
{
    public class DataConexion
    {
        public IMongoDatabase database;
        public DataConexion()
        {
            //MongoClient mc = new MongoClient("mongodb://172.27.1.34");
            ////MongoClient mc = new MongoClient("mongodb://superAdmin:pass1234@127.0.0.1");

            //database = mc.GetDatabase(ConfigurationManager.AppSettings["MongoDBName"]);

            MongoClient mc = new MongoClient(WebConfigurationManager.AppSettings["MongoConexion"].ToString());

            database = mc.GetDatabase(ConfigurationManager.AppSettings["MongoDBName"]);
        }
    }
}