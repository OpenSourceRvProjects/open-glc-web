using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGLC.Infrastructure.Services;
using OpenGLC.MVC.Filters;

namespace OpenGLC.MVC.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class MealItemsController : ControllerBase
	{
		private readonly ControllerUtilities _utilities;
		private readonly IMealItemService _mealItemSC;
		public MealItemsController(ControllerUtilities utilities, IMealItemService mealSC)
		{
			_utilities = utilities;
			_mealItemSC = mealSC;
		}


		// GET api/<MealItemsController>/5
		[HttpGet]
		[AutomaticExceptionHandler]
		[UserActionFilter]
		public async Task<IActionResult> Get(int page, int itemsPerPage, string searchTerm = "")
		{
			var items = await _mealItemSC.GetMealItems(page, itemsPerPage, searchTerm);
			return Ok(items);
		}

		//// POST api/<MealItemsController>
		//[HttpPost]
		//[Route("addMealItems")]
		//public async Task<IActionResult> Post([FromBody] List<NewMealItemModel> meals)
		//{
		//    var userID =  _utilities.GetUserIdFromRequestContext(HttpContext);
		//    var items =  await _mealItemSC.AddMealItemsFromUserID(userID, meals);
		//    return Ok(items);

		//}
	}
}
