using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Models.Entity
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string? UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        public Role? Role { get; set; }
    }

    public enum Role
    {
        Admin,
        Manager,
        Employee,
        Guest
    }
}
