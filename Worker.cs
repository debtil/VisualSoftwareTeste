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
    private readonly IIBGERepository _ibgeRepository;
    private readonly IClimatempoRepository _climaRepository;
    private readonly BancoDeDadosService _bancoDeDadosService;

    public Worker(ILogger<Worker> logger, BancoDeDadosService bancoDeDadosService, IIBGERepository ibgeRepository, IClimatempoRepository climaRepository)
    {
        _logger = logger;
        _bancoDeDadosService = bancoDeDadosService;
        _ibgeRepository = ibgeRepository;
        _climaRepository = climaRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bancoDeDadosService.CriarBancoETabela();

       // while (!stoppingToken.IsCancellationRequested)
        //{
            _logger.LogInformation("Worker iniciado às: {time}", DateTimeOffset.Now);

            await IBGEService.ConsultarIBGE(_ibgeRepository);
            await ClimatempoService.ConsultarClimatempo(_climaRepository);

            _logger.LogInformation("Execução finalizada.");
            _logger.LogInformation("Worker finalizado às: {time}", DateTimeOffset.Now);

            //await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        //}
    }
}

