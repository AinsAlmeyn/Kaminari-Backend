namespace WMS.Core.Entities.ConnectionAndSettings.MongoDb
{
    /// <summary>
    /// MongoDb Baglanti bilgilerini barindirmak icin kullanilir.
    /// </summary>
    public class MongoDbConnection
    {
        public string? ConnectionString { get; set; }
        public string? Database { get; set; }
    }
}
