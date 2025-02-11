using System;
using System.Collections.Generic;

namespace OpenGLC.Data.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public string? Email { get; set; }

    public string Salt { get; set; } = null!;

    public long UserNumber { get; set; }

    public string Name { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public virtual ICollection<MealEvent> MealEvents { get; set; } = new List<MealEvent>();

    public virtual ICollection<MealItem> MealItems { get; set; } = new List<MealItem>();
}
