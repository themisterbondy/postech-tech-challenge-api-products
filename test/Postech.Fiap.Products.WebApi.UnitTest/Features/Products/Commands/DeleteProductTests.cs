using FluentAssertions;
using FluentValidation.TestHelper;
using Mongo2Go;
using MongoDB.Driver;
using NSubstitute;
using PosTech.Fiap.Products.WebApi.Features.Products.Commands;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using PosTech.Fiap.Products.WebApi.Features.Products.Repositories;


namespace Postech.Fiap.Products.WebApi.UnitTest.Features.Products.Commands;
public class DeleteProductTests
{
    private readonly IProductRepository _productRepository;
    private readonly MongoDbRunner _runner;
    private readonly IMongoDatabase _database;
    private readonly ProductRepository _repository;
    private readonly DeleteProduct.DeleteProductValidator _validator;
    private readonly DeleteProduct.DeleteProductHandler _handler;

    public DeleteProductTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _runner = MongoDbRunner.Start();
        var client = new MongoClient(_runner.ConnectionString);
        _database = client.GetDatabase("TestDatabase");
        _repository = new ProductRepository(_database);
        _validator = new DeleteProduct.DeleteProductValidator();
        _handler = new DeleteProduct.DeleteProductHandler(_productRepository);

    }

    [Fact]
    public void Validator_ShouldHaveError_WhenIdIsEmpty()
    {
        var command = new DeleteProduct.Command { Id = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenIdIsNotEmpty()
    {
        var command = new DeleteProduct.Command { Id = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenProductIsDeleted()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = Product.Create(Guid.NewGuid(), "Test Product", null, 10, ProductCategory.Acompanhamento, null);
        _productRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(product);

        var command = new DeleteProduct.Command { Id = productId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(productId);
        await _productRepository.Received(1).DeleteAsync(product, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNotFound()
    {
        // Arrange
        _productRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Product)null);

        var command = new DeleteProduct.Command { Id = Guid.NewGuid() };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("DeleteProductHandler.Handle");
        await _productRepository.DidNotReceive().DeleteAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }
}