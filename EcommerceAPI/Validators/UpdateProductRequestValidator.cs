using EcommerceAPI.DTOs.Products;
using FluentValidation;

namespace EcommerceAPI.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            When(x => x.Name != null, () => RuleFor(x => x.Name).NotEmpty().MaximumLength(200));
            When(x => x.Price.HasValue, () => RuleFor(x => x.Price).GreaterThan(0));
            When(x => x.Stock.HasValue, () => RuleFor(x => x.Stock).GreaterThanOrEqualTo(0));
        }
    }
}