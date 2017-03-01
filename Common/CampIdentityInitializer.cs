using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRentalApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarRentalApi.Common
{
  public class CampIdentityInitializer
  {
    private readonly RoleManager<IdentityRole> _roleMgr;
    private readonly UserManager<CampUser> _userMgr;

    public CampIdentityInitializer(UserManager<CampUser> userMgr, RoleManager<IdentityRole> roleMgr)
    {
      _userMgr = userMgr;
      _roleMgr = roleMgr;
    }

    public async Task Seed()
    {
      var user = await _userMgr.FindByNameAsync("shawnwildermuth");

      // Add User
      if (user == null)
      {
        if (!(await _roleMgr.RoleExistsAsync("Admin")))
        {
          var role = new IdentityRole("Admin");
          role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "IsAdmin", ClaimValue = "True" });
          await _roleMgr.CreateAsync(role);
        }

        user = new CampUser()
        {
          UserName = "shawnwildermuth",
          FirstName = "Shawn",
          LastName = "Wildermuth",
          Email = "shawn@wildermuth.com"
        };
      
        var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");
        var roleResult = await _userMgr.AddToRoleAsync(user, "Admin");
        var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

        if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
        {
          throw new InvalidOperationException("Failed to build user and roles");
        }

      }
    }
  }
}
