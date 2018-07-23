using System.Diagnostics.Contracts;
using Castle.Windsor;
using Quartz;
using Quartz.Spi;

namespace QuartzCluster
{
  public class WindsorJobFactory : IJobFactory
  {
    private readonly IWindsorContainer container;

    public WindsorJobFactory(IWindsorContainer container)
    {
      Contract.Requires(container != null);

      this.container = container;
    }
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
      Contract.Requires(bundle != null);
      Contract.Requires(scheduler != null);

      return this.container.Resolve(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job)
    {
      Contract.Requires(job != null);

      this.container.Release(job);
    }
  }
}