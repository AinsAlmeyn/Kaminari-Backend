using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using WMS.Core.Entities.Base;
using WMS.Core.Entities.ConnectionAndSettings.MongoDb;

namespace WMS.Core.RepositoryInterfaces
{
    public interface IGenericRepository<T> where T : class
    {

        #region Get Islemleri

        /// <summary>
        /// Tüm T nesnelerini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <returns>GetManyResult tipinde bir nesne içerisinde alınan tüm T nesneleri veya hata durumu.</returns>
        BaseResponse<T> GetAll();

        /// <summary>
        /// Tüm T nesnelerini MongoDB koleksiyonundan alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde alınan ve sıralanmış tüm T nesneleri veya hata durumu.</returns>
        BaseResponse<T> GetAll(List<SortOption> sortOptions);
        /// <summary>
        /// Tüm T nesnelerini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak alınan tüm T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> GetAllAsync();

        /// <summary>
        /// Tüm T nesnelerini MongoDB koleksiyonundan asenkron olarak alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak alınan ve sıralanmış tüm T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> GetAllAsync(List<SortOption> sortOptions);

        #region FilterBy & FilterByAsync
        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş T nesneleri veya hata durumu.</returns>
        BaseResponse<T> FilterBy(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş ve sıralanmış T nesneleri veya hata durumu.</returns>
        BaseResponse<T> FilterBy(Expression<Func<T, bool>> filter, List<SortOption> sortOptions);

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan alır ve belirtilen sayfalama seçeneklerine göre sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş ve sayfalanmış T nesneleri veya hata durumu.</returns>
        BaseResponse<T> FilterBy(Expression<Func<T, bool>> filter, PageOptions pageOptions);


        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan alır, belirtilen sıralama ve sayfalama seçeneklerine göre işler.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş, sıralanmış ve sayfalanmış T nesneleri veya hata durumu.</returns>
        BaseResponse<T> FilterBy(
            Expression<Func<T, bool>> filter,
            List<SortOption> sortOptions,
            PageOptions pageOptions);

        /// <summary>
        /// Belirtilen filtre kriterlerine göre T nesnelerini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş T nesneleri veya hata durumu.</returns>
        BaseResponse<T> FilterBy(IEnumerable<Expression<Func<T, bool>>> filters);

        /// <summary>
        /// Belirtilen filtre kriterlerine ve sıralama seçeneklerine göre T nesnelerini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş ve sıralanmış T nesneleri veya hata durumu.</returns>
        BaseResponse<T> FilterBy(IEnumerable<Expression<Func<T, bool>>> filters, List<SortOption> sortOptions);

        /// <summary>
        /// Belirtilen filtre kriterlerine göre T nesnelerini MongoDB koleksiyonundan alır ve belirtilen sayfalama seçeneklerine göre sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş ve sayfalanmış T nesneleri veya hata durumu.</returns>
        BaseResponse<T> FilterBy(IEnumerable<Expression<Func<T, bool>>> filters, PageOptions pageOptions);

        /// <summary>
        /// Belirtilen filtre kriterlerine, sıralama seçeneklerine ve sayfalama ayarlarına göre T nesnelerini MongoDB koleksiyonundan alır.
        /// Bu metod, verilen filtrelerle uyumlu nesneleri seçer, belirtilen sıralama kriterlerine göre sıralar ve sayfalama ayarlarına uygun şekilde sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür. 
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş, sıralanmış ve sayfalanmış T nesneleri veya hata durumu.</returns>
        BaseResponse<T> FilterBy(
    IEnumerable<Expression<Func<T, bool>>> filters,
    List<SortOption> sortOptions,
    PageOptions pageOptions);

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> FilterByAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sıralanmış T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> FilterByAsync(Expression<Func<T, bool>> filter, List<SortOption> sortOptions);

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır ve belirtilen sayfalama seçeneklerine göre sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sayfalanmış T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> FilterByAsync(
    Expression<Func<T, bool>> filter,
    PageOptions pageOptions);

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır, belirtilen sıralama ve sayfalama seçeneklerine göre işler.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş, sıralanmış ve sayfalanmış T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> FilterByAsync(
    Expression<Func<T, bool>> filter,
    List<SortOption> sortOptions,
    PageOptions pageOptions);

        /// <summary>
        /// Belirtilen filtre kriterlerine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> FilterByAsync(IEnumerable<Expression<Func<T, bool>>> filters);

        /// <summary>
        /// Belirtilen filtre kriterlerine ve sıralama seçeneklerine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sıralanmış T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> FilterByAsync(IEnumerable<Expression<Func<T, bool>>> filters, List<SortOption> sortOptions);

        /// <summary>
        /// Belirtilen filtre kriterlerine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır ve belirtilen sayfalama ayarlarına göre sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sayfalanmış T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> FilterByAsync(IEnumerable<Expression<Func<T, bool>>> filters, PageOptions pageOptions);

        /// <summary>
        /// Belirtilen filtre kriterlerine, sıralama seçeneklerine ve sayfalama ayarlarına göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır.
        /// Bu metod, verilen filtrelerle uyumlu nesneleri seçer, belirtilen sıralama kriterlerine göre sıralar ve sayfalama ayarlarına uygun şekilde sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş, sıralanmış ve sayfalanmış T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> FilterByAsync(
            IEnumerable<Expression<Func<T, bool>>> filters,
            List<SortOption> sortOptions,
            PageOptions pageOptions);
        #endregion

