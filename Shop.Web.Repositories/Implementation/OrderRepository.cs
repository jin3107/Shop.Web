using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersWithItemsAsync()
        {
            return await _context.Orders.AsNoTracking()
                .Include(o => o.OrderItems!)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithItemsAsync(Guid orderId)
        {
            return await _context.Orders.AsNoTracking()
                .Include(o => o.OrderItems!)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<bool> ProductExistsAsync(Guid productId)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == productId);
        }

        public async Task AddOrderAsync(Order order)
        {
            try
            {
                await _context.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

    }
}
