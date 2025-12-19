//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.EntityFrameworkCore;
//using System;



//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSession();
//app.UseSession();
////Dbcontext configuration (Sara)
//builder.Services.AddDbContext<OnlineBookStore.Data.AppDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("DefaultConnection")));

//// Add services to the container.
//builder.Services.AddControllersWithViews();


//// Cookie Authentication
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Account/Login";    
//        options.LogoutPath = "/Account/Logout";
//        options.AccessDeniedPath = "/Account/Login";
//        options.ExpireTimeSpan = TimeSpan.FromHours(12); 
//    });

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

////app.MapControllerRoute(
////    name: "areas",
////    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
////);


//app.UseHttpsRedirection(); 
//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();

////Seed database (Sara)
//OnlineBookStore.Data.AppDbInitializer.Seed(app);

//app.Run();

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
    });

var app = builder.Build();   

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();        
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

AppDbInitializer.Seed(app);

app.Run();
