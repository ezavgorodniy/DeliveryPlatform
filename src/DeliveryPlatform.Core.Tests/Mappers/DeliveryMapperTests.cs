using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Mappers;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.DataModels;
using Moq;
using Xunit;

namespace DeliveryPlatform.Core.Tests.Mappers
{
    public class DeliveryMapperTests
    {
        private readonly Mock<IAccessWindowMapper> _mockAccessWindowMapper;
        private readonly Mock<IOrderMapper> _mockOrderMapper;
        private readonly Mock<IRecipientMapper> _mockRecipientMapper;

        private readonly DeliveryMapper _deliveryMapper;

        public DeliveryMapperTests()
        {
            _mockOrderMapper = new Mock<IOrderMapper>();
            _mockAccessWindowMapper = new Mock<IAccessWindowMapper>();
            _mockRecipientMapper = new Mock<IRecipientMapper>();

            _deliveryMapper = new DeliveryMapper(_mockOrderMapper.Object,
                _mockRecipientMapper.Object,
                _mockAccessWindowMapper.Object);
        }

        [Fact]
        public void FromNullExpectNull()
        {
            var actual = _deliveryMapper.From(null);

            Assert.Null(actual);
        }

        [Fact]
        public void FromExpectMapAllProperties()
        {
            var entity = new Delivery
            {
                Id = "id", 
                Recipient = new Recipient(),
                AccessWindow = new AccessWindow(),
                Order = new Order(),
                State = DeliveryState.Completed
            };

            var expectedRecipientDto = new RecipientDto();
            _mockRecipientMapper.Setup(mapper => mapper.From(entity.Recipient)).Returns(expectedRecipientDto);

            var expectedAccessWindowDto = new AccessWindowDto();
            _mockAccessWindowMapper.Setup(mapper => mapper.From(entity.AccessWindow)).Returns(expectedAccessWindowDto);

            var expectedOrderDto = new OrderDto();
            _mockOrderMapper.Setup(mapper => mapper.From(entity.Order)).Returns(expectedOrderDto);

            var actual = _deliveryMapper.From(entity);

            Assert.NotNull(actual);
            Assert.Equal(entity.Id, actual.Id);
            Assert.Equal(expectedOrderDto, actual.Order);
            Assert.Equal(expectedRecipientDto, actual.Recipient);
            Assert.Equal(expectedAccessWindowDto, actual.AccessWindow);
            Assert.Equal(entity.State, actual.State);
        }
        
        [Fact]
        public void ToNullExpectNull()
        {
            var actual = _deliveryMapper.To(null);

            Assert.Null(actual);
        }

        [Fact]
        public void ToExpectMapAllProperties()
        {
            var dto = new DeliveryDto
            {
                Id = "expectedId",
                Recipient = new RecipientDto(),
                AccessWindow = new AccessWindowDto(),
                Order = new OrderDto(),
                State = DeliveryState.Completed
            };

            var expectedRecipient = new Recipient();
            _mockRecipientMapper.Setup(mapper => mapper.To(dto.Recipient)).Returns(expectedRecipient);

            var expectedAccessWindow = new AccessWindow();
            _mockAccessWindowMapper.Setup(mapper => mapper.To(dto.AccessWindow)).Returns(expectedAccessWindow);

            var expectedOrder = new Order();
            _mockOrderMapper.Setup(mapper => mapper.To(dto.Order)).Returns(expectedOrder);

            var actual = _deliveryMapper.To(dto);

            Assert.NotNull(actual);
            Assert.Equal(dto.Id, actual.Id);
            Assert.Equal(expectedOrder, actual.Order);
            Assert.Equal(expectedRecipient, actual.Recipient);
            Assert.Equal(expectedAccessWindow, actual.AccessWindow);
            Assert.Equal(dto.State, actual.State);
        }
    }
}
