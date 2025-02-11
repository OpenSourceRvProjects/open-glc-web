﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Models.Responses
{
	public class MealEventModel
	{
		public Guid Id { get; set; }
		public DateTime EventDate { get; set; }
		public int GlcLevel { get; set; }
		public bool Pospandrial { get; set; }
	}
}
