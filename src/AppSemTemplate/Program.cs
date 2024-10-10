using AppSemTemplate.Data;
using AppSemTemplate.Extensions;
using AppSemTemplate.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// TRANSFORMADOR DE ROTA
//// Adiciona e configura os serviços de roteamento
//builder.Services.AddRouting(options =>
//{
//    // Mapeia o constraint "slugify" para o tipo RouteSlugifyParameterTransformer
//    // Isso permite o uso de [slugify] em atributos de rota para aplicar a transformação
//    options.ConstraintMap["slugify"] = typeof(RouteSlugifyParameterTransformer);
//});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IImageUploadService, ImageUploadService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

// TRANSFORMADOR DE ROTA
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller:slugify=Home}/{action:slugify=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
