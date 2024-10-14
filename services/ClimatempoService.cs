using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

//Serviço que consulta a API do Climatempo e armazena os dados no repositório de clima.
public class ClimatempoService
{
    //Consulta a API do Climatempo, obtém dados de clima e insere no repositório.
    public static async Task ConsultarClimatempo(IClimatempoRepository climaRepository)
    {
        using (HttpClient client = ApiClientFactory.CreateClimatempoClient())
        {
            try
            {
                //// Faz uma solicitação GET à API do Climatempo para obter os dados do clima
                string response = await client.GetStringAsync("/api/v1/anl/synoptic/locale/BR?token=4dc636246f768016532b08ea5cf20994");

                using (JsonDocument document = JsonDocument.Parse(response))
                {
                    JsonElement root = document.RootElement;

                    foreach (JsonElement clima in root.EnumerateArray())
                    {
                        // Extrai as propriedades do clima: país, data e descrição
                        string pais = clima.GetProperty("country").GetString();
                        string data = clima.GetProperty("date").GetString();
                        string descricao = clima.GetProperty("text").GetString();

                        // Insere o clima no repositório, armazenando os dados no banco
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
