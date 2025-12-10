using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        [Required]
        public string UserName { get; set; }  // name displayed

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // FK
        public int BookID { get; set; }
        public Book Book { get; set; }
    }
}
