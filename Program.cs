using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Thêm dòng này
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LAPTRINHWEB.Data;
using System;
using Microsoft.Extensions.Logging;
using LAPTRINHWEB.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

using DotNetEnv;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
var googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");

if (!string.IsNullOrEmpty(googleClientId))
{
    builder.Configuration["Authentication:Google:ClientId"] = googleClientId;
}

if (!string.IsNullOrEmpty(googleClientSecret))
{
    builder.Configuration["Authentication:Google:ClientSecret"] = googleClientSecret;
}

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();



// Thêm Entity Framework
builder.Services.AddDbContext<TourDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // User settings
    options.User.RequireUniqueEmail = false;
})
.AddEntityFrameworkStores<TourDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.CallbackPath = "/signin-google";

        // Map Google claims to Identity claims
        options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");

        options.SaveTokens = true;
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

});


var app = builder.Build();

// Cấu hình pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
// Định tuyến mặc định


app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

// Manager area route
app.MapControllerRoute(
    name: "manager",
    pattern: "Manager/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Manager" });

// TourGuide area route
app.MapControllerRoute(
    name: "tourguide",
    pattern: "TourGuide/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "TourGuide" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");

if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        Console.WriteLine($"🔍 Request: {context.Request.Method} {context.Request.Path}");
        await next();
    });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Đã xảy ra lỗi khi tạo dữ liệu mẫu.");
    }
}

app.Run();