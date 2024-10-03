using System.Text.RegularExpressions;

namespace AppSemTemplate.Extensions
{
    /// <summary>
    /// Implementa IOutboundParameterTransformer para transformar parâmetros de rota em formato "slugify".
    /// </summary>
    /// <remarks>
    /// Esta classe transforma strings em um formato adequado para URLs, convertendo camelCase ou PascalCase
    /// em kebab-case (todas as letras minúsculas separadas por hífens).
    /// </remarks>
    public class RouteSlugifyParameterTransformer : IOutboundParameterTransformer
    {
        /// <summary>
        /// Transforma o valor de saída em um formato "slugify".
        /// </summary>
        /// <param name="value">O valor a ser transformado.</param>
        /// <returns>
        /// Uma string transformada em formato "slugify" ou null se o valor de entrada for null.
        /// </returns>
        public string TransformOutbound(object value)
        {
            // Retorna null se o valor for null
            if (value is null)
            {
                return null;
            }

            // Usa regex para inserir um hífen entre uma letra minúscula seguida de uma maiúscula
            // Exemplo: "camelCase" se torna "camel-Case"
            var slugified = Regex.Replace(
                value.ToString()!,
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100));

            // Converte toda a string para minúsculas
            // Exemplo: "camel-Case" se torna "camel-case"
            return slugified.ToLowerInvariant();
        }
    }
}