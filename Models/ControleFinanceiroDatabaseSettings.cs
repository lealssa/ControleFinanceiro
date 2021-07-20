namespace ControleFinanceiro.Models
{
    public class ControleFinanceiroDatabaseSettings : IControleFinanceiroDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IControleFinanceiroDatabaseSettings
    {
        string ConnectionString { get; set; }  
        string DatabaseName { get; set; }    
    }
}