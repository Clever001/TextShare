using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TextShareApi.Extensions;

namespace TextShareApi.Attributes;

public class ValidateModelStateAttribute : ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
        if (!context.ModelState.IsValid) {
            var exceptionDto = context.ModelState.ToExceptionDto();
            
            context.Result = new BadRequestObjectResult(exceptionDto);
        }
    }
}
