using OpenGLC.Models.Accounts;
using OpenGLC.Models.Security;

namespace OpenGLC.Infrastructure.Services
{
	public interface IUserService
	{
		public Task<EncryptorResultModel> RegisterUser(NewRegisterModel newRegister);
		public Task<TokenResultModel> Login(string userName, string password, bool? tokenForDeleteAction = false);
		public Task<Object> GetServerStatus();
	}
}
