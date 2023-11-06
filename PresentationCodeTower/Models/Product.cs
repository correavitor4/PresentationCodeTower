using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PresentationCodeTower.Models
{
    public class Product : BaseModel
    {
        public Product(string Name)
        {
            this.Name = Name;
            this.CreatedAt = DateTime.Now;
        }

        [NotNull]
        public string Name { get; set; }
        [NotNull]
        [Range(0.01, 999.99)]
        public decimal Price { get; set; }
    }
}
