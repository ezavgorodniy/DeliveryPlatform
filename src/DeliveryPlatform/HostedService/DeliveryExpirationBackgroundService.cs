using System.Threading;
using System.Threading.Tasks;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DeliveryPlatform.HostedService
{
    public class DeliveryExpirationBackgroundService : BackgroundService
    {
        private readonly DeliveryExpirationSettings _settings;
        private readonly IExpirationService _expirationService;
        private readonly ILogger<DeliveryExpirationBackgroundService> _logger;

        public DeliveryExpirationBackgroundService(IOptions<DeliveryExpirationSettings> settings,
            ILogger<DeliveryExpirationBackgroundService> logger,
            IExpirationService expirationService)
        {
            _settings = settings.Value;
            _logger = logger;
            _expirationService = expirationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("DeliveryExpirationBackgroundService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug("DeliveryExpiration background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("DeliveryExpiration task doing background work.");

                await _expirationService.UpdateExpirations();

                await Task.Delay(_settings.CheckExpirationTime, stoppingToken);
            }
        }
    }
}
