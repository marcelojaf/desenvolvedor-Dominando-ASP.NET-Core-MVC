using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AppSemTemplate.Extensions
{
    /// <summary>
    /// DataAnnotation personalizada para validar valores monetários no formato brasileiro (pt-BR).
    /// </summary>
    public class MoedaAttribute : ValidationAttribute
    {
        /// <summary>
        /// Sobrescreve o método de validação para verificar se o valor informado está no formato correto de moeda.
        /// </summary>
        /// <param name="value">Valor a ser validado.</param>
        /// <param name="validationContext">Contexto da validação.</param>
        /// <returns>ValidationResult.Success se válido, caso contrário, retorna uma mensagem de erro.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                // Converte o valor para decimal considerando a cultura brasileira (pt-BR)
                var moeda = Convert.ToDecimal(value, new CultureInfo("pt-BR"));
            }
            catch (Exception)
            {
                // Retorna uma mensagem de erro caso a conversão falhe
                return new ValidationResult("Moeda em formato inválido");
            }
            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// Adaptador para permitir que o atributo MoedaAttribute seja reconhecido no lado do cliente.
    /// </summary>
    public class MoedaAttributeAdapter : AttributeAdapterBase<MoedaAttribute>
    {
        /// <summary>
        /// Construtor do adaptador de validação de moeda.
        /// </summary>
        /// <param name="attribute">Atributo MoedaAttribute.</param>
        /// <param name="stringLocalizer">Localizador de strings para mensagens de erro.</param>
        public MoedaAttributeAdapter(MoedaAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
        }

        /// <summary>
        /// Adiciona atributos de validação ao HTML gerado para o campo.
        /// </summary>
        /// <param name="context">Contexto da validação do modelo.</param>
        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            // Adiciona os atributos de validação no HTML para suporte à validação do lado do cliente
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-moeda", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));
        }

        /// <summary>
        /// Retorna a mensagem de erro personalizada.
        /// </summary>
        /// <param name="validationContext">Contexto da validação.</param>
        /// <returns>Mensagem de erro de validação.</returns>
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return "Moeda em formato inválido";
        }
    }

    /// <summary>
    /// Provedor de adaptadores de validação para registrar o MoedaAttribute no ASP.NET Core.
    /// </summary>
    public class MoedaValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        /// <summary>
        /// Obtém um adaptador para um atributo de validação.
        /// </summary>
        /// <param name="attribute">Atributo de validação.</param>
        /// <param name="stringLocalizer">Localizador de strings para mensagens de erro.</param>
        /// <returns>Adaptador correspondente ao atributo.</returns>
        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is MoedaAttribute moedaAttribute)
            {
                return new MoedaAttributeAdapter(moedaAttribute, stringLocalizer);
            }
            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
