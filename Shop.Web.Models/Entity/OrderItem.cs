using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Models.Entity
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; }


        [ForeignKey("Order")]
        public Guid OrderId { get; set; }


        [ForeignKey("Product")]
        public Guid ProductId { get; set; }


        [Required]
        public int Quantity { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}

