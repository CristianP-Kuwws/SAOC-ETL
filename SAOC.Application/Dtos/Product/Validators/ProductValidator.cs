using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Dtos.Product.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.IdProducto)
                .GreaterThan(0).WithMessage("IdProducto must be a positive integer.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("Nombre is required")
                .MaximumLength(50).WithMessage("Nombre can't exceed 50 characters.");

            RuleFor(x => x.Categoría)
                .NotEmpty().WithMessage("Categoría is required")
                .MaximumLength(30).WithMessage("Categoría can't exceed 30 characters.");
        }
    }
}
