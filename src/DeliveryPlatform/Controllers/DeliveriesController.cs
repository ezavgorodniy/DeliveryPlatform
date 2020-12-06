using System;
using System.Threading.Tasks;
using DeliveryPlatform.Attributes;
using DeliveryPlatform.Core.Exceptions;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using Identity.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace DeliveryPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IExecutionContext _executionContext;
        private readonly ILogger<DeliveriesController> _logger;

        public DeliveriesController(IDeliveryService deliveryService,
            IExecutionContext executionContext,
            ILogger<DeliveriesController> logger)
        {
            _deliveryService = deliveryService;
            _executionContext = executionContext;
            _logger = logger;
        }

        [Authorize(Role.User | Role.Partner)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var allDeliveries = await _deliveryService.GetAll(_executionContext);
                return Ok(allDeliveries);
            }
            catch (UnauthorizedException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                _logger.LogError("Error happened while Get all", e);
                return BadRequest();
            }
        }

        [Authorize(Role.User | Role.Partner)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var delivery = await _deliveryService.Get(_executionContext, id);
                return Ok(delivery);
            }
            catch (UnauthorizedException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                _logger.LogError("Error happened while Get by id", e);
                return BadRequest();
            }
        }

        [Authorize(Role.User)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleteResult = await _deliveryService.Delete(_executionContext, id);
                if (deleteResult)
                {
                    return NoContent();
                }

                return BadRequest();
            }
            catch (UnauthorizedException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                _logger.LogError("Error happened while Delete", e);
                return BadRequest();
            }
        }

        [Authorize(Role.User | Role.Partner)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] DeliveryDto deliveryDto)
        {
            try
            {
                if (deliveryDto == null)
                {
                    throw new ArgumentNullException(nameof(deliveryDto));
                }

                deliveryDto.Id = id;
                var updateResult = await _deliveryService.Update(_executionContext, deliveryDto);
                return Ok(updateResult);
            }
            catch (UnauthorizedException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                _logger.LogError("Error happened while Update", e);
                return BadRequest();
            }
        }

        [Authorize(Role.User)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeliveryDto deliveryDto)
        {
            try
            {
                if (deliveryDto == null)
                {
                    throw new ArgumentNullException(nameof(deliveryDto));
                }

                var createResult = await _deliveryService.Create(_executionContext, deliveryDto);
                return Ok(createResult);
            }
            catch (UnauthorizedException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                _logger.LogError("Error happened while Create", e);
                return BadRequest();
            }
        }
    }
}
