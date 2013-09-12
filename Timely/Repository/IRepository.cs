using System.Linq;
using MongoDB.Driver;

namespace Moravia.Timely
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity Create(TEntity entity);
        IQueryable<TEntity> Read();
        TEntity Read(string id);
        bool Update(string id, TEntity entity);
        bool Delete(string id);

        MongoCollection<TEntity> Collection { get; }
        MongoDatabase Database { get; }
    }
}
