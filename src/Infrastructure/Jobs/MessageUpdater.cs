using System;
using System.Threading.Tasks;
using Quartz;
using QuartzCluster.Entities;
using QuartzCluster;
using QuartzCluster.Repositories;
using System.Diagnostics.Contracts;

namespace QuartzCluster.Jobs
{
  [PersistJobDataAfterExecution]
  [DisallowConcurrentExecution]
  public class MessageUpdater : IJob
  {
    private readonly IMessageRepository repository;

    public MessageUpdater(IMessageRepository repostory)
    {
      Contract.Requires(repository != null);

      this.repository = repostory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
      Contract.Requires(context != null);
      
      var (dateTime, time) = context.JobDetail.JobDataMap.GetExecutionData();
      Console.WriteLine($"The job is running at {dateTime} for time{time}");
      await this.AddMessageAsync(dateTime, time).ConfigureAwait(false);
      context.JobDetail.JobDataMap.UpdateExecutionData();
    }

    private Task AddMessageAsync(DateTime dateTime, int time)
    {
      var message = Message.Create(dateTime, time);
      return this.repository.AddAsync(message);
    }
  }
}