using Mayhem.Util;
using Microsoft.AspNetCore.Mvc;

namespace Mayhem.WebApi.Base
{
    public class OwnControllerBase : ControllerBase
    {
        protected int UserId => JwtSecurityTokenHelper.GetUserId(Request);
    }
}
