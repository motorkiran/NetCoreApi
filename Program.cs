var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSentry(o =>
                   {
                       o.Dsn = Constants.SentryDsn;
                       o.Debug = true;
                   });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

ConfigurationManager configuration = builder.Configuration;
builder.Services.InstallServicesInAssembly(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
