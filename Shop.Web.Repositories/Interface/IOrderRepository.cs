using Shop.Web.DTOs;
using Shop.Web.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Repositories.Interface
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task AddOrderWithItemsAsync(OrderDTO orderDto, List<OrderItemDTO> orderItemDtos);
        Task UpdateOrderWithItemsAsync(Order order, List<OrderItem> newOrderItems, List<OrderItem> existingOrderItems);
    }
}
