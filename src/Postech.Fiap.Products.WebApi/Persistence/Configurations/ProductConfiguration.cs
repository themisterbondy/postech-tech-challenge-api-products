using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosTech.Fiap.Products.WebApi.Features.Products.Entities;

namespace PosTech.Fiap.Products.WebApi.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasConversion(productId => productId.Value,
                value => new ProductId(value));

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Category)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (ProductCategory)Enum.Parse(typeof(ProductCategory), v))
            .HasMaxLength(50);

        builder.Property(p => p.ImageUrl)
            .IsRequired(false)
            .HasMaxLength(800);

        builder.HasData(
            Product.Create(new ProductId(Guid.Parse("81e0a7f0-77e9-433f-9f2c-1b131c3317c3")), "Big Mac",
                "Dois hambúrgueres (100% carne bovina), alface americana, queijo processado sabor cheddar, molho especial, cebola, picles e pão com gergelim.",
                5.99m, ProductCategory.Lanche,
                "https://cache-backend-mcd.mcdonaldscupones.com/media/image/product$kzXCTbnv/200/200/original?country=br"),
            Product.Create(new ProductId(Guid.Parse("6937a222-4e5e-4a75-abde-9ab3b9f58b0f")), "McFritas Média",
                "A batata frita mais famosa do mundo. Deliciosas batatas selecionadas, fritas, crocantes por fora, macias por dentro, douradas, irresistíveis, saborosas, famosas, e todos os outros adjetivos positivos que você quiser dar.",
                2.99m, ProductCategory.Acompanhamento,
                "https://cache-backend-mcd.mcdonaldscupones.com/media/image/product$kUXGZHtB/200/200/original?country=br"),
            Product.Create(new ProductId(Guid.Parse("84d18030-66cc-4f12-bf5f-988667805bf8")), "Coca-Cola 300ml",
                "Refrescante e geladinha. Uma bebida assim refresca a vida. Você pode escolher entre Coca-Cola, Coca-Cola Zero, Sprite sem Açúcar, Fanta Guaraná e Fanta Laranja.",
                1.99m, ProductCategory.Bebida,
                "https://cache-backend-mcd.mcdonaldscupones.com/media/image/product$kNXZJR6V/200/200/original?country=br"),
            Product.Create(new ProductId(Guid.Parse("024fb6ba-5ebe-4131-a27e-d10a4041b32d")), "Casquinha Chocolate",
                "A sobremesa que o Brasil todo adora. Uma casquinha supercrocante, com bebida láctea sabor chocolate que vai bem a qualquer hora.",
                1.49m, ProductCategory.Sobremesa,
                "https://cache-backend-mcd.mcdonaldscupones.com/media/image/product$kpXyfJ7k/200/200/original?country=br")
        );
    }
}