using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Authors
    {
        [Key]
        public int AuthorID { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Author-Name")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Bio")]
        public string Biography { get; set; }

        public virtual List<Book> Books { get; set; }
    }
}
