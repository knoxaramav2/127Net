using Microsoft.AspNetCore.Identity;
using OTSCommon.Database;
using OTSCommon.Models;
using System.Security.Cryptography.X509Certificates;

namespace OTSServerAuth
{
    public class OTSAuthenticate(UserManager<UserAccount> userManager,
            IUserStore<UserAccount> userStore,
            SignInManager<UserAccount> signInManager,
            OTSDbService dbService)
    {
        private readonly UserManager<UserAccount> UserManager = userManager;
        private readonly IUserStore<UserAccount> UserStore = userStore;
        private readonly SignInManager<UserAccount> SignInManager = signInManager;
        private readonly OTSDbService DbService = dbService;

        public async Task<(IdentityResult, UserAccount?)> TryCreateAuthenticUser(
            string username, 
            string password,
            bool asGuest = false
            )
        {
            var userExists = DbService.GetUser(username) != null;
            if ( userExists ) { return (IdentityResult.Failed(
                new IdentityError()
                ), null); }

            //Create user settings first
            //Roll back if new user fails

            var user = Activator.CreateInstance<UserAccount>();
            await UserStore.SetUserNameAsync(user, username, CancellationToken.None);
            var idRes = await UserManager.CreateAsync(user, password);

            if (idRes.Succeeded)
            {
                var userAuth = DbService.GetRoleAuthority("User")!;
                var guestAuth = DbService.GetRoleAuthority("Guest")!;

                List<RoleAuthority> roles = [];

                if (!asGuest)
                {
                    roles.Add(userAuth);
                }

                roles.Add(guestAuth);
                DbService.AddUserRoles(user.Id, [userAuth, guestAuth]);
            }

            return (idRes, user);
        }
    }
}
