using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class ProductDetails
    {
        [Key]
        public int Id { get; set; } 

        public int ProductId { get; set; }   


        [Required]
        [StringLength(30)]
        public string Color { get; set; }

        [Required]
        [StringLength(30)]
        public string Qty { get; set; }

        [Required]
        [StringLength(30)]
        public string Images { get; set; }

        [Required]
        public double Price { get; set; }

  
    }

}
