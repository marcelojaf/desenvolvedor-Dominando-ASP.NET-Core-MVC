using Microsoft.AspNetCore.Mvc;

namespace AppSemTemplate.Components
{
    /// <summary>
    /// ViewComponent responsável por contar e exibir o número de registros em uma lista.
    /// </summary>
    public class CounterViewComponent : ViewComponent
    {
        /// <summary>
        /// Método assíncrono que é chamado quando o ViewComponent é invocado.
        /// </summary>
        /// <param name="modelCounter">O número de registros a ser exibido.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo o resultado da view.</returns>
        public async Task<IViewComponentResult> InvokeAsync(int modelCounter)
        {
            // Simula uma operação assíncrona (pode ser removido se não houver necessidade real de assincronicidade)
            await Task.Delay(1);

            // Retorna a view padrão (Default.cshtml) passando o modelCounter como modelo
            return View(modelCounter);
        }
    }
}