using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();

    public virtual User? User { get; set; }
}
