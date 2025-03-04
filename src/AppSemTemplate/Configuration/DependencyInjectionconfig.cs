using AppSemTemplate.Extensions;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace AppSemTemplate.Configuration
{
    public static class DependencyInjectionconfig
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IImageUploadService, ImageUploadService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            return builder;
        }
    }
}
