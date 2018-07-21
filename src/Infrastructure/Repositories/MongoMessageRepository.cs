using System.Threading.Tasks;
using MongoDB.Driver;
using QuartzCluster.Entities;

namespace QuartzCluster.Repositories
{
  public class MongoMesageRepository : IMessageRepository
  {
    private readonly IMongoCollection<Message> collecttion;
    public MongoMesageRepository(MongoUrl url )
    {
      var client = new MongoClient(url);
      var database = client.GetDatabase(url.DatabaseName);
      this.collecttion = database.GetCollection<Message>("Messages");
    }

    public Task AddAsync(Message message) =>    
      this.collecttion.InsertOneAsync(message);    
  }

}