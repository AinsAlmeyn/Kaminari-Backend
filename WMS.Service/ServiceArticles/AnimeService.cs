using System.Linq.Expressions;
using MassTransit.Configuration;
using Wms.Service.ServiceConnector;
using WMS.Core.Entities.Api.AnimeController;
using WMS.Core.Entities.Api.JukanV4Api.BaseObjects;
using WMS.Core.Entities.Api.JukanV4Api.BaseObjects.AnimeAPIs;
using WMS.Core.Entities.Base;
using WMS.Core.Entities.Database.MongoDb;
using WMS.Core.RepositoryInterfaces;
using WMS.Core.ServiceInterfaces;
using WMS.DataAccess.RepositoryArticles;
using WMS.Service.ErrorHandling;
using WMS.Service.ServiceConnector.ApiClient;

namespace WMS.Service.ServiceArticles
{
    public class AnimeService : IAnimeService
    {
        private readonly ServiceConnector<SearchAnimeInfo> _animeConnector;
        private readonly IUserAnimeRepository _userAnimeRepository;
        private readonly BaseUrlContainer _baseUrlContainer;
        public AnimeService(ServiceConnector<SearchAnimeInfo> animeConnector, BaseUrlContainer baseUrlContainer, IUserAnimeRepository userAnimeRepository)
        {
            _animeConnector = animeConnector;
            _baseUrlContainer = baseUrlContainer;
            _userAnimeRepository = userAnimeRepository;
        }

        #region CREATE OPERATIONS
        #endregion

