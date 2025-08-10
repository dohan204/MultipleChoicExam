using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MultipleChoicExam.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<UserAccount, IdentityRole>()
    .AddEntityFrameworkStores<EFCoreDbContext>() // thêm DbContext vào đây
    .AddDefaultTokenProviders();
builder.Services.AddDbContext<EFCoreDbContext>(options =>
    options.UseSqlServer(@"Server=FC-HAN\SQLEXPRESS;Database=TestProjectDB;Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddSession();
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Home/NoPermission";
    });

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
