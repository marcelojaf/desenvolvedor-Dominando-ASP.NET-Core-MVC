using Microsoft.AspNetCore.Localization;
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
        public static WebApplication UseGlobalizationConfig(this WebApplication app)
        {
            var defaultCulture = new CultureInfo("pt-BR");

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };

            app.UseRequestLocalization(localizationOptions);

            return app;
        }
    }
}
