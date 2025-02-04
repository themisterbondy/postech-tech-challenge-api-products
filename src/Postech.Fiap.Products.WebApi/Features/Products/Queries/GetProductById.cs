using Postech.Fiap.Products.WebApi.Common.ResultPattern;
using Postech.Fiap.Products.WebApi.Features.Products.Contracts;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using PosTech.Fiap.Products.WebApi.Features.Products.Repositories;

namespace PosTech.Fiap.Products.WebApi.Features.Products.Queries;

public abstract class GetProductById
{
    public class Query : IRequest<Result<ProductResponse>>
    {
        public Guid Id { get; set; }
    }

    public class GetProductByIdHandler(IProductRepository productRepository)
        : IRequestHandler<Query, Result<ProductResponse>>
    {
        public async Task<Result<ProductResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var products = await productRepository.FindByIdAsync(request.Id, cancellationToken);

            return new ProductResponse
            {
                    Id = products.Id,
                    Name = products.Name,
                    Description = products.Description,
                    Category = products.Category,
                    Price = products.Price,
                    ImageUrl = products.ImageUrl
            };
        }
    }
}