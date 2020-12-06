using DeliveryPlatform.Core.Mappers;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.DataModels;
using Xunit;

namespace DeliveryPlatform.Core.Tests.Mappers
{
    public class OrderMapperTests
    {
        private readonly OrderMapper _orderMapper;

        public OrderMapperTests()
        {
            _orderMapper = new OrderMapper();
        }

        [Fact]
        public void FromNullExpectNull()
        {
            var actual = _orderMapper.From(null);

            Assert.Null(actual);
        }

        [Fact]
        public void FromExpectMapAllProperties()
        {
            var entity = new Order
            {
                Sender = "expectedSender",
                OrderNumber = "expectedOrderNumber"
            };

            var actual = _orderMapper.From(entity);

            Assert.NotNull(actual);
            Assert.Equal(entity.Sender, actual.Sender);
            Assert.Equal(entity.OrderNumber, actual.OrderNumber);
        }
        
        [Fact]
        public void ToNullExpectNull()
        {
            var actual = _orderMapper.To(null);

            Assert.Null(actual);
        }

        [Fact]
        public void ToExpectMapAllProperties()
        {
            var dto = new OrderDto
            {
                Sender = "expectedSender",
                OrderNumber = "expectedOrderNumber"
            };

            var actual = _orderMapper.To(dto);

            Assert.NotNull(actual);
            Assert.Equal(dto.Sender, actual.Sender);
            Assert.Equal(dto.OrderNumber, actual.OrderNumber);
        }
    }
}
