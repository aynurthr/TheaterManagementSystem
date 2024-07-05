using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Theater.Infrastructure.Abstracts;

namespace Theater.Application.Services.Identity
{
    public class FakeIdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FakeIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetPrincipialId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userId, out var id))
            {
                return id;
            }
            return null;
        }
    }
}
