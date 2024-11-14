using BlazorApp_Auth.Data;
using BlazorApp_Auth.Components;
using Microsoft.EntityFrameworkCore;
using BlazorApp_Auth.Services;
using DevExpress.Blazor;
using Blazored.SessionStorage;

var builder = WebApplication.CreateBuilder(args);

// Thêm UserAuthDbContext với MySQL
builder.Services.AddDbContext<UserAuthDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// Đăng ký AuthService
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddBlazoredSessionStorage(); 

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Đăng ký DevExpress Blazor
builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);
builder.Services.AddMvc();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserAuthDbContext>();
    UserAuthDbContext.SeedData(dbContext);
}

// Cấu hình đường dẫn xử lý yêu cầu HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Cấu hình Blazor Server
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
