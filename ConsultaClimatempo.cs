using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class ConsultaClimatempo
{
    public static async Task ConsultarClimatempo()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string response = await client.GetStringAsync("http://apiadvisor.climatempo.com.br/api/v1/anl/synoptic/locale/BR?token=4dc636246f768016532b08ea5cf20994");

                using (JsonDocument document = JsonDocument.Parse(response))
                {
                    JsonElement root = document.RootElement;

                    foreach (JsonElement clima in root.EnumerateArray())
                    {
                        string pais = clima.GetProperty("country").GetString();
                        string data = clima.GetProperty("date").GetString();
                        string descricao = clima.GetProperty("text").GetString();

                        BancoDeDados.InserirClima(pais, data, descricao);
                    }
                    Console.WriteLine("Dados climáticos inseridos no banco de dados.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Erro ao acessar a nova API: " + e.Message);
            }
        }
    }
}
