using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using WMS.Core.ContextInterfaces;
using WMS.Core.RepositoryInterfaces;
using WMS.Core.Entities.Base;
using WMS.Core.Entities.ConnectionAndSettings.MongoDb;
using MongoDB.Driver.Linq;

namespace WMS.DataAccess.RepositoryEntities
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        protected readonly IMongoCollection<T> _collection;

        public GenericRepository(IMongoDbContext _context)
        {
            _collection = _context.GetCollection<T>();
        }

        #region Get Islemleri
        /// <summary>
        /// Tüm T nesnelerini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <returns>GetManyResult tipinde bir nesne içerisinde alınan tüm T nesneleri veya hata durumu.</returns>
        public virtual BaseResponse<T> GetAll()
        {
            BaseResponse<T> result = new BaseResponse<T>();
            try
            {
                List<T> data = _collection.AsQueryable().ToList();
                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        result.Data = data;
                        result.DefinitionLang = "Veriler basariyla alindi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.GetAll";
                        result.Detail = "";
                    }
                    else
                    {
                        result.Data = data;
                        result.DefinitionLang = "Listelenecek Veri bulunamadi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.GetAll";
                        result.Detail = "";
                    }
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Veriler alinamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.GetAll";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.GetAll";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Tüm T nesnelerini MongoDB koleksiyonundan alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde alınan ve sıralanmış tüm T nesneleri veya hata durumu.</returns>
        public virtual BaseResponse<T> GetAll(List<SortOption> sortOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var query = _collection.AsQueryable();

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda(property, param);

                    query = sortOption.Ascending
                        ? Queryable.OrderBy(query, (dynamic)lambda)
                        : Queryable.OrderByDescending(query, (dynamic)lambda);
                }

                List<T> data = query.ToList();
                result.Data = data;
                result.DefinitionLang = data.Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.GetAll";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.GetAll";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Tüm T nesnelerini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak alınan tüm T nesneleri veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> GetAllAsync()
        {
            BaseResponse<T> result = new BaseResponse<T>();
            try
            {
                List<T> data = await _collection.AsQueryable().ToListAsync();
                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        result.Data = data;
                        result.DefinitionLang = "Veriler basariyla alindi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.GetAllAsync";
                        result.Detail = null;
                    }
                    else
                    {
                        result.Data = data;
                        result.DefinitionLang = "Listelenecek Veri bulunamadi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.GetAllAsync";
                        result.Detail = null;
                    }
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Veriler alinamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.GetAllAsync";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.GetAllAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }


        /// <summary>
        /// Tüm T nesnelerini MongoDB koleksiyonundan asenkron olarak alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak alınan ve sıralanmış tüm T nesneleri veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> GetAllAsync(List<SortOption> sortOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var query = _collection.AsQueryable();

                // İlk sıralama seçeneği için özel bir işlem yapılır
                if (sortOptions.Count > 0)
                {
                    var firstSortOption = sortOptions[0];
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, firstSortOption.FieldName);
                    var lambda = Expression.Lambda(property, param);

                    query = firstSortOption.Ascending
                        ? Queryable.OrderBy(query, (dynamic)lambda)
                        : Queryable.OrderByDescending(query, (dynamic)lambda);

                    // Diğer sıralama seçenekleri için işlem yapılır
                    foreach (var sortOption in sortOptions.Skip(1))
                    {
                        param = Expression.Parameter(typeof(T), "x");
                        property = Expression.Property(param, sortOption.FieldName);
                        lambda = Expression.Lambda(property, param);

                        query = sortOption.Ascending
                            ? Queryable.ThenBy((IOrderedQueryable<T>)query, (dynamic)lambda)
                            : Queryable.ThenByDescending((IOrderedQueryable<T>)query, (dynamic)lambda);
                    }
                }

                List<T> data = await query.ToListAsync();
                result.Data = data;
                result.DefinitionLang = data.Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.GetAllAsync";
                result.Detail = null;
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.GetAllAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş T nesneleri veya hata durumu.</returns>
        public virtual BaseResponse<T> FilterBy(Expression<Func<T, bool>> filter)
        {
            BaseResponse<T> result = new BaseResponse<T>();
            try
            {
                List<T> data = _collection.Find(filter).ToList(); // collectiondaki tum datalari don
                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        result.Data = data;
                        result.DefinitionLang = "Veriler basariyla alindi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.FilterBy";
                        result.Detail = "";
                    }
                    else
                    {
                        result.Data = data;
                        result.DefinitionLang = "Listelenecek Veri bulunamadi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.FilterBy";
                        result.Detail = "";
                    }
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Veriler alinamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.FilterBy";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterBy";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş ve sıralanmış T nesneleri veya hata durumu.</returns>
        public virtual BaseResponse<T> FilterBy(Expression<Func<T, bool>> filter, List<SortOption> sortOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var findFluent = _collection.Find(filter);
                var data = findFluent.ToList(); // Verileri önce listeye çevir
                var queryableData = data.AsQueryable(); // Daha sonra IQueryable'a dönüştür

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda(property, param);

                    queryableData = sortOption.Ascending
                        ? Queryable.OrderBy(queryableData, (dynamic)lambda)
                        : Queryable.OrderByDescending(queryableData, (dynamic)lambda);
                }

                result.Data = queryableData.ToList(); // Sonuçları tekrar listeye çevir
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterBy";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterBy";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan alır ve belirtilen sayfalama seçeneklerine göre sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş ve sayfalanmış T nesneleri veya hata durumu.</returns>
        public virtual BaseResponse<T> FilterBy(Expression<Func<T, bool>> filter, PageOptions pageOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                // MongoDB koleksiyonundan filtre koşuluna göre verileri al
                var filteredData = _collection.Find(filter).ToList();

                // Sayfalama için IQueryable'a dönüştür
                var queryableData = filteredData.AsQueryable();

                // Sayfalama uygula
                var pagedData = queryableData
                    .Skip((pageOptions.PageNumber - 1) * pageOptions.PageSize)
                    .Take(pageOptions.PageSize)
                    .ToList();

                // Sonuçları döndür
                result.Data = pagedData;
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterBy";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterBy";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan alır, belirtilen sıralama ve sayfalama seçeneklerine göre işler.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş, sıralanmış ve sayfalanmış T nesneleri veya hata durumu.</returns>
        public virtual BaseResponse<T> FilterBy(Expression<Func<T, bool>> filter, List<SortOption> sortOptions, PageOptions pageOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                // MongoDB koleksiyonundan filtre koşuluna göre verileri al
                var filteredData = _collection.Find(filter).ToList();

                // IQueryable'a dönüştür
                var queryableData = filteredData.AsQueryable();

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda<Func<T, object>>(property, param);

                    queryableData = sortOption.Ascending
                        ? queryableData.OrderBy(lambda)
                        : queryableData.OrderByDescending(lambda);
                }

                // Sayfalama uygula
                var pagedData = queryableData
                    .Skip((pageOptions.PageNumber - 1) * pageOptions.PageSize)
                    .Take(pageOptions.PageSize)
                    .ToList();

                // Sonuçları döndür
                result.Data = pagedData;
                result.DefinitionLang = pagedData != null && pagedData.Any() ? "Veriler basariyla alindi" : "Listelenecek veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterBy";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterBy";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterlerine göre T nesnelerini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş T nesneleri veya hata durumu.</returns>
        public virtual BaseResponse<T> FilterBy(IEnumerable<Expression<Func<T, bool>>> filters)
        {
            var result = new BaseResponse<T>();
            try
            {
                var combinedFilter = Builders<T>.Filter.And(filters.Select(f => Builders<T>.Filter.Where(f)));
                var data = _collection.Find(combinedFilter).ToList();

                result.Data = data;
                result.DefinitionLang = data.Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterBy";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterBy";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterlerine ve sıralama seçeneklerine göre T nesnelerini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş ve sıralanmış T nesneleri veya hata durumu.</returns>
        public virtual BaseResponse<T> FilterBy(IEnumerable<Expression<Func<T, bool>>> filters, List<SortOption> sortOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var combinedFilter = Builders<T>.Filter.And(filters.Select(f => Builders<T>.Filter.Where(f)));
                var findFluent = _collection.Find(combinedFilter);
                var data = findFluent.ToList(); // Verileri önce listeye çevir

                var queryableData = data.AsQueryable(); // Daha sonra IQueryable'a dönüştür

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda<Func<T, object>>(property, param);

                    queryableData = sortOption.Ascending
                        ? queryableData.OrderBy(lambda)
                        : queryableData.OrderByDescending(lambda);
                }

                var resultList = queryableData.ToList(); // Sonuçları tekrar listeye çevir
                result.Data = resultList;
                result.DefinitionLang = resultList.Any() ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterBy";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterBy";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterlerine göre T nesnelerini MongoDB koleksiyonundan alır ve belirtilen sayfalama seçeneklerine göre sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde filtrelenmiş ve sayfalanmış T nesneleri veya hata durumu.</returns>
        public virtual BaseResponse<T> FilterBy(IEnumerable<Expression<Func<T, bool>>> filters, PageOptions pageOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var combinedFilter = Builders<T>.Filter.And(filters.Select(f => Builders<T>.Filter.Where(f)));
                var findFluent = _collection.Find(combinedFilter);
                var data = findFluent.ToList();

                // IQueryable'a dönüştür ve sayfalama uygula
                var pagedData = data
                    .AsQueryable()
                    .Skip((pageOptions.PageNumber - 1) * pageOptions.PageSize)
                    .Take(pageOptions.PageSize)
                    .ToList();

                result.Data = pagedData;
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterBy";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterBy";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }



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
        public virtual BaseResponse<T> FilterBy(
    IEnumerable<Expression<Func<T, bool>>> filters,
    List<SortOption> sortOptions,
    PageOptions pageOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                // Filtreleri birleştir
                var combinedFilter = Builders<T>.Filter.And(filters.Select(f => Builders<T>.Filter.Where(f)));

                // Veritabanından filtrelenmiş verileri al
                var filteredData = _collection.Find(combinedFilter).ToList();

                // Sıralama ve sayfalama için IQueryable'a dönüştür
                var queryableData = filteredData.AsQueryable();

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda<Func<T, object>>(property, param);

                    queryableData = sortOption.Ascending
                        ? queryableData.OrderBy(lambda)
                        : queryableData.OrderByDescending(lambda);
                }

                // Sayfalama uygula
                var pagedData = queryableData
                    .Skip((pageOptions.PageNumber - 1) * pageOptions.PageSize)
                    .Take(pageOptions.PageSize);

                result.Data = pagedData.ToList();
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterBy";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterBy";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }


        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş T nesneleri veya hata durumu.</returns
        public virtual async Task<BaseResponse<T>> FilterByAsync(Expression<Func<T, bool>> filter)
        {
            var result = new BaseResponse<T>();
            try
            {
                var data = await _collection.Find(filter).ToListAsync(); // collectiondaki tum datalari don
                if (data == null)
                {
                    result.Data = null;
                    result.DefinitionLang = "Veriler alinamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.FilterByAsync";
                    result.Detail = "";
                }
                else
                {
                    if (data.Count > 0)
                    {
                        result.Data = data;
                        result.DefinitionLang = "Veriler basariyla alindi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.FilterByAsync";
                        result.Detail = "";
                    }
                    else
                    {
                        result.Data = data;
                        result.DefinitionLang = "Listelenecek Veri bulunamadi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.FilterByAsync";
                        result.Detail = "";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sıralanmış T nesneleri veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> FilterByAsync(Expression<Func<T, bool>> filter, List<SortOption> sortOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var findFluent = _collection.Find(filter);
                var data = await findFluent.ToListAsync(); // Verileri önce listeye çevir
                var queryableData = data.AsQueryable(); // Daha sonra IQueryable'a dönüştür

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda(property, param);

                    queryableData = sortOption.Ascending
                        ? Queryable.OrderBy(queryableData, (dynamic)lambda)
                        : Queryable.OrderByDescending(queryableData, (dynamic)lambda);
                }

                result.Data = queryableData.ToList(); // Sonuçları tekrar listeye çevir
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterByAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır ve belirtilen sayfalama seçeneklerine göre sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sayfalanmış T nesneleri veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> FilterByAsync(
    Expression<Func<T, bool>> filter,
    PageOptions pageOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var findFluent = _collection.Find(filter);
                var data = await findFluent.ToListAsync();

                // IQueryable'a dönüştür ve sayfalama uygula
                var pagedData = data
                    .AsQueryable()
                    .Skip((pageOptions.PageNumber - 1) * pageOptions.PageSize)
                    .Take(pageOptions.PageSize)
                    .ToList();

                result.Data = pagedData;
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterByAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }


        /// <summary>
        /// Belirtilen filtre kriterine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır, belirtilen sıralama ve sayfalama seçeneklerine göre işler.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş, sıralanmış ve sayfalanmış T nesneleri veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> FilterByAsync(
    Expression<Func<T, bool>> filter,
    List<SortOption> sortOptions,
    PageOptions pageOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var findFluent = _collection.Find(filter);
                var data = await findFluent.ToListAsync();

                var queryableData = data.AsQueryable();

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda<Func<T, object>>(property, param);

                    queryableData = sortOption.Ascending
                        ? queryableData.OrderBy(lambda)
                        : queryableData.OrderByDescending(lambda);
                }

                // Sayfalama uygula
                var pagedData = queryableData
                    .Skip((pageOptions.PageNumber - 1) * pageOptions.PageSize)
                    .Take(pageOptions.PageSize);

                result.Data = pagedData.ToList();
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterByAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterlerine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş T nesneleri veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> FilterByAsync(IEnumerable<Expression<Func<T, bool>>> filters)
        {
            var result = new BaseResponse<T>();
            try
            {
                var combinedFilter = Builders<T>.Filter.And(filters.Select(f => Builders<T>.Filter.Where(f)));
                var data = await _collection.Find(combinedFilter).ToListAsync();

                result.Data = data;
                result.DefinitionLang = data.Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterByAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterlerine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır ve belirtilen sayfalama ayarlarına göre sonuçları döndürür.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="pageOptions">Sayfalama yapmak için kullanılacak sayfa numarası ve sayfa başına kayıt sayısı.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sayfalanmış T nesneleri veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> FilterByAsync(IEnumerable<Expression<Func<T, bool>>> filters, PageOptions pageOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var combinedFilter = Builders<T>.Filter.And(filters.Select(f => Builders<T>.Filter.Where(f)));
                var findFluent = _collection.Find(combinedFilter);

                // Verileri önce listeye çevir
                var data = await findFluent.ToListAsync();

                // IQueryable'a dönüştür ve sayfalama uygula
                var pagedData = data
                    .AsQueryable()
                    .Skip((pageOptions.PageNumber - 1) * pageOptions.PageSize)
                    .Take(pageOptions.PageSize)
                    .ToList();

                result.Data = pagedData;
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterByAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterlerine ve sıralama seçeneklerine göre T nesnelerini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filters">T nesnelerini asenkron olarak filtrelemek için kullanılacak lambda ifadelerinin listesi.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sıralanmış T nesneleri veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> FilterByAsync(IEnumerable<Expression<Func<T, bool>>> filters, List<SortOption> sortOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var combinedFilter = Builders<T>.Filter.And(filters.Select(f => Builders<T>.Filter.Where(f)));
                var findFluent = _collection.Find(combinedFilter);
                var data = await findFluent.ToListAsync(); // Verileri önce listeye çevir

                var queryableData = data.AsQueryable(); // Daha sonra IQueryable'a dönüştür

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda<Func<T, object>>(property, param);

                    queryableData = sortOption.Ascending
                        ? queryableData.OrderBy(lambda)
                        : queryableData.OrderByDescending(lambda);
                }

                result.Data = queryableData.ToList(); // Sonuçları tekrar listeye çevir
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterByAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }


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
        public virtual async Task<BaseResponse<T>> FilterByAsync(
            IEnumerable<Expression<Func<T, bool>>> filters,
            List<SortOption> sortOptions,
            PageOptions pageOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                // Filtreleri birleştir
                var combinedFilter = Builders<T>.Filter.And(filters.Select(f => Builders<T>.Filter.Where(f)));

                // Veritabanından filtrelenmiş verileri al
                var filteredData = await _collection.Find(combinedFilter).ToListAsync();

                // Sıralama ve sayfalama için IQueryable'a dönüştür
                var queryableData = filteredData.AsQueryable();

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda<Func<T, object>>(property, param);

                    queryableData = sortOption.Ascending
                        ? queryableData.OrderBy(lambda)
                        : queryableData.OrderByDescending(lambda);
                }

                // Sayfalama uygula
                var pagedData = queryableData.Skip((pageOptions.PageNumber - 1) * pageOptions.PageSize)
                                             .Take(pageOptions.PageSize);

                result.Data = pagedData.ToList();
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterByAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen MongoDB filtre tanımına göre T nesnelerini asenkron olarak MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür. Eğer sonuç bulunamazsa boş bir liste döner.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">MongoDB filtre tanımı. T nesnelerini filtrelemek için kullanılır.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş T nesneleri, başarı durumu ve mesaj bilgisi.</returns>
        public virtual async Task<BaseResponse<T>> FilterDefinitionByAsync(FilterDefinition<T> filter)
        {
            var result = new BaseResponse<T>();
            try
            {
                var data = await _collection.Find(filter).ToListAsync();

                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        result.Data = data;
                        result.DefinitionLang = "Veriler basariyla alindi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.FilterDefinitionByAsync";
                        result.Detail = "";
                    }
                    else
                    {
                        result.Data = data;
                        result.DefinitionLang = "Listelenecek Veri bulunamadi";
                        result.Type = ResponseType.SUCCESS;
                        result.Sender = "GenericRepository.FilterDefinitionByAsync";
                        result.Detail = "";
                    }
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Veriler alinamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.FilterDefinitionByAsync";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterDefinitionByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen MongoDB filtre tanımına göre T nesnelerini asenkron olarak MongoDB koleksiyonundan alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür. Eğer sonuç bulunamazsa boş bir liste döner.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">MongoDB filtre tanımı. T nesnelerini filtrelemek için kullanılır.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sıralanmış T nesneleri, başarı durumu ve mesaj bilgisi.</returns>
        public virtual async Task<BaseResponse<T>> FilterDefinitionByAsync(FilterDefinition<T> filter, List<SortOption> sortOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var findFluent = _collection.Find(filter);
                var data = await findFluent.ToListAsync(); // Verileri önce listeye çevir
                var queryableData = data.AsQueryable(); // Daha sonra IQueryable'a dönüştür

                // Sıralama seçeneklerini uygula
                foreach (var sortOption in sortOptions)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(param, sortOption.FieldName);
                    var lambda = Expression.Lambda(property, param);

                    queryableData = sortOption.Ascending
                        ? Queryable.OrderBy(queryableData, (dynamic)lambda)
                        : Queryable.OrderByDescending(queryableData, (dynamic)lambda);
                }

                result.Data = queryableData.ToList(); // Sonuçları tekrar listeye çevir
                result.DefinitionLang = result.Data.ToList().Count > 0 ? "Veriler basariyla alindi" : "Listelenecek Veri bulunamadi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.FilterDefinitionByAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FilterDefinitionByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }


        /// <summary>
        /// Belirtilen MongoDB filtre tanımına göre T nesnelerini asenkron olarak MongoDB koleksiyonundan alır ve belirtilen sıralama seçeneklerine göre sıralar.
        /// Başarılı bir şekilde alınırsa, bu nesneleri içeren bir liste döndürülür. Eğer sonuç bulunamazsa boş bir liste döner.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">MongoDB filtre tanımı. T nesnelerini filtrelemek için kullanılır.</param>
        /// <param name="sortOptions">Sıralama yapmak için kullanılacak alanlar ve sıralama türleri.</param>
        /// <param name="pageOptions">Sayfalama seçenekleri.</param>
        /// <returns>BaseResponse_T tipinde bir nesne içerisinde asenkron olarak filtrelenmiş ve sıralanmış T nesneleri, başarı durumu ve mesaj bilgisi.</returns>
        public virtual async Task<BaseResponse<T>> FromDb_FilterByAsync(
            Expression<Func<T, bool>> filter,
            List<SortOption>? sortOptions,
            PageOptions? pageOptions)
        {
            var result = new BaseResponse<T>();
            try
            {
                var filterDefinition = Builders<T>.Filter.And(filter);
                var sortDefinition = sortOptions != null
                    ? Builders<T>.Sort.Combine(sortOptions.Select(sortOption =>
                        sortOption.Ascending
                            ? Builders<T>.Sort.Ascending(sortOption.FieldName)
                            : Builders<T>.Sort.Descending(sortOption.FieldName)
                    ))
                    : null;

                var findOptions = new FindOptions<T>
                {
                    Sort = sortDefinition,
                    Skip = pageOptions != null ? (pageOptions.PageNumber - 1) * pageOptions.PageSize : 0,
                    Limit = pageOptions?.PageSize
                };

                var data = await _collection.FindAsync(filterDefinition, findOptions);
                if (data != null)
                {
                    List<T> finalData = await data.ToListAsync();
                    result.Data = finalData.ToList();
                    result.DefinitionLang = finalData.Count > 0 ? "Veriler başarıyla alındı" : "Listelenecek veri bulunamadı";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.FromDb_FilterByAsync";
                    result.Detail = "";
                }
                else
                {
                    result.Data = new List<T>();
                    result.DefinitionLang = "Listelenecek veri bulunamadı";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.FromDb_FilterByAsync";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.FromDb_FilterByAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        public async Task<BaseResponse<BsonDocument>> GroupAndAggregateAsync<T>(
    Expression<Func<T, object>> groupByExpression,
    Dictionary<Expression<Func<T, object>>, MongoAggregateOperation>? aggregations)
        {
            var result = new BaseResponse<BsonDocument>();

            try
            {
                // Gruplama ifadesinin alan adını elde etme
                var groupByFieldName = GetMemberName(groupByExpression);

                var groupFields = new BsonDocument { { "_id", $"${groupByFieldName}" } };
                if (aggregations != null)
                {
                    foreach (var aggregation in aggregations)
                    {
                        var fieldName = GetMemberName(aggregation.Key);
                        switch (aggregation.Value)
                        {
                            case MongoAggregateOperation.Sum:
                                groupFields.Add(fieldName, new BsonDocument("$sum", $"${fieldName}"));
                                break;
                            case MongoAggregateOperation.Avg:
                                groupFields.Add(fieldName, new BsonDocument("$avg", $"${fieldName}"));
                                break;
                                // Diğer hesaplama türleri için benzer ekleme yapılabilir
                        }
                    }
                }

                var pipeline = new[] { new BsonDocument("$group", groupFields) };
                var aggregationResult = await _collection.Aggregate<BsonDocument>(pipeline).ToListAsync();

                result.Data = aggregationResult;
                result.DefinitionLang = "Gruplama ve hesaplama başarılı";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.GroupAndAggregateAsync";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.GroupAndAggregateAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "Gruplama ve hesaplama sırasında hata oluştu";
                result.Detail = ex.Message;
            }

            return result;
        }


        private string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression member)
            {
                return member.Member.Name;
            }

            var op = ((UnaryExpression)expression.Body).Operand;
            return ((MemberExpression)op).Member.Name;
        }


        /// <summary>
        /// Belirtilen ID'ye sahip olan tek bir T nesnesini MongoDB koleksiyonundan alır.
        /// Başarılı bir şekilde alınırsa, bu nesneyi döndürülür.
        /// Herhangi bir hata oluşursa veya nesne bulunamazsa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="id">Alınacak T nesnesinin ID'si.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde alınan T nesnesi veya hata durumu.</returns>
        public virtual BaseResponse<T> GetById(string id)
        {
            var result = new BaseResponse<T>();
            try
            {
                ObjectId objectId = ObjectId.Parse(id);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var data = _collection.Find(filter).FirstOrDefault();
                if (data != null)
                {
                    result.Data = new List<T> { data };
                    result.DefinitionLang = "Veriler basariyla alindi";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.GetById";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Veriler alinamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.GetById";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.GetById";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen ID'ye sahip olan tek bir T nesnesini MongoDB koleksiyonundan asenkron olarak alır.
        /// Başarılı bir şekilde alınırsa, bu nesneyi döndürülür.
        /// Herhangi bir hata oluşursa veya nesne bulunamazsa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="id">Asenkron olarak alınacak T nesnesinin ID'si.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak alınan T nesnesi veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> GetByIdAsync(string id)
        {
            var result = new BaseResponse<T>();
            try
            {
                ObjectId objectId = ObjectId.Parse(id);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var data = await _collection.Find(filter).FirstOrDefaultAsync();
                if (data != null)
                {
                    result.Data = new List<T> { data };
                    result.DefinitionLang = "Veriler basariyla alindi";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.GetByIdAsync";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Veriler alinamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.GetByIdAsync";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.GetByIdAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        #endregion
        #region Delete Islemleri
        /// <summary>
        /// Belirtilen ID'ye sahip olan tek bir T nesnesini MongoDB koleksiyonundan siler.
        /// Başarılı bir şekilde silinirse, silinen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="id">Silinecek T nesnesinin ID'si.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde silinen T nesnesi veya hata durumu.</returns>

        public virtual BaseResponse<T> DeleteById(string id)
        {
            var result = new BaseResponse<T>();
            try
            {
                ObjectId objectId = ObjectId.Parse(id);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var data = _collection.FindOneAndDelete(filter);
                if (data != null)
                {
                    result.Data = new List<T> { data };
                    result.DefinitionLang = "Veri basariyla alindi";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.DeleteById";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Veri silinemedi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.DeleteById";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.DeleteById";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen ID'ye sahip olan tek bir T nesnesini MongoDB koleksiyonundan asenkron olarak siler.
        /// Başarılı bir şekilde silinirse, silinen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="id">Asenkron olarak silinecek T nesnesinin ID'si.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak silinen T nesnesi veya hata durumu.</returns>

        public virtual async Task<BaseResponse<T>> DeleteByIdAsync(string id)
        {
            var result = new BaseResponse<T>();
            try
            {
                ObjectId objectId = ObjectId.Parse(id);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var data = await _collection.FindOneAndDeleteAsync(filter);
                if (data != null)
                {
                    result.Data = new List<T> { data };
                    result.DefinitionLang = "Veri basariyla alindi";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.DeleteByIdAsync";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Silinmek istenen veri bulunamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.DeleteByIdAsync";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.DeleteByIdAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre birden fazla T nesnesini MongoDB koleksiyonundan siler.
        /// Başarılı bir şekilde silinirse, silme işlemi ile ilgili bilgi mesajı döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Silinecek T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde silme işlemi sonucu veya hata durumu.</returns>
        public virtual BaseResponse<T> DeleteMany(Expression<Func<T, bool>> filter)
        {
            var result = new BaseResponse<T>();
            try
            {
                var deletedValues = _collection.DeleteMany(filter);
                if (deletedValues.DeletedCount > 0)
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veriler basariyla silindi. Silinen veri sayisi : {deletedValues.DeletedCount}";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.DeleteMany";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Silinmek istenen veri bulunamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.DeleteMany";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.DeleteMany";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;

        }

        /// <summary>
        /// Belirtilen filtre kriterine göre birden fazla T nesnesini MongoDB koleksiyonundan asenkron olarak siler.
        /// Başarılı bir şekilde silinirse, silme işlemi ile ilgili bilgi mesajı döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Asenkron olarak silinecek T nesnelerini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak silme işlemi sonucu veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            var result = new BaseResponse<T>();
            try
            {
                var deletedValues = await _collection.DeleteManyAsync(filter);
                if (deletedValues.DeletedCount > 0)
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veriler basariyla silindi. Silinen veri sayisi : {deletedValues.DeletedCount}";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.DeleteManyAsync";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Silinmek istenen veri bulunamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.DeleteManyAsync";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.DeleteManyAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace; ;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre tek bir T nesnesini MongoDB koleksiyonundan siler.
        /// Başarılı bir şekilde silinirse, silinen nesneyi döndürülür.
        /// Eğer uygun bir nesne bulunamazsa veya hata oluşursa, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Silinecek tek bir T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde silinen T nesnesi veya hata durumu.</returns>
        public virtual BaseResponse<T> DeleteOne(Expression<Func<T, bool>> filter)
        {
            var result = new BaseResponse<T>();
            try
            {
                var deletedDocument = _collection.FindOneAndDelete(filter);
                if (deletedDocument != null)
                {
                    result.Data = new List<T>() { deletedDocument }; ;
                    result.DefinitionLang = "Veri basariyla silindi";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.DeleteOne";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Silinmek istenen veri bulunamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.DeleteOne";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.DeleteOne";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace; ;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre tek bir T nesnesini MongoDB koleksiyonundan asenkron olarak siler.
        /// Başarılı bir şekilde silinirse, silinen nesneyi döndürülür.
        /// Eğer uygun bir nesne bulunamazsa veya hata oluşursa, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Asenkron olarak silinecek tek bir T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak silinen T nesnesi veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> DeleteOneAsync(Expression<Func<T, bool>> filter)
        {
            var result = new BaseResponse<T>();
            try
            {
                T deletedDocument = await _collection.FindOneAndDeleteAsync(filter);
                if (deletedDocument != null)
                {
                    result.Data = new List<T>() { deletedDocument };
                    result.DefinitionLang = "Veri basariyla silindi";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.DeleteOneAsync";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = "Silinmek istenen veri bulunamadi";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.DeleteOneAsync";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.DeleteOneAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace; ;
                result.Data = null;
            }
            return result;
        }

        #endregion
        #region Insert Islemleri
        /// <summary>
        /// Birden fazla T nesnesini MongoDB koleksiyonuna ekler.
        /// Başarılı bir şekilde eklenirse, eklenen nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="entities">MongoDB koleksiyonuna eklenmek üzere olan T nesnelerinin koleksiyonu.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde eklenen T nesneleri veya hata durumu.</returns>

        public virtual BaseResponse<T> InsertMany(ICollection<T> entities)
        {
            var result = new BaseResponse<T>();
            try
            {
                _collection.InsertMany(entities);
                result.Data = entities;
                result.DefinitionLang = "Veriler basariyla eklendi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.InsertMany";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.InsertMany";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace; ;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Birden fazla T nesnesini MongoDB koleksiyonuna asenkron olarak ekler.
        /// Başarılı bir şekilde eklenirse, eklenen nesneleri içeren bir liste döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="entities">Asenkron olarak MongoDB koleksiyonuna eklenmek üzere olan T nesnelerinin koleksiyonu.</param>
        /// <returns>GetManyResult tipinde bir nesne içerisinde asenkron olarak eklenen T nesneleri veya hata durumu.</returns>

        public virtual async Task<BaseResponse<T>> InsertManyAsync(ICollection<T> entities)
        {
            var result = new BaseResponse<T>();
            try
            {
                await _collection.InsertManyAsync(entities);
                result.Data = entities;
                result.DefinitionLang = "Veriler basariyla eklendi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.InsertManyAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.InsertManyAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace; ;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Tek bir T nesnesini MongoDB koleksiyonuna ekler.
        /// Başarılı bir şekilde eklenirse, eklenen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="entity">MongoDB koleksiyonuna eklenmek üzere olan tek bir T nesnesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde eklenen T nesnesi veya hata durumu.</returns>

        public virtual BaseResponse<T> InsertOne(T entity)
        {
            var result = new BaseResponse<T>();
            try
            {
                _collection.InsertOne(entity);
                result.Data = new List<T>() { entity };
                result.DefinitionLang = "Veri basariyla eklendi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.InsertOne";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.InsertOne";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace; ;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Tek bir T nesnesini MongoDB koleksiyonuna asenkron olarak ekler.
        /// Başarılı bir şekilde eklenirse, eklenen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa, hata mesajı ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="entity">Asenkron olarak MongoDB koleksiyonuna eklenmek üzere olan tek bir T nesnesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak eklenen T nesnesi veya hata durumu.</returns>
        public virtual async Task<BaseResponse<T>> InsertOneAsync(T entity)
        {
            var result = new BaseResponse<T>();
            try
            {
                await _collection.InsertOneAsync(entity);
                result.Data = new List<T>() { entity };
                result.DefinitionLang = "Veri basariyla eklendi";
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.InsertOneAsync";
                result.Detail = "";
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.InsertOneAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace; ;
                result.Data = null;
            }
            return result;
        }


        /// <summary>
        /// Asenkron olarak bir MongoDB koleksiyonunda belirtilen filtreye uygun bir kaydı günceller, eğer uygun kayıt yoksa yeni bir kayıt ekler (upsert işlemi).
        /// Bu işlem, genel bir veri tipi (T) üzerinde çalışır ve işlemin sonucunu bir BaseResponse_T nesnesi olarak döndürür.
        /// İşlem başarılı olduğunda, işlem türüne (ekleme veya güncelleme) göre özelleştirilmiş bir başarı mesajı içerir.
        /// Hata oluşması durumunda, hata detaylarını içeren bir hata yanıtı döndürür.
        /// </summary>
        /// <param name="filter">MongoDB koleksiyonunda arama yapmak için kullanılan lambda ifadesi.</param>
        /// <param name="entity">Eklenecek veya güncellenecek varlık nesnesi.</param>
        /// <returns>İşlem sonucunu içeren BaseResponse_T nesnesi.</returns>
        public virtual async Task<BaseResponse<T>> UpsertOneAsync(Expression<Func<T, bool>> filter, T entity)
        {
            var result = new BaseResponse<T>();
            try
            {
                var options = new ReplaceOptions { IsUpsert = true };
                var replaceResult = await _collection.ReplaceOneAsync(filter, entity, options);

                if (replaceResult.MatchedCount == 0)
                {
                    // Insert işlemi yapıldı
                    result.DefinitionLang = "Veri başarıyla eklendi";
                }
                else
                {
                    // Update işlemi yapıldı
                    result.DefinitionLang = "Veri başarıyla güncellendi";
                }

                result.Data = new List<T>() { entity };
                result.Type = ResponseType.SUCCESS;
                result.Sender = "GenericRepository.UpsertOneAsync";
                result.Detail = "";
            }
            catch (System.Exception ex)
            {
                result.Sender = "GenericRepository.UpsertOneAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace; ;
                result.Data = null;
            }
            return result;
        }

        #endregion
        #region Update Islemleri

        /// <summary>
        /// Belirtilen filtre kriterine göre bir T nesnesini MongoDB koleksiyonunda başka bir nesne ile değiştirir.
        /// Başarılı bir şekilde değiştirilirse, değiştirilen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa veya değiştirme işlemi gerçekleşmezse, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Değiştirilecek T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="entity">MongoDB koleksiyonunda yerine konacak yeni T nesnesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde değiştirilen T nesnesi veya hata durumu.</returns>

        public virtual BaseResponse<T> ReplaceOne(Expression<Func<T, bool>> filter, T entity)
        {
            var result = new BaseResponse<T>();
            try
            {
                var replaceResult = _collection.ReplaceOne(filter, entity);
                if (replaceResult.IsAcknowledged)
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veri basariyla guncellendi. Guncellenen veri sayisi : Bulunan kayit sayisi: {replaceResult.MatchedCount}, Guncellenen kayit sayisi: {replaceResult.ModifiedCount}";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.ReplaceOne";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veri guncellenemedi : Bulunan kayit sayisi: {replaceResult.MatchedCount}, Guncellenen kayit sayisi: {replaceResult.ModifiedCount}";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.ReplaceOne";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.ReplaceOne";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre bir T nesnesini MongoDB koleksiyonunda asenkron olarak başka bir nesne ile değiştirir.
        /// Başarılı bir şekilde değiştirilirse, değiştirilen nesneyi döndürülür.
        /// Herhangi bir hata oluşursa veya değiştirme işlemi gerçekleşmezse, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Asenkron olarak değiştirilecek T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="entity">Asenkron olarak MongoDB koleksiyonunda yerine konacak yeni T nesnesi.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak değiştirilen T nesnesi veya hata durumu.</returns>

        public virtual async Task<BaseResponse<T>> ReplaceOneAsync(Expression<Func<T, bool>> filter, T entity)
        {
            var result = new BaseResponse<T>();
            try
            {
                var replaceResult = await _collection.ReplaceOneAsync(filter, entity);
                if (replaceResult.IsAcknowledged)
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veri basariyla guncellendi. Guncellenen veri sayisi : Bulunan kayit sayisi: {replaceResult.MatchedCount}, Guncellenen kayit sayisi: {replaceResult.ModifiedCount}";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.ReplaceOneAsync";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veri guncellenemedi : Bulunan kayit sayisi: {replaceResult.MatchedCount}, Guncellenen kayit sayisi: {replaceResult.ModifiedCount}";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.ReplaceOneAsync";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.ReplaceOneAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre bir T nesnesini MongoDB koleksiyonunda asenkron olarak günceller.
        /// Başarılı bir şekilde güncellenirse, güncellenen nesne sayısı ile birlikte başarılı bir sonuç döndürülür.
        /// Herhangi bir hata oluşursa veya güncelleme işlemi gerçekleşmezse, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Asenkron olarak güncellenecek T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="update">MongoDB koleksiyonunda yapılacak güncellemeleri tanımlayan bir güncelleme tanımlayıcısı.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde asenkron olarak güncellenen T nesnesi veya hata durumu.</returns>

        public virtual async Task<BaseResponse<T>> UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            var result = new BaseResponse<T>();
            try
            {
                UpdateResult updateResult = await _collection.UpdateOneAsync(filter, update);
                if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veri başarıyla güncellendi. Bulunan kayıt sayısı: {updateResult.MatchedCount}, Güncellenen kayıt sayısı: {updateResult.ModifiedCount}";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.UpdateOneAsync";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veri güncellenemedi ya da değişiklik yapılmadı. Bulunan kayıt sayısı: {updateResult.MatchedCount}, Güncellenen kayıt sayısı: {updateResult.ModifiedCount}";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.UpdateOneAsync";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.UpdateOneAsync";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "Bir hata oluştu.";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// Belirtilen filtre kriterine göre bir T nesnesini MongoDB koleksiyonunda senkron olarak günceller.
        /// Başarılı bir şekilde güncellenirse, güncellenen nesne sayısı ile birlikte başarılı bir sonuç döndürülür.
        /// Herhangi bir hata oluşursa veya güncelleme işlemi gerçekleşmezse, ilgili mesaj ile birlikte başarısız bir sonuç döner.
        /// </summary>
        /// <param name="filter">Senkron olarak güncellenecek T nesnesini filtrelemek için kullanılacak lambda ifadesi.</param>
        /// <param name="update">MongoDB koleksiyonunda yapılacak güncellemeleri tanımlayan bir güncelleme tanımlayıcısı.</param>
        /// <returns>GetOneResult tipinde bir nesne içerisinde senkron olarak güncellenen T nesnesi veya hata durumu.</returns>

        public virtual BaseResponse<T> UpdateOne(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            var result = new BaseResponse<T>();
            try
            {
                var updateResult = _collection.UpdateOne(filter, update);
                if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veri başarıyla güncellendi. Bulunan kayıt sayısı: {updateResult.MatchedCount}, Güncellenen kayıt sayısı: {updateResult.ModifiedCount}";
                    result.Type = ResponseType.SUCCESS;
                    result.Sender = "GenericRepository.UpdateOne";
                    result.Detail = "";
                }
                else
                {
                    result.Data = null;
                    result.DefinitionLang = $"Veri güncellenemedi ya da değişiklik yapılmadı. Bulunan kayıt sayısı: {updateResult.MatchedCount}, Güncellenen kayıt sayısı: {updateResult.ModifiedCount}";
                    result.Type = ResponseType.WARNING;
                    result.Sender = "GenericRepository.UpdateOne";
                    result.Detail = "";
                }
            }
            catch (Exception ex)
            {
                result.Sender = "GenericRepository.UpdateOne";
                result.Type = ResponseType.ERROR;
                result.DefinitionLang = "Bir hata oluştu.";
                result.Detail = ex.Message + " " + ex.InnerException?.Message + " " + ex.StackTrace;
                result.Data = null;
            }
            return result;
        }

        #endregion
    }
}
