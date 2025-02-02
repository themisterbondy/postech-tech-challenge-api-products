using FluentAssertions;
using NSubstitute;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using PosTech.Fiap.Products.WebApi.Features.Products.Queries;
using PosTech.Fiap.Products.WebApi.Features.Products.Repositories;
using Postech.Fiap.Products.WebApi.UnitTest.Features.Mocks;

namespace PosTech.MyFood.WebApi.UnitTests.Features.Products.Queries;

public class ListProductsTests
{
    private readonly ListProducts.ListProductsHandler _handler;
    private readonly GetProductById.GetProductByIdHandler _gethandler;
    private readonly IProductRepository _productRepository;

    public ListProductsTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new ListProducts.ListProductsHandler(_productRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenCategoryIsProvided()
    {
        // Arrange
        var category = ProductCategory.Lanche;
        var products = new List<Product>
        {
            ProductMocks.GenerateValidProduct(),
            ProductMocks.GenerateValidProduct()

        };

        _productRepository.FindByCategoryAsync(category, Arg.Any<CancellationToken>()).Returns(products);

        var query = new ListProducts.Query { Category = category };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Products.Should().HaveCount(2);
        result.Value.Products.Should().BeEquivalentTo(products, options => options.ExcludingMissingMembers());
    }
    


    [Fact]
    public async Task Handle_ShouldReturnAllProducts_WhenCategoryIsNotProvided()
    {
        // Arrange
        var products = new List<Product>
        {
            ProductMocks.GenerateValidProduct(),
            ProductMocks.GenerateValidProduct()
        };

        _productRepository.FindByCategoryAsync(null, Arg.Any<CancellationToken>()).Returns(products);

        var query = new ListProducts.Query { Category = null };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Products.Should().HaveCount(2);
        result.Value.Products.Should().BeEquivalentTo(products, options => options.ExcludingMissingMembers());
    }
}