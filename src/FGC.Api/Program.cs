using FGC.Api.Extensions;
using FGC.Api.Middlewares;
using FGC.Application.Common;
using FGC.Application.Services.Data;
using FGC.Infra.CrossCutting;
using FGC.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//TODO: Configurate appsettings for production
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddApplication();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ApplicationDbContextInitialiser>();

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddLocalHttpClients(builder.Configuration);
builder.Services.AddLocalServices(builder.Configuration);
 
builder.Services.AddCustomMvc();

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGlobalCorsPolicy();
builder.Services.AddApiVersioningConfiguration();
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();
 
builder.Services.AddSwaggerServices();
 
builder.Host.UseSerilog((context, services, configuration) =>
{
    SerilogExtensions.ConfigureSerilog(context, services, configuration);
});
var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();



// Initialise and seed the database on start-up
using (var scope = app.Services.CreateScope())
{
    try
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        initialiser.Initialise();
        initialiser.Seed();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialisation.");

        throw;
    }
}



//if (app.Environment.IsDevelopment())
//{
    app.UseSwaggerWithUI();
//}

app.UseRouting();
app.UseHttpMetrics();

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseMiddleware<UnauthorizedResponseMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapMetrics();
app.MapHealthChecks("/health");

app.Run();



