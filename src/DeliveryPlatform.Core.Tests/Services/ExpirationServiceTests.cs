using System.Threading.Tasks;
using DeliveryPlatform.Core.Services;
using DeliveryPlatform.DataLayer.Interfaces;
using Moq;
using Xunit;

namespace DeliveryPlatform.Core.Tests.Services
{
    public class ExpirationServiceTests
    {
        private readonly ExpirationService _expirationService;
        private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;

        public ExpirationServiceTests()
        {
            _mockDeliveryRepository = new Mock<IDeliveryRepository>();
            _expirationService = new ExpirationService(_mockDeliveryRepository.Object);
        }

        [Fact]
        public async Task UpdateExpirationsShouldCallMarkAsExpired()
        {
            await _expirationService.UpdateExpirations();

            _mockDeliveryRepository.Verify(repo => repo.MarkExpiredDeliveries(), Times.Once);
        }
    }
}
