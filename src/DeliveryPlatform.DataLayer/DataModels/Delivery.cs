using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryPlatform.DataLayer.DataModels
{
    public class Delivery
    {
        public DeliveryState State { get; set; }

        public AccessWindow AccessWindow { get; set; }

        public Recipient Recipient { get; set; }

        public Order Order { get; set; }
    }
}
