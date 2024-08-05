using Shop.Web.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Services.Interface
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDTO>> GetAllOrderItemsAsync();
        Task<OrderItemDTO> GetOrderItemByIdAsync(Guid id);
        Task AddOrderItemAsync(OrderItemDTO orderItemDto);
        Task UpdateOrderItemAsync(OrderItemDTO orderItemDto);
        Task DeleteOrderItemAsync(Guid id);
    }
}
