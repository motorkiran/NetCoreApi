public interface IPermissionService
{
    bool CheckUserPermission(int userId, int actionId, int controllerId);
    string GetActionName(int actionId, int controllerId);
    string GetControllerName(int controllerId);
}