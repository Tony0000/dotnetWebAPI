﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Data.Repositories.Interfaces;
using Domain.Model;
using dotnetWebAPI.Dtos;
using dotnetWebAPI.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace dotnetWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
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
        public IActionResult Login(LoginViewModel loginModel)
        {
            var user = AuthenticateUser(loginModel);

            if (user == null) 
                return Unauthorized();

            var token = CreateTokenJwt(user);
            var response = Ok(new
            {
                token,
                user = _mapper.Map<UserDto>(user)
            });

            return response;
        }

        private User AuthenticateUser(LoginViewModel credentials)
        {
            var user = _repository.First(x => x.Username.Equals(credentials.Username));
            return (user.ConfirmPassword(credentials.Password) && user.Active) ? user : null;
        }

        private string CreateTokenJwt(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("email", user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}