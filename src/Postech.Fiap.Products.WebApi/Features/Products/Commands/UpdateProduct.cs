using FluentValidation;
using Postech.Fiap.Products.WebApi.Common.ResultPattern;
using Postech.Fiap.Products.WebApi.Common.Validation;
using Postech.Fiap.Products.WebApi.Features.Products.Contracts;
using Postech.Fiap.Products.WebApi.Features.Products.Entities;
using Postech.Fiap.Products.WebApi.Features.Products.Repositories;

namespace Postech.Fiap.Products.WebApi.Features.Products.Commands;

public class UpdateProduct
{
    public class Command : IRequest<Result<ProductResponse>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UpdateProductValidator : AbstractValidator<Command>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithError(Error.Validation("Id", "Id is required."));
            RuleFor(x => x.Name)
                .NotEmpty().WithError(Error.Validation("Name", "Name is required."));
            RuleFor(x => x.Price)
                .GreaterThan(0).WithError(Error.Validation("Price", "Price must be greater than 0."));
            RuleFor(x => x.Category)
                .IsInEnum().WithError(Error.Validation("Category", "Category is invalid."));
        }
    }

    public class UpdateProductHandler(IProductRepository productRepository)
        : IRequestHandler<Command, Result<ProductResponse>>
    {
        public async Task<Result<ProductResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var getProduct = await productRepository.FindByIdAsync(request.Id, cancellationToken);

            if (getProduct == null)
                return Result.Failure<ProductResponse>(Error.NotFound("UpdateProductHandler.Handle",
                    "Product not found."));

            var product = await productRepository.UpdateAsync(
                Product.Create(
                    request.Id,
                    request.Name,
                    request.Description,
                    request.Price,
                    request.Category,
                    request.ImageUrl),
                cancellationToken);

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                ImageUrl = product.ImageUrl
            };
        }
    }
}