using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

public class IBGERepository : IIBGERepository
{
    private const string connectionString = "Data Source=ApisData.db";
    private readonly ILogger<IBGERepository> _logger;

    public IBGERepository(ILogger<IBGERepository> logger) // Injeção do logger via construtor
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Garantir que logger não é nulo
    }

    public bool NoticiaJaExiste(string noticiaId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            _logger.LogInformation($"Consultando se notícia já existe no banco: {noticiaId}");
            var select = connection.CreateCommand();
            select.CommandText = @"
                SELECT COUNT(1) 
                FROM Noticias 
                WHERE NoticiaId = $noticiaId";
            select.Parameters.AddWithValue("$noticiaId", noticiaId);
            long count = (long)select.ExecuteScalar();
            return count > 0;
        }
    }

    public void InserirNoticia(string noticiaId, string titulo, string editorias, string introducao, string dataPublicacao, string link)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            if (!NoticiaJaExiste(noticiaId))
            {
                var insert = connection.CreateCommand();
                insert.CommandText = @"
                    INSERT INTO Noticias (NoticiaId, Titulo, Tipo, Introducao, DataPublicacao, Link)
                    VALUES ($noticiaId, $titulo, $editorias, $introducao, $dataPublicacao, $link)";
                insert.Parameters.AddWithValue("$noticiaId", noticiaId);
                insert.Parameters.AddWithValue("$titulo", titulo);
                insert.Parameters.AddWithValue("$editorias", editorias);
                insert.Parameters.AddWithValue("$introducao", introducao);
                insert.Parameters.AddWithValue("$dataPublicacao", dataPublicacao);
                insert.Parameters.AddWithValue("$link", link);
                insert.ExecuteNonQuery();
                _logger.LogInformation($"Notícia '{titulo}' inserida com sucesso.");
            }else{
                _logger.LogInformation($"Notícia já existente no banco de dados: {noticiaId}");
            }
        }
    }
}
