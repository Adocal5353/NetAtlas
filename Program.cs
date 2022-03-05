using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetAtlas.Data;
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<NetAtlasContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("NetAtlasContext")));

var connect = builder.Configuration.GetConnectionString("NetAtlasContext");
IServiceCollection serviceCollection = builder.Services.AddDbContext<NetAtlasContext>(options => options.UseMySql(connect, ServerVersion.AutoDetect(connect)));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseRouting();

app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