        /// <summary>
        /// Belirtilen MongoDB filtre tanımına göre T nesnelerini asenkron olarak MongoDB koleksiyonundan alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür. Eğer sonuç bulunamazsa boş bir liste döner.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">MongoDB filtre tanımı. T nesnelerini filtrelemek için kullanılır.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <param name="pageOptions">Sayfalama seçenekleri.</param>
        /// <returns>BaseResponse_T tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sıralanmış T nesneleri, başarı durumu ve mesaj bilgisi.</returns>
        Task<BaseResponse<T>> FromDb_FilterByAsync(
            Expression<Func<T, bool>> filter,
            List<SortOption>? sortOptions,
            PageOptions? pageOptions);

        /// <summary>
        /// Belirtilen ID'ye sahip olan tek bir T nesnesini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneyi döndürülür.
        /// Herhangi bir hata oluşursa veya nesne bulunamazsa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="id">Alınacak T nesnesinin ID'si.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde alınan T nesnesi veya hata durumu.</returns>
        BaseResponse<T> GetById(string id);

        /// <summary>
        /// Belirtilen ID'ye sahip olan tek bir T nesnesini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneyi döndürülür.
        /// Herhangi bir hata oluşursa veya nesne bulunamazsa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="id">Asenkron olarak alınacak T nesnesinin ID'si.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak alınan T nesnesi veya hata durumu.</returns>
        Task<BaseResponse<T>> GetByIdAsync(string id);

        /// <summary>
        /// MongoDB koleksiyonundaki T nesnelerini gruplar ve gruplanmış sonuçları döndürür.
        /// </summary>
        /// <param name="groupExpression">Gruplama için kullanılacak ifade.</param>
        /// <returns>Gruplanmış sonuçları içeren bir liste.</returns>
        Task<BaseResponse<BsonDocument>> GroupAndAggregateAsync<T>(
    Expression<Func<T, object>> groupByExpression,
    Dictionary<Expression<Func<T, object>>, MongoAggregateOperation>? aggregations);
        //Task<BaseResponse<BsonDocument>> GroupAndAggregateAsync<T>(
        //    Expression<Func<T, object>> groupByExpression,
        //    Dictionary<Expression<Func<T, object>>, MongoAggregateOperation> aggregations);
        /// <summary>
        /// Belirtilen MongoDB filtre tanımına göre T nesnelerini asenkron olarak MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür. Eğer sonuç bulunamazsa boş bir liste döner.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">MongoDB filtre tanımı. T nesnelerini filtrelemek için kullanılır.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş T nesneleri, başarı durumu ve mesaj bilgisi.</returns>
        Task<BaseResponse<T>> FilterDefinitionByAsync(FilterDefinition<T> filter);

