namespace WMS.Core.Entities.ConnectionAndSettings.MongoDb
{
    /// <summary>
    /// Sayfalama işlemleri için kullanılan sınıf.
    /// Bu sınıf, verilerin sayfalama kriterlerine göre işlenmesi ve sunulması için gerekli bilgileri içerir.
    /// </summary>
    public class PageOptions
    {
        /// <summary>
        /// Alınacak sayfanın numarası. Bu değer, hangi sayfanın alınacağını belirler.
        /// Örneğin, 1 değeri ilk sayfayı, 2 değeri ikinci sayfayı ifade eder.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Her sayfada gösterilecek kayıt sayısı. Bu değer, her sayfada kaç adet kaydın listeleneceğini belirler.
        /// Örneğin, 10 değeri her sayfada 10 kayıt gösterileceğini ifade eder.
        /// </summary>
        public int PageSize { get; set; }
    }


}