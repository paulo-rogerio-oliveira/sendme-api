using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SendMe.Services;

namespace SendMe.Models
{
    public class AuthUser : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? Id { get; set; }
        public string? Occupation { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
    }
}
