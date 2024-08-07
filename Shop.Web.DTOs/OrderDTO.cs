using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        //public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        //public string? UserName { get; set; }

        public List<OrderItemDTO>? OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
