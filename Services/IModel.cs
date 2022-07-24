using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SendMe.Services
{
    public interface IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? Id { get; set; }
    }
}
