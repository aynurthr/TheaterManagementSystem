using Autofac;
using Microsoft.EntityFrameworkCore;
using Theater.Repository;
using Theater.DataAccessLayer.Contexts;

namespace Theater.Presentation
{
    public class TheaterDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register all repository interfaces and their implementations
            var assembly = typeof(IRepositoryReference).Assembly;
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().InstancePerLifetimeScope();

            // Directly register the DataContext type
            builder.RegisterType<DataContext>().As<DbContext>().InstancePerLifetimeScope();
        }
    }
}
