using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace VsCommuunitySample
{
    internal class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly TelemetryClient _tc;

        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger,
            IHostApplicationLifetime appLifetime,
            TelemetryClient tc)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _tc = tc;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (_tc.StartOperation<RequestTelemetry>("operation at StartAsync"))
            {
                _logger.LogWarning("StartAsync has been called.");

                _appLifetime.ApplicationStarted.Register(OnStarted);
                _appLifetime.ApplicationStopping.Register(OnStopping);
                _appLifetime.ApplicationStopped.Register(OnStopped);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("StopAsync has been called.");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogWarning("OnStarted has been called.");

            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            _logger.LogWarning("OnStopping has been called.");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogWarning("OnStopped has been called.");

            _tc.Flush();

            // https://github.com/microsoft/ApplicationInsights-dotnet/issues/407
            Task.Delay(5000).Wait();

            // Perform post-stopped activities here
        }
    }
}