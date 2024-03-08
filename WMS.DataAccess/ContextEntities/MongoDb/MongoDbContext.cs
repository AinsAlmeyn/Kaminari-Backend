using MongoDB.Driver;
using WMS.Core.ContextInterfaces;
using WMS.Core.Entities.ConnectionAndSettings.MongoDb;

namespace WMS.DataAccess.ContextEntities.MongoDb
{
    /// <summary>
    /// WMS library'sinin default MongoDb context nesnesidir.
    /// </summary>
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoClient _client;

        #region Constructorlar

        /// <summary>
        /// Manuel olarak baglanti adresi ve databse bilgisi set etmeye izin verir
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="database"></param>
        public MongoDbContext(string connectionString, string database)
        {
            var client = new MongoClient(connectionString);
            _client = client;
            _database = client.GetDatabase(database);
        }


        /// <summary>
        /// Bir factory fonksiyonu aracılığıyla MongoDB bağlantı ayarlarını sağlar.
        /// Bu constructor, uygulamanın çalışma zamanında dinamik olarak bağlantı ayarlarını 
        /// almasına ve yönetmesine olanak tanır. Özellikle, yapılandırma bilgilerinin 
        /// dış kaynaklardan alınması veya test sırasında mock bağlantı bilgileri kullanılması 
        /// gereken durumlarda kullanışlıdır.
        /// </summary>
        /// <param name="mongoDbConnectionFactory">MongoDb bağlantı ayarlarını sağlayan bir factory fonksiyonu.</param>
        public MongoDbContext(Func<MongoDbConnection> mongoDbConnectionFactory)
        {
            var mongoDbConnection = mongoDbConnectionFactory();
            _client = new MongoClient(mongoDbConnection.ConnectionString);
            _database = _client.GetDatabase(mongoDbConnection.Database);
        }

        /// <summary>
        /// Default baglanti adreslerini kullanir.
        /// </summary>
        public MongoDbContext()
        {
            var client = new MongoClient("baglanti adresi");
            _client = client;
            _database = client.GetDatabase("database name");
        }


        #endregion

        /// <summary>
        /// Belirtilen türdeki koleksiyonun var olup olmadığını kontrol eder ve 
        /// yoksa oluşturur. Opsiyonel olarak endeksler ekler.
        /// </summary>
        /// <typeparam name="T">MongoDB koleksiyonuna karşılık gelen sınıf.</typeparam>
        /// <param name="indexes">Oluşturulacak endeksler (opsiyonel).</param>
        public async Task EnsureCollectionExistsAsync<T>(List<CreateIndexModel<T>> indexes = null) where T : class
        {
            var collectionName = typeof(T).Name;
            var collections = await _database.ListCollectionNamesAsync();
            var collectionList = await collections.ToListAsync();

            if (!collectionList.Contains(collectionName))
            {
                await _database.CreateCollectionAsync(collectionName);

                if (indexes != null && indexes.Count > 0)
                {
                    var collection = _database.GetCollection<T>(collectionName);
                    await collection.Indexes.CreateManyAsync(indexes);
                }
            }
        }

        /// <summary>
        /// T ile calisir ve T bir sinif olmak zorundadir. Girlen T sinifi ile ayni isimde olan Collection'u birbirine baglar. Collection icerisinde Tur birligi saglar.
        /// </summary>
        /// <typeparam name="T">MongoDb collectionuna karsilik gelen bir sinif</typeparam>
        /// <returns>IMongoCollection_T</returns>
        public IMongoCollection<T> GetCollection<T>() where T : class
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// T ile calisir ve T bir sinif olmak zorundadir. Girlen T sinifi ile CollectionName parametresinden gelen Collection'u birbirine baglar. Collection icerisindeki tur birligi bozulabilir.
        /// </summary>
        /// <typeparam name="T">sinif</typeparam>
        /// <param name="CollectionName">mongodb collection adi</param>
        /// <returns>IMongoCollection_T</returns>
        public IMongoCollection<T> GetSelectedCollection<T>(string CollectionName) where T : class
        {
            return _database.GetCollection<T>(CollectionName);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}
