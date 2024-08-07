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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Orders.AddAsync(order);
                    await _context.SaveChangesAsync();

                    foreach (var item in orderItems)
                    {
                        item.OrderId = order.Id;
                        await _context.OrderItems.AddAsync(item);

                        var product = await _context.Products.FindAsync(item.ProductId);
                        if (product == null || product.Stock < item.Quantity)
                        {
                            throw new Exception($"Insufficient stock for product: {product?.Name ?? "Unknown"}");
                        }

                        product.Stock -= item.Quantity;
                        _context.Products.Update(product);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task UpdateOrderWithItemsAsync(Order order, List<OrderItem> newOrderItems, List<OrderItem> existingOrderItems)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Orders.Update(order);
                    _context.OrderItems.RemoveRange(existingOrderItems);
                    foreach (var item in newOrderItems)
                    {
                        item.Id = Guid.NewGuid();
                        item.OrderId = order.Id;
                        await _context.OrderItems.AddAsync(item);

                        var product = await _context.Products.FindAsync(item.ProductId);
                        if (product == null || product.Stock < item.Quantity)
                        {
                            throw new Exception($"Insufficient stock for product: {product?.Name ?? "Unknown"}");
                        }

                        product.Stock -= item.Quantity;
                        _context.Products.Update(product);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
