using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Lucky9.Domain.Entities;
using Lucky9.Infrastructure.Identity;
using Lucky9.Application.Common.Interfaces;
using Lucky9.Infrastructure.Persistence;

namespace Lucky9.Infrastructure.Data;
public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly AppDbContext _context;
    //private readonly UserManager<ApplicationUser> _userManager;
    //private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, IDataContext context)
    {
        _logger = logger;
        _context = context as AppDbContext;
        //_userManager = userManager;
        //_roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    //public async Task SeedAsync()
    //{
    //    try
    //    {
    //        await TrySeedAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while seeding the database.");
    //        throw;
    //    }
    //}

    //public async Task TrySeedAsync()
    //{
    //    // Default roles
    //    var administratorRole = new IdentityRole("Administrator");

    //    if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
    //    {
    //        await _roleManager.CreateAsync(administratorRole);
    //    }

    //    // Default users
    //    var administrator = new ApplicationUser { UserName = "administrator@localhost",FirstName ="Mon", LastName="G"
    //        , Email = "administrator@localhost" };

    //    if (_userManager.Users.All(u => u.UserName != administrator.UserName))
    //    {
    //        await _userManager.CreateAsync(administrator, "Administrator1!");
    //        if (!string.IsNullOrWhiteSpace(administratorRole.Name))
    //        {
    //            await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
    //        }
    //    }


    //}
}
