using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.DataModels;

namespace DeliveryPlatform.Core.Mappers
{
    public class AccessWindowMapper : IAccessWindowMapper
    {
        public AccessWindow To(AccessWindowDto from)
        {
            if (from == null)
            {
                return null;
            }

            return new AccessWindow
            {
                StartTime = from.StartTime,
                EndTime = from.EndTime
            };
        }

        public AccessWindowDto From(AccessWindow to)
        {
            if (to == null)
            {
                return null;
            }

            return new AccessWindowDto
            {
                StartTime = to.StartTime,
                EndTime = to.EndTime
            };
        }
    }
}
