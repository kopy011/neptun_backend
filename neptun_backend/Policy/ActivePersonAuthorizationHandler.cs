using Microsoft.AspNetCore.Authorization;
using neptun_backend.Utils;
using System.Security.Claims;

namespace neptun_backend.Policy
{
    public class ActivePersonAuthorizationHandler : AuthorizationHandler<ActivePersonOnlyPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActivePersonOnlyPolicy requirement)
        {
            //TODO nem műkszik ez a fostos

            var userDataClaim = context.User.FindFirst(claim => claim.Type == ClaimTypes.UserData);
            if(userDataClaim is null)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
