using FluentValidation;
using Postech.Fiap.Products.WebApi.Common.ResultPattern;
using Postech.Fiap.Products.WebApi.Common.Validation;
using Postech.Fiap.Products.WebApi.Features.Products.Contracts;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using Postech.Fiap.Products.WebApi.Features.Products.Repositories;

namespace Postech.Fiap.Products.WebApi.Features.Products.Commands;

public abstract class CreateProduct
{
    public class Command : IRequest<Result<ProductResponse>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class UpdateProductValidator : AbstractValidator<Command>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithError(Error.Validation("Name", "Name is required."));
            RuleFor(x => x.Price)
                .GreaterThan(0).WithError(Error.Validation("Price", "Price must be greater than 0."));
            RuleFor(x => x.Category)
                .IsInEnum().WithError(Error.Validation("Category", "Category is invalid."));
        }
    }

    public class CreateProductHandler(IProductRepository productRepository)
        : IRequestHandler<Command, Result<ProductResponse>>
    {
        public async Task<Result<ProductResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await productRepository.CreateAsync(
                Product.Create(Guid.NewGuid(),
                    request.Name,
                    request.Description,
                    request.Price,
                    request.Category,
                    request.ImageUrl),
                cancellationToken);

            return Result.Success(new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                ImageUrl = product.ImageUrl
            });
        }
    }
}