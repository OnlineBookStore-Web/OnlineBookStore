using OnlineBookStore.Models;
using System;
using System.ComponentModel.DataAnnotations;


// Models/CartItem.cs
public class CartItem
{
    public int BookID { get; set; }
    public string BookTitle { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Price * Quantity;
}

public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    public decimal CartTotal => Items.Sum(i => i.Total);
}
