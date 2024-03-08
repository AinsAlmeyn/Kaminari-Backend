using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WMS.Core.Entities.Api.AuthController;
using WMS.Core.Entities.Base;
using WMS.Core.ServiceInterfaces;
using WMS.Service.ErrorHandling;

namespace KaminariAniList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginRequest loginRequestModel)
        {
            try
            {
                BaseResponse<LoginRespose> loginResult = _userService.LoginUser(loginRequestModel);
                if (loginResult.Type == ResponseType.SUCCESS)
                    return Ok(loginResult);
                else
                    return BadRequest(loginResult);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<LoginRequest>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequestModel)
        {
            try
            {
                BaseResponse<RegisterRequest> registerResult = await _userService.RegisterUser(registerRequestModel);
                if (registerResult.Type == ResponseType.SUCCESS)
                    return Ok(registerResult);
                else
                    return BadRequest(registerResult);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<RegisterRequest>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequestModel)
        {
            try
            {
                BaseResponse<ChangePasswordRequest> changePasswordResult = await _userService.ChangePassword(changePasswordRequestModel);
                if (changePasswordResult.Type == ResponseType.SUCCESS)
                    return Ok(changePasswordResult);
                else
                    return BadRequest(changePasswordResult);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<ChangePasswordRequest>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("ChangeUserName")]
        public async Task<IActionResult> ChangeUserName([FromBody] ChangeUserNameRequest changeUserNameRequestModel)
        {
            try
            {
                BaseResponse<ChangeUserNameRequest> changeUserNameResult = await _userService.ChangeUserName(changeUserNameRequestModel);
                if (changeUserNameResult.Type == ResponseType.SUCCESS)
                    return Ok(changeUserNameResult);
                else
                    return BadRequest(changeUserNameResult);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<ChangeUserNameRequest>.ExceptionToResponse(ex));
            }
        }
    }
}
