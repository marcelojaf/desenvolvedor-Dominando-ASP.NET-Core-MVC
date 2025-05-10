using AppSemTemplate.Data;
using AppSemTemplate.Models;

namespace AppSemTemplate.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        private static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            {
                await context.Database.EnsureCreatedAsync();
                await EnsureSeedProducts(context);
            }
        }

        private static async Task EnsureSeedProducts(AppDbContext context)
        {
            if (context.Produtos.Any())
            {
                return;
            }

            var produtos = new List<Produto>
            {
                new Produto{
                    Nome = "Coca-Cola",
                    Imagem = "cocacola.jpg",
                    Valor = 10,
                    Processado = false,
                },
                new Produto{
                    Nome = "Fanta",
                    Imagem = "fanta.jpg",
                    Valor = 10,
                    Processado = false,
                },
                new Produto{
                    Nome = "Guaraná",
                    Imagem = "guarana.jpg",
                    Valor = 10,
                    Processado = false,
                },
                new Produto{
                    Nome = "Pepsi",
                    Imagem = "pepsi.jpg",
                    Valor = 10,
                    Processado = false,
                },
            };

            await context.Produtos.AddRangeAsync(produtos);
            await context.SaveChangesAsync();

            if (context.Users.Any())
            {
                return;
            }

            string userId = Guid.NewGuid().ToString();

            await context.Users.AddAsync(new Microsoft.AspNetCore.Identity.IdentityUser()
            {
                Id = userId,
                UserName = "teste@teste.com",
                NormalizedUserName = "TESTE@TESTE.COM",
                Email = "teste@teste.com",
                NormalizedEmail = "TESTE@TESTE.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEL+tM8oVZvZ+6VnO7rpGb1o3sexmz6wggHU6fIFHtMu0mDB2gDDWlMpBkk6VB0+hcw==",
                SecurityStamp = "B7RBYISWCPLGE4T7HDRMHBZZ7EYDBCCB",
                ConcurrencyStamp = "46df9a59-7258-482b-8402-4c07eee30193",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0
            });

            await context.SaveChangesAsync();

            if (context.UserClaims.Any())
            {
                return;
            }

            await context.UserClaims.AddAsync(new Microsoft.AspNetCore.Identity.IdentityUserClaim<string>()
            {
                UserId = userId,
                ClaimType = "Produtos",
                ClaimValue = "VI,ED,AD,EX"
            });

            await context.SaveChangesAsync();
        }
    }
}
