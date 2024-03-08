using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WMS.Core.Entities.Api.AuthController;
using WMS.Core.Entities.Base;
using WMS.Core.Entities.ConnectionAndSettings.App;
using WMS.Core.Entities.Database.MongoDb;
using WMS.Core.RepositoryInterfaces;
using WMS.Core.ServiceInterfaces;
using WMS.Service.ErrorHandling;

namespace WMS.Service.ServiceArticles
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettins;
        public UserService(IUserRepository userRepository, JwtSettings jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettins = jwtSettings;
        }

        public bool IsUserExists(LoginRequest loginRequestModel)
        {
            return _userRepository.FilterByAsync(x => x.UserName == loginRequestModel.Email & x.Password == loginRequestModel.Password).Result.Data.Any();
        }

        public bool IsUserNameExists(string userName)
        {
            return _userRepository.FilterByAsync(x => x.UserName == userName).Result.Data.Any();
        }

        public User GetUserInfo(LoginRequest loginRequestModel)
        {
            return _userRepository.FilterByAsync(x => x.UserName == loginRequestModel.Email & x.Password == loginRequestModel.Password).Result.Data.FirstOrDefault();
        }

        public BaseResponse<LoginRespose> LoginUser(LoginRequest loginRequestModel)
        {
            try
            {
                bool checkResult = IsUserExists(loginRequestModel);
                if (checkResult != true)
                {
                    throw new Exception("User not found");
                }

                User userInfo = GetUserInfo(loginRequestModel);
                Claim[] claims = new[]
                {
                new Claim("Email", loginRequestModel.Email),
                new Claim("Password", loginRequestModel.Password)
            };

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettins.SecurityKey));
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _jwtSettins.Issuer,
                    audience: _jwtSettins.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
                string tokenToString = new JwtSecurityTokenHandler().WriteToken(token);

                return new BaseResponse<LoginRespose>
                {
                    Type = ResponseType.SUCCESS,
                    Data = new List<LoginRespose>
                    {
                        new LoginRespose
                        {
                            NameSurname = userInfo.NameSurname,
                            MongoId = userInfo._id.ToString(),
                            ImageUrl = userInfo.ImageUrl,
                            UserName = userInfo.UserName,
                            Token = tokenToString,
                            IsSuccess = true,
                            ExpireDate = token.ValidTo
                        }
                    },
                    DefinitionLang = "User logged in successfully"
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception(ExceptionHandler<User>.ExceptionToErrorMessage(ex));
            }

        }
        public async Task<BaseResponse<User>> UpdateUserPassword(UpdatePassword updatePasswordRequest)
        {
            try
            {
                BaseResponse<User> updateResult = await _userRepository.UpdateOneAsync(x => x.UserName == updatePasswordRequest.Email && x.Password == updatePasswordRequest.Password, Builders<User>.Update.Set(u => u.Password, updatePasswordRequest.UpdatedPassword));

                if (updateResult.Type == ResponseType.SUCCESS)
                {
                    return new BaseResponse<User>()
                    {
                        Type = ResponseType.SUCCESS,
                        Data = null,
                        DefinitionLang = "Password updated successfully"
                    };
                }
                else
                {
                    throw new Exception("Password update failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionHandler<User>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<BaseResponse<RegisterRequest>> RegisterUser(RegisterRequest registerRequest)
        {
            try
            {
                bool checkResult = IsUserNameExists(registerRequest.Email);
                if (checkResult == true)
                {
                    throw new Exception("User already exists");
                }
                
                BaseResponse<User> userResult = await _userRepository.InsertOneAsync(new User
                {
                    UserName = registerRequest.Email,
                    Password = registerRequest.Password,
                });

                if (userResult.Type == ResponseType.SUCCESS)
                {
                    return new BaseResponse<RegisterRequest>()
                    {
                        Type = ResponseType.SUCCESS,
                        Data = new List<RegisterRequest>
                        {
                            new RegisterRequest
                            {
                                Email = registerRequest.Email,
                                Password = registerRequest.Password
                            }
                        },
                        DefinitionLang = "User registered successfully"
                    };
                }
                else
                {
                    throw new Exception("User registration failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionHandler<LoginRequest>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<BaseResponse<ChangePasswordRequest>> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                BaseResponse<User> userResult = await _userRepository.UpdateOneAsync(x => x.UserName == changePasswordRequest.userName && x.Password == changePasswordRequest.currentPassword, Builders<User>.Update.Set(u => u.Password, changePasswordRequest.newPassword));

                if (userResult.Type == ResponseType.SUCCESS)
                {
                    return new BaseResponse<ChangePasswordRequest>()
                    {
                        Type = ResponseType.SUCCESS,
                        Data = null,
                        DefinitionLang = "Password changed successfully"
                    };
                }
                else
                {
                    throw new Exception("Password change failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionHandler<User>.ExceptionToErrorMessage(ex));
            }
        }
        public async Task<BaseResponse<ChangeUserNameRequest>> ChangeUserName(ChangeUserNameRequest changeUserNameRequest)
        {
            try
            {
                BaseResponse<User> userResult = await _userRepository.UpdateOneAsync(x => x._id == changeUserNameRequest.userId, Builders<User>.Update.Set(u => u.UserName, changeUserNameRequest.newUserName));

                if (userResult.Type == ResponseType.SUCCESS)
                {
                    return new BaseResponse<ChangeUserNameRequest>()
                    {
                        Type = ResponseType.SUCCESS,
                        Data = null,
                        DefinitionLang = "Username changed successfully"
                    };
                }
                else
                {
                    throw new Exception("Username change failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionHandler<User>.ExceptionToErrorMessage(ex));
            }
        }
    }
}
