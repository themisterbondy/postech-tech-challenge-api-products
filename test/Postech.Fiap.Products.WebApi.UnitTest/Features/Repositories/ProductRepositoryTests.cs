using MongoDB.Driver;
using NSubstitute;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Threading;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using PosTech.Fiap.Products.WebApi.Features.Products.Repositories;
using Postech.Fiap.Products.WebApi.UnitTest.Features.Mocks;

public class ProductRepositoryTests
{
    private readonly IProductRepository _repository;
    private readonly IMongoDatabase _mockDatabase;
    private readonly IMongoCollection<Product> _mockCollection;

    public ProductRepositoryTests()
    {
        _mockDatabase = Substitute.For<IMongoDatabase>();
        _mockCollection = Substitute.For<IMongoCollection<Product>>();
        _mockDatabase.GetCollection<Product>(Arg.Any<string>(), Arg.Any<MongoCollectionSettings>()).Returns(_mockCollection);
        _repository = new ProductRepository(_mockDatabase);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = ProductMocks.GenerateValidProduct();
        var cursor = Substitute.For<IAsyncCursor<Product>>();
        cursor.Current.Returns(new List<Product> { product });
        cursor.MoveNextAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(true));
        _mockCollection.FindAsync(Arg.Any<FilterDefinition<Product>>(), Arg.Any<FindOptions<Product>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(cursor));

        // Act
        var result = await _repository.FindByIdAsync(product.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }


}