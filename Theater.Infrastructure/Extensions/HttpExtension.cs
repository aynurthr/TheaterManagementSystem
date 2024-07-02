using Microsoft.AspNetCore.Http;

namespace Theater.Infrastructure.Extensions
{
    public static partial class Extension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request is null)
                return false;

            //'X-Requested-With', 'XMLHttpRequest'
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
