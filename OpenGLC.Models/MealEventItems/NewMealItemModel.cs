using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Models.MealEventItems
{
	public class NewMealItemModel
	{
		public string Name { get; set; }
	}

	public class NewMealItemModelDB
	{
		public string Name { get; set; }
		public Guid ID { get; set; }
		public int Quantity { get; set; }
	}
}
