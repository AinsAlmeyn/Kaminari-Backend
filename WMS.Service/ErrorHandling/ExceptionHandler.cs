using WMS.Core.Entities.Base;

namespace WMS.Service.ErrorHandling
{
    public static class ExceptionHandler<T> where T : class, new()
    {
        public static BaseResponse<T> ExceptionToResponse(Exception ex)
        {
            BaseResponse<T> error = new BaseResponse<T>
            {
                Data = null,
                DefinitionLang = ex.Message,
                Detail = ex.StackTrace + " " + ex.InnerException?.Message,
                Type = ResponseType.ERROR
            };
            return error;
        }

        public static string ExceptionToErrorMessage(Exception ex)
        {
            return ex.Message + " " + ex.StackTrace + " " + ex.InnerException?.Message;
        }
    }
}