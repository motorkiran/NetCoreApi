public class PermissionService : IPermissionService
{
    private readonly IRedisService _redisService;
    private readonly IPageService _pageService;
    private readonly IActionService _actionService;
    public PermissionService(IRedisService redisService, IPageService pageService, IActionService actionService)
    {
        _redisService = redisService;
        _pageService = pageService;
        _actionService = actionService;
    }
    public bool CheckUserPermission(int userId, int actionId, int controllerId)
    {
        var userAction = string.Format(Constants.GetActionByUserId, userId);

        var userActionList = _redisService.Get<List<UserAction>>(userAction);

        if (userActionList == null)
        {

            userActionList = _actionService.GetUserActionList(userId, actionId).Where(x => x.UserId == userId).ToList();
            if (userActionList != null)
            {
                _redisService.Set(userAction, userActionList);
                var userActionPermision = userActionList.Where(sa => sa.PageId == controllerId).FirstOrDefault();
                if (userActionPermision != null)
                {
                    return true;
                }
                else return false;
            }
        }
        else
        {
            var userActionPermision = _actionService.GetUserActionList(userId, actionId).Where(x => x.PageId == controllerId).FirstOrDefault(); // page means controller
            if (userActionPermision != null)
            {
                return true;
            }
            else return false;
        }

        return false;
    }

    public string GetActionName(int actionId, int controllerId)
    {
        var actionList = _redisService.Get<List<Action>>(Constants.ActionList);

        if (actionList == null)
        {
            actionList = _actionService.Get();

            if (actionList != null)
            {
                _redisService.Set(Constants.ActionList, actionList);

                string actionName = actionList.Where(action => action.Id == actionId && action.PageId == controllerId).FirstOrDefault()?.ActionName ?? string.Empty;
                return actionName;
            }
        }
        else
        {
            string actionName = actionList.Where(action => action.Id == actionId && action.PageId == controllerId).FirstOrDefault()?.ActionName ?? string.Empty;
            return actionName;
        }

        return string.Empty;
    }

    public string GetControllerName(int controllerId)
    {
        var pageList = _redisService.Get<List<Page>>(Constants.PageList);

        if (pageList == null)
        {
            pageList = _pageService.Get();

            if (pageList != null)
            {
                _redisService.Set(Constants.PageList, pageList);

                var controllerName = pageList.Where(controller => controller.Id == controllerId).FirstOrDefault()?.PageName ?? string.Empty;
                return controllerName;
            }
        }
        else
        {
            var controllerName = pageList.Where(controller => controller.Id == controllerId).FirstOrDefault()?.PageName ?? string.Empty;
            return controllerName;
        }

        return string.Empty;
    }
}