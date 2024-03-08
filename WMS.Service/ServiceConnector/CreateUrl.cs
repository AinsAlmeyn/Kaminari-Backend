namespace WMS.Service.ServiceConnector
{
    using System;

    namespace WMS.Service.ServiceConnector
    {
        /// <summary>
        /// Provides methods for creating URLs and URIs.
        /// </summary>
        public static class CreateUrl
        {
            /// <summary>
            /// Combines the base URL and action name to create a URL string.
            /// </summary>
            /// <param name="baseUrlContainer">The base URL.</param>
            /// <param name="actionNameContainer">The action name.</param>
            /// <returns>The combined URL string.</returns>
            public static string CombineUrl(string baseUrlContainer, string actionNameContainer)
            {
                return $"{baseUrlContainer}/{actionNameContainer}";
            }

            /// <summary>
            /// Combines the base URL and action name to create a URI.
            /// </summary>
            /// <param name="baseUrlContainer">The base URL.</param>
            /// <param name="actionNameContainer">The action name.</param>
            /// <returns>The combined URI.</returns>
            public static Uri CombineUri(string baseUrlContainer, string actionNameContainer)
            {
                return new Uri($"{baseUrlContainer}/{actionNameContainer}");
            }
        }
    }
}