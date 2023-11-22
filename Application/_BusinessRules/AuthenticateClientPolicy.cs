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
using Lucky9.Application.Helpers;

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
                user.Token = IdentityHelper.GettToken(user);//CreateJwt(user);
                var newAccessToken = user.Token;
                var newRefreshToken =IdentityHelper.CreateRefreshToken();
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
     
        #endregion-------------------------end authentication
    }
}
