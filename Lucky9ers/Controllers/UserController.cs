using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Application.Helpers;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Domain.Models;
using Lucky9.Infrastructure.Persistence;
using Lucky9.Application.Features.Queries;
using MediatR;
using Lucky9.WebUI.Controllers;
using Lucky9.Application.Helpers.Dto;
using Lucky9.Application.Commands;
using Lucky9.Application.Common.Models;
using Microsoft.AspNetCore.Cors;
using System.Diagnostics.CodeAnalysis;
using Lucky9.Application._Features.ClientFeatures.Commands;
using IdentityServer4.Models;

namespace Application.Controllers
{
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly AppDbContext _authContext;
        public UserController(AppDbContext context)
        {
            _authContext = context;
        }

        [AllowAnonymous]
        [EnableCors("MyPolicy")]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            try
            {
                var param = new AuthenticateCommand()
                {
                   Email = userObj.Email,
                 
                   Password = userObj.Password
                };

                var result = await Mediator.Send(param);

                if(result.ValidationResult!=null)
                {

                    // Return BadRequest with the custom error model
                    return BadRequest(new
                    {
                        Errors = result.ValidationResult,
                        Message = result.ValidationResult.FirstOrDefault(),
                        Email = userObj.Email
                    }); ;
                }



                return Ok(result);



            }
            catch (Exception e)
            {

                LogBadRequest(e);
                return BadRequest(e.Message);
            }

        }

        [AllowAnonymous]
        [EnableCors("MyPolicy")]
        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            try
            {
                var param = new AddClientCommand()
                {
                    Email = userObj.Email,
                    Password = userObj.Password,
                    FirstName = userObj.FirstName,
                    LastName = userObj.LastName,
                    
                };

                var result = await Mediator.Send(param);

                if (result.ValidationResult.Count > 0)
                {

                    // Return BadRequest with the custom error model
                    return BadRequest(new
                    {
                        Errors = result.ValidationResult,
                        Message = result.ValidationResult.FirstOrDefault(),
                        Email = userObj.Email
                    }); ;
                }

                return Ok(new
                {
                    Status = 200,
                    Message = "User Added!",
                    Email = userObj.Email
                });



            }
            catch (Exception e)
            {

                LogBadRequest(e);
                return BadRequest(e.Message);
            }
        }


        [HttpGet("find/{search?}")]
        public async Task<IActionResult> GetPlayers([FromRoute] string? search)
        {
            try
            {
                var param = new GetClientsQuery()
                {
                    search = search
                };

                var result = await Mediator.Send(param);

                return Ok(result);

            }
            catch (Exception e)
            {

                LogBadRequest(e);
                return BadRequest(e.Message);
            }



        }

        [HttpPut("updateclient")]

        public async Task<IActionResult> Updateclient([FromBody] UpdateGameClientCommand client)
        {
            try
            {
                var param = new UpdateGameClientCommand
                {
                    Email = client.Email,
                    BetMoney = client.BetMoney,
                    GameId = client.GameId,
                    PlayerCards = client.PlayerCards,
                    ServerCards = client.ServerCards,

                };

                var result = await Mediator.Send(param);

                return Ok(result);

            }
            catch (Exception e)
            {

                LogBadRequest(e);
                return BadRequest(e.Message);
            }



        }

        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.Username}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _authContext.Users
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token,tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;
                
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _authContext.Users.ToListAsync());
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid Request");
            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _authContext.SaveChangesAsync();
            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }
    }

}
