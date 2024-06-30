using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Theater.Application;
using Theater.Application.Modules.ContactPostModule.Commands.ContactPostApplyCommand;
using Theater.Application.Services.File;
using Theater.Application.Services.Identity;
using Theater.DataAccessLayer.Contexts;
using Theater.Infrastructure.Abstracts;

namespace Theater.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("cString")));

            builder.Services.AddControllersWithViews();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<IApplicationReference>());

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterModule<TheaterDIModule>();
            });

            builder.Services.AddScoped<IIdentityService, FakeIdentityService>();
            builder.Services.AddSingleton<IFileService, FileService>();
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            builder.Services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ContactPostApplyRequestValidator>());

            //// Add logging
            //builder.Services.AddLogging(config =>
            //{
            //    config.ClearProviders();
            //    config.AddConsole();
            //    config.AddDebug();
            //});

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}
