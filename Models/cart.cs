using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public class Cart
    {
        // Primary Key - Unique ID for each cart item
        [Key]
        public int CartID { get; set; }                    // ⚠️ Must be CartID (not CartId)

        // Foreign Key to User - Who owns this cart item?
        [Required]
        public int UserID { get; set; }                    // ⚠️ Must be UserID (not CustomerId)

        // Foreign Key to Book - Which book is in cart?
        [Required]
        public int BookID { get; set; }                    // ⚠️ Must be BookID (not BookId)

        // How many copies of this book?
        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }

        // When was this added to cart?
        public DateTime AddedDate { get; set; } = DateTime.Now;

        // NAVIGATION PROPERTIES - Connect to other tables

        // Links to the User who owns this cart item
        [ForeignKey("UserID")]                            // ⚠️ Must be UserID
        public virtual User User { get; set; }            // ⚠️ Must be User (not Customer)

        // Links to the Book details
        [ForeignKey("BookID")]                            // ⚠️ Must be BookID
        public virtual Book Book { get; set; }
    }
}