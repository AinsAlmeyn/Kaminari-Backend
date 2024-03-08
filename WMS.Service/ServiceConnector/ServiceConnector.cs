using System.Text;
using System.Web;
using Newtonsoft.Json;
using WMS.Service.ErrorHandling;
using WMS.Service.ServiceConnector.ApiClient;

namespace Wms.Service.ServiceConnector
{
    public class ServiceConnector<T> where T : class, new()
    {
        private readonly HttpClient _httpClient;

        public ServiceConnector()
        {
            _httpClient = new HttpClient();
        }

        // public async Task<T> GetAsync(string url, Dictionary<string, string>? queryParams = null, KaminariHttpClientSettings? settings = null)
        // {
        //     try
        //     {
        //         // Sorgu parametrelerini URL'ye ekle
        //         if (queryParams != null && queryParams.Count > 0)
        //         {
        //             System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
        //             foreach (KeyValuePair<string, string> param in queryParams)
        //             {

        //                 query[param.Key] = param.Value;
        //                 // query[param.Key] = param.Value;
        //             }
        //             url += "?" + query.ToString();
        //         }

        //         ApplyHeaders(settings);
        //         HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);
        //         ClearHeaders(settings);
        //         return await ProcessResponseGet(httpResponse);
        //     }
        //     catch (Exception ex)
        //     {
        //         return HandleExceptionGet(ex);
        //     }
        //     finally
        //     {
        //         ClearHeaders(settings);
        //     }
        // }

        public async Task<TResponse> GetAsync<TResponse>(string url, Dictionary<string, string>? queryParams = null, KaminariHttpClientSettings? settings = null) where TResponse : class
        {
            try
            {
                // Sorgu parametrelerini URL'ye ekle
                if (queryParams != null && queryParams.Count > 0)
                {
                    System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                    foreach (KeyValuePair<string, string> param in queryParams)
                    {
                        query[param.Key] = param.Value;
                    }
                    url += "?" + query.ToString();
                }

                ApplyHeaders(settings);
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);
                ClearHeaders(settings);

                // ProcessResponseGet metodu TResponse tipinde bir sonuç dönmelidir.
                // Örneğin, JSON içeriğini deserialize ederek TResponse türüne dönüştürebilirsiniz.
                if (httpResponse.IsSuccessStatusCode)
                {
                    string jsonData = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(jsonData);
                }

                throw new Exception("HTTP request failed with status code " + httpResponse.StatusCode);
            }
            catch (Exception ex)
            {
                return HandleException<TResponse>(ex);
            }
            finally
            {
                ClearHeaders(settings);
            }
        }


        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest? request = null, KaminariHttpClientSettings? settings = null)
            where TRequest : class
            where TResponse : class
        {
            try
            {
                ApplyHeaders(settings);
                HttpResponseMessage httpResponse;
                if (request != null)
                {
                    string jsonRequest = JsonConvert.SerializeObject(request);
                    StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    httpResponse = await _httpClient.PostAsync(url, content);
                }
                else
                {
                    httpResponse = await _httpClient.PostAsync(url, null);
                }
                ClearHeaders(settings);
                return await ProcessResponse<TResponse>(httpResponse);
            }
            catch (Exception ex)
            {
                return HandleException<TResponse>(ex);
            }
            finally
            {
                ClearHeaders(settings);
            }
        }
        public async Task<HttpResponseMessage> PostAsync<TRequest>(string url, TRequest? request = null, KaminariHttpClientSettings? settings = null)
    where TRequest : class
        {
            try
            {
                ApplyHeaders(settings);
                HttpResponseMessage httpResponse;
                if (request != null)
                {
                    string jsonRequest = JsonConvert.SerializeObject(request);
                    StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    httpResponse = await _httpClient.PostAsync(url, content);
                }
                else
                {
                    httpResponse = await _httpClient.PostAsync(url, null);
                }
                ClearHeaders(settings);
                return httpResponse;
            }
            catch (Exception ex)
            {
                return HandleException<HttpResponseMessage>(ex);
            }
            finally
            {
                ClearHeaders(settings);
            }
        }


        /// <summary>
        /// Applies the headers from the provided KaminariHttpClientSettings to the HttpClient's default request headers.
        /// </summary>
        /// <param name="settings">The KaminariHttpClientSettings containing the headers to be applied.</param>
        private void ApplyHeaders(KaminariHttpClientSettings settings)
        {
            if (settings != null)
            {
                foreach (KeyValuePair<string, string> header in settings.Headers)
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// Clears the default request headers of the HttpClient and the headers of the provided KaminariHttpClientSettings.
        /// </summary>
        /// <param name="settings">The KaminariHttpClientSettings object containing the headers to be cleared.</param>
        private void ClearHeaders(KaminariHttpClientSettings settings)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            settings?.ClearHeaders();
        }

        private async Task<T> ProcessResponseGet(HttpResponseMessage httpResponse)
        {
            try
            {
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                T lastResult = JsonConvert.DeserializeObject<T>(jsonResponse);
                return lastResult;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<T>.ExceptionToErrorMessage(ex), ex);
            }
        }

        private T HandleExceptionGet(Exception ex)
        {
            throw new Exception(ExceptionHandler<T>.ExceptionToErrorMessage(ex), ex);
        }


        /// <summary>
        /// Processes the HTTP response and returns a BaseResponse object.
        /// </summary>
        /// <param name="httpResponse">The HttpResponseMessage to process.</param>
        /// <returns>A Task containing the processed BaseResponse object.</returns>
        private async Task<TResponse> ProcessResponse<TResponse>(HttpResponseMessage httpResponse)
            where TResponse : class
        {
            try
            {
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                TResponse lastResult = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
                return lastResult;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<T>.ExceptionToErrorMessage(ex), ex);
            }
        }

        /// <summary>
        /// Handles an exception and returns a BaseResponse object with an error response.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        /// <returns>A BaseResponse object with an error response.</returns>
        private TResponse HandleException<TResponse>(Exception ex)
            where TResponse : class
        {
            throw new Exception(ExceptionHandler<T>.ExceptionToErrorMessage(ex), ex);
        }
    }


}