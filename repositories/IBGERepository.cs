using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

//Repositório para realizar as operações da API do IBGE
public class IBGERepository : IIBGERepository
{
    private const string connectionString = "Data Source=ApisData.db";
    private readonly ILogger<IBGERepository> _logger;

    public IBGERepository(ILogger<IBGERepository> logger)
    {
        //Verifica se o logger foi injetado corretamente
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    //Verifica se a notícia já existe, para que não fique duplicada no banco de dados
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

            //Executa a consulta e retorna o resultado
            long count = (long)select.ExecuteScalar();
            return count > 0;
        }
    }

    //Insere uma notícia no banco de dados
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
