using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Core.Entities.Base;
using WMS.Core.Entities.Database.MongoDb;
using WMS.Service.ServiceArticles;

namespace KaminariAniList.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserAnimeProfileController : ControllerBase
    {
        private readonly IUserAnimeProfileService _userAnimeProfileService;
        public UserAnimeProfileController(IUserAnimeProfileService userAnimeProfileService)
        {
            _userAnimeProfileService = userAnimeProfileService;
        }

        [HttpPost("FetchMyAnimeList")]
        public async Task<IActionResult> FetchMyAnimeList([FromBody] FetchMyList fetchMyList)
        {
            try
            {
                BaseResponse<FetchMyList> result = await _userAnimeProfileService.FetchMyAnimeList(fetchMyList);
                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("MyAllAnimes")]
        public async Task<IActionResult> MyAllAnimes([FromBody] SelectUserId userId)
        {
            try
            {
                BaseResponse<FetchMyList> result = await _userAnimeProfileService.MyAllAnimes(userId);
                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteUserAnime")]
        public async Task<IActionResult> DeleteUserAnime([FromBody] UserAnime userAnime)
        {
            try
            {
                BaseResponse<UserAnime> result = await _userAnimeProfileService.DeleteUserAnime(userAnime);
                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertUserAnime")]
        public async Task<IActionResult> InsertUserAnime([FromBody] UserAnime userAnime)
        {
            try
            {
                BaseResponse<UserAnime> result = await _userAnimeProfileService.InsertUserAnime(userAnime);
                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteUserAnimes")]
        public async Task<IActionResult> DeleteUserAnimes([FromBody] SelectUserId userId)
        {
            try
            {
                BaseResponse<UserAnime> result = await _userAnimeProfileService.DeleteUserAnimes(userId);
                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}