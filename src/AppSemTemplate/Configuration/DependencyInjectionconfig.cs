using AppSemTemplate.Services;

namespace AppSemTemplate.Configuration
{
    public static class DependencyInjectionconfig
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IImageUploadService, ImageUploadService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return builder;
        }
    }
}
