using Autofac;
using SilverzoneERP.Context;

namespace SilverzoneERP.Api.Modules
{
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(SilverzoneERPContext)).As(typeof(SilverzoneERPContext)).InstancePerLifetimeScope();
        }
    }
}