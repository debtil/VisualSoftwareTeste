using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

class BancoDeDados
{
    private const string connectionString = "Data Source=noticias.db";
    private static ILogger _logger;

    public BancoDeDados(ILogger logger)
    {
        _logger = logger;
    }

    public static void CriarBancoETabela(){
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            _logger.LogInformation("Conexão com o banco de dados aberta.");

            try{
                // Criar a tabela de notícias se não existir
                var createTableNoticia = connection.CreateCommand();
                createTableNoticia.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Noticias (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Tipo TEXT,
                        Titulo TEXT NOT NULL,
                        Introducao TEXT NOT NULL,
                        DataPublicacao TEXT NOT NULL,
                        Link TEXT NOT NULL,
                        NoticiaId TEXT UNIQUE
                    )";
                createTableNoticia.ExecuteNonQuery();
                _logger.LogInformation("Tabela 'Noticias' criada com sucesso.");

                var createTableClimaCmd = connection.CreateCommand();
                createTableClimaCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Clima (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Pais TEXT NOT NULL,
                        Data TEXT NOT NULL,
                        Descricao TEXT NOT NULL
                    )";
                createTableClimaCmd.ExecuteNonQuery();
                _logger.LogInformation("Tabela 'Clima' criada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar tabelas no banco de dados.");
            }
        }
    }

    // Método para verificar se a notícia já existe no banco pelo NoticiaId
    public static bool NoticiaJaExiste(string noticiaId){
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
            return count > 0; // Retorna true se a notícia já existir
        }
    }

    public static void InserirNoticia(string noticiaId, string titulo, string editorias, string introducao, string dataPublicacao, string link){
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Verificar se a notícia já existe
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
            }
            else
            {
                _logger.LogInformation($"Notícia já existente no banco de dados: {noticiaId}");
            }
        }
    }

     public static bool DataClimaExiste(string data){
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

    public static void InserirClima(string pais, string data, string descricao){
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
            }
            else
            {
                _logger.LogInformation($"Já existe um clima com esta data: {data}");
            }
        }
    }
}