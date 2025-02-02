

using FluentAssertions;
using Mongo2Go;
using MongoDB.Driver;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using PosTech.Fiap.Products.WebApi.Features.Products.Repositories;
using Postech.Fiap.Products.WebApi.UnitTest.Features.Mocks;

namespace PosTech.Fiap.Products.WebApi.UnitTest.Features.ProductRepositoryTests;

public class ProductRepositoryTests
{
    private readonly MongoDbRunner _runner;
    private readonly IMongoDatabase _database;
    private readonly ProductRepository _repository;
    
    
    public ProductRepositoryTests()
    {
        _runner = MongoDbRunner.Start();
        var client = new MongoClient(_runner.ConnectionString);
        _database = client.GetDatabase("TestDatabase");
        _repository = new ProductRepository(_database);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = ProductMocks.GenerateValidProduct();
        await _repository.CreateAsync(product, CancellationToken.None);

        // Act
        var result = await _repository.FindByIdAsync(product.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }

}