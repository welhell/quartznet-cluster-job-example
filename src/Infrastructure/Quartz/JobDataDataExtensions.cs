using System;
using Quartz;

namespace QuartzCluster
{
  public static class JobDataMapxtensions
  {
    private const string CURRENT_DATE_TIME = "k_currentDateTime";
    private const string TIME = "k_time";
    public static (DateTime, int) GetExecutionData(this JobDataMap data)
    {
      var time = data.GetInt(TIME);
      var datetime = data.GetDateTime(CURRENT_DATE_TIME);
      return (datetime, time);
    }

    public static void UpdateExecutionData(this JobDataMap data)
    {
      var time = data.GetInt(TIME) + 1;
      data.Put(TIME, time);
      data.Put(CURRENT_DATE_TIME, DateTime.Now);
    }

    public static void SetExecutionData(this JobDataMap data)
    {
      data.Put(TIME, 0);
      data.Put(CURRENT_DATE_TIME, DateTime.Now);
    }
  }

}