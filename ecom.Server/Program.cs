using Microsoft.EntityFrameworkCore;
using ecom.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure DbContext based on environment
var environment = builder.Environment.EnvironmentName;
var connectionString = environment switch
{
    "Production" => builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Production connection string not found."),
    "Beta" => builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Beta connection string not found."),
    "QA" => builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("QA connection string not found."),
    _ => builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Development connection string not found.")
};

// Add DbContext configuration with environment-specific connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
