using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

//Serviço responsável por operações no banco de dados e integração com repositórios.
public class BancoDeDadosService
{
    private readonly ILogger<BancoDeDadosService> _logger;
    private readonly IIBGERepository _ibgeRepository;
    private readonly IClimatempoRepository _climaRepository;

    public BancoDeDadosService(ILogger<BancoDeDadosService> logger, IIBGERepository ibgeRepository, IClimatempoRepository climaRepository)
    {
        _logger = logger;
        _ibgeRepository = ibgeRepository;
        _climaRepository = climaRepository;
    }

    public void CriarBancoETabela()
    {
        using (var connection = new SqliteConnection("Data Source=ApisData.db"))
        {
            connection.Open();
            _logger.LogInformation("Conexão com o banco de dados aberta.");

            try
            {
                // Criação da tabela "Noticias"
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

                // Criação da tabela "Clima"
                var createTableClima = connection.CreateCommand();
                createTableClima.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Clima (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Pais TEXT NOT NULL,
                        Data TEXT NOT NULL,
                        Descricao TEXT NOT NULL
                    )";
                createTableClima.ExecuteNonQuery();
                _logger.LogInformation("Tabela 'Clima' criada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar tabelas no banco de dados.");
            }
        }
    }

    public void InserirNoticia(string noticiaId, string titulo, string editorias, string introducao, string dataPublicacao, string link)
    {
        _ibgeRepository.InserirNoticia(noticiaId, titulo, editorias, introducao, dataPublicacao, link);
    }

    public void InserirClima(string pais, string data, string descricao)
    {
        _climaRepository.InserirClima(pais, data, descricao);
    }
}
