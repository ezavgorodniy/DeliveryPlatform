using DeliveryPlatform.DataLayer.DataModels;

namespace DeliveryPlatform.Core.Models
{
    public class DeliveryDto
    {
        public string Id { get; set; }
        
        public DeliveryState State { get; set; }

        public AccessWindowDto AccessWindow { get; set; }

        public RecipientDto Recipient { get; set; }

        public OrderDto Order { get; set; }
    }
}
