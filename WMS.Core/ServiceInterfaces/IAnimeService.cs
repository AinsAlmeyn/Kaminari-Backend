using WMS.Core.Entities.Api.AnimeController;
using WMS.Core.Entities.Api.JukanV4Api.BaseObjects;
using WMS.Core.Entities.Api.JukanV4Api.BaseObjects.AnimeAPIs;
using WMS.Core.Entities.Database.MongoDb;

namespace WMS.Core.ServiceInterfaces
{
    public interface IAnimeService
    {
        Task<SearchAnimeInfo> SearchAnime(SearchFilter searchFilter);
        Task<SearchAnimeInfo> TopTvAnimes();
        Task<SearchAnimeInfo> SeasonNowAnimes(PageInformation? page);
        Task<Seasons> AllSeasons();
        Task<AnimeUserStatistics> AnimeUserStats(MalIdInformation malIdInformation);
        Task<StaffInfo> AnimeStaff(MalIdInformation malIdInformation);
        Task<AnimeCharacter> AnimeCharacters(MalIdInformation malIdInformation);
        Task<SearchAnimeInfo> SearchSeasons(SeasonInfo seasonInfo);
        Task<IdAnimeInfo> AnimeById(SelectAnimeId selectAnimeId);
        Task<AnimeImages> AnimePictures(SelectAnimeId selectAnimeId);
    }
}