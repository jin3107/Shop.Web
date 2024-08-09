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
        Task<IEnumerable<Order>> GetAllOrdersWithItemsAsync();
        Task<Order?> GetOrderWithItemsAsync(Guid orderId);
        Task AddOrderAsync(Order order);
        Task<bool> ProductExistsAsync(Guid productId);
    }
}
