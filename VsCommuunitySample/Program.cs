using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VsCommuunitySample
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<LifetimeEventsHostedService>();
                    services.AddApplicationInsightsTelemetryWorkerService();
                });
    }
}
