using Microsoft.EntityFrameworkCore;
using LogiTrackApi.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Add In-Memory Entity Framework Core Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("LogiTrackDb"));

// 2. Configure CORS to allow any origin, method, and header
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 3. Add API Controllers with JSON cycle handling
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// 4. Add API Explorer & Swagger (useful for direct API inspections in development)
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// 5. Seed In-Memory Database with realistic sample shipments
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    AppDbContext.SeedData(context);
}

// 6. Enable CORS
app.UseCors("AllowAll");

// 7. CRITICAL: app.UseDefaultFiles() MUST be called BEFORE app.UseStaticFiles()
// This ensures requesting '/' automatically serves 'wwwroot/index.html'
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
