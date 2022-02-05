using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

public class PermissonFilter : IActionFilter
{
    private readonly IPermissionService _permissionService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissonFilter(IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
    {
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        int userId = ((User)_httpContextAccessor.HttpContext.Items["User"]).Id;

        if (HasSecurityActionAttribute(context))
        {
            try
            {
                var arguments = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.CustomAttributes.FirstOrDefault(fd => fd.AttributeType == typeof(SecurityActionAttribute)).ConstructorArguments;

                int idSecurityController = (int)arguments[0].Value;
                int actionNumber = 0;
                actionNumber = (int)arguments[1].Value;

                if (!_permissionService.CheckUserPermission(userId, actionNumber, idSecurityController))
                {
                    string actionName = _permissionService.GetActionName(actionNumber, idSecurityController);
                    string controllerName = _permissionService.GetControllerName(idSecurityController);

                    context.Result = new ObjectResult(context.ModelState)
                    {
                        Value = $"\"{controllerName}\" sayfa için, \"{actionName}\" işlemi için geçerli bir yetkiniz yoktur.",
                        StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
                    };
                }
            }
            catch
            {

            }
        }
    }

    public bool HasSecurityActionAttribute(FilterContext context)
    {
        return ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.CustomAttributes.
            Any(filterDescriptors => filterDescriptors.AttributeType == typeof(SecurityActionAttribute));
    }
}