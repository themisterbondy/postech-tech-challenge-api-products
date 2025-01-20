using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Postech.Fiap.Products.WebApi.Features.Products.Entities
{
    public class Product
    {
        private Product(Guid id, string name, string description, decimal price, ProductCategory category,
            string? imageUrl)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            ImageUrl = imageUrl;
        }

        private Product()
        {
        }

        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }

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

        public static Product? Create(Guid id, string name, string description, decimal price, ProductCategory category,
            string? imageUrl)
        {
            return new Product(id, name, description, price, category, imageUrl);
        }
    }
}