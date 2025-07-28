using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var config = hostContext.Configuration;

        // Registra o contexto do banco normalmente
        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        // Verifica se o Service Bus deve ser desativado
        var disableServiceBus = Environment.GetEnvironmentVariable("DISABLE_SERVICE_BUS");
        if (disableServiceBus == "true")
        {
            Console.WriteLine("⚠️ DISABLE_SERVICE_BUS habilitado — não registrando Worker.");
            return;
        }

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
