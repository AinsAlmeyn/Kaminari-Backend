using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Core.Entities.Base;
using WMS.Core.Entities.Database.MongoDb;
using WMS.Core.RepositoryInterfaces;
using WMS.Service.ErrorHandling;

namespace WMS.Service.ServiceArticles
{
    public interface IUserAnimeProfileService
    {
        Task<BaseResponse<FetchMyList>> FetchMyAnimeList(FetchMyList fetchMyList);
        Task<BaseResponse<FetchMyList>> MyAllAnimes(SelectUserId userId);
        Task<BaseResponse<UserAnime>> InsertUserAnime(UserAnime userAnime);
        Task<BaseResponse<UserAnime>> DeleteUserAnime(UserAnime userAnime);
        Task<BaseResponse<UserAnime>> DeleteUserAnimes(SelectUserId selectUserId);
    }
    public class UserAnimeProfileService : IUserAnimeProfileService
    {
        private readonly IUserAnimeProfileRepository _userAnimeProfileRepository;
        private readonly IUserAnimeRepository _userAnimeRepository;
        public UserAnimeProfileService(IUserAnimeProfileRepository userAnimeProfileRepository, IUserAnimeRepository userAnimeRepository)
        {
            _userAnimeProfileRepository = userAnimeProfileRepository;
            _userAnimeRepository = userAnimeRepository;
        }

