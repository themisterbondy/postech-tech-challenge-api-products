using Postech.Fiap.Products.WebApi.Common.ResultPattern;
using Postech.Fiap.Products.WebApi.Features.Products.Contracts;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using PosTech.MyFood.WebApi.Features.Products.Repositories;

namespace PosTech.MyFood.WebApi.Features.Products.Queries;

public abstract class ListProducts
{
    public class Query : IRequest<Result<ListProductsResponse>>
    {
        public ProductCategory? Category { get; set; }
    }

    public class ListProductsHandler(IProductRepository productRepository)
        : IRequestHandler<Query, Result<ListProductsResponse>>
    {
        public async Task<Result<ListProductsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var products = await productRepository.FindByCategoryAsync(request.Category, cancellationToken);

            return new ListProductsResponse
            {
                Products = products.Select(x => new ProductResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Category = x.Category,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl
                }).ToList()
            };
        }
    }
}