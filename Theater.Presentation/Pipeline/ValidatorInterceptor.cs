using Azure;
using Theater.Infrastructure.Exceptions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Theater.Presentation.Pipeline
{
    public class ValidatorInterceptor : IValidatorInterceptor
    {

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }


        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                var errors = result.Errors.GroupBy(m => m.PropertyName)
                   .ToDictionary(m => m.Key, v => v.Select(m => m.ErrorMessage));


                throw new BadRequestException("Gonderilen melumatlar serti odemir", errors);
            }

            return result;
        }
    }
}
