namespace WMS.Service.ServiceConnector.ApiClient
{
    public class KaminariHttpClientSettings
    {

        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();

        public void AddHeader(string key, string value)
        {
            Headers[key] = value;
        }

        public void ClearHeaders()
        {
            Headers.Clear();
        }
    }
}