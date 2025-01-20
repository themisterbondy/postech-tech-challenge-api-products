namespace Postech.Fiap.Products.WebApi.Features.Products.Entities;

public record ProductId(Guid Value)
{
    public static ProductId New()
    {
        return new ProductId(Guid.NewGuid());
    }
}