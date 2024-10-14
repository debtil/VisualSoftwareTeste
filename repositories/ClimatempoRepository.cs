using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

public class ClimatempoRepository : IClimatempoRepository
{
    private const string connectionString = "Data Source=ApisData.db";
    private readonly ILogger<ClimatempoRepository> _logger;

    public ClimatempoRepository(ILogger<ClimatempoRepository> logger){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool DataClimaExiste(string data)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            _logger.LogInformation($"Consultando se a data já existe no banco: {data}");
            var select = connection.CreateCommand();
            select.CommandText = @"
                SELECT COUNT(1) 
                FROM Clima 
                WHERE Data = $date";
            select.Parameters.AddWithValue("$date", data);
            long count = (long)select.ExecuteScalar();
            return count > 0;
        }
    }

    public void InserirClima(string pais, string data, string descricao)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            if (!DataClimaExiste(data))
            {
                var insert = connection.CreateCommand();
                insert.CommandText = @"
                    INSERT INTO Clima (Pais, Data, Descricao)
                    VALUES ($country, $date, $text)";
                insert.Parameters.AddWithValue("$country", pais);
                insert.Parameters.AddWithValue("$date", data);
                insert.Parameters.AddWithValue("$text", descricao);
                insert.ExecuteNonQuery();
                _logger.LogInformation($"Clima do dia '{data}' inserido com sucesso.");
            }else{
                _logger.LogInformation($"Já existe um clima com esta data: {data}");
            }
        }
    }
}
