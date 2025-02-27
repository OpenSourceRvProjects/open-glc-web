using System;
using System.Collections.Generic;

namespace OpenGLC.Data.Entities;

public partial class PasswordResetRequest
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string Email { get; set; } = null!;

    public Guid UserId { get; set; }

    public bool Status { get; set; }

    public virtual User User { get; set; } = null!;
}
