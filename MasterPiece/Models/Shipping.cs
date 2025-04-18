using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Shipping
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public DateOnly? ShippingDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public string? Status { get; set; }

    public virtual Order? Order { get; set; }
}
