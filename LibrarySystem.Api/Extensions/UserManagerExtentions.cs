using LibrarySystem.Core.Entitties.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibrarySystem.Api.Extensions
{
    public static class UserManagerExtentions
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager ,ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            var  AppUser = await userManager.Users.Include(user => user.Address).FirstOrDefaultAsync(user => user.Email == email);
            return AppUser;
        }
    }
}
 