using FluentValidation.Results;
using Lucky9.Application.Common.Models;
using Lucky9.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky9.Application.Common.Exceptions
{
    internal class ValidationHelper
    {
        public static List<ValidationDto> Summarize(ValidationResult validationResult)
        {

            // Check if there are any validation errors
            if (!validationResult.IsValid)
            {
                // Convert Fluent Validation errors to a custom error model
                var errors = validationResult.Errors.Select(e => new ValidationDto
                {
                    Property = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                });

                // Return BadRequest with the custom error model
                //BadRequest(new { Errors = errors });
                return errors.ToList();
            }

            return null; //new List<ValidationDto>();
        }

    }
}
