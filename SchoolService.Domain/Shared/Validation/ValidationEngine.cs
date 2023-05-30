using FluentValidation.Results;
using SchoolService.Domain.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Validation
{
    public class ValidationEngine : IValidationEngine
    {
        public List<ValidationFailure>? Validate<T>(IValidationModel<T>? input, bool throwException = true)
        {
            if (input is null)
            {
                return null;
            }

            if (input.IsValid)
            {
                return null;
            }

            if (throwException)
            {
                throw new DataNotValidException(errors: input.ValidationErrors);
            }
            return input.ValidationErrors;
        }
    }
}
