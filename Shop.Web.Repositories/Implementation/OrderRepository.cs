using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Web.DTOs;
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
        public OrderRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public async Task AddOrderWithItemsAsync(OrderDTO orderDto, List<OrderItemDTO> orderItemDtos)
        {
            var order = _mapper.Map<Order>(orderDto);
            var orderItems = _mapper.Map<List<OrderItem>>(orderItemDtos);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Thêm Order trước
                    await _context.Orders.AddAsync(order);
                    await _context.SaveChangesAsync();

                    foreach (var item in orderItems)
                    {
                        var product = await _context.Products.FindAsync(item.ProductId);
                        if (product == null || product.Stock < item.Quantity)
                        {
                            throw new Exception($"Insufficient stock for product: {product?.Name ?? "Unknown"}");
                        }

                        // Giảm stock của sản phẩm
                        product.Stock -= item.Quantity;
                        _context.Products.Update(product);

                        // Gán OrderId đã được tạo cho OrderItem
                        item.OrderId = order.Id;
                        await _context.OrderItems.AddAsync(item);
                    }

                    // Lưu tất cả các thay đổi
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Concurrency conflict: " + ex.Message);
                    throw;
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
                        var product = await _context.Products.FindAsync(item.ProductId);
                        if (product == null || product.Stock < item.Quantity)
                        {
                            throw new Exception($"Insufficient stock for product: {product?.Name ?? "Unknown"}");
                        }

                        product.Stock -= item.Quantity;
                        _context.Products.Update(product);
                    }

                    await _context.OrderItems.AddRangeAsync(newOrderItems);
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
