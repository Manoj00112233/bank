using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class BaseApiController : ControllerBase
    {
        protected int GetUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        protected string GetUsername()
        {
            return User.Identity?.Name ?? "Anonymous";
        }

        protected string GetUserRole()
        {
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role);
            return roleClaim?.Value ?? "Unknown";
        }

        protected bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }
    }
}