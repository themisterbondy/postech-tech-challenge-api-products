using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using Postech.Fiap.Products.WebApi.Features.Products.Commands;
using Postech.Fiap.Products.WebApi.Features.Products.Contracts;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using Postech.Fiap.Products.WebApi.Features.Products.Repositories;

namespace Postech.Fiap.Products.WebApi.UnitTest.Features.Products.Commands;

public class CreateProductTests
{
    private readonly CreateProduct.CreateProductHandler _handler;
    private readonly IProductRepository _productRepository;
    private readonly CreateProduct.UpdateProductValidator _validator;

    public CreateProductTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new CreateProduct.CreateProductHandler(_productRepository);
        _validator = new CreateProduct.UpdateProductValidator();
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenNameIsEmpty()
    {
        var command = new CreateProduct.Command { Name = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenNameIsNotEmpty()
    {
        var command = new CreateProduct.Command { Name = "Test Product" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenPriceIsZero()
    {
        var command = new CreateProduct.Command { Price = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenPriceIsGreaterThanZero()
    {
        var command = new CreateProduct.Command { Price = 10 };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenCategoryIsInvalid()
    {
        var command = new CreateProduct.Command { Category = (ProductCategory)999 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Category);
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenCategoryIsValid()
    {
        var command = new CreateProduct.Command { Category = ProductCategory.Lanche };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenProductIsCreated()
    {
        // Arrange
        var command = new CreateProduct.Command
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 10,
            Category = ProductCategory.Lanche,
            ImageUrl = "http://example.com/image.jpg"
        };

        var id = Guid.NewGuid();
        var product = Product.Create(id, command.Name, command.Description, command.Price,
            command.Category, command.ImageUrl);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new ProductResponse()
        {
            Id = id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            ImageUrl = product.ImageUrl
        });
    }

    [Fact]
    public async Task Handle_ShouldCallCreateAsync_WithCorrectProduct()
    {
        // Arrange
        var command = new CreateProduct.Command
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 10,
            Category = ProductCategory.Lanche,
            ImageUrl = "http://example.com/image.jpg"
        };

        var product = Product.Create(Guid.NewGuid(), command.Name, command.Description, command.Price,
            command.Category, command.ImageUrl);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _productRepository.Received(1).CreateAsync(Arg.Is<Product>(p =>
            p.Name == command.Name &&
            p.Description == command.Description &&
            p.Price == command.Price &&
            p.Category == command.Category &&
            p.ImageUrl == command.ImageUrl), Arg.Any<CancellationToken>());
    }
}