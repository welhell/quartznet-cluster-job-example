using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace QuartzCluster
{
  public class SchedulerBuilder
  {
    private readonly IDictionary<string, JobScheduler> schedulers;
    private IJobFactory jobFactory;

    public SchedulerBuilder()
    {
      this.schedulers = new Dictionary<string, JobScheduler>();
      this.jobFactory = null;
    }
    public SchedulerBuilder ScheduleJob<TJob>(string runEvery, JobDataMap data, bool mustReplace = false) where TJob : IJob
    {
      Contract.Requires(string.IsNullOrEmpty(runEvery));
      Contract.Requires(data != null);

      var JobName = typeof(TJob).FullName;
      var jobScheduler = new JobScheduler(typeof(TJob), runEvery, data, mustReplace);
      this.schedulers.Add(JobName, jobScheduler);
      return this;
    }

    public SchedulerBuilder WithJobFactory(IJobFactory jobFactory)
    {
      Contract.Requires(jobFactory != null);
      this.jobFactory = jobFactory;
      return this;
    }

    public async Task<IScheduler> BuildAsync()
    {
      var properties = ConfigurationManager.GetSection("quartz") as NameValueCollection;
      var factory = new StdSchedulerFactory(properties);
      var scheduler = await factory.GetScheduler().ConfigureAwait(false);
      if (this.jobFactory != null)
      {
        scheduler.JobFactory = jobFactory;
      }
      var tasks = this.schedulers.Values.Select(s => s.ScheduleAsync(scheduler));
      await Task.WhenAll(tasks).ConfigureAwait(false);
      return scheduler;
    }
    public static SchedulerBuilder Create() => new SchedulerBuilder();
  }


}

