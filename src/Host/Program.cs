using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using QuartzCluster.Repositories;
using QuartzCluster.Jobs;
using Quartz;
using System.Threading.Tasks;
using Quartz.Impl;
using MongoDB.Driver;
using System.Configuration;
using System.Collections.Specialized;
using QuartzCluster;
using Quartz.Spi;
using Castle.Windsor.Installer;

namespace QuartzCluster.Host
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Starting ...");
      var scheduler = CreateAndConfigureSchedulerAsync().Result;
      scheduler.Start().Wait();
      Console.WriteLine("Press a key to exit ...");
      Console.ReadLine();
      scheduler.Shutdown().Wait();
    }

    public static Task<IScheduler> CreateAndConfigureSchedulerAsync()
    {
      var runEvery = ConfigurationManager.AppSettings["runEvery"];
      var jobData = new JobDataMap();
      jobData.SetExecutionData();
      return SchedulerBuilder.Create()
                             .WithJobFactory(CreateJobFactory())
                             .ScheduleJob<MessageUpdater>(runEvery, jobData)
                             .BuildAsync();
    }

    private static IJobFactory CreateJobFactory()
    {
      var container = new WindsorContainer().Install(new []{new RegisterServices()});
      return new WindsorJobFactory(container);
    }
  }
}
