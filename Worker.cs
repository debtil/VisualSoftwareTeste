using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _httpClient;
    private readonly BancoDeDados _bancoDeDados;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _bancoDeDados = new BancoDeDados(_logger);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        BancoDeDados.CriarBancoETabela();
        _logger.LogInformation("Banco e tabela criados com sucesso.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConsultaIBGE.ConsultarIBGE();
                _logger.LogInformation("Chamada à API IBGE realizada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao chamar a API IBGE.");
            }

            try
            {
                await ConsultaClimatempo.ConsultarClimatempo();
                _logger.LogInformation("Chamada à API do ClimaTempo realizada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao chamar a API do ClimaTempo.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker parando em: {time}", DateTimeOffset.Now);
        return base.StopAsync(stoppingToken);
    }
}
