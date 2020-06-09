using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoOplacementApi.ViewModels
{
    public class MongoViewModels
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("idUsuario")]
        public int idUsuario { get; set; }
        [BsonElement("ciudad_localizacion")]
        public string ciudad_localizacion { get; set; }
        
        [BsonElement("fechaServidor")]
        public string fechaServidor { get; set; }
        [BsonElement("horaServidor")]
        public string horaServidor { get; set; }

        [BsonElement("fechaLocal")]
        public string fechaLocal { get; set; }
        [BsonElement("horaLocal")]
        public string horaLocal { get; set; }
        [BsonElement("direccion_ip")]
        public string direccion_ip { get; set; }

        [BsonElement("pagina")]
        public string pagina { get; set; }
        [BsonElement("control")]
        public string control { get; set; }
        [BsonElement("extra")]
        public string extra { get; set; }
    }
}
