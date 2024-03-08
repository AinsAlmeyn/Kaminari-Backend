namespace WMS.DataAccess.Helpers.GroupByHelper
{
    public static class GroupByHelper
    {
        /// <summary>
        /// Verilen bir kaynak koleksiyonunu belirtilen tek bir anahtar seçiciye göre gruplar.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <typeparam name="TKey">Gruplama anahtarının türü.</typeparam>
        /// <param name="source">Gruplanacak öğelerin koleksiyonu.</param>
        /// <param name="keySelector">Gruplama için kullanılacak anahtar seçici fonksiyonu.</param>
        /// <returns>Belirtilen anahtar seçiciye göre gruplanmış öğelerin koleksiyonu.</returns>

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBySingleField<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector);
        }

        /// <summary>
        /// Verilen bir kaynak koleksiyonunu belirtilen anahtar seçiciye göre gruplar ve her grup için belirtilen sonuç seçici fonksiyonunu kullanarak yeni bir sonuç seti oluşturur.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <typeparam name="TKey">Gruplama anahtarının türü.</typeparam>
        /// <typeparam name="TResult">Sonuç koleksiyonunun öğe türü.</typeparam>
        /// <param name="source">Gruplanacak öğelerin koleksiyonu.</param>
        /// <param name="keySelector">Gruplama için kullanılacak anahtar seçici fonksiyonu.</param>
        /// <param name="resultSelector">Her grup için sonuç öğesi oluşturacak fonksiyon.</param>
        /// <returns>Gruplama anahtarına ve sonuç seçici fonksiyonuna göre oluşturulmuş yeni sonuç koleksiyonu.</returns>

        public static IEnumerable<TResult> GroupBySingleField<TSource, TKey, TResult>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
        {
            return source.GroupBy(keySelector).Select(group => resultSelector(group.Key, group));
        }

        /// <summary>
        /// Verilen bir kaynak koleksiyonunu birden fazla anahtar seçiciye göre gruplar. Her bir anahtar seçici, gruplama için bir özellik belirler.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <param name="source">Gruplanacak öğelerin koleksiyonu.</param>
        /// <param name="keySelectors">Gruplama için kullanılacak anahtar seçicilerin dizisi.</param>
        /// <returns>Birden fazla anahtar seçiciye göre gruplanmış öğelerin koleksiyonu.</returns>

        public static IEnumerable<IGrouping<object, TSource>> GroupByMultipleFields<TSource>(IEnumerable<TSource> source, params Func<TSource, object>[] keySelectors)
        {
            return source.GroupBy(item => keySelectors.Select(keySelector => keySelector(item)).ToArray(), new ArrayComparer<object>());
        }

        /// <summary>
        /// Verilen bir kaynak koleksiyonunu birden fazla anahtar seçiciye göre gruplar ve her grup için belirtilen sonuç seçici fonksiyonunu kullanarak yeni bir sonuç seti oluşturur.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <typeparam name="TResult">Sonuç koleksiyonunun öğe türü.</typeparam>
        /// <param name="source">Gruplanacak öğelerin koleksiyonu.</param>
        /// <param name="resultSelector">Her grup için sonuç öğesi oluşturacak fonksiyon.</param>
        /// <param name="keySelectors">Gruplama için kullanılacak anahtar seçicilerin dizisi.</param>
        /// <returns>Birden fazla anahtar seçiciye ve sonuç seçici fonksiyona göre oluşturulmuş yeni sonuç koleksiyonu.</returns>

        public static IEnumerable<TResult> GroupByMultipleFields<TSource, TResult>(IEnumerable<TSource> source, Func<object[], IEnumerable<TSource>, TResult> resultSelector, params Func<TSource, object>[] keySelectors)
        {
            return source.GroupBy(
                item => keySelectors.Select(keySelector => keySelector(item)).ToArray(),
                new ArrayComparer<object>()
            ).Select(group => resultSelector(group.Key, group));
        }


        /// <summary>
        /// Verilen kaynak koleksiyonunu birden fazla koşula göre filtreler ve her filtrelenmiş grubun belirli bir sayısal değerin toplamını veya ortalamasını hesaplar.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <typeparam name="TValue">Hesaplama yapılacak değerin türü (sayısal olmalı).</typeparam>
        /// <param name="source">Filtrelenecek öğelerin koleksiyonu.</param>
        /// <param name="predicates">Filtrelemeyi belirleyecek koşul fonksiyonları dizisi.</param>
        /// <param name="valueSelector">Hesaplama için kullanılacak değer seçici fonksiyon.</param>
        /// <param name="operation">Yapılacak işlem türü (Ortalama veya Toplam).</param>
        /// <returns>
        /// Her filtrelenmiş grubun anahtar değeri ve hesaplanan toplam veya ortalama değeri içeren bir koleksiyon.
        /// </returns>
        public static IEnumerable<KeyValuePair<string, TValue>> GroupByAndCalculateMultipleFilters<TSource, TValue>(
    IEnumerable<TSource> source,
    IEnumerable<Func<TSource, bool>> predicates,
    Func<TSource, TValue> valueSelector,
    AggregateOperation operation)
    where TValue : struct, IConvertible
        {
            var results = new List<KeyValuePair<string, TValue>>();

            // Tüm predicate'leri birleştir
            Func<TSource, bool> combinedPredicate = item => predicates.All(predicate => predicate(item));

            var filteredGroup = source.Where(combinedPredicate);

            TValue result;
            switch (operation)
            {
                case AggregateOperation.Sum:
                    result = (TValue)Convert.ChangeType(filteredGroup.Sum(item => valueSelector(item).ToDecimal(null)), typeof(TValue));
                    break;
                case AggregateOperation.Average:
                    result = (TValue)Convert.ChangeType(filteredGroup.Average(item => valueSelector(item).ToDecimal(null)), typeof(TValue));
                    break;
                default:
                    throw new ArgumentException("Unsupported operation.");
            }

            results.Add(new KeyValuePair<string, TValue>("CombinedPredicate", result));

            return results;
        }


        /// <summary>
        /// Verilen kaynak koleksiyonunu belirli bir anahtar değerine göre gruplar ve her grup için belirli bir değerin toplamını veya ortalamasını hesaplar.
        /// </summary>
        /// <typeparam name="TSource">Koleksiyon öğelerinin türü.</typeparam>
        /// <typeparam name="TKey">Gruplama anahtarının türü.</typeparam>
        /// <typeparam name="TValue">Hesaplama yapılacak değerin türü (sayısal olmalı).</typeparam>
        /// <param name="source">Gruplanacak öğelerin koleksiyonu.</param>
        /// <param name="keySelector">Gruplama için kullanılacak anahtar seçici fonksiyon.</param>
        /// <param name="valueSelector">Hesaplama için kullanılacak değer seçici fonksiyon.</param>
        /// <param name="operation">Yapılacak işlem türü (Ortalama veya Toplam).</param>
        /// <returns>
        /// Her grup için anahtar değeri ve hesaplanan toplam veya ortalama değeri içeren bir koleksiyon.
        /// </returns>
        public static IEnumerable<KeyValuePair<TKey, TValue>> GroupByAndCalculate<TSource, TKey, TValue>(
    IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector,
    Func<TSource, TValue> valueSelector,
    AggregateOperation operation)
    where TValue : struct, IConvertible
        {
            var groupedData = source.GroupBy(keySelector);

            switch (operation)
            {
                case AggregateOperation.Sum:
                    return groupedData.Select(group => new KeyValuePair<TKey, TValue>(
                        group.Key,
                        (TValue)Convert.ChangeType(group.Sum(item => Convert.ToDecimal(valueSelector(item))), typeof(TValue))));

                case AggregateOperation.Average:
                    return groupedData.Select(group => new KeyValuePair<TKey, TValue>(
                        group.Key,
                        (TValue)Convert.ChangeType(group.Average(item => Convert.ToDecimal(valueSelector(item))), typeof(TValue))));

                default:
                    throw new ArgumentException("Unsupported operation.");
            }
        }


        public enum AggregateOperation
        {
            Sum,
            Average
        }
        /// <summary>
        /// Dizileri karşılaştırmak için kullanılan bir IEqualityComparer implementasyonu. 
        /// Bu sınıf, GroupByMultipleFields metodunda çoklu alanlara göre gruplama yaparken kullanılır.
        /// </summary>
        /// <typeparam name="T">Dizi öğelerinin türü.</typeparam>
        private class ArrayComparer<T> : IEqualityComparer<T[]>
        {
            /// <summary>
            /// İki dizi arasında eşitlik kontrolü yapar. 
            /// Dizilerin içeriklerinin aynı sıra ve değerlere sahip olup olmadığını kontrol eder.
            /// </summary>
            /// <param name="x">Karşılaştırılacak ilk dizi.</param>
            /// <param name="y">Karşılaştırılacak ikinci dizi.</param>
            /// <returns>Eğer iki dizi eşitse true, değilse false döner.</returns>
            public bool Equals(T[] x, T[] y)
            {
                return x.SequenceEqual(y);
            }

            /// <summary>
            /// Bir dizinin hash kodunu hesaplar. 
            /// Dizideki her öğenin hash kodunu kullanarak bir toplam hash kodu oluşturur.
            /// </summary>
            /// <param name="obj">Hash kodu hesaplanacak dizi.</param>
            /// <returns>Hesaplanan hash kodu.</returns>
            public int GetHashCode(T[] obj)
            {
                unchecked
                {
                    int hash = 19;
                    foreach (var o in obj)
                    {
                        hash = hash * 31 + (o == null ? 0 : o.GetHashCode());
                    }
                    return hash;
                }
            }
        }

    }

}
