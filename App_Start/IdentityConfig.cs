using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DNyC.Helpers;
using DNyC.Models.Authentication;
using DNyC.Models.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;

namespace DNyC
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<CustomIdentityUser, decimal>
    {
        private static DataProtectorTokenProvider<CustomIdentityUser, decimal> tokenProvider;
        public static DataProtectorTokenProvider<CustomIdentityUser, decimal> TokenProvider(IdentityFactoryOptions<ApplicationUserManager> options)
        {
            if (tokenProvider != null)
                return tokenProvider;
            //var dataProtectionProvider = new DpapiDataProtectionProvider();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
                tokenProvider = new DataProtectorTokenProvider<CustomIdentityUser, decimal>(dataProtectionProvider.Create("ConfirmationToken"))
                {
                    TokenLifespan = TimeSpan.FromHours(24)
                };
            return tokenProvider;
        }

        public ApplicationUserManager(CustomUserStore store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new CustomUserStore());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<CustomIdentityUser, decimal>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            manager.UserTokenProvider = TokenProvider(options);

            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider = new DataProtectorTokenProvider<CustomIdentityUser, decimal>(dataProtectionProvider.Create("ASP.NET Identity"))
            //    {
            //        TokenLifespan = TimeSpan.FromHours(24)
            //    };
            //}

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = false;
            //manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            //{
            //    MessageFormat = "Your security code is {0}"
            //});
            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();



            return manager;
        }

        public override Task<IdentityResult> ResetPasswordAsync(decimal Id, string token, string newPassword)
        {
            //token = System.Net.WebUtility.UrlDecode(token);
            bool verified = this.VerifyUserToken(Id, "ConfirmationToken", token);
            return base.ResetPasswordAsync(Id, token, newPassword);
        }

        public override Task<IdentityResult> ChangePasswordAsync(decimal Id, string currentPassword, string newPassword)
        {
            var store = this.Store as IUserPasswordStore<CustomIdentityUser, decimal>;
            if (store == null)
                return Task.FromResult(new IdentityResult(new string[] { "Falta implementar IUserPasswordStore" }));

            // Para guardar con hash.
            //var passHash = this.PasswordHasher.HashPassword(newPassword);
            //await store.SetPasswordHashAsync(Id, newPasswordHash);

            CustomIdentityUser user;
            using (AuthenticationRepository repo = new AuthenticationRepository())
            {
                DBResponseCodes status;
                user = repo.SearchUserById(Id, out status);

                if (status != DBResponseCodes.Success)
                {
                    // TODO: log
                    return Task.FromResult(new IdentityResult(new string[] { "Error en usuario" }));
                }
            }

            store.SetPasswordHashAsync(user, newPassword).RunSynchronously();
            return Task.FromResult(IdentityResult.Success);
        }

        /// <summary>
        /// Revisa que los passwords sean iguales.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override Task<bool> CheckPasswordAsync(CustomIdentityUser user, string password)
        {
            // TODO: implementar si hace falta alguna transformación a hash.
            if (user.VAL_Clave == password)
                return Task.FromResult(true);

            return Task.FromResult(false);
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<CustomIdentityUser, decimal>
    {
        private Logger LOG = new Logger();

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(CustomIdentityUser user, ApplicationUserManager manager)
        {
            try
            {
                var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                // Add custom user claims here
                return userIdentity;
            }
            catch (System.Exception ex)
            {
                LOG.Error("Error al crear identity", ex);
                throw;
            }
        }

        public async override Task<ClaimsIdentity> CreateUserIdentityAsync(CustomIdentityUser user)
        {
            return await GenerateUserIdentityAsync(user, (ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
