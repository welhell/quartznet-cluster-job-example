using System.Configuration;
using System.Diagnostics.Contracts;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MongoDB.Driver;
using QuartzCluster.Jobs;
using QuartzCluster.Repositories;

namespace QuartzCluster
{
  public class RegisterServices : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      Contract.Requires(container != null);

      container.Register(Component.For<IMessageRepository>()
                                .UsingFactoryMethod(this.CreateRepository)
                                .LifestyleSingleton())
             .Register(Component.For<MessageUpdater>()
                                .ImplementedBy<MessageUpdater>()
                                .LifestyleSingleton());
    }

    private IMessageRepository CreateRepository()
    {
      var url = MongoUrl.Create(ConfigurationManager.ConnectionStrings["messagesMongoDB"].ConnectionString);
      return new MongoMesageRepository(url);
    }
  }
}