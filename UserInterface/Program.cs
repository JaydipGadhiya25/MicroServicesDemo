using Microsoft.AspNetCore.Authentication.JwtBearer;
using UserInterface.Controllers.CustomClasses;
using UserInterface.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MyAppSetting>(builder.Configuration.GetSection("CuustomValue"));
builder.Services.AddScoped<RequestSender>();
builder.Services.AddScoped<ErrorHandler>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time   
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
