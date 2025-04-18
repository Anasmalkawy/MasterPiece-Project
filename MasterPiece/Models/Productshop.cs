using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Productshop
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Address { get; set; }

    public string? Productname { get; set; }

    public int? Productprice { get; set; }

    public string? Productimg { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public string? Street { get; set; }

    public int? Phone { get; set; }

    public DateOnly? ShippingDate { get; set; }

    public string? Status { get; set; }

    public virtual User? User { get; set; }
}
