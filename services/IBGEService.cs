using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

//Serviço responsável por consultar a API de notícias do IBGE e armazenar os dados no repositório de notícias.
public class IBGEService
{
    public static async Task ConsultarIBGE(IIBGERepository ibgeRepository)
    {
        using (HttpClient client = ApiClientFactory.CreateIBGEClient())
        {
            // Faz a solicitação GET à API de notícias do IBGE
            string response = await client.GetStringAsync("/api/v3/noticias/");

            using (JsonDocument document = JsonDocument.Parse(response))
            {
                JsonElement root = document.RootElement;
                JsonElement items = root.GetProperty("items");

                foreach (JsonElement item in items.EnumerateArray())
                {
                    // Extrai o ID da notícia, verificando se é string ou número
                    string noticiaId;
                    if (item.GetProperty("id").ValueKind == JsonValueKind.String)
                    {
                        noticiaId = item.GetProperty("id").GetString();
                    }
                    else if (item.GetProperty("id").ValueKind == JsonValueKind.Number)
                    {
                        //Se o id for inteiro, converte para string
                        noticiaId = item.GetProperty("id").GetInt32().ToString();
                    }
                    else
                    {
                        continue;
                    }

                    // Obtém a categoria da notícia (editoria)
                    string editorias = item.GetProperty("editorias").GetString();

                    //Como um filtro a mais, somente notícias do tipo 'economicas' são adicionadas ao banco
                    if (editorias == "economicas")
                    {
                        string titulo = item.GetProperty("titulo").GetString();
                        string tipo = item.GetProperty("editorias").GetString();
                        string introducao = item.GetProperty("introducao").GetString();
                        string dataPublicacao = item.GetProperty("data_publicacao").GetString();
                        string link = item.GetProperty("link").GetString();

                        ibgeRepository.InserirNoticia(noticiaId, titulo, tipo, introducao, dataPublicacao, link);
                    }
                }
            }
        }
    }
}
