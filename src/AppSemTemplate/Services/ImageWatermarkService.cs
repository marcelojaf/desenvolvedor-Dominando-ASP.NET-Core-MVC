using AppSemTemplate.Configuration;
using AppSemTemplate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace AppSemTemplate.Services
{
    /// <summary>
    /// Serviço que processa imagens de produtos em background e aplica uma marca d'água.
    /// </summary>
    public class ImageWatermarkService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _watermarkPath;

        public ImageWatermarkService(IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
        {
            _serviceProvider = serviceProvider;
            _webHostEnvironment = webHostEnvironment;
            _watermarkPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "logo_watermark.png");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var configuration = scope.ServiceProvider.GetRequiredService<IOptions<ApiConfiguration>>();

                var produtosNaoProcessados = await dbContext.Produtos
                    .Where(p => !p.Processado)
                    .ToListAsync();

                foreach (var produto in produtosNaoProcessados)
                {
                    try
                    {
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", produto.Imagem);
                        var newImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "prod_" + produto.Imagem);

                        using Image image = await Image.LoadAsync(imagePath, stoppingToken);
                        using Image watermark = await Image.LoadAsync(_watermarkPath, stoppingToken);

                        image.Mutate(ctx =>
                        {
                            ctx.Resize(381, 499);
                            var position = new Point(image.Width - watermark.Width - 10, image.Height - watermark.Height - 10);
                            ctx.DrawImage(watermark, position, 1f);
                        });

                        await image.SaveAsync(newImagePath, new PngEncoder(), stoppingToken);

                        produto.Processado = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                await dbContext.SaveChangesAsync();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