        public async Task<BaseResponse<FetchMyList>> FetchMyAnimeList(FetchMyList fetchMyList)
        {
            try
            {
                await this.CreateNewUserAnimeProfileAsync(fetchMyList.myanimelist.myinfo, fetchMyList.username, fetchMyList.userId);
                foreach (UserAnime userAnime in fetchMyList.myanimelist.anime)
                {
                    await this.CreateNewUserAnimeAsync(userAnime, fetchMyList.userId);
                }
                return new BaseResponse<FetchMyList>
                {
                    Data = null,
                    DefinitionLang = "My Anime List Fetched Successfully",
                    Type = ResponseType.SUCCESS
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<FetchMyList>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<BaseResponse<FetchMyList>> MyAllAnimes(SelectUserId userId)
        {
            try
            {
                BaseResponse<UserAnimeProfile> userAnimeProfile = await _userAnimeProfileRepository.FilterByAsync(x => x.user_id == userId.userId);
                if (userAnimeProfile.Type == ResponseType.ERROR)
                    throw new Exception("User Anime Profile Not Found");
                BaseResponse<UserAnime> userAnimes = await _userAnimeRepository.FilterByAsync(x => x.UserId == userId.userId);
                if (userAnimes.Type == ResponseType.ERROR)
                    throw new Exception("User Animes Not Found");

                FetchMyList fetchMyList = new FetchMyList
                {
                    myanimelist = new MyAnimeList
                    {
                        myinfo = userAnimeProfile.Data.FirstOrDefault(),
                        anime = userAnimes.Data.ToList()
                    },
                    userId = userAnimeProfile.Data.FirstOrDefault().user_id,
                    username = userAnimeProfile.Data.FirstOrDefault().user_name
                };

                return new BaseResponse<FetchMyList>
                {
                    Data = new List<FetchMyList> { fetchMyList },
                    DefinitionLang = "My All Animes Fetched Successfully",
                    Type = ResponseType.SUCCESS
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<FetchMyList>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<BaseResponse<UserAnime>> InsertUserAnime(UserAnime userAnime)
        {
            try
            {
                string mongoId = await this.FindMongoDbIdByCodeAsync(userAnime);
                if (mongoId != null)
                    userAnime._id = mongoId;

                BaseResponse<UserAnime> insertResult = await _userAnimeRepository.UpsertOneAsync(x => x.series_animedb_id == userAnime.series_animedb_id & x.UserId == userAnime.UserId, userAnime);
                BaseResponse<UserAnimeProfile> userAnimeResult = await _userAnimeProfileRepository.FilterByAsync(x => x.user_id == userAnime.UserId);
                
                UpdateUserAnimeProfile(userAnime, userAnimeResult, insertResult.DefinitionLang);

                await _userAnimeProfileRepository.ReplaceOneAsync(x => x.user_id == userAnime.UserId, userAnimeResult.Data.FirstOrDefault());

                if (insertResult.Type == ResponseType.SUCCESS)
                    return new BaseResponse<UserAnime>
                    {
                        Data = insertResult.Data,
                        DefinitionLang = "User Anime Inserted Successfully",
                        Type = ResponseType.SUCCESS
                    };

                throw new Exception("User Anime Insertion Failed");
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<UserAnime>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<BaseResponse<UserAnime>> DeleteUserAnimes(SelectUserId selectUserId)
        {
            try
            {
                BaseResponse<UserAnime> deleteResult = await _userAnimeRepository.DeleteManyAsync(x => x.UserId == selectUserId.userId);
                if (deleteResult.Type == ResponseType.SUCCESS)
                {
                    BaseResponse<UserAnimeProfile> userAnimeProfileResult = await _userAnimeProfileRepository.FilterByAsync(x => x.user_id == selectUserId.userId);
                    UserAnimeProfile? userProfile = userAnimeProfileResult.Data.FirstOrDefault();
                    if (userProfile != null)
                    {
                        userProfile.user_total_anime = 0;
                        userProfile.user_total_completed = 0;
                        userProfile.user_total_dropped = 0;
                        userProfile.user_total_onhold = 0;
                        userProfile.user_total_plantowatch = 0;
                        userProfile.user_total_watching = 0;
                        await _userAnimeProfileRepository.ReplaceOneAsync(x => x.user_id == selectUserId.userId, userProfile);
                    }
                    return new BaseResponse<UserAnime>
                    {
                        Data = null,
                        DefinitionLang = "User Animes Deleted Successfully",
                        Type = ResponseType.SUCCESS
                    };
                }
                else
                {
                    throw new Exception("User Animes Deletion Failed");
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<UserAnime>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<BaseResponse<UserAnime>> DeleteUserAnime(UserAnime userAnime)
        {
            try
            {
                BaseResponse<UserAnime> deleteResult = await _userAnimeRepository.DeleteOneAsync(x => x.series_animedb_id == userAnime.series_animedb_id & x.UserId == userAnime.UserId);
                if (deleteResult.Type == ResponseType.SUCCESS)
                {
                    await UpdateUserAnimeProfileOnDelete(userAnime);

                    return new BaseResponse<UserAnime>
                    {
                        Data = null,
                        DefinitionLang = "User Anime Deleted Successfully",
                        Type = ResponseType.SUCCESS
                    };
                }
                else
                {
                    throw new Exception("User Anime Deletion Failed");
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<UserAnime>.ExceptionToErrorMessage(ex));
            }
        }
        private async Task UpdateUserAnimeProfileOnDelete(UserAnime userAnime)
        {
            BaseResponse<UserAnimeProfile> userAnimeProfileResult = await _userAnimeProfileRepository.FilterByAsync(x => x.user_id == userAnime.UserId);
            var userProfile = userAnimeProfileResult.Data.FirstOrDefault();

            if (userProfile != null)
            {
                switch (userAnime.my_status)
                {
                    case "Completed":
                        userProfile.user_total_completed--;
                        userProfile.user_total_anime--;
                        break;
                    case "Dropped":
                        userProfile.user_total_dropped--;
                        userProfile.user_total_anime--;
                        break;
                    case "On-Hold":
                        userProfile.user_total_onhold--;
                        userProfile.user_total_anime--;
                        break;
                    case "Plan to Watch":
                        userProfile.user_total_plantowatch--;
                        userProfile.user_total_anime--;
                        break;
                    case "Watching":
                        userProfile.user_total_watching--;
                        userProfile.user_total_anime--;
                        break;
                }
                await _userAnimeProfileRepository.ReplaceOneAsync(x => x.user_id == userAnime.UserId, userProfile);
            }
        }
        private void UpdateUserAnimeProfile(UserAnime userAnime, BaseResponse<UserAnimeProfile> userAnimeResult, string insertResultMessage)
        {
            var statusChanges = new Dictionary<string, int>
    {
        {"Completed", 0},
        {"Dropped", 0},
        {"On-Hold", 0},
        {"Plan to Watch", 0},
        {"Watching", 0}
    };

            if (userAnime.my_old_status != null && statusChanges.ContainsKey(userAnime.my_old_status))
            {
                statusChanges[userAnime.my_old_status]--;
            }

            if (statusChanges.ContainsKey(userAnime.my_status))
            {
                statusChanges[userAnime.my_status]++;
            }

            var userProfile = userAnimeResult.Data.FirstOrDefault();
            if (userProfile != null)
            {
                foreach (var statusChange in statusChanges)
                {
                    switch (statusChange.Key)
                    {
                        case "Completed":
                            userProfile.user_total_completed += statusChange.Value;
                            break;
                        case "Dropped":
                            userProfile.user_total_dropped += statusChange.Value;
                            break;
                        case "On-Hold":
                            userProfile.user_total_onhold += statusChange.Value;
                            break;
                        case "Plan to Watch":
                            userProfile.user_total_plantowatch += statusChange.Value;
                            break;
                        case "Watching":
                            userProfile.user_total_watching += statusChange.Value;
                            break;
                    }
                }
                if (insertResultMessage == "Veri ba�ar�yla eklendi")
                {
                    userProfile.user_total_anime++;
                }
            }
        }
        private async Task<string> FindMongoDbIdByCodeAsync(UserAnime userAnime)
        {
            try
            {
                BaseResponse<UserAnime> foundedUserAnime = await _userAnimeRepository.FilterByAsync(x => x.series_animedb_id == userAnime.series_animedb_id & x.UserId == userAnime.UserId);
                if (foundedUserAnime.Data.Count() > 0)
                {
                    return foundedUserAnime.Data.FirstOrDefault()._id;
                }
                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<UserAnime>.ExceptionToErrorMessage(ex));
            }
        }
        private async Task<BaseResponse<UserAnimeProfile>> CreateNewUserAnimeProfileAsync(UserAnimeProfile userAnimeProfile, string userName, string userId)
        {
            try
            {
                userAnimeProfile.user_id = userId;
                userAnimeProfile.user_name = userName;
                BaseResponse<UserAnimeProfile> insertResult = await _userAnimeProfileRepository.InsertOneAsync(userAnimeProfile);
                if (insertResult.Type == ResponseType.SUCCESS)
                    return new BaseResponse<UserAnimeProfile>
                    {
                        Data = insertResult.Data,
                        DefinitionLang = "User Anime Profile Created Successfully",
                        Type = ResponseType.SUCCESS
                    };
                else
                    throw new Exception("User Anime Profile Creation Failed");
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<UserAnimeProfile>.ExceptionToErrorMessage(ex));
            }
        }
        private async Task<BaseResponse<UserAnime>> CreateNewUserAnimeAsync(UserAnime userAnime, string userId)
        {
            try
            {
                userAnime.UserId = userId;
                BaseResponse<UserAnime> insertResult = await _userAnimeRepository.InsertOneAsync(userAnime);
                if (insertResult.Type == ResponseType.SUCCESS)
                    return new BaseResponse<UserAnime>
                    {
                        Data = insertResult.Data,
                        DefinitionLang = "User Anime Created Successfully",
                        Type = ResponseType.SUCCESS
                    };
                else
                    throw new Exception("User Anime Creation Failed");
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<UserAnime>.ExceptionToErrorMessage(ex));
            }
        }
    }
}