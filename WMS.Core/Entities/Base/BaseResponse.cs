namespace WMS.Core.Entities.Base
{
    /// <summary>
    /// Represents the type of response.
    /// </summary>
    public enum ResponseType
    {
        SUCCESS,
        WARNING,
        ERROR,
        INFO
    }

    /// <summary>
    /// Represents a base response with optional data.
    /// </summary>
    /// <typeparam name="T">The type of data.</typeparam>
    public class BaseResponse<T> where T : class
    {
        /// <summary>
        /// Gets or sets the type of the response.
        /// </summary>
        public ResponseType? Type { get; set; }

        /// <summary>
        /// Gets or sets the definition language of the response.
        /// </summary>
        public string? DefinitionLang { get; set; }

        /// <summary>
        /// Gets or sets the sender of the response.
        /// </summary>
        public string? Sender { get; set; } = "FUNC01";

        /// <summary>
        /// Gets or sets the detail of the response.
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// Gets or sets the list of data.
        /// </summary>
        public IEnumerable<T>? Data { get; set; }
    }
}