        #endregion

        #region Insert Islemleri

        /// <summary>
        /// Birden fazla T nesnesini MongoDB koleksiyonuna ekler.
        /// Başarılı bir şekilde eklenirse, eklenen nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="entities">MongoDB koleksiyonuna eklenmek üzere olan T nesnelerinin koleksiyonu.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde eklenen T nesneleri veya hata durumu.</returns>
        BaseResponse<T> InsertMany(ICollection<T> entities);

        /// <summary>
        /// Birden fazla T nesnesini MongoDB koleksiyonuna asenkron olarak ekler.
        /// Başarılı bir şekilde eklenirse, eklenen nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="entities">Asenkron olarak MongoDB koleksiyonuna eklenmek üzere olan T nesnelerinin koleksiyonu.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak eklenen T nesneleri veya hata durumu.</returns>
        Task<BaseResponse<T>> InsertManyAsync(ICollection<T> entities);

        /// <summary>
        /// Tek bir T nesnesini MongoDB koleksiyonuna ekler.
        /// Başarılı bir şekilde eklenirse, eklenen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="entity">MongoDB koleksiyonuna eklenmek üzere olan tek bir T nesnesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde eklenen T nesnesi veya hata durumu.</returns>
        BaseResponse<T> InsertOne(T entity);

        /// <summary>
        /// Tek bir T nesnesini MongoDB koleksiyonuna asenkron olarak ekler.
        /// Başarılı bir şekilde eklenirse, eklenen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="entity">Asenkron olarak MongoDB koleksiyonuna eklenmek üzere olan tek bir T nesnesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak eklenen T nesnesi veya hata durumu.</returns>
        Task<BaseResponse<T>> InsertOneAsync(T entity);

        /// <summary>
        /// Asenkron olarak bir MongoDB koleksiyonunda belirtilen filtreye uygun bir kaydı günceller, eğer uygun kayıt yoksa yeni bir kayıt ekler (upsert işlemi).
        /// Bu işlem, genel bir veri tipi (T) üzerinde çalışır ve işlemin sonucunu bir BaseResponse_T nesnesi olarak döndürür.
        /// İşlem başarılı olduğunda, işlem türüne (ekleme veya güncelleme) göre özelleştirilmiş bir başarı mesajı içerir.
        /// Hata oluşması durumunda, hata detaylarını içeren bir hata yanıtı döndürür.
        /// </summary>
        /// <param name="filter">MongoDB koleksiyonunda arama yapmak için kullanılan lambda ifadesi.</param>
        /// <param name="entity">Eklenecek veya güncellenecek varlık nesnesi.</param>
        /// <returns>İşlem sonucunu içeren BaseResponse_T nesnesi.</returns>
        Task<BaseResponse<T>> UpsertOneAsync(Expression<Func<T, bool>> filter, T entity);

        #endregion

        #region Delete Islemleri
        /// <summary>
        /// Belirtilen ID'ye sahip olan tek bir T nesnesini MongoDB koleksiyonundan siler.
        /// Başarılı bir şekilde silinirse, silinen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="id">Silinecek T nesnesinin ID'si.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde silinen T nesnesi veya hata durumu.</returns>

        BaseResponse<T> DeleteById(string id);

        /// <summary>
        /// Belirtilen ID'ye sahip olan tek bir T nesnesini MongoDB koleksiyonundan asenkron olarak siler.
        /// Başarılı bir şekilde silinirse, silinen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="id">Asenkron olarak silinecek T nesnesinin ID'si.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak silinen T nesnesi veya hata durumu.</returns>

        Task<BaseResponse<T>> DeleteByIdAsync(string id);

