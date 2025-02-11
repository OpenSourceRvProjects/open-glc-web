using Microsoft.AspNetCore.Mvc.Filters;
using OpenGLC.Models.Exceptions;
using OpenGLC.Security;

namespace OpenGLC.MVC.Filters
{
	public class DeleteAuthorizationFilter : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{

			var userToken = ControllerUtilities
					.GetTokenFromContextRequest(filterContext.HttpContext);

			var tokenDecoded = TokenHandlerEngine.GetTokenDataByStringValue(userToken);

			var allowTokenOption = tokenDecoded.Claims.Where(W => W.Type == "allowDeleteActionToken").FirstOrDefault()?.Value;

			if (allowTokenOption == null || allowTokenOption != "true")
			{
				throw new FriendlyException("User token is not valid for this request");
			}


		}
	}
}
