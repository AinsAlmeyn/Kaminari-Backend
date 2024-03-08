using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wms.Service.ServiceConnector;
using WMS.Core.Entities.Api.MovieController;
using WMS.Core.Entities.Api.TmdbV3Api.BaseObjects;
using WMS.Core.Entities.Base;
using WMS.Service.ErrorHandling;
using WMS.Service.ServiceConnector.ApiClient;

namespace WMS.Service.ServiceArticles
{
    public interface IMovieService
    {
        Task<BaseResponse<BaseMovie>> DiscoverMovieAsync(BaseRequest<MovieDiscoverRequest> baseMovieDiscoverRequest);
        Task<BaseResponse<BaseMovie>> RecommandationMovieAsync(BaseRequest<MovieRecommendationRequest> baseMovieRecommendationRequest);
        Task<BaseResponse<MovieDetail>> MovieDetailAsync(BaseRequest<MovieDetailRequest> baseMovideDetailRequest);
        Task<BaseResponse<BaseMovie>> SearchMovieAsync(BaseRequest<MovieSearchRequest> baseMovieSearchRequest);
    }
    public class MovieService : IMovieService
    {
        private readonly BaseUrlContainer _baseUrlContainer;
        private readonly ServiceConnector<BaseMovie> _movieConnector;
        public MovieService(BaseUrlContainer baseUrlContainer, ServiceConnector<BaseMovie> movieConnector)
        {
            _baseUrlContainer = baseUrlContainer;
            _movieConnector = movieConnector;
        }

        public async Task<BaseResponse<BaseMovie>> DiscoverMovieAsync(BaseRequest<MovieDiscoverRequest> baseMovieDiscoverRequest)
        {
            try
            {
                MovieDiscoverRequest movieDiscoverRequest = baseMovieDiscoverRequest.Data.FirstOrDefault();

                KaminariHttpClientSettings settings = new KaminariHttpClientSettings();
                settings.AddHeader("Authorization", $"Bearer {_baseUrlContainer.Tmdb_Api_Token}");

                string url = _baseUrlContainer.Tmdb_Movie_Discover;
                var queryParams = new Dictionary<string, string>
                    {
                        {"include_adult", movieDiscoverRequest.include_adult.ToString()},
                        {"language", movieDiscoverRequest.language},
                        {"page", movieDiscoverRequest.page.ToString()}
                    };

                BaseMovie discoverMovieResult = await _movieConnector.GetAsync<BaseMovie>(url, queryParams, settings);

                if (discoverMovieResult == null)
                    throw new Exception("DiscoverMovieAsync method returned null");

                return new BaseResponse<BaseMovie>
                {
                    Data = new List<BaseMovie> { discoverMovieResult },
                    DefinitionLang = "DiscoverMovieAsync method executed successfully",
                    Type = ResponseType.SUCCESS
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<BaseMovie>.ExceptionToErrorMessage(ex));
            }
        }

        public async Task<BaseResponse<BaseMovie>> RecommandationMovieAsync(BaseRequest<MovieRecommendationRequest> baseMovieRecommendationRequest)
        {
            try
            {
                MovieRecommendationRequest movieRecommendationRequest = baseMovieRecommendationRequest.Data.FirstOrDefault();

                KaminariHttpClientSettings settings = new KaminariHttpClientSettings();
                settings.AddHeader("Authorization", $"Bearer {_baseUrlContainer.Tmdb_Api_Token}");

                string url = _baseUrlContainer.Tmdb_Movie_Recommendations + $"{movieRecommendationRequest.id}/recommendations";
                var queryParams = new Dictionary<string, string>
                    {
                        {"language", movieRecommendationRequest.language},
                        {"page", movieRecommendationRequest.page.ToString()}
                    };

                BaseMovie recommendationMovieResult = await _movieConnector.GetAsync<BaseMovie>(url, queryParams, settings);

                if (recommendationMovieResult == null)
                    throw new Exception("DiscoverMovieAsync method returned null");

                return new BaseResponse<BaseMovie>
                {
                    Data = new List<BaseMovie> { recommendationMovieResult },
                    DefinitionLang = "RecommandationMovieAsync method executed successfully",
                    Type = ResponseType.SUCCESS
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<BaseMovie>.ExceptionToErrorMessage(ex));
            }
        }

        public async Task<BaseResponse<MovieDetail>> MovieDetailAsync(BaseRequest<MovieDetailRequest> baseMovideDetailRequest)
        {
            try
            {
                MovieDetailRequest movieDetailRequest = baseMovideDetailRequest.Data.FirstOrDefault();

                KaminariHttpClientSettings settings = new KaminariHttpClientSettings();
                settings.AddHeader("Authorization", $"Bearer {_baseUrlContainer.Tmdb_Api_Token}");

                string url = _baseUrlContainer.Tmdb_Movie_Details + $"{movieDetailRequest.id}";
                var queryParams = new Dictionary<string, string>
                    {
                        {"language", movieDetailRequest.language},
                        {"append_to_response", movieDetailRequest.append_to_response}
                    };

                MovieDetail movieDetailResult = await _movieConnector.GetAsync<MovieDetail>(url, queryParams, settings);

                if (movieDetailResult == null)
                    throw new Exception("MovieDetailAsync method returned null");

                return new BaseResponse<MovieDetail>
                {
                    Data = new List<MovieDetail> { movieDetailResult },
                    DefinitionLang = "MovieDetailAsync method executed successfully",
                    Type = ResponseType.SUCCESS
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<MovieDetail>.ExceptionToErrorMessage(ex));
            }
        }
    
        public async Task<BaseResponse<BaseMovie>> SearchMovieAsync(BaseRequest<MovieSearchRequest> baseMovieSearchRequest)
        {
            try
            {
                MovieSearchRequest movieSearchRequest = baseMovieSearchRequest.Data.FirstOrDefault();

                KaminariHttpClientSettings settings = new KaminariHttpClientSettings();
                settings.AddHeader("Authorization", $"Bearer {_baseUrlContainer.Tmdb_Api_Token}");

                string url = _baseUrlContainer.Tmdb_Movie_Search;
                var queryParams = new Dictionary<string, string>
                    {
                        {"query", movieSearchRequest.query},
                        {"language", movieSearchRequest.language},
                        {"page", movieSearchRequest.page.ToString()}
                    };

                BaseMovie searchMovieResult = await _movieConnector.GetAsync<BaseMovie>(url, queryParams, settings);

                if (searchMovieResult == null)
                    throw new Exception("SearchMovieAsync method returned null");

                return new BaseResponse<BaseMovie>
                {
                    Data = new List<BaseMovie> { searchMovieResult },
                    DefinitionLang = "SearchMovieAsync method executed successfully",
                    Type = ResponseType.SUCCESS
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<BaseMovie>.ExceptionToErrorMessage(ex));
            }
        }
    }
}