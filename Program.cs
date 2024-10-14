using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.IO;
using Serilog.Sinks.File;


public class Program
{
    public static void Main(string[] args)
    {
        // Garantir que o diretório de logs exista
        if (!Directory.Exists("logs"))
        {
            Directory.CreateDirectory("logs");
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()                   
            .WriteTo.File("logs/log.txt",
                          rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Iniciando o serviço de integração.");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Erro fatal ao iniciar o serviço.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();

                services.AddSingleton<IIBGERepository, IBGERepository>();
                services.AddSingleton<IClimatempoRepository, ClimatempoRepository>();

                services.AddSingleton<IBGEService>();
                services.AddSingleton<ClimatempoService>();
                services.AddSingleton<BancoDeDadosService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog();
            });
}
