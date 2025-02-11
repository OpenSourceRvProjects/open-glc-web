using OpenGLC.Models.MealEvents;
using OpenGLC.Models.Pagination;
using OpenGLC.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Infrastructure.Services
{
	public interface IMealEventService
	{
		public Task<Guid> AddMealEvent(NewMealEventModel mealEvent);
		public Task<PaginationListEntityModel<MealEventModel>> GetEvents(int page, int itemsPerPage, string searchTerm = null);
		public Task<UserMetricsModel> GetEventsGlcAverage();
		MealEventDedtailsModel GetMealEventDetails(Guid eventId);
		public Task<bool> DeleteEventMealWithItems(Guid mealEvent);
	}
}
