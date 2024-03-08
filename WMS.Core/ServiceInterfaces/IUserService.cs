using WMS.Core.Entities.Api.AuthController;
using WMS.Core.Entities.Base;
using WMS.Core.Entities.Database.MongoDb;

namespace WMS.Core.ServiceInterfaces
{
    public interface IUserService
    {
        BaseResponse<LoginRespose> LoginUser(LoginRequest loginRequestModel);
        Task<BaseResponse<User>> UpdateUserPassword(UpdatePassword updatePasswordRequest);
        Task<BaseResponse<RegisterRequest>> RegisterUser(RegisterRequest registerRequest);
        Task<BaseResponse<ChangePasswordRequest>> ChangePassword(ChangePasswordRequest changePasswordRequest);
        Task<BaseResponse<ChangeUserNameRequest>> ChangeUserName(ChangeUserNameRequest changeUserNameRequest);
    }
}
