using BlazorApp_Auth.Data;
using BlazorApp_Auth.Components;
using BlazorApp_Auth.Services;
using Microsoft.EntityFrameworkCore;
using DevExpress.Blazor;
using Blazored.SessionStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserAuthDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))
    ));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddScoped<IAuthService, AuthService>();      
builder.Services.AddScoped<IProductService, ProductService>(); 
builder.Services.AddBlazoredSessionStorage();    

builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);
builder.Services.AddMvc();

var app = builder.Build();

// Tạo dữ liệu ban đầu
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserAuthDbContext>();
    UserAuthDbContext.SeedData(dbContext);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();             
}

app.UseHttpsRedirection();
app.UseStaticFiles();      

//Xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// Cấu hình đường dẫn Blazor Server để render component
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
