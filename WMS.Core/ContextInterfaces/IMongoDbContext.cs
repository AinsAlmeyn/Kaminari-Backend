using MongoDB.Driver;

namespace WMS.Core.ContextInterfaces
{
    /// <summary>
    /// MongoDb baglanti islerini yonetecek olan context nesnesinin temel yapilanmalarini belirleyen Interface'dir.
    /// </summary>
    public interface IMongoDbContext
    {
        /// <summary>
        /// T ile calisir ve T bir sinif olmak zorundadir. Girlen T sinifi ile ayni isimde olan Collection'u birbirine baglar. Collection icerisinde Tur birligi saglar.
        /// </summary>
        /// <typeparam name="T">MongoDb collectionuna karsilik gelen bir sinif</typeparam>
        /// <returns>IMongoCollection_T</returns>
        IMongoCollection<T> GetCollection<T>() where T : class;

        /// <summary>
        /// T ile calisir ve T bir sinif olmak zorundadir. Girlen T sinifi ile CollectionName parametresinden gelen Collection'u birbirine baglar. Collection icerisindeki tur birligi bozulabilir.
        /// </summary>
        /// <typeparam name="T">sinif</typeparam>
        /// <param name="CollectionName">mongodb collection adi</param>
        /// <returns>IMongoCollection_T</returns>
        IMongoCollection<T> GetSelectedCollection<T>(string CollectionName) where T : class;

        /// <summary>
        /// Belirtilen türdeki koleksiyonun var olup olmadığını kontrol eder ve 
        /// yoksa oluşturur. Opsiyonel olarak endeksler ekler.
        /// </summary>
        /// <typeparam name="T">MongoDB koleksiyonuna karşılık gelen sınıf.</typeparam>
        /// <param name="indexes">Oluşturulacak endeksler (opsiyonel).</param>
        Task EnsureCollectionExistsAsync<T>(List<CreateIndexModel<T>> indexes = null) where T : class;

        IMongoDatabase GetDatabase();
    }
}
