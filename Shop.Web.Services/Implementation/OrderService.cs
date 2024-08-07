using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Web.DTOs;
using Shop.Web.Models.Entity;
using Shop.Web.Repositories.Interface;
using Shop.Web.Services.Interface;

namespace Shop.Web.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
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
            order.OrderItems = new List<OrderItem>();

            foreach (var itemDto in orderDto.OrderItems!)
            {
                var orderItem = _mapper.Map<OrderItem>(itemDto);
                orderItem.Id = Guid.NewGuid();
                orderItem.OrderId = order.Id;
                order.OrderItems.Add(orderItem);
            }

            await _orderRepository.AddOrderWithItemsAsync(order, order.OrderItems.ToList());
        }

        public async Task UpdateOrderAsync(OrderDTO orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderDto.Id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            _mapper.Map(orderDto, order);

            var existingOrderItems = await _orderItemRepository.GetAll()
                .Where(oi => oi.OrderId == order.Id)
                .ToListAsync();

            var orderItems = _mapper.Map<List<OrderItem>>(orderDto.OrderItems);
            await _orderRepository.UpdateOrderWithItemsAsync(order, orderItems, existingOrderItems);
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
