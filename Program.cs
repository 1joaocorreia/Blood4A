using Microsoft.EntityFrameworkCore;
using Blood4A.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Blood4A.Services;
using Blood4A.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => {
	options.Filters.Add(new RequireHttpsAttribute());
});

string? connectionString = builder.Configuration["Blood4A:ConnectionString"];

if (connectionString == null)
{
    throw new Exception("Not possible to get the ConnectionString from Secrets Manager");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<InformationService>();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/auth/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<RedirectMiddleware>();


// 🔹 Chamada ao SeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=auth}/{action=login}")
    .WithStaticAssets();

app.UseStaticFiles();

app.Run();
