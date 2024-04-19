using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace EstudoBDM.Infraestructure
{
    public class ScopeRequirementAttribute : TypeFilterAttribute
    {
        public ScopeRequirementAttribute(string claimValue, string claimType = "scopes") : base(typeof(ScopeRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ScopeRequirementFilter: IAuthorizationFilter
    {
        private readonly Claim _claim;

        public ScopeRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string[] requiredScopes = _claim.Value.Split(", ");

            var hasScopeClaim = context.HttpContext.User.Claims.ToList().Find(c => c.Type.Equals("scopes"));

            if (hasScopeClaim == null && requiredScopes.Length > 1)
            {
                context.Result = new ForbidResult();
            }

            string[] userScopes = hasScopeClaim!.Value.Split(", ");

            bool isAllowed = false;

            foreach(var userScope in userScopes)
            {
                if (requiredScopes.Contains(userScope))
                {
                    isAllowed = true;
                    break;
                }
            }

            if (!isAllowed)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
