using System;
using System.Collections.Generic;

namespace OpenGLC.Data.Entities;

public partial class MealEventItem
{
    public Guid Id { get; set; }

    public Guid MealEventId { get; set; }

    public string Description { get; set; } = null!;

    public Guid MealItemId { get; set; }

    public int Unit { get; set; }

    public virtual MealEvent MealEvent { get; set; } = null!;

    public virtual MealItem MealItem { get; set; } = null!;
}
