using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

public class MvcInstaller : IInstaller
{
   public void InstallServices(IServiceCollection services, IConfiguration configuration)
   {
      services.AddMvc(options =>
              {
                 options.Filters.Add<ValidationFilter>();
              })
      .AddFluentValidation(mvcConfiguration => mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Program>());

      services.AddMemoryCache();

      var profiles = AppDomain.CurrentDomain.GetAssemblies()
                  .SelectMany(a => a.GetTypes().Where(type => typeof(Profile).IsAssignableFrom(type)));

      var mapperConfig = new MapperConfiguration(mc =>
           {
              mc.AddMaps(profiles.ToList());
           });

      IMapper mapper = mapperConfig.CreateMapper();
      services.AddSingleton(mapper);

      services.AddSwaggerGen(c =>
          {
             c.SwaggerDoc("v1", new OpenApiInfo
             {
                Title = "NetCoreApi",
                Version = "1.0.0",
             });
          });
   }
}