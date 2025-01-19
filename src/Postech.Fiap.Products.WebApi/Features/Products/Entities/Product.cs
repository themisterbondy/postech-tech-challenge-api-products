namespace Postech.Fiap.Products.WebApi.Features.Products.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Postech.Fiap.Products.WebApi.Features.Products.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("category")]
        public ProductCategory Category { get; set; }

        [BsonElement("imageUrl")]
        public string? ImageUrl { get; set; }

        private Product(ProductId id, string name, string description, decimal price, ProductCategory category, string? imageUrl)
        {
            Id = id.ToString();
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            ImageUrl = imageUrl;
        }

        private Product() { }

        public static Product? Create(ProductId id, string name, string description, decimal price, ProductCategory category, string? imageUrl)
        {
            return new Product(id, name, description, price, category, imageUrl);
        }
    }
}
