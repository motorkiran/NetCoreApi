public class ActionRepository : GenericRepository<Action>, IActionRepository
{
    public ActionRepository(IConfiguration configuration, ILogService logService) : base(configuration, logService)
    {
    }

    public List<UserAction> GetUserActionList(int userId, int actionId)
    {
        var searchQuery = string.Format(ActionQuery.GetUserActionList, userId, actionId);
        var userActionList = GetMultipleQuery(searchQuery).Read<UserAction>().ToList();

        return userActionList;
    }
}