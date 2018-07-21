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
        this.container = container;
    }
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) => 
    this.container.Resolve(bundle.JobDetail.JobType) as IJob;

    public void ReturnJob(IJob job) =>
      this.container.Release(job);
    
  }
}