using Microsoft.AspNetCore.Authorization;

namespace neptun_backend.Policy
{
    public class ActivePersonOnlyPolicy : IAuthorizationRequirement
    {
    }
}
