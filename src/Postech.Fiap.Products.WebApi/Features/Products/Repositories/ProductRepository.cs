using MongoDB.Driver;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;

namespace PosTech.MyFood.WebApi.Features.Products.Repositories;

public class ProductRepository(IMongoDatabase database) : IProductRepository
{
    private readonly IMongoCollection<Product> _productsCollection = database.GetCollection<Product>("Products");

    public async Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _productsCollection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Product?>> FindByCategoryAsync(ProductCategory? category,
        CancellationToken cancellationToken)
    {
        if (category.HasValue)
            return await _productsCollection
                .Find(x => x.Category == category)
                .ToListAsync(cancellationToken);

        return await _productsCollection
            .Find(_ => true) // Retorna todos os produtos caso nenhuma categoria seja especificada.
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> CreateAsync(Product? product, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);

        await _productsCollection.InsertOneAsync(product, cancellationToken: cancellationToken);
        return product;
    }

    public async Task<Product?> UpdateAsync(Product? product, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);

        var result = await _productsCollection.ReplaceOneAsync(
            x => x.Id == product.Id,
            product,
            new ReplaceOptions(),
            cancellationToken
        );

        if (result.IsAcknowledged && result.ModifiedCount > 0) return product;

        return null; // Retorna null caso a atualização não seja aplicada.
    }

    public async Task DeleteAsync(Product? product, CancellationToken cancellationToken)
    {
        if (product is null) throw new ArgumentNullException(nameof(product));

        await _productsCollection.DeleteOneAsync(x => x.Id == product.Id, cancellationToken);
    }
}