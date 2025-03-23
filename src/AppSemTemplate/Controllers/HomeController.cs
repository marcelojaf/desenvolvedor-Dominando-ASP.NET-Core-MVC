using AppSemTemplate.Configuration;
using AppSemTemplate.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace AppSemTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApiConfiguration _apiConfig;
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IConfiguration configuration, 
            IOptions<ApiConfiguration> apiConfig, 
            ILogger<HomeController> logger,
            IStringLocalizer<HomeController> localizer)
        {
            _configuration = configuration;
            _apiConfig = apiConfig.Value;
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            //_logger.LogCritical(">>>>> Log de Critical");
            //_logger.LogError(">>>>> Log de Error");
            //_logger.LogWarning(">>>>> Log de Warning");
            //_logger.LogInformation(">>>>> Log de Information");
            //_logger.LogDebug(">>>>> Log de Debug");
            //_logger.LogTrace(">>>>> Log de Trace");


            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            var secret = _apiConfig.UserSecret;


            ViewData["Mensagem"] = _localizer["Seja bem vindo!"];

            if (Request.Cookies.TryGetValue("MeuCookie", out string valor))
            {
                ViewData["MeuCookie"] = valor;
            }

            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [Route("cookies")]
        public IActionResult Cookie()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            };

            Response.Cookies.Append("MeuCookie", "Dados do Cookie", cookieOptions);

            return View();
        }

        [Route("teste")]
        public IActionResult Teste()
        {
            throw new Exception("ALGO ERRADO NÃO ESTAVA CERTO!");

            return View("Index");
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}
