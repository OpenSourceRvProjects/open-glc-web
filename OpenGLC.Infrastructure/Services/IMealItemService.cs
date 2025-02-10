using OpenGLC.Models.MealEventItems;
using OpenGLC.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Infrastructure.Services
{
	public interface IMealItemService
	{
		public Task<List<NewMealItemModel>> AddMealItemsFromUserID(List<NewMealItemModel> meals);
		public Task<NewMealItemModelDB> AddSingleMealItemToUser(Guid userID, NewMealItemModel meal);
		public Task<PaginationListEntityModel<NewMealItemModelDB>> GetMealItems(int page, int itemsPerPage, string searchTerm = null);

	}
}
