using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Repositories.Interfaces;
using WebAPI.Dtos;
using WebAPI.Dtos.UserDtos;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public LoginController(IConfiguration config, IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _repository = repository;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginModel loginModel)
        {
            var user = AuthenticateUser(loginModel);

            if (user == null) 
                return Unauthorized();

            var token = CreateTokenJwt(user, loginModel.RememberMe);
            var response = Ok(new
            {
                token,
                user = _mapper.Map<UserDto>(user)
            });

            return response;
        }

        private User AuthenticateUser(LoginModel credentials)
        {
            var user = _repository.First(x => x.Username.Equals(credentials.Username) && x.Active);

            if (user == null || !user.ConfirmPassword(credentials.Password))
                return null;

            return user;
        }

        private string CreateTokenJwt(User user, bool rememberMe)
        {
            var timeToLive = rememberMe? int.Parse(_config["Jwt:TTL"]) : 1;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.Role.ToString()), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(timeToLive),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
