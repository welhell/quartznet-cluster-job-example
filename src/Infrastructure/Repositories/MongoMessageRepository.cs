using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using MongoDB.Driver;
using QuartzCluster.Entities;

namespace QuartzCluster.Repositories
{
  public class MongoMesageRepository : IMessageRepository
  {
    private readonly IMongoCollection<Message> messages;
    public MongoMesageRepository(MongoUrl url)
    {
      Contract.Requires(url != null);

      var client = new MongoClient(url);
      var database = client.GetDatabase(url.DatabaseName);
      this.messages = database.GetCollection<Message>("Messages");
    }

    public Task AddAsync(Message message)
    {
      Contract.Requires(message != null);
      return this.messages.InsertOneAsync(message);
    }
  }

}