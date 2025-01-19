using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Postech.Fiap.Products.WebApi.Features.Products.Entities;

public record ProductId(Guid Value)
{
    public static ProductId New()
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        return new ProductId(Guid.NewGuid());
    }
}