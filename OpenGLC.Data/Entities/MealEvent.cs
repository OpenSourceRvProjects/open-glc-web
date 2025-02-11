using System;
using System.Collections.Generic;

namespace OpenGLC.Data.Entities;

public partial class MealEvent
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime MealDate { get; set; }

    public DateTime CreationDate { get; set; }

    public int GlcLevel { get; set; }

    public string? Notes { get; set; }

    public string MealAtDay { get; set; } = null!;

    public virtual ICollection<MealEventItem> MealEventItems { get; set; } = new List<MealEventItem>();

    public virtual User User { get; set; } = null!;
}
