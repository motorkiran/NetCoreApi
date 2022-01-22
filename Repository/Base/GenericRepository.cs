using System.ComponentModel;
using System.Data;
using System.Reflection;
using Dapper;
using Npgsql;

public class GenericRepository<T> : IGenericRepository<T> where T : AbstractEntity
{
    private string connectionString;

    public GenericRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("tacirden");
    }

    internal IDbConnection Connection
    {
        get
        {
            return new NpgsqlConnection(connectionString);
        }
    }

    public ResultObjectDto Insert(T entity)
    {
        var columns = GetColumns(false);
        var columnDefinitions = GetColumnDefinitions(false);
        var stringOfColumns = string.Join(", ", columnDefinitions);
        var stringOfParameters = string.Join(", ", columns.Select(c => "@" + c));
        var query = $"INSERT INTO {TableName()} ({stringOfColumns}) VALUES ({stringOfParameters}) RETURNING id";
        var resultDto = new ResultObjectDto();
        var result = new List<int>();

        try
        {
            using (IDbConnection connection = Connection)
            {
                connection.Open();
                result = connection.Query<int>(query, entity).ToList();
                connection.Close();
                connection.Dispose();
            }

            if (result.FirstOrDefault() > 0)
            {
                resultDto.IsSuccess = true;
                resultDto.Message = "Kayıt işlemi başarılı.";
                resultDto.Result = result.FirstOrDefault().ToString();
                resultDto.ResultType = ResultType.Success;
            }
            else
            {
                resultDto.IsSuccess = false;
                resultDto.Message = "Kayıt sırasında bir hata meydana geldi.";
                resultDto.Result = result;
                resultDto.ResultType = ResultType.Error;
            }
        }
        catch (Exception ex)
        {
            resultDto.IsSuccess = false;
            resultDto.Message = "Kayıt sırasında bir hata meydana geldi.";
            resultDto.Result = result;
            resultDto.ResultType = ResultType.Error;
        }

        return resultDto;
    }

    public ResultObjectDto Update(T entity)
    {
        var columns = GetColumns(false);
        var columnDefinitions = GetColumnDefinitions(false);
        var stringOfColumns = string.Join(", ", ColumnList());
        var stringOfParameters = string.Join(", ", columns.Select(c => "@" + c));
        var query = $"UPDATE {TableName()} SET ({ColumnList}) = ({stringOfParameters}) WHERE id = {entity.Id}";
        var resultDto = new ResultObjectDto();
        var result = 0;

        try
        {
            using (IDbConnection connection = Connection)
            {
                connection.Open();
                result = connection.Execute(query, entity);
                connection.Close();
                connection.Dispose();
            }

            if (result > 0)
            {
                resultDto.IsSuccess = true;
                resultDto.Message = "Güncelleme işlemi başarılı.";
                resultDto.Result = result;
                resultDto.ResultType = ResultType.Success;
            }
            else
            {
                resultDto.IsSuccess = false;
                resultDto.Message = "Güncelleme sırasında bir hata meydana geldi.";
                resultDto.Result = result;
                resultDto.ResultType = ResultType.Error;
            }
        }
        catch (Exception ex)
        {
            resultDto.IsSuccess = false;
            resultDto.Message = "Güncelleme sırasında bir hata meydana geldi.";
            resultDto.Result = result;
            resultDto.ResultType = ResultType.Error;
        }

        return resultDto;
    }

    public IEnumerable<T> Get()
    {
        var result = new List<T>();

        try
        {
            using (IDbConnection connection = Connection)
            {
                connection.Open();

                var query = $"SELECT * FROM {TableName()} WHERE is_deleted = false";
                result = connection.Query<T>(query).ToList();

                connection.Close();
                connection.Dispose();
            }
        }
        catch (Exception ex)
        {

            throw;
        }

        return result;
    }

    public T Get(int id)
    {
        var result = new List<T>();

        try
        {
            using (IDbConnection connection = Connection)
            {
                connection.Open();

                result = connection.Query<T>($"SELECT * FROM {TableName()} WHERE Id = @Id", new { Id = id }).ToList();

                connection.Close();
                connection.Dispose();
            }
        }
        catch (Exception ex)
        {

        }

        return result.FirstOrDefault();
    }

    public IEnumerable<T> GetAll()
    {
        var result = new List<T>();

        try
        {
            using (IDbConnection connection = Connection)
            {

                connection.Open();

                var query = $"SELECT * FROM {TableName()}";
                result = connection.Query<T>(query).ToList();

                connection.Close();
                connection.Dispose();
            }
        }
        catch (Exception ex)
        {

        }

        return result;
    }

    public virtual bool Delete(T entity)
    {
        return Delete(entity.Id);
    }

    public virtual bool Delete(int id)
    {
        try
        {
            using (IDbConnection connection = Connection)
            {

                connection.Open();

                var query = $"DELETE FROM {TableName()} WHERE Id = {id}";
                connection.Query<T>(query).ToList();

                connection.Close();
                connection.Dispose();

                return true;
            }
        }
        catch (Exception ex)
        {

        }

        return false;
    }

    public void ExecuteScript(string script)
    {
        try
        {
            using (IDbConnection connection = Connection)
            {

                connection.Open();

                connection.Query<T>(script).ToList();

                connection.Close();
                connection.Dispose();
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public virtual IEnumerable<T> GetListByExpression(string query)
    {
        var result = new List<T>();

        try
        {
            using (IDbConnection connection = Connection)
            {
                connection.Open();

                result = connection.Query<T>(query).ToList();

                connection.Close();
                connection.Dispose();
            }

            return result;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public virtual dynamic GetMultipleQuery(string query)
    {
        dynamic result;

        try
        {
            using (IDbConnection connection = Connection)
            {
                connection.Open();

                result = connection.QueryMultiple(query);

                connection.Close();
                connection.Dispose();
            }

            return result;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public List<string> ColumnList()
    {
        var columns = GetColumns(true);
        var columnDefinitions = GetColumnDefinitions(true);
        return columnDefinitions.Zip(columns, Tuple.Create)
            .Select(zip => "\"" + zip.Item1 + "\" AS \"" + zip.Item2 + "\"").ToList();
    }

    private static IEnumerable<string> GetColumns(bool withId)
    {
        if (withId)
            return typeof(T).GetProperties().Select(e => e.Name);
        return typeof(T).GetProperties().Where(e => e.Name != "Id").Select(e => e.Name);
    }

    private static IEnumerable<string> GetColumnDefinitions(bool withId)
    {
        if (withId)
            return typeof(T).GetProperties().Select(p => p.GetCustomAttribute<DescriptionAttribute>().Description);
        return typeof(T).GetProperties()
            .Where(p => p.GetCustomAttribute<DescriptionAttribute>() != null && p.Name != "Id")
            .Select(p => p.GetCustomAttribute<DescriptionAttribute>().Description);
    }

    private static string GetDescription(Type type)
    {
        var descriptions = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (descriptions.Length == 0) return null;

        return descriptions[0].Description;
    }

    private static string TableName()
    {
        var entityName = GetDescription(typeof(T));
        return $"{Constants.DbSchema}.{entityName}";
    }
}