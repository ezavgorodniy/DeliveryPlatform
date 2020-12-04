namespace DeliveryPlatform.Core.Models
{
    public class DeliveryDto
    {
        public DeliveryStateDto State { get; set; }

        public AccessWindowDto AccessWindow { get; set; }

        public RecipientDto Recipient { get; set; }

        public OrderDto Order { get; set; }
    }
}
