using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Domain.Entities
{
    public class Restaurant : MyEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string? Image { get; set; }   

        public virtual ICollection<ProductRestaurant> ProductRestaurants { get; set; } = new HashSet<ProductRestaurant>();  
    }
}
