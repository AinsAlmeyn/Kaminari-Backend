namespace WMS.Core.Entities.Base
{
    public class BaseRequest<T> where T : class
    {
        public string? RequestId { get; set; }
        public string? Sender { get; set; }
        public IEnumerable<T>? Data { get; set; }
    }
}
