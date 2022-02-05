public class ActionService : IActionService
{
    private readonly IActionRepository _actionRepository;

    public ActionService(IActionRepository actionRepository)
    {
        _actionRepository = actionRepository;
    }

    public Action Get(int id)
    {
        var action = _actionRepository.Get(id);

        return action;
    }

    public List<Action> Get()
    {
        var action = _actionRepository.Get().ToList();

        return action;
    }

    public List<UserAction> GetUserActionList(int userId, int actionId)
    {
        var list = _actionRepository.GetUserActionList(userId, actionId);

        return list;
    }
}