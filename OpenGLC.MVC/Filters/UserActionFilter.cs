using Microsoft.AspNetCore.Mvc.Filters;
using OpenGLC.Models.Exceptions;

namespace OpenGLC.MVC.Filters
{
	public class UserActionFilter : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{

			var userToken = ControllerUtilities
					.GetUserIdFromRequestContext(filterContext.HttpContext);

			var userID = filterContext.HttpContext.Session.GetString("userID");

			if (userID != null)
			{
				if (userToken == Guid.Parse(userID))
					filterContext.HttpContext.Session.SetString("userID", userID);
				else
					throw new UserSessionException("Provided token do not correspond to session");
			}
			else
			{
				filterContext.HttpContext.Session.SetString("userID", userToken.ToString());
			}
		}
	}
}
