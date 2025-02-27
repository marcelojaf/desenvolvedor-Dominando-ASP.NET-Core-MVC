using AppSemTemplate.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppSemTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApiConfiguration _apiConfig;

        public HomeController(IConfiguration configuration, IOptions<ApiConfiguration> apiConfig)
        {
            _configuration = configuration;
            _apiConfig = apiConfig.Value;
        }

        public IActionResult Index()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var secret = _apiConfig.UserSecret;

            return View();
        }
    }
}
