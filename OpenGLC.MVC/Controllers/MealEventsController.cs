using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGLC.Infrastructure.Services;
using OpenGLC.Models.API;
using OpenGLC.Models.MealEvents;
using OpenGLC.MVC.Filters;

namespace OpenGLC.MVC.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class MealEventsController : ControllerBase
	{
		// GET: api/<MealEventsController>
		private readonly IMealEventService _eventSC;
		private readonly ControllerUtilities _utilities;
		public MealEventsController(IMealEventService eventSC, ControllerUtilities utilities)
		{
			_eventSC = eventSC;
			_utilities = utilities;
		}
		[HttpGet]
		[AutomaticExceptionHandler]
		[UserActionFilter]
		public async Task<IActionResult> Get(int page, int itemsPerPage, string searchTerm = "")
		{
			var result = await _eventSC.GetEvents(page, itemsPerPage, searchTerm);
			return Ok(result);
		}

		[HttpGet("id")]
		[AutomaticExceptionHandler]
		[UserActionFilter]
		public IActionResult Get(Guid eventId)
		{
			var eventDetail = _eventSC.GetMealEventDetails(eventId);
			return Ok(eventDetail);
		}

		[HttpGet]
		[Route("getEventMealTypes")]
		[AutomaticExceptionHandler]
		public IActionResult GetEventMealTypes()
		{
			var result = MealTypes.GetMealTypesDefinition();
			return Ok(result);
		}

		[HttpGet]
		[Route("userEventMetrics")]
		[AutomaticExceptionHandler]
		[UserActionFilter]
		public async Task<IActionResult> GetEventsGlcAverage()
		{
			var result = await _eventSC.GetEventsGlcAverage();
			return Ok(result);
		}

		[HttpGet]
		[Route("lastThreeMonthsLevels")]
		[AutomaticExceptionHandler]
		[UserActionFilter]
		public async Task<IActionResult> LastThreeMonthsLevels()
		{
			var result = await _eventSC.GetLast3MonthsLevels();
			return Ok(result);
		}

		// POST api/<MealEventsController>
		[HttpPost]
		[AutomaticExceptionHandler]
		[UserActionFilter]
		public async Task<IActionResult> Post([FromBody] NewMealEventModel newEvent)
		{
			var result = await _eventSC.AddMealEvent(newEvent);
			return Ok(result);
		}

		[HttpDelete("id")]
		[AutomaticExceptionHandler]
		[UserActionFilter]
		[DeleteAuthorizationFilter]
		public async Task<IActionResult> Delete(Guid eventId)
		{
			var eventDetail = await _eventSC.DeleteEventMealWithItems(eventId);
			return Ok(eventDetail);
		}

	}
}
