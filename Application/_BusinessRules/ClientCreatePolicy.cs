using System;
using Lucky9.Application.Commands;
using Lucky9.Application.Interfaces;
using Lucky9.Application.IServices;
using Lucky9.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Lucky9.Application.Common.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Server.IIS.Core;
using Application.Helpers;
using Lucky9.Application._Features.ClientFeatures.Commands;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Lucky9.Application.Helpers.Dto;
using System.Text.RegularExpressions;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Lucky9.Application.Common.Models;
using Lucky9.Application.Common.Exceptions;
using Lucky9.Application._Features.Utilities.SecurityToken.Queries;
using Microsoft.AspNetCore.DataProtection;

namespace Lucky9.Application._BusinessRules;

public static  class ClientPolicy
{

    #region -------------Users


    public class AddClientCommandValidator : AbstractValidator<AddClientCommand>
    {
        public AddClientCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.Email)
             .NotEmpty()
             .WithMessage("Email address is required.")
             .EmailAddress()
             .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Phone number is required.");
        }

    }

    public static async Task<User> AsserCreateUserPolicy(AddClientCommand request,
        IValidator<AddClientCommand> _validator,
        IPlayerRepository _repo)
    {  
        
        //if (request == null)
        //    return BadRequest();

       var validationResult = _validator.Validate(request);

        var userObj = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Money = 10000.00,
            Password = request.Password,
        };



        var entity = await _repo.FindAsync(c => c.Email == request.Email);
        var current = entity.FirstOrDefault();

        // check email
        if (current!=null)
                {
            var failure = new ValidationFailure("Email", "Email Already Exist");
            validationResult.Errors.Add(failure);

        }

        //check username
        var userNames = await _repo.FindAsync(c => c.Email == request.Email);
        var userName = userNames.FirstOrDefault();
        if (current != null)
        {
            var failure = new ValidationFailure("Username", "Username Already Exist");
            validationResult.Errors.Add(failure);

        }     

        

        userObj.Password = PasswordHasher.HashPassword(request.Password);
        userObj.Role = "User";
        userObj.Token = "";
        userObj.Username = userObj.Email;

        userObj.ValidationResult = ValidationHelper.Summarize(validationResult);

       await  _repo.Add(userObj);
      
  
        return userObj;

    }

    private static string CheckPasswordStrength(string pass)
    {
        StringBuilder sb = new StringBuilder();
        if (pass.Length < 9)
            sb.Append("Minimum password length should be 8" + Environment.NewLine);
        if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
            sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
        if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
            sb.Append("Password should contain special charcter" + Environment.NewLine);
        return sb.ToString();
    }


    #endregion



}



