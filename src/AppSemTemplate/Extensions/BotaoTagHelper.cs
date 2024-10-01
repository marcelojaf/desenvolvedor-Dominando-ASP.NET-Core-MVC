using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AppSemTemplate.Extensions
{
    /// <summary>
    /// TagHelper personalizado para criar botões com ícones e estilos predefinidos.
    /// </summary>
    /// <remarks>
    /// Este TagHelper pode ser aplicado a qualquer elemento HTML que tenha os atributos
    /// 'tipo-botao' e 'route-id'.
    /// </remarks>
    [HtmlTargetElement("*", Attributes = "tipo-botao, route-id")]
    public class BotaoTagHelper : TagHelper
    {
        // Injeção de dependência para acessar o contexto HTTP atual
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Construtor que injeta o IHttpContextAccessor.
        /// </summary>
        /// <param name="contextAccessor">Fornece acesso ao contexto HTTP atual.</param>
        public BotaoTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Define o tipo do botão a ser renderizado.
        /// </summary>
        [HtmlAttributeName("tipo-botao")]
        public TipoBotao TipoBotaoSelecao { get; set; }

        /// <summary>
        /// ID da rota para a ação do botão.
        /// </summary>
        [HtmlAttributeName("route-id")]
        public int RouteId { get; set; }

        // Variáveis para armazenar as configurações do botão
        private string nomeAction;
        private string nomeClasse;
        private string spanIcone;

        /// <summary>
        /// Processa o TagHelper, renderizando o botão conforme as propriedades definidas.
        /// </summary>
        /// <param name="context">Fornece informações sobre o contexto de execução do TagHelper.</param>
        /// <param name="output">Permite manipular a saída HTML do TagHelper.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Chama o método da classe base (opcional, mas boa prática)
            base.Process(context, output);

            // Define as configurações do botão com base no tipo selecionado
            switch (TipoBotaoSelecao)
            {
                case TipoBotao.Detalhes:
                    nomeAction = "Details";
                    nomeClasse = "btn btn-info";
                    spanIcone = "fa fa-search";
                    break;
                case TipoBotao.Editar:
                    nomeAction = "Edit";
                    nomeClasse = "btn btn-warning";
                    spanIcone = "fa fa-pencil-alt";
                    break;
                case TipoBotao.Excluir:
                    nomeAction = "Delete";
                    nomeClasse = "btn btn-danger";
                    spanIcone = "fa fa-trash";
                    break;
            }

            // Obtém o nome do controller atual da rota
            var controller = _contextAccessor.HttpContext.GetRouteData().Values["controller"].ToString();

            // Define o elemento de saída como uma âncora (link)
            output.TagName = "a";

            // Define o atributo href com a URL da ação
            output.Attributes.SetAttribute("href", $"{controller}/{nomeAction}/{RouteId}");

            // Define a classe CSS do botão
            output.Attributes.SetAttribute("class", nomeClasse);

            // Cria um elemento span para o ícone
            var iconSpan = new TagBuilder("span");
            iconSpan.AddCssClass(spanIcone);

            // Adiciona o span do ícone ao conteúdo do botão
            output.Content.AppendHtml(iconSpan);
        }
    }

    /// <summary>
    /// Enumeração dos tipos de botões suportados pelo BotaoTagHelper.
    /// </summary>
    public enum TipoBotao
    {
        Detalhes = 1,
        Editar = 2,
        Excluir = 3
    }
}