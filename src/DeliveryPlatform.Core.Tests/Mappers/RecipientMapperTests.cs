using DeliveryPlatform.Core.Mappers;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.DataModels;
using Xunit;

namespace DeliveryPlatform.Core.Tests.Mappers
{
    public class RecipientMapperTests
    {
        private readonly RecipientMapper _recipientMapper;

        public RecipientMapperTests()
        {
            _recipientMapper = new RecipientMapper();
        }

        [Fact]
        public void FromNullExpectNull()
        {
            var actual = _recipientMapper.From(null);

            Assert.Null(actual);
        }

        [Fact]
        public void FromExpectMapAllProperties()
        {
            var entity = new Recipient
            {
                Address = "expectedAddress",
                Email = "expectedEmail",
                Name = "expectedName",
                PhoneNumber = "expectedPhoneNumber"
            };

            var actual = _recipientMapper.From(entity);

            Assert.NotNull(actual);
            Assert.Equal(entity.Address, actual.Address);
            Assert.Equal(entity.Email, actual.Email);
            Assert.Equal(entity.Name, actual.Name);
            Assert.Equal(entity.PhoneNumber, actual.PhoneNumber);
        }
        
        [Fact]
        public void ToNullExpectNull()
        {
            var actual = _recipientMapper.To(null);

            Assert.Null(actual);
        }

        [Fact]
        public void ToExpectMapAllProperties()
        {
            var dto = new RecipientDto
            {
                Address = "expectedAddress",
                Email = "expectedEmail",
                Name = "expectedName",
                PhoneNumber = "expectedPhoneNumber"
            };

            var actual = _recipientMapper.To(dto);

            Assert.NotNull(actual);
            Assert.Equal(dto.Address, actual.Address);
            Assert.Equal(dto.Email, actual.Email);
            Assert.Equal(dto.Name, actual.Name);
            Assert.Equal(dto.PhoneNumber, actual.PhoneNumber);
        }
    }
}
