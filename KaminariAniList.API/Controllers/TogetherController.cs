using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Core.Entities.Api.Watch2GetherApi.BaseObjects;
using WMS.Core.Entities.Api.YoutubeV3Api;
using WMS.Core.Entities.Base;
using WMS.Service.ErrorHandling;
using WMS.Service.ServiceArticles;

namespace KaminariAniList.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TogetherController : ControllerBase
    {
        private readonly ITogetherService _togetherService;
        public TogetherController(ITogetherService togetherService)
        {
            _togetherService = togetherService;
        }

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoom(BaseRequest<CreateRoomRequest> request)
        {
            try
            {
                BaseResponse<CreateRoomResponse> result = await _togetherService.CreateRoom(request);

                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler<CreateRoomResponse>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("UpdateRoom")]
        public async Task<IActionResult> UpdateRoom(UpdateRoomRequest updateRoomRequest)
        {
            try
            {
                BaseResponse<UpdateRoomRequest> result = await _togetherService.UpdateRoom(updateRoomRequest);

                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler<UpdateRoomRequest>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            try
            {
                BaseResponse<TogetherRoom> result = await _togetherService.GetAllRooms();

                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler<TogetherRoom>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("EnterRoom")]
        public async Task<IActionResult> EnterRoom(EnterRoomRequest request)
        {
            try
            {
                BaseResponse<EnterRoomRequest> result = await _togetherService.EnterRoom(request);

                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler<EnterRoomRequest>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("LeaveRoom")]
        public async Task<IActionResult> LeaveRoom(EnterRoomRequest request)
        {
            try
            {
                BaseResponse<EnterRoomRequest> result = await _togetherService.LeaveRoom(request);

                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler<EnterRoomRequest>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("YTSearchVideo")]
        public async Task<IActionResult> YTSearchVideo(YTSearchRequest ytSearchRequest)
        {
            try
            {
                BaseResponse<YTVideoDetailResponse> result = await _togetherService.YTSearchVideo(ytSearchRequest);

                if (result.Type == ResponseType.ERROR)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler<YTVideoDetailResponse>.ExceptionToResponse(ex));
            }
        }
    }
}