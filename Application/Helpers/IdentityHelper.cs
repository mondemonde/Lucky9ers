using api.Startup;
using Domain.Models;
using Lucky9.Application.IServices;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers;

public static  class IdentityHelper
{

   public static string CreateTestToken()
    {

        var user = new User
        {
            Email = "test@123.com"

        };

        return GettToken(user);


    //    var authSetting = AppGlobal.Settings.JWT;

    //    var now = DateTime.UtcNow;

    //    // add the registered claims for JWT (RFC7519).
    //    var claims = new List<Claim>
    //        {
    //            new Claim(JwtRegisteredClaimNames.Sub,"test"),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new Claim(JwtRegisteredClaimNames.UniqueName,"test@Lucky9.com"), //user.Entity.DisplayName),
				//new Claim(JwtRegisteredClaimNames.Email, "test@Lucky9.com"),
    //            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
    //        };

    //    var issuer = authSetting.Issuer;

    //    var tokenExpirationMins = authSetting.TokenExpirationInMinutes;

    //    var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSetting.Secret));

    //    var token = new JwtSecurityToken(
    //        issuer,
    //        authSetting.Audience,
    //        claims,
    //        now,
    //        now.Add(TimeSpan.FromMinutes(tokenExpirationMins)),
    //        new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256));

    //    var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);


    //    return encodedToken;


    }

   public static string GettToken(User user)
    {

        var _config = AppGlobal.Settings;
        // Your user claims or payload data
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Role, "user"),
            // Add any additional claims as needed
        };

        // Your secret key used to sign the token
        var secretKey = _config.JWT.Secret;
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

        // Choose the signing algorithm
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Set token expiration time
        var expiration = DateTime.UtcNow.AddDays(10);

        // Create the JWT
        var token = new JwtSecurityToken(
            issuer: _config.JWT.Issuer, //"rgalvez@blastasia.com",
            audience:_config.JWT.Audience, //"your_audience",
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        // Serialize the token to a string
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }

    public static string CreateRefreshToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(tokenBytes);

        return refreshToken;
    }


}
