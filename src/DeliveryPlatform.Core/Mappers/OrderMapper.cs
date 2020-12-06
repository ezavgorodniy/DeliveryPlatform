using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.DataModels;

namespace DeliveryPlatform.Core.Mappers
{
    public class OrderMapper : IOrderMapper
    {
        public Order To(OrderDto from)
        {
            if (from == null)
            {
                return null;
            }

            return new Order
            {
                OrderNumber = from.OrderNumber,
                Sender = from.Sender
            };
        }

        public OrderDto From(Order to)
        {
            if (to == null)
            {
                return null;
            }

            return new OrderDto
            {
                OrderNumber = to.OrderNumber,
                Sender = to.Sender
            };
        }
    }
}
