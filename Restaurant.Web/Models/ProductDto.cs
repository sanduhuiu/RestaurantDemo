using System.ComponentModel.DataAnnotations;

namespace Restaurant.Web.Models
{ 
    public class ProductDto
    {
        public ProductDto()
        {
            Count = 1;
        }

        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 1000)]
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        [Range(1,100)]
        public int Count { get; set; }  
    }
}
