using AppSemTemplate.Data;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Identity;
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

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PodeExcluirPermanentemente", policy => policy.RequireRole("Amin"));
});

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
