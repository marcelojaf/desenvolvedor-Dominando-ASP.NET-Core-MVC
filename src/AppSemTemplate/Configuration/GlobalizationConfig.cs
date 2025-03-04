using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace AppSemTemplate.Configuration
{
    public static class GlobalizationConfig
    {
        /// <summary>
        /// Forçar uma cultura no sistema pois por padrão, usa a cultura do browser
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        //public static WebApplication UseGlobalizationConfig(this WebApplication app)
        //{
        //    var defaultCulture = new CultureInfo("pt-BR");

        //    var localizationOptions = new RequestLocalizationOptions
        //    {
        //        DefaultRequestCulture = new RequestCulture(defaultCulture),
        //        SupportedCultures = new List<CultureInfo> { defaultCulture },
        //        SupportedUICultures = new List<CultureInfo> { defaultCulture }
        //    };

        //    app.UseRequestLocalization(localizationOptions);

        //    return app;
        //}

        public static WebApplication UseGlobalizationConfig(this WebApplication app)
        {
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            return app;
        }


        /// <summary>
        /// Adiciona a configuração de globalização ao construtor do aplicativo.
        /// </summary>
        /// <param name="builder">O construtor do aplicativo web.</param>
        /// <returns>O construtor do aplicativo web com a configuração de globalização adicionada.</returns>
        public static WebApplicationBuilder AddGlobalizationConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    "pt-BR",
                    "en-US"
                };

                options.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            });

            return builder;
        }
    }
}
