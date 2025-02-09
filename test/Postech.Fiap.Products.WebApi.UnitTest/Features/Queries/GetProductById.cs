using FluentAssertions;
using NSubstitute;
using Postech.Fiap.Products.WebApi.Features.Products.Queries;
using Postech.Fiap.Products.WebApi.Features.Products.Repositories;
using Postech.Fiap.Products.WebApi.UnitTest.Features.Mocks;

namespace Postech.Fiap.Products.WebApi.UnitTest.Features.Queries;

public class GetProductByIdTests
{
    private readonly GetProductById.GetProductByIdHandler _handler;
    private readonly IProductRepository _productRepository;

    public GetProductByIdTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetProductById.GetProductByIdHandler(_productRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenByIsProvided()
    {
        // Arrange
        var products =
            ProductMocks.GenerateValidProduct();

        _productRepository.FindByIdAsync(products.Id
            , Arg.Any<CancellationToken>()).Returns(products);

        var query = new GetProductById.Query { Id = products.Id };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}