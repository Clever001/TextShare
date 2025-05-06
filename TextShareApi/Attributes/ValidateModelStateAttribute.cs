using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TextShareApi.Dtos.Exception;
using TextShareApi.Exceptions;

namespace TextShareApi.Attributes;

public class ValidateModelStateAttribute : ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
        if (!context.ModelState.IsValid) {
            List<string> errors = new();

            foreach (var (_, value) in context.ModelState) {
                foreach (var error in value.Errors) {
                    errors.Add(error.ErrorMessage);
                }
            }
            
            var exceptionDto = new ExceptionDto {
                Code = "ValidationFailed",
                Description = "One or more validation errors occurred.",
                Details = errors
            };
            
            context.Result = new BadRequestObjectResult(exceptionDto);
        }
    }
}
