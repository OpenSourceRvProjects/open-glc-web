using System;
using System.Collections.Generic;

namespace OpenGLC.Data.Entities;

public partial class MealItem
{
    public Guid Id { get; set; }

    public string MealName { get; set; } = null!;

    public Guid UserId { get; set; }

    public virtual ICollection<MealEventItem> MealEventItems { get; set; } = new List<MealEventItem>();

    public virtual User User { get; set; } = null!;
}
