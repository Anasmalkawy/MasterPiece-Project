using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? PaymentMethod { get; set; }

    public int? Amount { get; set; }

    public string? Status { get; set; }

    public virtual Order? Order { get; set; }
}
