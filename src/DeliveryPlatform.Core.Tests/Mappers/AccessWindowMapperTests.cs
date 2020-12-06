using System;
using DeliveryPlatform.Core.Mappers;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.DataModels;
using Xunit;

namespace DeliveryPlatform.Core.Tests.Mappers
{
    public class AccessWindowMapperTests
    {
        private readonly AccessWindowMapper _accessWindowMapper;

        public AccessWindowMapperTests()
        {
            _accessWindowMapper = new AccessWindowMapper();
        }

        [Fact]
        public void FromNullExpectNull()
        {
            var actual = _accessWindowMapper.From(null);

            Assert.Null(actual);
        }

        [Fact]
        public void FromExpectMapAllProperties()
        {
            var entity = new AccessWindow
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            var actual = _accessWindowMapper.From(entity);

            Assert.NotNull(actual);
            Assert.Equal(entity.StartTime, actual.StartTime);
            Assert.Equal(entity.EndTime, actual.EndTime);
        }
        
        [Fact]
        public void ToNullExpectNull()
        {
            var actual = _accessWindowMapper.To(null);

            Assert.Null(actual);
        }

        [Fact]
        public void ToExpectMapAllProperties()
        {
            var dto = new AccessWindowDto
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            var actual = _accessWindowMapper.To(dto);

            Assert.NotNull(actual);
            Assert.Equal(dto.StartTime, actual.StartTime);
            Assert.Equal(dto.EndTime, actual.EndTime);
        }
    }
}
