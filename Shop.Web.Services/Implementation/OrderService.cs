using AutoMapper;
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
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task AddOrderAsync(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.Id = Guid.NewGuid();
            await _orderRepository.AddAsync(order);
        }

        public async Task UpdateOrderAsync(OrderDTO orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderDto.Id);
            if (order == null)
            {
                throw new Exception($"Order Id does not found");
            }

            _mapper.Map(order, orderDto);
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception($"Order Id does not found");
            }

            await _orderRepository.DeleteAsync(id);
        }
    }
}
