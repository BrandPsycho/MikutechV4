using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.Cookies;
using Protov4.DAO;


var builder = WebApplication.CreateBuilder(args);
//hola
// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews();
// Configuración para acceder a la cadena de conexión desde appsettings.json
IConfiguration configuration = builder.Configuration;
builder.Services.AddSingleton(new DbConnection(configuration));

builder.Services.AddTransient<UsuariosDAO>();
builder.Services.AddSingleton<IMongoClient>(new MongoClient(configuration.GetConnectionString("MongoDBConnection")));
builder.Services.AddScoped<DBMongo>();
builder.Services.AddScoped<MikuTechFactory, MikutechDAO>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Acceso/Login";

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("id_rol_user", "1"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireClaim("id_rol_user", "2"));
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
app.UseSession();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();