using Theater.Infrastructure.Abstracts;

namespace Theater.Application.Services.Identity
{
    public class FakeIdentityService : IIdentityService
    {
        public int? GetPrincipialId()
        {
            return null;
        }
    }
}
