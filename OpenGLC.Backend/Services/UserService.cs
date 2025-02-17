﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenGLC.Data.Entities;
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
		private readonly OpenglclevelContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserService(ISecurityKeys securityKeysValues, OpenglclevelContext dbContext,
			EncryptorEngine encryptor, IUserRepository userRepository, DecryptorEngine decryptor,
			TokenHandlerEngine tokenHandler, IHttpContextAccessor httpContextAccessor)
		{
			_securityKeysValues = securityKeysValues;
			_encryptor = encryptor;
			_decryptor = decryptor;
			_tokenHandler = tokenHandler;
			_userRepository = userRepository;
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;

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
				return new { connection = false};
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
