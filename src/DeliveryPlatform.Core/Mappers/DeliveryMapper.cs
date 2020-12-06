using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.DataModels;

namespace DeliveryPlatform.Core.Mappers
{
    public class DeliveryMapper : IDeliveryMapper
    {
        private readonly IOrderMapper _orderMapper;
        private readonly IRecipientMapper _recipientMapper;
        private readonly IAccessWindowMapper _accessWindowMapper;

        public DeliveryMapper(IOrderMapper orderMapper, IRecipientMapper recipientMapper,
            IAccessWindowMapper accessWindowMapper)
        {
            _orderMapper = orderMapper;
            _accessWindowMapper = accessWindowMapper;
            _recipientMapper = recipientMapper;
        }

        public Delivery To(DeliveryDto from)
        {
            if (from == null)
            {
                return null;
            }

            return new Delivery
            {
                Id = from.Id,
                AccessWindow = _accessWindowMapper.To(from.AccessWindow),
                Order = _orderMapper.To(from.Order),
                Recipient = _recipientMapper.To(from.Recipient),
                State = from.State
            };
        }

        public DeliveryDto From(Delivery to)
        {
            if (to == null)
            {
                return null;
            }

            return new DeliveryDto
            {
                Id = to.Id,
                AccessWindow = _accessWindowMapper.From(to.AccessWindow),
                Order = _orderMapper.From(to.Order),
                Recipient = _recipientMapper.From(to.Recipient),
                State = to.State
            };
        }
    }
}
