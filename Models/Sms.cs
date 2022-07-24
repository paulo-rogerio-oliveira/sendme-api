using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SendMe.Services;
using System.ComponentModel.DataAnnotations;


namespace SendMe.Models
{
    public class Sms: IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? Id { get; set; }

        [Required]
        public String? ApplicationName { get; set; }

        [Required]
        public String? ProviderName { get; set; }

    }
}
