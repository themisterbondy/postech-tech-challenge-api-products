using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using PosTech.Fiap.Products.WebApi.Features.Products.Commands;
using Postech.Fiap.Products.WebApi.Features.Products.Contracts;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using PosTech.Fiap.Products.WebApi.Features.Products.Repositories;

namespace Postech.Fiap.Products.WebApi.UnitTest.Features.Products.Commands;
public class UpdateProductTests
{
    private readonly UpdateProduct.UpdateProductHandler _handler;
    private readonly IProductRepository _productRepository;
    private readonly UpdateProduct.UpdateProductValidator _validator;

    public UpdateProductTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new UpdateProduct.UpdateProductHandler(_productRepository);
        _validator = new UpdateProduct.UpdateProductValidator();
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenIdIsEmpty()
    {
        var command = new UpdateProduct.Command { Id = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenIdIsNotEmpty()
    {
        var command = new UpdateProduct.Command { Id = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenNameIsEmpty()
    {
        var command = new UpdateProduct.Command { Name = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenNameIsNotEmpty()
    {
        var command = new UpdateProduct.Command { Name = "Test Product" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenPriceIsZero()
    {
        var command = new UpdateProduct.Command { Price = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenPriceIsGreaterThanZero()
    {
        var command = new UpdateProduct.Command { Price = 10 };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenCategoryIsInvalid()
    {
        var command = new UpdateProduct.Command { Category = (ProductCategory)999 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Category);
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenCategoryIsValid()
    {
        var command = new UpdateProduct.Command { Category = ProductCategory.Lanche };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenProductIsUpdated()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct =
            Product.Create(Guid.NewGuid(), "Test Product", null, 10, ProductCategory.Acompanhamento, null);
        _productRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(existingProduct);

        var command = new UpdateProduct.Command
        {
            Id = productId,
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 20,
            Category = ProductCategory.Lanche,
            ImageUrl = "http://example.com/updated_image.jpg"
        };

        var updatedProduct = Product.Create(Guid.NewGuid(), command.Name, command.Description, command.Price,
            command.Category, command.ImageUrl);
        _productRepository.UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(updatedProduct);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new ProductResponse
        {
            Id = Guid.NewGuid(),
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Price = updatedProduct.Price,
            Category = updatedProduct.Category,
            ImageUrl = updatedProduct.ImageUrl
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNotFound()
    {
        // Arrange
        _productRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Product)null);

        var command = new UpdateProduct.Command
        {
            Id = Guid.NewGuid(),
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 20,
            Category = ProductCategory.Lanche,
            ImageUrl = "http://example.com/updated_image.jpg"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("UpdateProductHandler.Handle");
        await _productRepository.DidNotReceive().UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }
}