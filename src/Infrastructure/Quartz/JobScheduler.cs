using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Quartz;

namespace QuartzCluster
{
  internal class JobScheduler
  {
    private readonly Type type;
    private readonly string runEvery;
    private readonly bool mustReplace;
    private readonly JobDataMap data;
    private readonly JobKey jobKey;

    public JobScheduler(Type type, string runEvery, JobDataMap data, bool mustReplace)
    {
      Contract.Requires(type != null);
      Contract.Requires(string.IsNullOrEmpty(runEvery));
      Contract.Requires(data != null);

      this.type = type;
      this.runEvery = runEvery;
      this.mustReplace = mustReplace;
      this.data = data;
      this.jobKey = new JobKey(this.type.Name);
    }
    public async Task ScheduleAsync(IScheduler scheduler)
    {
      Contract.Requires(scheduler != null);

      await this.DeleteJobIfHaveToAsync(scheduler).ConfigureAwait(false);
      await this.TryToScheduleAsync(scheduler).ConfigureAwait(false);
    }

    private async Task DeleteJobIfHaveToAsync(IScheduler scheduler)
    {
      if (this.mustReplace)
      {
        if (await scheduler.CheckExists(this.jobKey).ConfigureAwait(false))
        {
          await scheduler.DeleteJob(this.jobKey).ConfigureAwait(false);
        }
      }
    }
    private async Task TryToScheduleAsync(IScheduler scheduler)
    {
      if (!await scheduler.CheckExists(this.jobKey).ConfigureAwait(false))
      {
        var jobDetail = this.CreateJob();
        var trigger = this.CreateTrigger();
        await scheduler.ScheduleJob(jobDetail, trigger).ConfigureAwait(false);
      }
    }
    private IJobDetail CreateJob() =>
    JobBuilder.Create(this.type)
            .WithIdentity(this.jobKey)
            .StoreDurably(true)
            .SetJobData(this.data)
            .RequestRecovery()
            .Build();

    private ITrigger CreateTrigger() =>
      TriggerBuilder.Create()
                  .WithIdentity(this.jobKey.Name)
                  .WithCronSchedule(this.runEvery)
                  .Build();

  }
}