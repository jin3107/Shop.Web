using Shop.Web.DTOs.Auth;
using Shop.Web.Infratructures.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterUser(SignInDTO signInDto);
        Task<AuthResponse> LoginUser(LogInDTO loginDto);
    }
}
