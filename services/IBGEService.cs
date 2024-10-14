using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

public class IBGEService
{
    public static async Task ConsultarIBGE(IIBGERepository ibgeRepository)
    {
        using (HttpClient client = ApiClientFactory.CreateIBGEClient())
        {
            string response = await client.GetStringAsync("/api/v3/noticias/");

            using (JsonDocument document = JsonDocument.Parse(response))
            {
                JsonElement root = document.RootElement;
                JsonElement items = root.GetProperty("items");

                foreach (JsonElement item in items.EnumerateArray())
                {
                    string noticiaId;
                    if (item.GetProperty("id").ValueKind == JsonValueKind.String)
                    {
                        noticiaId = item.GetProperty("id").GetString();
                    }
                    else if (item.GetProperty("id").ValueKind == JsonValueKind.Number)
                    {
                        noticiaId = item.GetProperty("id").GetInt32().ToString();
                    }
                    else
                    {
                        continue;
                    }

                    string editorias = item.GetProperty("editorias").GetString();

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
