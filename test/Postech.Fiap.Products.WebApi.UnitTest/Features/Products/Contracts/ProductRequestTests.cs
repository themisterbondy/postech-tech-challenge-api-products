using FluentAssertions;
using Postech.Fiap.Products.WebApi.Features.Products.Contracts;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;

namespace Postech.Fiap.Products.WebApi.UnitTest.Features.Products.Contracts;


public class ProductRequestTests
{
    [Fact]
    public void ProductRequest_ShouldInitializeCorrectly_WhenValidParameters()
    {
        var name = "Test Product";
        var description = "Test Description";
        var price = 10.99m;
        var category = ProductCategory.Lanche;
        var imageUrl = "http://example.com/image.jpg";

        var productRequest = new ProductRequest()
        {
            Name = name,
            Description = description,
            Price = price,
            Category = category,
            ImageUrl = imageUrl
        };

        productRequest.Should().NotBeNull();
        productRequest.Name.Should().Be(name);
        productRequest.Description.Should().Be(description);
        productRequest.Price.Should().Be(price);
        productRequest.Category.Should().Be(category);
        productRequest.ImageUrl.Should().Be(imageUrl);
    }
}