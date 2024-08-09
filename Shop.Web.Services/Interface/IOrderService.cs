using Shop.Web.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Services.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO?> GetOrderByIdAsync(Guid id);
        Task<OrderDTO?> GetOrderWithItemsAsync(Guid orderId);
        Task<IEnumerable<OrderDTO>> GetOrdersByProductIdAsync(Guid productId);
        Task AddOrderAsync(OrderDTO orderDto);
        Task UpdateOrderAsync(OrderDTO orderDto);
        Task DeleteOrderAsync(Guid id);
    }
}
