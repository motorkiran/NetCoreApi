using System.Security.Claims;

public class ServiceBase : IServiceBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ServiceBase(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void BindBaseProperty(dynamic entity)
    {
        if (entity.Id > 0)
        {
            entity.UpdateUser = GetLoggedUser();
            entity.UpdateDate = DateTime.Now;
        }
        else
        {
            entity.UpdateUser = GetLoggedUser();
            entity.UpdateDate = DateTime.Now;
            entity.CreateUser = GetLoggedUser();
            entity.CreateDate = DateTime.Now;
            entity.IsDeleted = false;
        }
    }

    public int GetLoggedUser()
    {
        var userId = ((User)_httpContextAccessor.HttpContext.Items["User"]).Id;

        return userId;
    }
}