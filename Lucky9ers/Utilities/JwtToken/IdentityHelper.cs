using api.Startup;
using Lucky9.Application.IServices;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Utilities.JwtToken;

public  class IdentityHelper:ISecurityService
{

   public string CreateTestToken()
    {
        var authSetting = AppGlobal.Settings.JWT;

        var now = DateTime.UtcNow;

        // add the registered claims for JWT (RFC7519).
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,"test"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,"test@Lucky9.com"), //user.Entity.DisplayName),
				new Claim(JwtRegisteredClaimNames.Email, "test@Lucky9.com"),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
            };

        var issuer = authSetting.Issuer;

        var tokenExpirationMins = authSetting.TokenExpirationInMinutes;

        var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSetting.Secret));

        var token = new JwtSecurityToken(
            issuer,
            authSetting.Audience,
            claims,
            now,
            now.Add(TimeSpan.FromMinutes(tokenExpirationMins)),
            new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256));

        var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);


        return encodedToken;


    }



}
