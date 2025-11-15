using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using StackExchange.Redis;
using Transactions.API.Db;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting Transactions API web application");

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddOpenApi();

    builder.Services.AddDbContext<TransactionsDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
               .EnableServiceProviderCaching(true)
               .EnableDetailedErrors()
               .EnableSensitiveDataLogging()
               .LogTo(Console.WriteLine, LogLevel.Information)
    );

    var redisConfig = builder.Configuration.GetSection("Redis:Configuration").Value;
    var redisInstanceName = builder.Configuration.GetSection("Redis:InstanceName").Value;

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConfig;
        options.InstanceName = redisInstanceName;
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transactions API V1");
            c.RoutePrefix = string.Empty;
        });
        app.MapOpenApi();
    }

    app.UseSerilogRequestLogging(); 
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("Transactions API started successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
