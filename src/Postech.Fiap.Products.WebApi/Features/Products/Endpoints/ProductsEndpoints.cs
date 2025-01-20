using Microsoft.AspNetCore.Mvc;
using Postech.Fiap.Products.WebApi.Common.Extensions;
using PosTech.Fiap.Products.WebApi.Features.Products.Commands;
using Postech.Fiap.Products.WebApi.Features.Products.Contracts;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using PosTech.MyFood.WebApi.Features.Products.Queries;

namespace Postech.Fiap.Products.WebApi.Features.Products.Endpoints;

[ExcludeFromCodeCoverage]
public class ProductsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products");

        group.MapGet("/category", async ([FromQuery] ProductCategory request, [FromServices] IMediator mediator) =>
            {
                var query = new ListProducts.Query
                {
                    Category = request
                };

                var result = await mediator.Send(query);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("ListProducts")
            .Produces<ListProductsResponse>(200)
            .WithTags("Products")
            .WithOpenApi();

        group.MapPost("/", async ([FromBody] ProductRequest request, [FromServices] IMediator mediator) =>
            {
                var command = new CreateProduct.Command
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Category = request.Category,
                    ImageUrl = request.ImageUrl
                };

                var result = await mediator.Send(command);

                return result.IsSuccess
                    ? Results.Created($"/Products/{result.Value.Id}", result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("CreateProduct")
            .Accepts<ProductRequest>("application/json")
            .Produces<ProductResponse>(201)
            .WithTags("Products")
            .WithOpenApi();

        group.MapPut("/{id:Guid}",
                async (Guid id, [FromBody] ProductRequest request, [FromServices] IMediator mediator) =>
                {
                    var command = new UpdateProduct.Command
                    {
                        Id = id,
                        Name = request.Name,
                        Description = request.Description,
                        Price = request.Price,
                        Category = request.Category,
                        ImageUrl = request.ImageUrl
                    };

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : result.ToProblemDetails();
                })
            .WithName("UpdateProduct")
            .Accepts<ProductRequest>("application/json")
            .Produces<ProductResponse>(200)
            .WithTags("Products")
            .WithOpenApi();

        group.MapDelete("/{id:Guid}", async (Guid id, [FromServices] IMediator mediator) =>
            {
                var command = new DeleteProduct.Command
                {
                    Id = id
                };

                var result = await mediator.Send(command);

                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("DeleteProduct")
            .Produces(204)
            .WithTags("Products")
            .WithOpenApi();
    }
}