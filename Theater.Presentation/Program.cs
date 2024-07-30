using Autofac;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Theater.Application;
using Theater.Application.Repositories;
using Theater.Application.Services;
using Theater.Application.Services.Identity;
using Theater.DataAccessLayer.Contexts;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Configurations;
using Theater.Presentation.Pipeline;
using Theater.Repository;
using Theater.Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity;
using Theater.Application.Modules.ContactPostModule.Commands.ContactPostApplyCommand;
using Theater.Application.Modules.ContactPostModule.Commands.ContactPostReplyCommand;


namespace Theater.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IClaimsTransformation, AppClaimsTransformation>();

            builder.Services.AddIdentity();

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("cString")));


            builder.Services.AddControllersWithViews(cfg =>
            {
                cfg.Filters.Add<GlobalErrorHandlingExceptionFilter>();
                cfg.Filters.Add<StopwatchActionFilterAttribute>();

                cfg.Filters.AppendAuthorization();
            });

            builder.Services.AddControllersWithViews()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SignupRequestValidator>());

            builder.Services.AddRouting(cfg => cfg.LowercaseUrls = true);

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationReference>());

            builder.Host.UseServiceProviderFactory(new TheaterServiceProviderFactory());


            builder.Services.Configure<EmailServiceOptions>(cfg => builder.Configuration.Bind(nameof(EmailServiceOptions), cfg))
                .AddSingleton<IEmailService, EmailService>();

            builder.Services.Configure<CryptoServiceOptions>(cfg => builder.Configuration.Bind(nameof(CryptoServiceOptions), cfg))
                .AddSingleton<ICryptoService, CryptoService>();

            builder.Services.AddScoped<IIdentityService, FakeIdentityService>();
            builder.Services.AddSingleton<IFileService, FileService>();
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


            builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ContactPostApplyRequestValidator>());
            builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ContactPostReplyRequestValidator>());


            //builder.Services.AddSingleton<IValidatorInterceptor, ValidatorInterceptor>();

            builder.Services.AddFluentValidationAutoValidation(cfg =>
            {
                cfg.DisableDataAnnotationsValidation = false;
            });
            builder.Services.AddValidatorsFromAssemblyContaining<IApplicationReference>(includeInternalTypes: true);

            //builder.Services.AddSingleton<IClaimsTransformation, AppClaimsTransformation>();

            var app = builder.Build();

            app.UseIdentity(builder.Configuration);

            app.UseStaticFiles();
            app.UseRouting(); // Ensure UseRouting is called
            app.UseAuthorization(); //added afterwards


            app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}");

            app.MapControllerRoute(name: "default", pattern: "{controller=home}/{action=index}/{id?}");
            app.MapControllerRoute(name: "api", pattern: "api/{controller}/{action=Index}/{id?}");


            app.Run();

        }
    }
}
