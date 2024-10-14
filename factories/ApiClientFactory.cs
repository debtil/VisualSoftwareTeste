using System.Net.Http;

public static class ApiClientFactory
{
    //Aqui é feita a criação das instâncias do HttpClient para acessar as APIs
    public static HttpClient CreateClimatempoClient()
    {
        var client = new HttpClient();
        //define endpoint base da API
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