        /// <summary>
        /// Belirtilen filtre kriterine göre birden fazla T nesnesini MongoDB koleksiyonundan siler.
        /// Başarılı bir şekilde silinirse, silme işlemi ile ilgili bilgi mesajı döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Silinecek T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde silme işlemi sonucu veya hata durumu.</returns>
        BaseResponse<T> DeleteMany(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Belirtilen filtre kriterine göre birden fazla T nesnesini MongoDB koleksiyonundan asenkron olarak siler.
        /// Başarılı bir şekilde silinirse, silme işlemi ile ilgili bilgi mesajı döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Asenkron olarak silinecek T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak silme işlemi sonucu veya hata durumu.</returns>
        Task<BaseResponse<T>> DeleteManyAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Belirtilen filtre kriterine göre tek bir T nesnesini MongoDB koleksiyonundan siler.
        /// Başarılı bir şekilde silinirse, silinen nesneyi döndürülür.
        /// Eğer uygun bir nesne bulunamazsa veya hata oluşursa, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Silinecek tek bir T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde silinen T nesnesi veya hata durumu.</returns>
        BaseResponse<T> DeleteOne(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Belirtilen filtre kriterine göre tek bir T nesnesini MongoDB koleksiyonundan asenkron olarak siler.
        /// Başarılı bir şekilde silinirse, silinen nesneyi döndürülür.
        /// Eğer uygun bir nesne bulunamazsa veya hata oluşursa, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Asenkron olarak silinecek tek bir T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak silinen T nesnesi veya hata durumu.</returns>
        Task<BaseResponse<T>> DeleteOneAsync(Expression<Func<T, bool>> filter);

        #endregion

        #region Update Islemleri

        /// <summary>
        /// Belirtilen filtre kriterine göre bir T nesnesini MongoDB koleksiyonunda asenkron olarak günceller.
        /// Başarılı bir şekilde güncellenirse, güncellenen nesne sayısı ile birlikte başarılı bir sonuç döndürülür.
        /// Herhangi bir hata oluşursa veya güncelleme işlemi gerçekleşmezse, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Asenkron olarak güncellenecek T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="update">MongoDB koleksiyonunda yapılacak güncellemeleri tanımlayan bir güncelleme tanımlayıcısı.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak güncellenen T nesnesi veya hata durumu.</returns>

        Task<BaseResponse<T>> UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update);

        /// <summary>
        /// Belirtilen filtre kriterine göre bir T nesnesini MongoDB koleksiyonunda senkron olarak günceller.
        /// Başarılı bir şekilde güncellenirse, güncellenen nesne sayısı ile birlikte başarılı bir sonuç döndürülür.
        /// Herhangi bir hata oluşursa veya güncelleme işlemi gerçekleşmezse, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Senkron olarak güncellenecek T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="update">MongoDB koleksiyonunda yapılacak güncellemeleri tanımlayan bir güncelleme tanımlayıcısı.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde senkron olarak güncellenen T nesnesi veya hata durumu.</returns>

        BaseResponse<T> UpdateOne(Expression<Func<T, bool>> filter, UpdateDefinition<T> update);

        /// <summary>
        /// Belirtilen filtre kriterine göre bir T nesnesini MongoDB koleksiyonunda başka bir nesne ile değiştirir.
        /// Başarılı bir şekilde değiştirilirse, değiştirilen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa veya değiştirme işlemi gerçekleşmezse, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Değiştirilecek T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="entity">MongoDB koleksiyonunda yerine konacak yeni T nesnesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde değiştirilen T nesnesi veya hata durumu.</returns>

        BaseResponse<T> ReplaceOne(Expression<Func<T, bool>> filter, T entity);

        /// <summary>
        /// Belirtilen filtre kriterine göre bir T nesnesini MongoDB koleksiyonunda asenkron olarak başka bir nesne ile değiştirir.
        /// Başarılı bir şekilde değiştirilirse, değiştirilen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa veya değiştirme işlemi gerçekleşmezse, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Asenkron olarak değiştirilecek T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="entity">Asenkron olarak MongoDB koleksiyonunda yerine konacak yeni T nesnesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak değiştirilen T nesnesi veya hata durumu.</returns>

        Task<BaseResponse<T>> ReplaceOneAsync(Expression<Func<T, bool>> filter, T entity);

        #endregion
    }
}
