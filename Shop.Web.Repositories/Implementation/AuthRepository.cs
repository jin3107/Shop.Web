using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shop.Web.Models.Entity.Auth;
using Shop.Web.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Repositories.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public AuthRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUser(SignIn signIn)
        {
            var user = _mapper.Map<IdentityUser>(signIn);
            return await _userManager.CreateAsync(user, signIn.Password!);
        }

        public async Task<SignInResult> LoginUser(LogIn login)
        {
            return await _signInManager.PasswordSignInAsync(login.UserName!, login.Password!, false, false);
        }

        public async Task<IdentityUser> FindUserByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
    }
}
