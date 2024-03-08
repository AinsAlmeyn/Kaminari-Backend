namespace WMS.DataAccess.Helpers.FilterHelper
{
    public static class FilterHelper
    {
        /// <summary>
        /// Verilen bir kaynak koleksiyonunu birden fazla koşula göre filtreler. 
        /// Her bir filtre, belirli bir koşulu karşılayan öğeleri içerir.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <param name="source">Filtrelenecek öğelerin koleksiyonu.</param>
        /// <param name="predicates">Filtrelemeyi belirleyecek koşul fonksiyonları dizisi.</param>
        /// <returns>
        /// Her bir koşula karşılık gelen filtrelenmiş öğelerin listesini döndürür.
        /// </returns>
        public static List<IEnumerable<TSource>> FilterByMultipleConditions<TSource>(IEnumerable<TSource> source, params Func<TSource, bool>[] predicates)
        {
            var filteredGroups = new List<IEnumerable<TSource>>();

            foreach (var predicate in predicates)
            {
                filteredGroups.Add(source.Where(predicate));
            }

            return filteredGroups;
        }
        
        /// <summary>
        /// Verilen bir kaynak koleksiyonunu tüm koşulları sağlayan kayıtlar için filtreler.
        /// Tüm koşulların ('&&') sağlanması gerekmektedir.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <param name="source">Filtrelenecek öğelerin koleksiyonu.</param>
        /// <param name="predicates">Filtrelemeyi belirleyecek koşul fonksiyonları dizisi.</param>
        /// <returns>
        /// Tüm koşulları sağlayan öğelerin listesini döndürür.
        /// </returns>
        public static IEnumerable<TSource> FilterByAllConditions<TSource>(IEnumerable<TSource> source, params Func<TSource, bool>[] predicates)
        {
            return source.Where(item => predicates.All(predicate => predicate(item)));
        }



        /// <summary>
        /// Verilen bir kaynak koleksiyonunu belirtilen bir koşula göre ikiye ayırır.
        /// Birinci grup koşulu karşılayan öğeleri, ikinci grup koşulu karşılamayan öğeleri içerir.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <param name="source">Ayırılacak öğelerin koleksiyonu.</param>
        /// <param name="predicate">Ayırmayı belirleyecek koşul fonksiyonu.</param>
        /// <returns>
        /// İki grup içeren bir Tuple. 
        /// Item1 koşulu karşılayan öğeleri, Item2 koşulu karşılamayan öğeleri içerir.
        /// </returns>
        public static (IEnumerable<TSource>, IEnumerable<TSource>) PartitionByCondition<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var matching = source.Where(predicate);
            var nonMatching = source.Where(item => !predicate(item));
            return (matching, nonMatching);
        }

        /// <summary>
        /// Belirli bir sayısal aralığa uyan öğeleri filtreler.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <param name="source">Filtrelenecek öğelerin koleksiyonu.</param>
        /// <param name="rangeSelector">Sayısal değeri belirleyen seçici fonksiyon.</param>
        /// <param name="min">Aralığın minimum değeri.</param>
        /// <param name="max">Aralığın maksimum değeri.</param>
        /// <returns>Aralığa uyan öğelerin koleksiyonu.</returns>
        public static IEnumerable<TSource> FilterByRange<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, decimal> rangeSelector,
            decimal min,
            decimal max)
        {
            return source.Where(item =>
            {
                var value = rangeSelector(item);
                return value >= min && value <= max;
            });
        }

        /// <summary>
        /// Metin alanlarındaki belirli kelimeleri veya ifadeleri içeren öğeleri filtreler.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <param name="source">Filtrelenecek öğelerin koleksiyonu.</param>
        /// <param name="textSelector">Metin değerini belirleyen seçici fonksiyon.</param>
        /// <param name="keyword">Aranacak kelime veya ifade.</param>
        /// <returns>Belirtilen kelimeyi veya ifadeyi içeren öğelerin koleksiyonu.</returns>
        public static IEnumerable<TSource> FilterByText<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, string> textSelector,
            string keyword)
        {
            return source.Where(item => textSelector(item)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false);
        }


        /// <summary>
        /// Belirli bir özelliğin değerlerinin başka bir koleksiyondaki değerlerle eşleşip eşleşmediğine göre öğeleri filtreler.
        /// Örneğin, bir Order nesneleri listesindeki CustomerId özelliklerinin, belirli bir müşteri ID'leri listesiyle eşleşip eşleşmediğini kontrol etmek isteyebilirsiniz
        /// </summary>
        /// <typeparam name="TSource">Kaynak koleksiyon öğelerinin türü.</typeparam>
        /// <typeparam name="TMatch">Eşleştirilecek değerlerin türü.</typeparam>
        /// <param name="source">Filtrelenecek öğelerin kaynak koleksiyonu.</param>
        /// <param name="matchSelector">Eşleştirme yapılacak özelliği belirleyen seçici fonksiyon.</param>
        /// <param name="matchCollection">Eşleştirilecek değerlerin koleksiyonu.</param>
        /// <returns>Eşleşen öğelerin koleksiyonu.</returns>
        public static IEnumerable<TSource> FilterByMatch<TSource, TMatch>(
            IEnumerable<TSource> source,
            Func<TSource, TMatch> matchSelector,
            IEnumerable<TMatch> matchCollection)
        {
            var matchSet = new HashSet<TMatch>(matchCollection);
            return source.Where(item => matchSet.Contains(matchSelector(item)));
        }

        /// <summary>
        /// Nesneleri veya olayları belirli bir duruma göre filtreler.
        /// Örneğin, işleme durumunda olan siparişleri veya tamamlanmış işlemleri filtrelemek için kullanılabilir.
        /// </summary>
        /// <typeparam name="TSource">Kaynak koleksiyon öğelerinin türü.</typeparam>
        /// <param name="source">Filtrelenecek öğelerin kaynak koleksiyonu.</param>
        /// <param name="statusSelector">Durumu belirleyen seçici fonksiyon.</param>
        /// <param name="desiredStatus">Filtrelenmek istenen durum.</param>
        /// <returns>Belirtilen durumu karşılayan öğelerin koleksiyonu.</returns>
        public static IEnumerable<TSource> FilterByStatus<TSource, TStatus>(
            IEnumerable<TSource> source,
            Func<TSource, TStatus> statusSelector,
            TStatus desiredStatus)
        {
            return source.Where(item => EqualityComparer<TStatus>.Default.Equals(statusSelector(item), desiredStatus));
        }
    }

}