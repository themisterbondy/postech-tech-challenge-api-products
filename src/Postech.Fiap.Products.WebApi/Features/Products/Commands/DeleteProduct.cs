using FluentValidation;
using Postech.Fiap.Products.WebApi.Common.ResultPattern;
using Postech.Fiap.Products.WebApi.Common.Validation;
using Postech.Fiap.Products.WebApi.Features.Products.Repositories;

namespace Postech.Fiap.Products.WebApi.Features.Products.Commands;

public class DeleteProduct
{
    public class Command : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductValidator : AbstractValidator<Command>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithError(Error.Validation("Id", "Id is required."));
        }
    }

    public class DeleteProductHandler(IProductRepository productRepository) : IRequestHandler<Command, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await productRepository.FindByIdAsync(request.Id, cancellationToken);

            if (product == null)
                return Result.Failure<Guid>(Error.NotFound("DeleteProductHandler.Handle", "Product not found."));

            await productRepository.DeleteAsync(product, cancellationToken);

            return Result.Success(request.Id);
        }
    }
}