using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shop.Web.DTOs.Auth;
using Shop.Web.Infratructures.Response;
using Shop.Web.Models.Entity.Auth;
using Shop.Web.Repositories.Interface;
using Shop.Web.Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IMapper mapper)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthResponse> RegisterUser(SignInDTO signInDto)
        {
            var signIn = _mapper.Map<SignIn>(signInDto);
            var result = await _authRepository.RegisterUser(signIn);

            if (result.Succeeded)
            {
                var response = await GenerateAuthResponse(signInDto.UserName!);
                return response;
            }

            return null!;
        }

        public async Task<AuthResponse> LoginUser(LogInDTO loginDto)
        {
            var login = _mapper.Map<LogIn>(loginDto);
            var result = await _authRepository.LoginUser(login);

            if (result.Succeeded)
            {
                var response = await GenerateAuthResponse(loginDto.UserName!);
                return response;
            }

            return null!;
        }

        private async Task<AuthResponse> GenerateAuthResponse(string userName)
        {
            var user = await _authRepository.FindUserByName(userName);
            if (user == null)
            {
                return null!;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            if (user.Email == "tanchuonghuynh3@gmail.com")
            {
                authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                authClaims.Add(new Claim(ClaimTypes.Role, "Manager"));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new AuthResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = token.ValidTo
            };
        }
    }
}
