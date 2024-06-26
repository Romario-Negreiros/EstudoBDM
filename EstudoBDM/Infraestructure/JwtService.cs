﻿using EstudoBDM.DTOs;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EstudoBDM.Infraestructure
{
    public interface IJwtService
    {
        UserDTOs.LoggedUserDTO GenerateJWT(UserDTOs.LoginUserDTO loginUser);
    }

    public class JwtService(IConfiguration configuration) : IJwtService
    {
        private readonly IConfiguration _configuration = configuration;

        public UserDTOs.LoggedUserDTO GenerateJWT(UserDTOs.LoginUserDTO loginUser)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginUser.name!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("scopes", string.Join(", ", loginUser.scopes!))
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.Now.AddMinutes(120);

            var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Audience"], claims: claims, expires: expiration, signingCredentials: credentials);

            return new UserDTOs.LoggedUserDTO
            {
                authenticated = true,
                token = new JwtSecurityTokenHandler().WriteToken(token), // Converts the JWT into a web compatible representation: JWS or JWE (strings)
                expiration = expiration
            };
        }
    }
}
