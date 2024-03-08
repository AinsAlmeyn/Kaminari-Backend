using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Core.Entities.Api.AnimeController;
using WMS.Core.Entities.Api.JukanV4Api.BaseObjects;
using WMS.Core.Entities.Api.JukanV4Api.BaseObjects.AnimeAPIs;
using WMS.Core.Entities.Database.MongoDb;
using WMS.Core.ServiceInterfaces;
using WMS.Service.ErrorHandling;

namespace KaminariAniList.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnimeController : ControllerBase
    {
        private readonly IAnimeService _animeService;

        public AnimeController(IAnimeService animeService)
        {
            _animeService = animeService;
        }

        [HttpPost("AnimeById")]
        public async Task<IActionResult> AnimeById([FromBody] SelectAnimeId selectedAnimeId)
        {
            try
            {
                IdAnimeInfo animeInfo = await _animeService.AnimeById(selectedAnimeId);
                if (animeInfo == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(animeInfo);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<IdAnimeInfo>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("AnimePictures")]
        public async Task<IActionResult> AnimePictures([FromBody] SelectAnimeId selectedAnimeId)
        {
            try
            {
                AnimeImages animeImages = await _animeService.AnimePictures(selectedAnimeId);
                if (animeImages == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(animeImages);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<AnimeImages>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("SearchAnime")]
        public async Task<IActionResult> SearchAnime([FromBody] SearchFilter searchFilter)
        {
            try
            {
                SearchAnimeInfo searchedAnimeInfos = await _animeService.SearchAnime(searchFilter);
                if (searchedAnimeInfos == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(searchedAnimeInfos);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<SearchAnimeInfo>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("TopTvAnimes")]
        public async Task<IActionResult> TopTvAnimes()
        {
            try
            {
                SearchAnimeInfo searchedAnimeInfos = await _animeService.TopTvAnimes();
                if (searchedAnimeInfos == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(searchedAnimeInfos);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<SearchAnimeInfo>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("SeasonNowAnimes")]
        public async Task<IActionResult> SeasonNowAnimes(PageInformation? page)
        {
            try
            {
                SearchAnimeInfo searchedAnimeInfos = await _animeService.SeasonNowAnimes(page);
                if (searchedAnimeInfos == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(searchedAnimeInfos);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<SearchAnimeInfo>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("AllSeasons")]
        public async Task<IActionResult> AllSeasons()
        {
            try
            {
                Seasons seasons = await _animeService.AllSeasons();
                if (seasons == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(seasons);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<Seasons>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("SearchSeasons")]
        public async Task<IActionResult> SearchSeasons([FromBody] SeasonInfo seasonInfo)
        {
            try
            {
                SearchAnimeInfo searchedAnimeInfos = await _animeService.SearchSeasons(seasonInfo);
                if (searchedAnimeInfos == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(searchedAnimeInfos);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<SearchAnimeInfo>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("AnimeUserStats")]
        public async Task<IActionResult> AnimeUserStats([FromBody] MalIdInformation malIdInformation)
        {
            try
            {
                AnimeUserStatistics animeUserStats = await _animeService.AnimeUserStats(malIdInformation);
                if (animeUserStats == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(animeUserStats);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<AnimeUserStatistics>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("AnimeStaff")]
        public async Task<IActionResult> AnimeStaff([FromBody] MalIdInformation malIdInformation)
        {
            try
            {
                StaffInfo staffInfo = await _animeService.AnimeStaff(malIdInformation);
                if (staffInfo == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(staffInfo);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<StaffInfo>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("AnimeCharacters")]
        public async Task<IActionResult> AnimeCharacters([FromBody] MalIdInformation malIdInformation)
        {
            try
            {
                AnimeCharacter animeCharacters = await _animeService.AnimeCharacters(malIdInformation);
                if (animeCharacters == null)
                    return BadRequest("No anime found with the given search parameters.");
                return Ok(animeCharacters);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<AnimeCharacter>.ExceptionToResponse(ex));
            }
        }
    }
}