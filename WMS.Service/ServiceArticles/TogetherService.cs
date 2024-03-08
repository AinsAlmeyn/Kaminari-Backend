using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wms.Service.ServiceConnector;
using WMS.Core.Entities.Api.Watch2GetherApi.BaseObjects;
using WMS.Core.Entities.Api.YoutubeV3Api;
using WMS.Core.Entities.Base;
using WMS.Core.RepositoryInterfaces;
using WMS.Service.ErrorHandling;
using WMS.Service.ServiceConnector.ApiClient;

namespace WMS.Service.ServiceArticles
{
    public interface ITogetherService
    {
        Task<BaseResponse<CreateRoomResponse>> CreateRoom(BaseRequest<CreateRoomRequest> request);
        Task<BaseResponse<UpdateRoomRequest>> UpdateRoom(UpdateRoomRequest updateRoomRequest);
        Task<BaseResponse<TogetherRoom>> GetAllRooms();
        Task<BaseResponse<EnterRoomRequest>> EnterRoom(EnterRoomRequest enterRoomRequest);
        Task<BaseResponse<EnterRoomRequest>> LeaveRoom(EnterRoomRequest enterRoomRequest);
        Task<BaseResponse<YTVideoDetailResponse>> YTSearchVideo(YTSearchRequest ytSearchRequest);
    }
    public class TogetherService : ITogetherService
    {
        private readonly ServiceConnector<CreateRoomRequest> _serviceConnector;
        private readonly BaseUrlContainer _baseUrlContainer;
        private readonly ITogetherRepository _togetherRepository;
        public TogetherService(ServiceConnector<CreateRoomRequest> serviceConnector, BaseUrlContainer baseUrlContainer, ITogetherRepository togetherRepository)
        {
            _serviceConnector = serviceConnector;
            _baseUrlContainer = baseUrlContainer;
            _togetherRepository = togetherRepository;
        }

        public async Task<BaseResponse<CreateRoomResponse>> CreateRoom(BaseRequest<CreateRoomRequest> request)
        {
            try
            {
                CreateRoomResponse createRoomResponse = new();
                CreateRoomRequest? createRoom = request?.Data?.FirstOrDefault();

                createRoom.w2g_api_key = "YOUR API KEY";

                if (createRoom == null)
                    throw new Exception("CreateRoomRequest is null");

                CreateRoomResponse baseCreateRoomResponse = (CreateRoomResponse)await _serviceConnector.PostAsync<CreateRoomRequest, CreateRoomResponse>(_baseUrlContainer.Watch2Gether_Create_Room, createRoom);

                if (baseCreateRoomResponse == null)
                    throw new Exception("CreateRoomResponse is null");

                createRoomResponse = baseCreateRoomResponse;

                var insertResult = await _togetherRepository.InsertOneAsync(new TogetherRoom()
                {
                    CreateDate = DateTime.UtcNow,
                    RoomConnectionString = "https://w2g.tv/tr/room/?r=" + createRoomResponse.Streamkey,
                });
                if (insertResult.Data != null || insertResult.Type == ResponseType.SUCCESS)
                {
                    return new BaseResponse<CreateRoomResponse>
                    {
                        Data = new List<CreateRoomResponse> { createRoomResponse },
                        Type = ResponseType.SUCCESS
                    };
                }
                throw new Exception(insertResult.Detail);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<CreateRoomRequest>.ExceptionToErrorMessage(ex));
            }
        }

