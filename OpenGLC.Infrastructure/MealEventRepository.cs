using OpenGLC.Data.Entities;
using OpenGLC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Infrastructure
{
	public class MealEventRepository : BaseRepository<MealEvent>, IMealEventRepository
	{
		public MealEventRepository(OpenglclevelContext dbContext) : base(dbContext)
		{
		}
	}
}
