using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Web.DTOs;
using Shop.Web.Models.Data;
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
        private readonly ApplicationDbContext _context;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMapper mapper, ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            _context = context;
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
            try
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

                    var product = await _context.Products.FindAsync(itemDto.ProductId);
                    if (product == null || product.Stock < itemDto.Quantity)
                    {
                        throw new Exception($"Insufficient stock for product: {product?.Name ?? "Unknown"}");
                    }

                    product.Stock -= itemDto.Quantity;
                    _context.Products.Update(product);
                }

                await _orderRepository.AddOrderWithItemsAsync(order, order.OrderItems.ToList());
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task UpdateOrderAsync(OrderDTO orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderDto.Id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            _mapper.Map(orderDto, order);

            var existingOrderItems = await _orderItemRepository.GetAll().Where(oi => oi.OrderId == order.Id).ToListAsync();
            _context.OrderItems.RemoveRange(existingOrderItems);

            var orderItems = _mapper.Map<List<OrderItem>>(orderDto.OrderItems);
            foreach (var item in orderItems)
            {
                item.Id = Guid.NewGuid();
                item.OrderId = order.Id;
            }

            await _orderRepository.UpdateOrderWithItemsAsync(order, orderItems);
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
