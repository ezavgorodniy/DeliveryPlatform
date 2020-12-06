using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.DataModels;

namespace DeliveryPlatform.Core.Mappers
{
    public class RecipientMapper : IRecipientMapper
    {
        public Recipient To(RecipientDto from)
        {
            if (from == null)
            {
                return null;
            }

            return new Recipient
            {
                Address = from.Address,
                Email = from.Email,
                Name = from.Name,
                PhoneNumber = from.PhoneNumber
            };
        }

        public RecipientDto From(Recipient to)
        {
            if (to == null)
            {
                return null;
            }

            return new RecipientDto
            {
                Address = to.Address,
                Email = to.Email,
                Name = to.Name,
                PhoneNumber = to.PhoneNumber
            };
        }
    }
}
