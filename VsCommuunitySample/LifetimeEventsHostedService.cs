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
            _logger.LogInformation("StartAsync has been called.");

            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync has been called.");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            using (_tc.StartOperation<RequestTelemetry>("operation"))
            {
                _logger.LogInformation("OnStarted has been called. (appsettings.json)");
                _tc.TrackEvent("OnStarted event has finised. (appsettings.json)");
                _tc.Flush();
            }

            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");

            // Perform post-stopped activities here
        }
    }
}