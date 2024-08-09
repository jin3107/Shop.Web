using Shop.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.DTOs
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDTO>? OrderItems { get; set; }

        public OrderDTO()
        {
            OrderItems = new List<OrderItemDTO>();
        }
    }
}
