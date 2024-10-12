using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

class ConsultaIBGE
{
    public static async Task ConsultarIBGE()
    {
        using (HttpClient client = new HttpClient())
        {
            string response = await client.GetStringAsync("http://servicodados.ibge.gov.br/api/v3/noticias/");

            using (JsonDocument document = JsonDocument.Parse(response))
            {
                JsonElement root = document.RootElement;
                JsonElement items = root.GetProperty("items");

                foreach (JsonElement item in items.EnumerateArray())
                {
                    // O Id da notícia pode ser String ou Number, então tratamos ambos os casos
                    string noticiaId;
                    if (item.GetProperty("id").ValueKind == JsonValueKind.String)
                    {
                        noticiaId = item.GetProperty("id").GetString();
                    }
                    else if (item.GetProperty("id").ValueKind == JsonValueKind.Number)
                    {
                        noticiaId = item.GetProperty("id").GetInt32().ToString(); // Converte o número para string
                    }
                    else
                    {
                        continue; // Se o tipo for outro, pula esse item
                    }

                    string editorias = item.GetProperty("editorias").GetString();

                    if (editorias == "economicas")
                    {
                        string titulo = item.GetProperty("titulo").GetString();
                        string tipo = item.GetProperty("editorias").GetString();
                        string introducao = item.GetProperty("introducao").GetString();
                        string dataPublicacao = item.GetProperty("data_publicacao").GetString();
                        string link = item.GetProperty("link").GetString();

                        BancoDeDados.InserirNoticia(noticiaId, titulo, tipo, introducao, dataPublicacao, link);
                    }
                }
            }
        }
    }
}
