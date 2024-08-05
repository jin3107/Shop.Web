using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.DTOs.Auth
{
    public class LogInDTO
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
