using ACPROpenAI.Services;
using ACPROpenAI.Services.Abstractions;
using Autofac;

namespace ACPROpenAI
{
    public class ACPRRegistrator : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            #region Services
            builder.RegisterType<ACPROpenAIRestService>().As<IACPROpenAIRestService>();
            #endregion
        }
    }
}
