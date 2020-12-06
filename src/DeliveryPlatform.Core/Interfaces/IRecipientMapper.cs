using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.DataModels;
using Shared.Interfaces;

namespace DeliveryPlatform.Core.Interfaces
{
    public interface IRecipientMapper : IMapper<RecipientDto, Recipient>
    {
    }
}