        public async Task<BaseResponse<UpdateRoomRequest>> UpdateRoom(UpdateRoomRequest updateRoomRequest)
        {
            try
            {
                updateRoomRequest.w2g_api_key = "YOUR API KEY";
                UpdateRoomTogetherRequest togetherRequest = new() { w2g_api_key = updateRoomRequest.w2g_api_key, item_url = updateRoomRequest.item_url };
                HttpResponseMessage updateResult = (HttpResponseMessage)await _serviceConnector.PostAsync<UpdateRoomTogetherRequest>(_baseUrlContainer.Watch2Gether_Update_Room + updateRoomRequest.streamkey + "/sync_update", togetherRequest);
                if (updateResult.IsSuccessStatusCode)
                {
                    return new BaseResponse<UpdateRoomRequest>
                    {
                        Data = null,
                        Type = ResponseType.SUCCESS
                    };
                }
                else
                {
                    return new BaseResponse<UpdateRoomRequest>
                    {
                        Data = null,
                        Type = ResponseType.ERROR
                    };
                }

            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<UpdateRoomRequest>.ExceptionToErrorMessage(ex));
            }
        }

        public async Task<BaseResponse<TogetherRoom>> GetAllRooms()
        {
            try
            {
                BaseResponse<TogetherRoom> baseTogetherRooms = await _togetherRepository.GetAllAsync();
                if (baseTogetherRooms.Data == null || baseTogetherRooms.Type == ResponseType.ERROR)
                    throw new Exception("TogetherRoom is null");

                foreach (TogetherRoom room in baseTogetherRooms.Data.ToList())
                {
                    if (DateTime.UtcNow - room.CreateDate > TimeSpan.FromDays(1))
                    {
                        await _togetherRepository.DeleteOneAsync(x => x.CreateDate == room.CreateDate & x.RoomConnectionString == room.RoomConnectionString);
                    }
                }

                BaseResponse<TogetherRoom> activeRooms = await _togetherRepository.GetAllAsync();
                return activeRooms;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<CreateRoomRequest>.ExceptionToErrorMessage(ex));
            }
        }

        public async Task<BaseResponse<EnterRoomRequest>> EnterRoom(EnterRoomRequest enterRoomRequest)
        {
            try
            {
                BaseResponse<TogetherRoom> enterResult = new();
                BaseResponse<TogetherRoom> oldRoom = await _togetherRepository.FilterByAsync(x => x.RoomConnectionString == enterRoomRequest.roomConnectionString);

                if (oldRoom.Data == null || oldRoom.Type == ResponseType.ERROR)
                    throw new Exception("TogetherRoom is null");

                if (oldRoom.Data.FirstOrDefault().ActiveUsers == null)
                {
                    enterResult = await _togetherRepository.ReplaceOneAsync(x => x.RoomConnectionString == enterRoomRequest.roomConnectionString, new TogetherRoom()
                    {
                        _id = oldRoom.Data.FirstOrDefault()._id,
                        CreateDate = oldRoom.Data.FirstOrDefault().CreateDate,
                        RoomConnectionString = oldRoom.Data.FirstOrDefault().RoomConnectionString,
                        ActiveUsers = new List<string> { enterRoomRequest.userName }
                    });
                }
                else
                {
                    enterResult = await _togetherRepository.ReplaceOneAsync(x => x.RoomConnectionString == enterRoomRequest.roomConnectionString, new TogetherRoom()
                    {
                        _id = oldRoom.Data.FirstOrDefault()._id,
                        CreateDate = oldRoom.Data.FirstOrDefault().CreateDate,
                        RoomConnectionString = oldRoom.Data.FirstOrDefault().RoomConnectionString,
                        ActiveUsers = oldRoom.Data.FirstOrDefault().ActiveUsers.Append(enterRoomRequest.userName).ToList()
                    });
                }

                if (enterResult.Type == ResponseType.SUCCESS)
                    return new BaseResponse<EnterRoomRequest>
                    {
                        Data = null,
                        Type = ResponseType.SUCCESS
                    };
                else
                    throw new Exception(enterResult.Detail);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<EnterRoomRequest>.ExceptionToErrorMessage(ex));
            }
        }

        public async Task<BaseResponse<EnterRoomRequest>> LeaveRoom(EnterRoomRequest enterRoomRequest)
        {
            try
            {
                BaseResponse<TogetherRoom> oldRoom = await _togetherRepository.FilterByAsync(x => x.RoomConnectionString == enterRoomRequest.roomConnectionString);

                if (oldRoom.Data == null || oldRoom.Type == ResponseType.ERROR)
                    throw new Exception("TogetherRoom is null");

                BaseResponse<TogetherRoom> enterResult = await _togetherRepository.ReplaceOneAsync(x => x.RoomConnectionString == enterRoomRequest.roomConnectionString, new TogetherRoom()
                {
                    _id = oldRoom.Data.FirstOrDefault()._id,
                    CreateDate = oldRoom.Data.FirstOrDefault().CreateDate,
                    RoomConnectionString = oldRoom.Data.FirstOrDefault().RoomConnectionString,
                    ActiveUsers = oldRoom.Data.FirstOrDefault().ActiveUsers.Where(x => x != enterRoomRequest.userName).ToList()
                });

                if (enterResult.Type == ResponseType.SUCCESS)
                    return new BaseResponse<EnterRoomRequest>
                    {
                        Data = null,
                        Type = ResponseType.SUCCESS
                    };
                else
                    throw new Exception(enterResult.Detail);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<EnterRoomRequest>.ExceptionToErrorMessage(ex));
            }
        }

        public async Task<BaseResponse<YTVideoDetailResponse>> YTSearchVideo(YTSearchRequest ytSearchRequest)
        {
            try
            {
                Dictionary<string, string> searchQueryParams = new Dictionary<string, string>
                    {
                        {"key", _baseUrlContainer.YoutubeV3_ApiKey},
                        {"type", ytSearchRequest.type},
                        {"videoEmbeddable", ytSearchRequest.videoEmbeddable.ToString()},
                        {"pageToken", ytSearchRequest.pageToken},
                        {"q", ytSearchRequest.q}
                    };

                YTSearchResponse ytSearchResponse = await _serviceConnector.GetAsync<YTSearchResponse>(_baseUrlContainer.YoutubeV3_Search, searchQueryParams);

                if (ytSearchResponse == null)
                    throw new Exception("YTSearchResponse is null");

                Dictionary<string, string> videoDetailQueryParams = new Dictionary<string, string>
                    {
                        {"key", _baseUrlContainer.YoutubeV3_ApiKey},
                        {"part", "snippet,statistics"},
                        {"id", string.Join(",", ytSearchResponse.items.Select(x => x.id.videoId))}
                    };

                YTVideoDetailResponse ytVideoDetailResponse = await _serviceConnector.GetAsync<YTVideoDetailResponse>(_baseUrlContainer.YoutubeV3_Video, videoDetailQueryParams);
                ytVideoDetailResponse.nextPageInformation = ytSearchResponse.nextPageToken;
                ytVideoDetailResponse.prevPageToken = ytSearchResponse.prevPageToken;

                if (ytVideoDetailResponse == null)
                    throw new Exception("YTVideoDetailResponse is null");

                return new BaseResponse<YTVideoDetailResponse>
                {
                    Data = new List<YTVideoDetailResponse> { ytVideoDetailResponse },
                    Type = ResponseType.SUCCESS
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<YTSearchRequest>.ExceptionToErrorMessage(ex));
            }
        }
    }
}