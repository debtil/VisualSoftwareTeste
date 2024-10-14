public interface IClimatempoRepository
{
    bool DataClimaExiste(string data);
    void InserirClima(string pais, string data, string descricao);
}
