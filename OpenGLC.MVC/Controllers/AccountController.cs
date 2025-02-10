using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGLC.Infrastructure.Services;
using OpenGLC.Models.Accounts;
using OpenGLC.Models.Security;
using OpenGLC.MVC.Filters;

namespace OpenGLC.MVC.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{

		private readonly IUserService _userSC;
		private readonly ISecurityKeys _securityKeysValues;
		private readonly IHttpContextAccessor _httpContext;

		public AccountController(IUserService userSC, ISecurityKeys securityKeys, IHttpContextAccessor httpContext)
		{
			_userSC = userSC;
			_securityKeysValues = securityKeys;
			_httpContext = httpContext;
		}

		[HttpGet]
		[Route("login")]
		[AutomaticExceptionHandler]
		public async Task<IActionResult> login(string userName, string password, bool? tokenForDeleteAction = false)
		{
			var result = await _userSC.Login(userName, password, tokenForDeleteAction);
			_httpContext.HttpContext.Session.SetString("userID", result.UserID.ToString());
			return Ok(result);
		}

		[HttpGet]
		[Route("userData")]
		[Authorize]
		public IActionResult userData(string userName)
		{
			var result = new { userName = "userName" };
			return Ok(result);
		}


		// POST api/<AccountController>
		[HttpPost]
		[Route("register")]
		[AutomaticExceptionHandler]
		public async Task<IActionResult> Post([FromBody] NewRegisterModel value)
		{
			var result = await _userSC.RegisterUser(value);
			return Ok(result);
		}


	}
}
