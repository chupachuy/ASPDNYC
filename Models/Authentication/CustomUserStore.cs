using DNyC.Models.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DNyC.Models.Authentication
{
    public class CustomUserStore :
        IUserStore<CustomIdentityUser, decimal>
        , IUserLoginStore<CustomIdentityUser, decimal>
        , IUserPasswordStore<CustomIdentityUser, decimal>
        , IUserRoleStore<CustomIdentityUser, decimal>
        , IUserSecurityStampStore<CustomIdentityUser, decimal>
        , IUserEmailStore<CustomIdentityUser, decimal>
        , IUserLockoutStore<CustomIdentityUser, decimal>
        , IUserTwoFactorStore<CustomIdentityUser, decimal>
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        public CustomUserStore()
        {
        }

        #region External Login
        public Task AddLoginAsync(CustomIdentityUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<CustomIdentityUser> FindAsync(UserLoginInfo login)
        {
            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                Task<CustomIdentityUser> result = repo.FindUserFromSocial(login.LoginProvider, login.ProviderKey);
                return result;
            }
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(CustomIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(CustomIdentityUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region User Store
        public Task CreateAsync(CustomIdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("Usuario no puede ser nulo.");

            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                bool result = repo.CreateUser(user).Result;
                return Task.FromResult(result);
            }
        }

        public async Task UpdateAsync(CustomIdentityUser user)
        {
            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                //bool updated = await repo.UpdateUser(user);
            };

            await Task.FromResult(0);
        }

        public Task DeleteAsync(CustomIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomIdentityUser> FindByIdAsync(decimal Id)
        {
            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                CustomIdentityUser user = await repo.FindUserAsync(Id);
                return user;
            };
        }

        public Task<CustomIdentityUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("Nombre de Usuario nulo.");

            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                CustomIdentityUser result = repo.FindUser(userName);
                return Task.FromResult(result);
            }

            //return Task<CustomIdentityUser>.Factory.StartNew(new Func<CustomIdentityUser>(() =>
            //{
            //    using (AuthenticationRepository repo = new AuthenticationRepository())
            //    {
            //        CustomIdentityUser result = repo.FindUser(userName).Result;
            //        //if (result != null)
            //        //    throw new AccountException("Usuario ya existente.");

            //        return result;
            //    }
            //}));
        }

        public void Dispose()
        {

        }
        #endregion

        #region Role
        public async Task AddToRoleAsync(CustomIdentityUser user, string roleName)
        {
            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                //await repo.AddRole(user, roleName);
            }

            await Task.FromResult(0);
        }

        public async Task RemoveFromRoleAsync(CustomIdentityUser user, string roleName)
        {
            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                //await repo.RemoveRole(user, roleName);
            }

            await Task.FromResult(0);
        }

        public async Task<IList<string>> GetRolesAsync(CustomIdentityUser user)
        {
            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                return await repo.GetRolesForUser(user);
            }
        }

        public async Task<bool> IsInRoleAsync(CustomIdentityUser user, string roleName)
        {
            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                return await repo.IsInRole(user.UserName, roleName);
            }
        }
        #endregion

        #region Password Store
        public Task<string> GetPasswordHashAsync(CustomIdentityUser user)
        {
            return Task.FromResult(user.VAL_Clave);
        }

        public Task<bool> HasPasswordAsync(CustomIdentityUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.VAL_Clave));
        }

        public Task SetPasswordHashAsync(CustomIdentityUser user, string passwordHash)
        {
            user.VAL_Clave = passwordHash;
            return Task.FromResult(true);
        }
        #endregion

        #region Email
        public Task<CustomIdentityUser> FindByEmailAsync(string email)
        {
            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                return Task.FromResult(repo.FindUserByEmail(email));
            }
        }

        public async Task<string> GetEmailAsync(CustomIdentityUser user)
        {
            if (string.IsNullOrEmpty(user.VAL_Correo))
                return await Task.FromResult("unknown@DNyC.com");

            return await Task.FromResult(user.VAL_Correo);
        }

        public Task<bool> GetEmailConfirmedAsync(CustomIdentityUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailAsync(CustomIdentityUser user, string email)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(CustomIdentityUser user, bool confirmed)
        {
            return Task.FromResult(true);
        }
        #endregion

        #region IUserLockout
        public Task<DateTimeOffset> GetLockoutEndDateAsync(CustomIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(CustomIdentityUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public async Task<int> IncrementAccessFailedCountAsync(CustomIdentityUser user)
        {
            return await Task.FromResult(0);
        }

        public async Task ResetAccessFailedCountAsync(CustomIdentityUser user)
        {
            await Task.FromResult(0);
        }

        public async Task<int> GetAccessFailedCountAsync(CustomIdentityUser user)
        {
            return await Task.FromResult(4);
        }

        public async Task<bool> GetLockoutEnabledAsync(CustomIdentityUser user)
        {
            return await Task.FromResult(false);
        }

        public async Task SetLockoutEnabledAsync(CustomIdentityUser user, bool enabled)
        {
            await Task.FromResult(0);
        }
        #endregion

        #region ITwoFactor
        public async Task SetTwoFactorEnabledAsync(CustomIdentityUser user, bool enabled)
        {
            await Task.FromResult(0);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(CustomIdentityUser user)
        {
            return await Task.FromResult(false);
        }
        #endregion

        #region Security Stamp Store
        public Task<string> GetSecurityStampAsync(CustomIdentityUser user)
        {
            if (string.IsNullOrEmpty(user.SecurityStamp))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
            }
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(CustomIdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(true);
        }
        #endregion
    }
}