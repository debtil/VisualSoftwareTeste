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

    //Constrututor que inicializa os serviços injetados
    public Worker(ILogger<Worker> logger, BancoDeDadosService bancoDeDadosService, IIBGERepository ibgeRepository, IClimatempoRepository climaRepository)
    {
        _logger = logger;
        _bancoDeDadosService = bancoDeDadosService;
        _ibgeRepository = ibgeRepository;
        _climaRepository = climaRepository;
    }

    //Método para a execução das consultas
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //Cria o banco de dados e as tabelas, se ainda não existirem.
        _bancoDeDadosService.CriarBancoETabela();

        //Log de inicialização do Worker
        _logger.LogInformation("Worker iniciado às: {time}", DateTimeOffset.Now);

        //Consultas nas APIs
        await IBGEService.ConsultarIBGE(_ibgeRepository);
        await ClimatempoService.ConsultarClimatempo(_climaRepository);

        //Logs de finalização
        _logger.LogInformation("Execução finalizada.");
        _logger.LogInformation("Worker finalizado às: {time}", DateTimeOffset.Now);
    }
}

