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
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Bio")]
        public string Biography { get; set; }=string.Empty;

        public virtual List<Book>? Books { get; set; }
    }
}
