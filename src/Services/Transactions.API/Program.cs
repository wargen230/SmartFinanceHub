using Microsoft.EntityFrameworkCore;
using Serilog;
using Transactions.API.Db;

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

builder.Services.AddDbContext<TransactionsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableServiceProviderCaching(true)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information)
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
