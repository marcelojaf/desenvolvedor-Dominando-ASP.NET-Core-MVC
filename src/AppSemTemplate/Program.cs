using AppSemTemplate.Data;
using AppSemTemplate.Extensions;
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

builder.Services.AddScoped<IImageUploadService, ImageUploadService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PodeExcluirPermanentemente", policy => policy.RequireRole("Amin"));
    options.AddPolicy("VerProdutos", policy => policy.RequireClaim("Produtos", "VI"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// TRANSFORMADOR DE ROTA
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller:slugify=Home}/{action:slugify=Index}/{id?}");

//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//app.MapAreaControllerRoute("AreaProdutos", "Produtos", "Produtos/{controller=Cadastro}/{action=Index}/{id?}");
//app.MapAreaControllerRoute("AreaVendas", "Vendas", "Vendas/{controller=Gestao}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
