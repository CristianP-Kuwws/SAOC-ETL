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
                .NotEmpty().WithMessage("IdProducto is required")
                .MaximumLength(10).WithMessage("IdProducto can't exceed 10 characters.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("Nombre is required")
                .MaximumLength(25).WithMessage("Nombre can't exceed 25 characters.");

            RuleFor(x => x.Categoría)
                .NotEmpty().WithMessage("Categoría is required")
                .MaximumLength(30).WithMessage("Categoría can't exceed 30 characters.");
        }
    }
}
