using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Validation
{
    public interface IValidationModel<T>
    {
        AbstractValidator<T> Validator { get; }
        bool IsValid => Validator.Validate((T)this).IsValid;
        List<ValidationFailure>? ValidationErrors => Validator.Validate((T)this).Errors;
    }
}
