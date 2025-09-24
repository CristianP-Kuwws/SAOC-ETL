using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Dtos.Source.Validators
{
    public class SourceValidator : AbstractValidator<SourceDto>
    {
        public SourceValidator()
        {
            RuleFor(x => x.IdFuente)
                .NotEmpty().WithMessage("IdFuente is required")
                .MaximumLength(10).WithMessage("IdFuente can't exceed 10 characters.");

            RuleFor(x => x.TipoFuente)
                .NotEmpty().WithMessage("TipoFuente is required")
                .MaximumLength(15).WithMessage("TipoFuente can't exceed 15 characters.")
                .Must(v => v.Equals("Web", StringComparison.OrdinalIgnoreCase) ||
                            v.Equals("CSV", StringComparison.OrdinalIgnoreCase) ||
                            v.Equals("Red Social", StringComparison.OrdinalIgnoreCase))
                .WithMessage("TipoFuente must be either 'Web', 'CSV', or 'Red Social'.");

            RuleFor(x => x.FechaCarga)
                .NotEmpty().WithMessage("FechaCarga is required")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("FechaCarga cannot be in the future.");
        }
    }
}
