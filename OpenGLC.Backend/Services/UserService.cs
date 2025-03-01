using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenGLC.Data.Entities;
using OpenGLC.Emailer;
using OpenGLC.Infrastructure.Interfaces;
using OpenGLC.Infrastructure.Services;
using OpenGLC.Models.Accounts;
using OpenGLC.Models.Exceptions;
using OpenGLC.Models.Security;
using OpenGLC.Security;

namespace OpenGLC.Backend.Services
{
	public class UserService : IUserService
	{

		private readonly ISecurityKeys _securityKeysValues;
		private readonly EncryptorEngine _encryptor;
		private readonly DecryptorEngine _decryptor;
		private readonly TokenHandlerEngine _tokenHandler;
		private readonly IUserRepository _userRepository;
		private readonly IPasswordResetRequestRepository _passwordResetRequestRepository;
		private readonly IEmailSender _emailSender;
		private readonly OpenglclevelContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserService(ISecurityKeys securityKeysValues, OpenglclevelContext dbContext,
			EncryptorEngine encryptor, IUserRepository userRepository, DecryptorEngine decryptor,
			TokenHandlerEngine tokenHandler, IHttpContextAccessor httpContextAccessor, 
			IPasswordResetRequestRepository passwordResetRequestRepository, IEmailSender emailSender)
		{
			_securityKeysValues = securityKeysValues;
			_encryptor = encryptor;
			_decryptor = decryptor;
			_tokenHandler = tokenHandler;
			_userRepository = userRepository;
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
			_passwordResetRequestRepository = passwordResetRequestRepository;
			_emailSender = emailSender;
		}

		public async Task CreateResetPasswordRequest(string emailOrUserName)
		{
			var user = _userRepository.FindByExpresion(f => f.Email == emailOrUserName);
			if ((await user.CountAsync()) == 0)
			{
				user = _userRepository.FindByExpresion(f => f.UserName == emailOrUserName);
			}

			if ((await user.CountAsync()) == 0)
				return;

			var userList = await user.ToListAsync();
			List<PasswordResetRequest> requests = new List<PasswordResetRequest>();
			TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
			DateTime utcNow = DateTime.UtcNow;
			DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, cstZone);

			var emailContent = "";
			emailContent += "<h2 style=\"text-align:center\">Reestablece tu contraseña</h2><br>";
			var request = _httpContextAccessor.HttpContext.Request;

			foreach (var u in userList)
			{
				ChangeAlreadyExistingChangePasswordRequest(u);
				var req = new PasswordResetRequest()
				{
					Id = Guid.NewGuid(),
					UserId = u.Id,
					CreationDate = cstTime,
					Email = u.Email,
					ExpirationDate = cstTime.AddDays(1),
				};

				await _passwordResetRequestRepository.AddAsync(req);
				_dbContext.SaveChanges();

				var currentBaseURL = $"{request.Scheme}://{request.Host}/resetPassword?id=" + req.Id;
				emailContent += $"<table style='border-collapse: collapse;width: 100%;'>" +
					$"<tr>" +
					$"<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>Usuario</th> " +
					$"<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>Link</th>" +
					$"</tr>" +
					$"<tr>" +
					$"<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + u.UserName + "</td>" +
					$"<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + currentBaseURL + "'>Link de recuperación: " + req.Id.ToString() + "</a></td>" +
					$"</tr>" +
					$"</table>";
				var message = new MessageModel(new string[] { u.Email }, "Recupera tu password de OpenGLC", emailContent, null, u.FirstName, "OpenGLC App") ;
				_emailSender.SendEmail(message, u, u.Id);

			}

		}

	

		private void ChangeAlreadyExistingChangePasswordRequest(User? u)
		{
			var preexistedActiveRequests = _passwordResetRequestRepository
								.FindByExpresion(f => f.UserId == u.Id && !f.Status);
			if (preexistedActiveRequests != null)
			{
				foreach (var pear in preexistedActiveRequests)
				{
					pear.Status = true;
					_passwordResetRequestRepository.UpdateAsync(pear);
				}
			}
		}

		public async Task<object> GetServerStatus()
		{
			try
			{
				var regsNumbs = await _dbContext.Users.CountAsync();
				return new { connection = regsNumbs >= 0 };
			}
			catch (Exception ex)
			{
				return new { connection = false };
			}
		}

		public async Task<TokenResultModel> Login(string userName, string password, bool? tokenForDeleteAction = false)
		{
			var user = _userRepository.FindByExpresion(u => u.UserName == userName).FirstOrDefault();

			if (user == null)
				throw new FriendlyException("User not found in system ");

			var decryptedSystemPassword = await _decryptor.Decrypt(user.HashedPassword, user.Salt);

			if (decryptedSystemPassword.PlainPassword != password)
				throw new FriendlyException("Provided password is wrong, try again");

			var tokenClaims = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("userID", user.Id.ToString()),
				new KeyValuePair<string, string>("userName", user.Name)
			};

			if (tokenForDeleteAction.HasValue && tokenForDeleteAction.Value)
			{
				tokenClaims.Add(new KeyValuePair<string, string>("allowDeleteActionToken", "true"));
			}

			var tokenData = _tokenHandler.GenerateToken(tokenClaims, user.Id);
			//https://github.com/dotnet/AspNetCore.Docs/issues/7076
			//_httpContextAccessor.HttpContext.Session.SetObject("userID", user.Id.ToString());

			return tokenData;

		}

		public async Task<EncryptorResultModel> RegisterUser(NewRegisterModel newRegister)
		{
			var preExistedUser = _userRepository.FindByExpresion(u => u.UserName == newRegister.UserName).FirstOrDefault();

			if (preExistedUser != null)
				throw new FriendlyException("User already exist on system ");

			var encryptedPassword = await _encryptor.PasswordEncrypt(newRegister.Password);

			var newUser = new User()
			{
				Id = Guid.NewGuid(),
				HashedPassword = encryptedPassword.PasswordHash,
				Salt = encryptedPassword.SaltValue,
				Email = newRegister.Email,
				FirstName = newRegister.FirstName,
				Name = newRegister.Name,
				UserName = newRegister.UserName,

			};

			await _userRepository.AddAsync(newUser);
			await _dbContext.SaveChangesAsync();

			return encryptedPassword;
		}
	}
}
