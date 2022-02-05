public class ServiceInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.Configure<RedisConfig>(configuration.GetSection("RedisConfig"));

        services.AddScoped<PermissonFilter>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IRedisService, RedisService>();

        services.AddScoped<IActionRepository, ActionRepository>();
        services.AddScoped<IPageRepository, PageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddTransient<IPermissionService, PermissionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IServiceBase, ServiceBase>();
        services.AddScoped<IJWTService, JWTService>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<IActionService, ActionService>();
        services.AddScoped<IPageService, PageService>();
        services.AddScoped<IRedisService, RedisService>();
    }
}