public interface IActionService
{
    public Action Get(int id);
    public List<Action> Get();
    public List<UserAction> GetUserActionList(int userId, int actionId);
}