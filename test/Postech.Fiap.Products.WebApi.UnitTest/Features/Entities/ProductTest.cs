using FluentAssertions;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;

namespace Postech.Fiap.Products.WebApi.UnitTest.Features.Entities;

public class ProductTest

{
    [Fact]
    public void ProductId_ShouldInitializeCorrectly_WhenValidGuidIsProvided()
    {
        var guid = Guid.NewGuid();
        var productId = new ProductId(guid);

        productId.Should().NotBeNull();
        productId.Value.Should().Be(guid);
    }

    [Fact]
    public void ProductId_ShouldGenerateNewGuid_WhenNewIsCalled()
    {
        var productId = ProductId.New();

        productId.Should().NotBeNull();
        productId.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void ProductCategory_ShouldContainAllDefinedCategories()
    {
        var categories = Enum.GetValues(typeof(ProductCategory)).Cast<ProductCategory>().ToList();

        categories.Should().Contain(ProductCategory.Lanche);
        categories.Should().Contain(ProductCategory.Acompanhamento);
        categories.Should().Contain(ProductCategory.Bebida);
        categories.Should().Contain(ProductCategory.Sobremesa);
    }

    [Fact]
    public void ProductCategory_ShouldHaveCorrectNumberOfCategories()
    {
        var categories = Enum.GetValues(typeof(ProductCategory)).Cast<ProductCategory>().ToList();

        categories.Should().HaveCount(4);
    }

    [Fact]
    public void Product_ShouldInitializeCorrectly_WhenValidParameters()
    {
        var id = Guid.NewGuid();
        var name = "Test Product";
        var description = "Test Description";
        var price = 10.99m;
        var category = ProductCategory.Lanche;
        var imageUrl = "http://example.com/image.jpg";

        var product = Product.Create(id, name, description, price, category, imageUrl);

        product.Should().NotBeNull();
        product.Id.Should().Be(id);
        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.Price.Should().Be(price);
        product.Category.Should().Be(category);
        product.ImageUrl.Should().Be(imageUrl);
    }

    [Fact]
    public void Product_ShouldInitializeCorrectly_WhenDescriptionIsNull()
    {
        var id = Guid.NewGuid();
        var name = "Test Product";
        var price = 10.99m;
        var category = ProductCategory.Lanche;
        var imageUrl = "http://example.com/image.jpg";

        var product = Product.Create(id, name, null, price, category, imageUrl);

        product.Should().NotBeNull();
        product.Id.Should().Be(id);
        product.Name.Should().Be(name);
        product.Description.Should().BeNull();
        product.Price.Should().Be(price);
        product.Category.Should().Be(category);
        product.ImageUrl.Should().Be(imageUrl);
    }

    [Fact]
    public void Product_ShouldInitializeCorrectly_WhenImageUrlIsNull()
    {
        var id = Guid.NewGuid();
        var name = "Test Product";
        var description = "Test Description";
        var price = 10.99m;
        var category = ProductCategory.Lanche;

        var product = Product.Create(id, name, description, price, category, null);

        product.Should().NotBeNull();
        product.Id.Should().Be(id);
        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.Price.Should().Be(price);
        product.Category.Should().Be(category);
        product.ImageUrl.Should().BeNull();
    }
}