using Autofac;
using Microsoft.EntityFrameworkCore;
using Theater.Repository;
using Theater.DataAccessLayer.Contexts;
using Theater.DataAccessLayer;

namespace Theater.Presentation
{
    public class TheaterDIModule : Module
    {
        //protected override void Load(ContainerBuilder builder)
        //{
        //    base.Load(builder);
        //    var assembly = typeof(IRepositoryReference).Assembly;

        //    builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().InstancePerLifetimeScope();

        //    builder.RegisterType<DataContext>().As<DbContext>().InstancePerLifetimeScope();
        //}

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var assembly = typeof(IRepositoryReference).Assembly;

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var type = typeof(IDataAccessLayerReference).Assembly.GetType("Theater.DataAccessLayer.Contexts.DataContext");
            builder.RegisterType(type).As<DbContext>().InstancePerLifetimeScope();
        }
    }
}
