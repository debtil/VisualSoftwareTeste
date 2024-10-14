using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ClimatempoService
{
    public static async Task ConsultarClimatempo(IClimatempoRepository climaRepository)
    {
        using (HttpClient client = ApiClientFactory.CreateClimatempoClient())
        {
            try
            {
                string response = await client.GetStringAsync("/api/v1/anl/synoptic/locale/BR?token=4dc636246f768016532b08ea5cf20994");

                using (JsonDocument document = JsonDocument.Parse(response))
                {
                    JsonElement root = document.RootElement;

                    foreach (JsonElement clima in root.EnumerateArray())
                    {
                        string pais = clima.GetProperty("country").GetString();
                        string data = clima.GetProperty("date").GetString();
                        string descricao = clima.GetProperty("text").GetString();

                        climaRepository.InserirClima(pais, data, descricao);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Erro ao acessar a API Climatempo: " + e.Message);
            }
        }
    }
}
