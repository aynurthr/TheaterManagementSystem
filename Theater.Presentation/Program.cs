using Microsoft.EntityFrameworkCore;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Theater.Infrastructure.Abstracts;
using Theater.Application.Services.File;
using Theater.Application.Services.Identity;
using Theater.Application;
using Theater.Application.Repositories;
using Theater.Repository;
using Theater.DataAccessLayer.Contexts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
//using Theater.Infrastructure.Repositories;

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
            //builder.Services.AddScoped<IPosterRepository, PostersRepository>();
            builder.Services.AddScoped<ISeatRepository, SeatRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
