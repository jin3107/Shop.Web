using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.DTOs;
using Shop.Web.Services.Interface;

namespace Shop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderItems()
        {
            var orderItems = await _orderItemService.GetAllOrderItemsAsync();
            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItemById(Guid id)
        {
            var orderItem = await _orderItemService.GetOrderItemByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem([FromBody] OrderItemDTO orderItemDto)
        {
            await _orderItemService.AddOrderItemAsync(orderItemDto);
            return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItemDto.Id }, orderItemDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(Guid id, [FromBody] OrderItemDTO orderItemDto)
        {
            if (id != orderItemDto.Id)
            {
                return BadRequest();
            }

            await _orderItemService.UpdateOrderItemAsync(orderItemDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(Guid id)
        {
            await _orderItemService.DeleteOrderItemAsync(id);
            return NoContent();
        }
    }
}
