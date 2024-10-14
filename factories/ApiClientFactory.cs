using System.Net.Http;

public static class ApiClientFactory
{
    public static HttpClient CreateClimatempoClient()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://apiadvisor.climatempo.com.br/");
        return client;
    }

    public static HttpClient CreateIBGEClient()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://servicodados.ibge.gov.br/");
        return client;
    }
}
