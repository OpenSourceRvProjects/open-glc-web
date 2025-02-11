using OpenGLC.Data.Entities;
using OpenGLC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Infrastructure
{
	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		private OpenglclevelContext _context;
		public UserRepository(OpenglclevelContext dbContext) : base(dbContext)
		{
			_context = dbContext;
		}
	}
}
