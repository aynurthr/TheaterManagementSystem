using Theater.Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Theater.DataAccessLayer.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Theater.Application.Services;
using Theater.Presentation.Pipeline;

namespace Theater.Presentation
{
    public static class IdentityInjection
    {
        public static FilterCollection AppendAuthorization(this FilterCollection filters)
        {
            var policy = new AuthorizationPolicyBuilder()
                                  .RequireAuthenticatedUser()
                                  .Build();

            filters.Add(new AuthorizeFilter(policy));

            return filters;
        }
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddScoped<IClaimsTransformation, AppClaimsTransformation>();

            services.AddIdentityCore<AppUser>()
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddUserManager<UserManager<AppUser>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.Lockout.MaxFailedAccessAttempts = 3;
                cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                cfg.User.RequireUniqueEmail = true;
                //cfg.User.AllowedUserNameCharacters = "abcde123456789";

                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 1;
                cfg.Password.RequiredLength = 3;

                cfg.SignIn.RequireConfirmedEmail = true;

            });

            services.AddAuthentication()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, cfg =>
            {
                cfg.LoginPath = "/signin.html";
                cfg.AccessDeniedPath = "/accessdenied.html";
                cfg.ExpireTimeSpan = TimeSpan.FromMinutes(10);

                cfg.Cookie.Name = "theater";
                cfg.Cookie.HttpOnly = true;
            });

            services.AddAuthorization(cfg => {

                foreach (var item in AppClaimsTransformation.policies)
                {
                    cfg.AddPolicy(item, p =>
                    {
                        //p.RequireClaim(item, "1");

                        p.RequireAssertion(handler => handler.User.IsInRole("SUPERADMIN") || handler.User.HasClaim(item, "1"));
                        //p.RequireAssertion(handler => handler.User.HasClaim(item, "1"));
                    });
                }

            });

            return services;
        }

        public static IApplicationBuilder UseIdentity(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            app.SeedUser(configuration);

            return app;
        }
    }
}
