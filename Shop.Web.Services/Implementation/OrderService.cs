using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Shop.Web.DTOs;
using Shop.Web.Models.Entity;
using Shop.Web.Repositories.Interface;
using Shop.Web.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAll().ToListAsync();
            return orders.Select(order => _mapper.Map<OrderDTO>(order));
        }

        public async Task<OrderDTO?> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDTO?>(order);
        }

        public async Task<OrderDTO?> GetOrderWithItemsAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderWithItemsAsync(orderId);
            return _mapper.Map<OrderDTO?>(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByProductIdAsync(Guid productId)
        {
            var orders = await _orderRepository.GetAll()
                .Where(o => o.OrderItems!.Any(oi => oi.ProductId == productId))
                .ToListAsync();
            return orders.Select(order => _mapper.Map<OrderDTO>(order));
        }

        public async Task AddOrderAsync(OrderDTO orderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDto);
                order.OrderId = Guid.NewGuid();

                foreach (var item in order.OrderItems)
                {
                    var productExists = await _orderRepository.ProductExistsAsync(item.ProductId);
                    if (!productExists)
                    {
                        throw new Exception($"Product with ID {item.ProductId} does not exist.");
                    }

                    item.OrderItemId = Guid.NewGuid();
                    item.OrderId = order.OrderId;
                    item.Product = null;
                }

                await _orderRepository.AddOrderAsync(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task UpdateOrderAsync(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);

            if (orderDto.OrderItems != null && orderDto.OrderItems.Any())
            {
                foreach (var orderItemDto in orderDto.OrderItems)
                {
                    var orderItem = _mapper.Map<OrderItem>(orderItemDto);
                    order.OrderItems?.Add(orderItem);
                }
            }

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }
    }
}
