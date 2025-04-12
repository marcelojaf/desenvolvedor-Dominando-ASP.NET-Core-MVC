using AppSemTemplate.Controllers;
using AppSemTemplate.Data;
using AppSemTemplate.Models;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Testes
{
    public class ControllerTests
    {
        [Fact]
        public void TesteController_Index_Sucesso()
        {
            // Arrange
            var controller = new TesteController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ProdutoController_Index_Sucesso()
        {
            // Arrange

            // Mocking the DbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Create the context using the options
            var ctx = new AppDbContext(options);

            ctx.Produtos.Add(new Produto
            {
                Id = 1,
                Nome = "Produto Teste",
                Imagem = "imagem-teste.jpg",
                Valor = 10.00m
            });
            ctx.Produtos.Add(new Produto
            {
                Id = 2,
                Nome = "Produto Teste 2",
                Imagem = "imagem-teste-2.jpg",
                Valor = 20.00m
            });
            ctx.Produtos.Add(new Produto
            {
                Id = 3,
                Nome = "Produto Teste 3",
                Imagem = "imagem-teste-3.jpg",
                Valor = 30.00m
            });
            ctx.SaveChanges();

            // Mocking the image upload service
            var imageUploadService = new Mock<IImageUploadService>();
            imageUploadService.Setup(s => s.UploadArquivo(It.IsAny<ModelStateDictionary>(), It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new ProdutosController(ctx, imageUploadService.Object);

            // Act
            var result = controller.Index().Result;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ProdutoController_CriarNovoProduto_Sucesso()
        {
            // Arrange

            // Mocking the DbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Create the context using the options
            var ctx = new AppDbContext(options);

            var fileMock = new Mock<IFormFile>();
            var fileName = "testfile.jpg";
            fileMock.Setup(f => f.FileName).Returns(fileName);

            // Mocking the image upload service
            var imageUploadService = new Mock<IImageUploadService>();
            imageUploadService.Setup(s => s.UploadArquivo(
                new ModelStateDictionary(),
                fileMock.Object,
                It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new ProdutosController(ctx, imageUploadService.Object);

            // Produto
            var produto = new Produto
            {
                Nome = "Produto Teste",
                ImagemUpload = fileMock.Object, // Mocking the file upload
                Valor = 10.00m
            };

            // Act
            var result = controller.CriarNovoProduto(produto).Result;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ProdutoController_CriarNovoProduto_ErroValidacaoProduto()
        {
            // Arrange

            // Mocking the DbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Create the context using the options
            var ctx = new AppDbContext(options);


            // Mocking the image upload service
            var imageUploadService = new Mock<IImageUploadService>();

            // Controller
            var controller = new ProdutosController(ctx, imageUploadService.Object);

            controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");

            // Produto
            var produto = new Produto
            {

            };

            // Act
            var result = controller.CriarNovoProduto(produto).Result;

            // Assert
            Assert.False(controller.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
        }
    }
}
