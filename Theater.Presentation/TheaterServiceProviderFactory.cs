using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Theater.Presentation
{
    public class TheaterServiceProviderFactory : AutofacServiceProviderFactory
    {
        public TheaterServiceProviderFactory()
            : base(Register) { }

        private static void Register(ContainerBuilder builder)
        {
            builder.RegisterModule<TheaterDIModule>();
        }
    }
}
