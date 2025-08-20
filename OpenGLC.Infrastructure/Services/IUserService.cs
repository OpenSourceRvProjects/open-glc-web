using OpenGLC.Data.Entities;
using OpenGLC.Models.Accounts;
using OpenGLC.Models.Security;

namespace OpenGLC.Infrastructure.Services
{
	public interface IUserService
	{
		public Task<EncryptorResultModel> RegisterUser(NewRegisterModel newRegister);
		public Task<TokenResultModel> Login(string userName, string password, bool? tokenForDeleteAction = false);
		public Task<Object> GetServerStatus();
		public Task CreateResetPasswordRequest(string emailOrUserName);
        public Task<GoogleUserInfo> VerifyGoogleToken(string idToken);
		public Task<User?> GetUserByUserName(string userNameOrEmail);
		public Task<TokenResultModel> ExternalProviderLogin(string userName);
    }
}
