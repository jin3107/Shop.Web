using Microsoft.EntityFrameworkCore;
using Shop.Web.Models.Data;
using Shop.Web.Models.Entity;
using Shop.Web.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Repositories.Implementation
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public async Task AddOrderWithItemsAsync(Order order, List<OrderItem> orderItems)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.OrderItems.AddRangeAsync(orderItems);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task UpdateOrderWithItemsAsync(Order order, List<OrderItem> orderItems)
        {
            _context.Orders.Update(order);
            _context.OrderItems.UpdateRange(orderItems);
            await _context.SaveChangesAsync();
        }
    }
}
