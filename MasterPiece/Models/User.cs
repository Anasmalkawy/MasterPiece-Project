using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public int? Phone { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Productshop> Productshops { get; set; } = new List<Productshop>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
