using Domain.Models;
using FluentValidation.Results;
using FluentValidation;
using Lucky9.Application.Commands;
using Lucky9.Application.Common.Exceptions;
using Lucky9.Application.Helpers.Dto;
using Lucky9.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using Lucky9.Application.Common.Models;

namespace Lucky9.Application._BusinessRules
{
    public static class AuthenticateClientPolicy
    {
        #region Authentication-----------------------------------------------

        public class AuthCommandValidator : AbstractValidator<AuthenticateCommand>
        {
            public AuthCommandValidator()
            {

                RuleFor(x => x.Email)
                 .NotEmpty()
                 .WithMessage("Email address is required.")
                 .EmailAddress()
                 .WithMessage("Please enter a valid email address.");

            }

        }

        public static AppSettings _config { get; set; } 
        public static async Task<TokenApiDto> AssertAuthenticatePolicy(AuthenticateCommand request,
            IValidator<AuthenticateCommand> _validator,
            IPlayerRepository _repo, AppSettings config)
        {
            _config = config as AppSettings;
            var validationResult = _validator.Validate(request);

            //check user exist
            var users = await _repo.FindAsync(x => x.Email == request.Email);
            var user = users.FirstOrDefault();

            //check username     
            if (user == null)
            {
                var failure = new ValidationFailure("Username", "User not found");
                validationResult.Errors.Add(failure);

            }
            else
            {
                //check password
                if (!PasswordHasher.VerifyPassword(request.Password, user.Password))
                {
                    // throw new ApplicationException("Password is Incorrect");
                    var failure = new ValidationFailure("Password", "Password is Incorrect");
                    validationResult.Errors.Add(failure);
                }

            }

            if (validationResult.Errors.Count > 0)
            {
                return new TokenApiDto
                {

                    ValidationResult = ValidationHelper.Summarize(validationResult)
                };

            }
            try
            {
                user.Token = GettToken(user);//CreateJwt(user);
                var newAccessToken = user.Token;
                var newRefreshToken = CreateRefreshToken();
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
                await _repo.Update(user);

                //var vResult = ValidationHelper.Summarize(validationResult);

                return new TokenApiDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    ValidationResult = null
                };
            }
            catch (Exception err)
            {
               
                throw;
            }
            

        }

       //const string SECRET = _config
        public static string GettToken(User user)
        {
            // Your user claims or payload data
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Role, "user"),
            // Add any additional claims as needed
        };

            // Your secret key used to sign the token
            var secretKey =  _config.JWT.Secret;
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            // Choose the signing algorithm
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Set token expiration time
            var expiration = DateTime.UtcNow.AddDays(10);

            // Create the JWT
            var token = new JwtSecurityToken(
                issuer: "rgalvez@blastasia.com",
                audience: "your_audience",
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

            // Serialize the token to a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        static string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.JWT.Secret);
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


        private static string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            return refreshToken;
        }

        #endregion-------------------------end authentication
    }
}
