using OpenGLC.Data.Entities;
using OpenGLC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Infrastructure
{
	public class MealItemRepository : BaseRepository<MealItem>, IMealItemRepository
	{
		public MealItemRepository(OpenglclevelContext dbContext) : base(dbContext)
		{
		}
	}
}
