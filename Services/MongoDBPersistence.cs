using MongoDB.Bson;
using MongoDB.Driver;

namespace SendMe.Services
{
    public class MongoDBPersistence<T> :  IPersistence<T> where T : IModel
    {
        private string _dataBaseName;
        private string _schemaName;
        private IMongoCollection<T> _collection;


        public MongoDBPersistence(string databaseName, string schemaName) {
            _dataBaseName = databaseName;
            _schemaName = schemaName;
            OpenDatabase(_schemaName);
        }

        private void OpenDatabase(string schemaName) {

            string? connection = Environment.GetEnvironmentVariable("MONGO_CONNECTION");
           
            if (string.IsNullOrEmpty(connection))
            {
                throw new ArgumentNullException("mongo connection is null or empty");
            }

            var settings = MongoClientSettings.FromConnectionString(connection);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(_dataBaseName);
            _collection = database.GetCollection<T>(schemaName);
            
            if (_collection == null)
            {
                 database.CreateCollection(schemaName);
                _collection = database.GetCollection<T>(schemaName);
            }


        }

        public async Task<IEnumerable<T>> FindAll()        {

            return await _collection.FindAsync<T>(f=> true).Result.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByEqualsFilter<V>(string property, V value)
        {
            var filter = Builders<T>.Filter.Eq(property, value);
            return await _collection.FindAsync<T>(filter).Result.ToListAsync();
        }

        public async Task<T> FindByID(string id)
        {
            return await _collection.FindAsync<T>(x => x.Id==id  ).Result.FirstOrDefaultAsync();
        }

        public async Task Remove(string id)
        {
            await _collection.DeleteOneAsync(f=> f.Id == id);
     
        }

        public async Task<T> Save(T entity)
        {


            if (!String.IsNullOrEmpty(entity.Id))
            {
                await Remove(entity.Id);
                await _collection.InsertOneAsync(entity);
                return entity;
            }
            else 
            {
                var result = await _collection.ReplaceOneAsync(f => false, entity, new ReplaceOptions { IsUpsert = true });
                entity.Id = result.UpsertedId.ToString();
                return entity;
            }

        }

    }
}
