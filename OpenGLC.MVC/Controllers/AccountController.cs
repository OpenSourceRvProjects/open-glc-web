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
        private readonly IConfiguration _configuration;


        public AccountController(IUserService userSC, ISecurityKeys securityKeys, IHttpContextAccessor httpContext, IConfiguration configuration)
		{
			_userSC = userSC;
			_securityKeysValues = securityKeys;
			_httpContext = httpContext;
			_configuration = configuration;
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
		[Route("getStatus")]
		[AutomaticExceptionHandler]
		public async Task<IActionResult> getStatus()
		{
			var status = await _userSC.GetServerStatus();
			return Ok(status);
		}

		[HttpGet]
		[Route("userData")]
		[Authorize]
		public IActionResult userData(string userName)
		{
			var result = new { userName = "userName" };
			return Ok(result);
		}

		[HttpGet]
		[Route("passwordChangeRequest")]
		public async Task<IActionResult> PasswordChangeRequest(string emailOrUserName)
		{
			await _userSC.CreateResetPasswordRequest(emailOrUserName);
			return Ok();
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


        [HttpGet]
        [Route("getGoogleClientID")]
        public IActionResult GetGoogleClientID()
        {
            var clientId = _configuration["security:googleClientID"];
            return Ok(new { googleClientID = clientId });
        }

        [HttpPost]
        [Route("loginOrRegisterGoogleAuth")]
        public async Task<IActionResult> registerOrLoginWithGoogle(GoogleAuthRequest request)
        {
            var googleUser = await _userSC.VerifyGoogleToken(request.IdToken);
            if (googleUser == null)
                return Unauthorized("Invalid Google token");

			var isRegistered = await _userSC.GetUserByUserName(googleUser.Email) != null;

			if (isRegistered) {
                var result = await _userSC.ExternalProviderLogin(userName: googleUser.Email);
                _httpContext.HttpContext.Session.SetString("userID", result.UserID.ToString());
                return Ok(result);

            }
            else
            {

                string fullName = googleUser.DisplayName ?? googleUser.Name ?? "";
                string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string firstName = nameParts.Length > 0 ? nameParts[0] : "";
                string lastName1 = nameParts.Length > 1 ? nameParts[1] : "";

                var response = await _userSC.RegisterUser(new NewRegisterModel
                {
                    Email = googleUser.Email,
                    Name = firstName,
                    FirstName = lastName1,
                    UserName = googleUser.Email,
                });

				var result = _userSC.ExternalProviderLogin(userName: googleUser.Email);
                _httpContext.HttpContext.Session.SetString("userID", googleUser.Email.ToString());
                return Ok(result);


            }

            //string fullName = googleUser.DisplayName ?? googleUser.Name ?? "";
            //string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            //string firstName = nameParts.Length > 0 ? nameParts[0] : "";
            //string lastName1 = nameParts.Length > 1 ? nameParts[1] : "";

            //var response = await _accountService.RegisterUserAccount(new RegisterModel()
            //{
            //    Email = googleUser.Email,
            //    Name = firstName,
            //    LastName1 = lastName1,
            //    LastName2 = "",
            //    UserName = googleUser.Email,
            //});


        }


    }
}
