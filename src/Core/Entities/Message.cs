using System;

namespace QuartzCluster.Entities
{
  public class Message
  {
    public Message(Guid id, DateTime dateTime,int time )
    {
      this.Id = id;
      this.DateTime = dateTime;
      this.Time = time;
    }

    public Guid Id { get; }
    public DateTime DateTime { get; }

    public int Time { get; }

    public static Message Create(DateTime datetime, int time) => new Message(Guid.NewGuid(), datetime, time);
  }
}