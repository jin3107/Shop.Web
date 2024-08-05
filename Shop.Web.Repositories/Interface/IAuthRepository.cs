using Microsoft.AspNetCore.Identity;
using Shop.Web.Models;
using Shop.Web.Models.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Repositories.Interface
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterUser(SignIn signIn);
        Task<SignInResult> LoginUser(LogIn login);
        Task<IdentityUser> FindUserByName(string userName);
    }
}
