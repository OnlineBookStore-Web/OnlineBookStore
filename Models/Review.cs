using OnlineBookStore.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class Review
{
    [Key]
    public int ReviewID { get; set; }

    [Required]
    public int BookID { get; set; }
    public Book Book { get; set; }

    [Required]
    [StringLength(100)]
    public string UserName { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [StringLength(1000)]
    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
