using PosTech.Fiap.Products.WebApi.Features.Products.Repositories;
using Postech.Fiap.Products.WebApi.UnitTest.Features.Mocks;

namespace Postech.Fiap.Products.WebApi.UnitTest.Features.Queries;


public class ProductRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        _context = ApplicationDbContextMock.Create();
        _repository = new ProductRepository(_context);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = ProductMocks.GenerateValidProduct();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.FindByIdAsync(product.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = new ProductId(Guid.NewGuid());

        // Act
        var result = await _repository.FindByIdAsync(productId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task FindByCategoryAsync_ShouldReturnProducts_WhenCategoryExists()
    {
        // Arrange
        var products = new[]
        {
            ProductMocks.GenerateValidProduct(),
            ProductMocks.GenerateValidProduct(),
            ProductMocks.GenerateValidProduct(),
            ProductMocks.GenerateValidProduct(),
            ProductMocks.GenerateValidProduct()
        };
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.FindByCategoryAsync(ProductCategory.Lanche, CancellationToken.None);

        // Assert
        result.Should().HaveCount(5);
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProductToDatabase()
    {
        // Arrange
        var product = ProductMocks.GenerateValidProduct();

        // Act
        var result = await _repository.CreateAsync(product, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        (await _context.Products.FindAsync(product.Id)).Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProductInDatabase()
    {
        // Arrange
        var product = ProductMocks.GenerateValidProduct();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var updatedName = "Updated Name";
        product.Name = updatedName;

        // Act
        var result = await _repository.UpdateAsync(product, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(updatedName);
        (await _context.Products.FindAsync(product.Id)).Name.Should().Be(updatedName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProductFromDatabase()
    {
        // Arrange
        var product = ProductMocks.GenerateValidProduct();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(product, CancellationToken.None);

        // Assert
        (await _context.Products.FindAsync(product.Id)).Should().BeNull();
    }
}