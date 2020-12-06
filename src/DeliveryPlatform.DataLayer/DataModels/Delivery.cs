namespace DeliveryPlatform.DataLayer.DataModels
{
    public class Delivery
    {
        public string Id { get; set; }

        public DeliveryState State { get; set; }

        public AccessWindow AccessWindow { get; set; }

        public Recipient Recipient { get; set; }

        public Order Order { get; set; }
    }
}
