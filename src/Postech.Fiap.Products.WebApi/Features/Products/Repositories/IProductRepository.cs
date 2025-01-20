using Postech.Fiap.Products.WebApi.Features.Products.Entities;

namespace PosTech.Fiap.Products.WebApi.Features.Products.Repositories;

public interface IProductRepository
{
    Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Product?>> FindByCategoryAsync(ProductCategory? category, CancellationToken cancellationToken);
    Task<Product?> CreateAsync(Product? product, CancellationToken cancellationToken);
    Task<Product?> UpdateAsync(Product? product, CancellationToken cancellationToken);
    Task DeleteAsync(Product? product, CancellationToken cancellationToken);
}