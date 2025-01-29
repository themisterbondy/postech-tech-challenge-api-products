using Bogus;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;

namespace Postech.Fiap.Products.WebApi.UnitTest.Features.Mocks;

public static class ProductMocks
{
    public static Product GenerateValidProduct()
    {
        var faker = new Faker();
        var productId = new ProductId(faker.Random.Guid());
        var name = faker.Commerce.ProductName();
        var description = faker.Commerce.ProductDescription();
        var price = faker.Random.Decimal(1, 1000);
        var category = ProductCategory.Lanche; // Assumindo que você tem um enum chamado ProductCategory
        var imageUrl = faker.Internet.Url();

        return Product.Create(Guid.NewGuid(), name, description, price, category, imageUrl);
    }

    public static Product GenerateInvalidProduct()
    {
        return Product.Create(Guid.NewGuid(), null, null, 0, default, null);
    }
}