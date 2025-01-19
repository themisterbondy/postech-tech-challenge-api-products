using PosTech.Fiap.Products.WebApi.Features.Products.Entities;

namespace Postech.Fiap.Products.WebApi.Features.Products.Contracts;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; }
    public string? ImageUrl { get; set; }
}