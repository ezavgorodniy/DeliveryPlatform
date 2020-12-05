using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryPlatform.Attributes;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using Identity.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace DeliveryPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly ILogger<DeliveriesController> _logger;
        private readonly IDeliveryService _deliveryService;
        private readonly IExecutionContext _executionContext;


        public DeliveriesController(ILogger<DeliveriesController> logger,
            IDeliveryService deliveryService,
            IExecutionContext executionContext)
        {
            _logger = logger;
            _deliveryService = deliveryService;
            _executionContext = executionContext;
        }

        [HttpGet]
        [Authorize(Role.User | Role.Partner)]
        public Task<IEnumerable<DeliveryDto>> Get()
        {
            return _deliveryService.GetAll(_executionContext);
        }

        [HttpGet("{id}")]
        [Authorize(Role.User | Role.Partner)]
        public Task<DeliveryDto> Get(string id)
        {
            return _deliveryService.Get(_executionContext, id);
        }

        [Authorize(Role.User)]
        [HttpDelete("{id}")]
        public Task Delete(string id)
        {
            return _deliveryService.Delete(_executionContext, id);
        }

        [HttpPut("{id}")]
        [Authorize(Role.User | Role.Partner)]
        public Task Update(string id, [FromBody] DeliveryDto deliveryDto)
        {
            // deliveryDto.Id = id;
            return _deliveryService.Update(_executionContext, deliveryDto);
        }

        [Authorize(Role.User)]
        [HttpPost]
        public Task<DeliveryDto> Create([FromBody] DeliveryDto deliveryDto)
        {
            return _deliveryService.Create(_executionContext, deliveryDto);
        }
    }
}
