public interface IActionRepository : IGenericRepository<Action>
{
    List<UserAction> GetUserActionList(int userId, int actionId);
}