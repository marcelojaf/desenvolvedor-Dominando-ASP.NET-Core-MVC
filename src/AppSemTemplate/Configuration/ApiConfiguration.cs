namespace AppSemTemplate.Configuration
{
    public class ApiConfiguration
    {
        /// <summary>
        /// Mesma chave do nó dentro do appsettings.json
        /// </summary>
        public const string ConfigName = "ApiConfiguration";

        public string Domain { get; set; }
        public string UserKey { get; set; }
        public string UserSecret { get; set; }
    }
}
