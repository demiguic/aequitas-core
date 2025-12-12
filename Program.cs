using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.InMemory;
using AequitasTracker.Data;
using AequitasTracker.Services;
using AequitasTracker.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfire(config =>
{
    config.UseInMemoryStorage();
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseRecommendedSerializerSettings();
});

builder.Services.AddHangfireServer();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Api, Swager, etc.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection for Services
builder.Services.AddHttpClient<AlphaVantageService>();
builder.Services.AddScoped<CalculationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Ensure database is created and apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire");
app.UseAuthorization();

app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

RecurringJob.AddOrUpdate<PriceUpdateJob>(
    "price-fetcher",
    job => job.ExecutePriceUpdateAsync(),
    Cron.Minutely
);

app.Run();