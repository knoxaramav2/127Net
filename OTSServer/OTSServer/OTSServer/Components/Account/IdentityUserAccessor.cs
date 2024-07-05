using Microsoft.AspNetCore.Identity;
using OTSCommon.Models;
using OTSServer.Data;

namespace OTSServer.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<UserAccount> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<UserAccount> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
