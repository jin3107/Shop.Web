using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Infratructures.Response
{
    public class AuthResponse
    {
        public string? AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