        #region READ OPERATIONS
        public async Task<SearchAnimeInfo> SearchAnime(SearchFilter searchFilter)
        {
            try
            {
                Dictionary<string, string> queryParams = SetQueryParameters(searchFilter);
                SearchAnimeInfo searchedAnimeInfos = await _animeConnector.GetAsync<SearchAnimeInfo>(_baseUrlContainer.JikanMoeV4_Anime, queryParams);
                searchedAnimeInfos = await AddMyStatus(searchedAnimeInfos, searchFilter.userId);
                return searchedAnimeInfos;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<SearchAnimeInfo>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<SearchAnimeInfo> TopTvAnimes()
        {
            try
            {
                SearchAnimeInfo topTvAnimes = await _animeConnector.GetAsync<SearchAnimeInfo>(_baseUrlContainer.JikanMoeV4_Top_Anime + "?type=tv");
                return topTvAnimes;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<SearchAnimeInfo>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<SearchAnimeInfo> SeasonNowAnimes(PageInformation? page)
        {
            try
            {
                SearchAnimeInfo topTvAnimes = new();
                if (page != null)
                {
                    topTvAnimes = await _animeConnector.GetAsync<SearchAnimeInfo>(_baseUrlContainer.JikanMoveV4_Season_Now + $"?page={page.page}");
                }
                else
                {
                    topTvAnimes = await _animeConnector.GetAsync<SearchAnimeInfo>(_baseUrlContainer.JikanMoveV4_Season_Now);
                }
                topTvAnimes = await AddMyStatus(topTvAnimes, page.userId);
                return topTvAnimes;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<SearchAnimeInfo>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<AnimeUserStatistics> AnimeUserStats(MalIdInformation malIdInformation)
        {
            try
            {
                AnimeUserStatistics animeUserStats = await _animeConnector.GetAsync<AnimeUserStatistics>(_baseUrlContainer.JikanMoeV4_Anime + $"/{malIdInformation.mal_id}/statistics");
                return animeUserStats;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<AnimeUserStatistics>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<StaffInfo> AnimeStaff(MalIdInformation malIdInformation)
        {
            try
            {
                StaffInfo animeStaff = await _animeConnector.GetAsync<StaffInfo>(_baseUrlContainer.JikanMoeV4_Anime + $"/{malIdInformation.mal_id}/staff");
                return animeStaff;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<StaffInfo>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<AnimeCharacter> AnimeCharacters(MalIdInformation malIdInformation)
        {
            try
            {
                AnimeCharacter animeCharacters = await _animeConnector.GetAsync<AnimeCharacter>(_baseUrlContainer.JikanMoeV4_Anime + $"/{malIdInformation.mal_id}/characters");
                return animeCharacters;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<AnimeCharacter>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<Seasons> AllSeasons()
        {
            try
            {
                Seasons seasons = await _animeConnector.GetAsync<Seasons>(_baseUrlContainer.JikanMoeV4_Season);
                return seasons;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<Seasons>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<IdAnimeInfo> AnimeById(SelectAnimeId selectAnimeId)
        {
            try
            {
                IdAnimeInfo animeById = await _animeConnector.GetAsync<IdAnimeInfo>(_baseUrlContainer.JikanMoeV4_Anime + $"/{selectAnimeId.series_animedb_id}");
                return animeById;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<IdAnimeInfo>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<AnimeImages> AnimePictures(SelectAnimeId selectAnimeId)
        {
            try
            {
                AnimeImages animePictures = await _animeConnector.GetAsync<AnimeImages>(_baseUrlContainer.JikanMoeV4_Anime + $"/{selectAnimeId.series_animedb_id}/pictures");
                return animePictures;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<AnimeImages>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<SearchAnimeInfo> SearchSeasons(SeasonInfo seasonInfo)
        {
            try
            {
                SearchAnimeInfo searchSeasons = await _animeConnector.GetAsync<SearchAnimeInfo>(_baseUrlContainer.JikanMoeV4_Season + $"/{seasonInfo.year}/{seasonInfo.season}");
                searchSeasons = await AddMyStatus(searchSeasons, seasonInfo.userId);
                return searchSeasons;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<SearchAnimeInfo>.ExceptionToErrorMessage(ex));
            }
        }
        #endregion

        #region UPDATE OPERATIONS
        #endregion

        #region DELETE OPERATIONS
        #endregion

        #region PRIVATE METHODS
        private Dictionary<string, string> SetQueryParameters(SearchFilter searchFilter)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            System.Reflection.PropertyInfo[] properties = typeof(SearchFilter).GetProperties();

            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                object? value = prop.GetValue(searchFilter);
                if (value != null)
                {
                    // Boolean türü için özel bir durum, çünkü 'true' veya 'false' olarak string'e çevrilmeli
                    if (prop.PropertyType == typeof(bool?))
                    {
                        queryParams.Add(prop.Name.ToLower(), (bool)value ? "true" : "false");
                    }
                    // Nullable int, double gibi tipler için kontrol
                    else if (IsNullableType(prop.PropertyType) && !prop.PropertyType.IsGenericType)
                    {
                        queryParams.Add(prop.Name.ToLower(), value.ToString());
                    }
                    // Nullable int, double için kontrol
                    else if (prop.PropertyType == typeof(int?) || prop.PropertyType == typeof(double?))
                    {
                        queryParams.Add(prop.Name.ToLower(), value.ToString());
                    }
                    // String ve diğer türler için genel durum
                    else if (value is string && !string.IsNullOrWhiteSpace(value as string))
                    {
                        queryParams.Add(prop.Name.ToLower(), value.ToString());
                    }
                }
            }

            return queryParams;
        }
        private static bool IsNullableType(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        private async Task<SearchAnimeInfo> AddMyStatus(SearchAnimeInfo searchAnimeInfo, string userId)
        {
            try
            {
                if (searchAnimeInfo.Data != null)
                {
                    for (int i = 0; i < searchAnimeInfo.Data.Count; i++)
                    {
                        BaseResponse<UserAnime> isAnimeInUserList = await _userAnimeRepository.FilterByAsync(x => x.UserId == userId && x.series_animedb_id == searchAnimeInfo.Data[i].mal_id);
                        if (isAnimeInUserList.Data.Count() > 0)
                        {
                            searchAnimeInfo.Data[i].my_status = isAnimeInUserList.Data.FirstOrDefault().my_status;
                            searchAnimeInfo.Data[i].my_score= isAnimeInUserList.Data.FirstOrDefault().my_score;
                        }
                        continue;
                    }
                }
                return searchAnimeInfo;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<SearchAnimeInfo>.ExceptionToErrorMessage(ex));
            }
        }
        #endregion
    }
}