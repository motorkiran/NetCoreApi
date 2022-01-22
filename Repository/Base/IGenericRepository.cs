using System.Linq.Expressions;

public interface IGenericRepository<T>
{
    T Get(int id);
    IEnumerable<T> Get();
    IEnumerable<T> GetAll();
    ResultObjectDto Insert(T entity);
    ResultObjectDto Update(T entity);
    bool Delete(T entity);
    bool Delete(int id);
    void ExecuteScript(string script);
    IEnumerable<T> GetListByExpression(string searchQuery);
    dynamic GetMultipleQuery(string query);
}