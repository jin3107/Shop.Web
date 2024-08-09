using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Web.Enums;

namespace Shop.Web.Models.Entity
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    
    }
}
