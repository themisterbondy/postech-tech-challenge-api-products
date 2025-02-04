using FluentValidation;
using Postech.Fiap.Products.WebApi.Common.ResultPattern;

namespace Postech.Fiap.Products.WebApi.Common.Validation;

[ExcludeFromCodeCoverage]
public static class FluentValidationErrorExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule,
        Error error)
    {
        if (error is null) throw new ArgumentNullException(nameof(error), "The error is required");

        return rule.WithErrorCode(error.Code).WithMessage(error.Message);
    }
}