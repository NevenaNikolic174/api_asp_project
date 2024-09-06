using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Domain.Entities
{
    public class ProductRestaurant
    {
        public int ProductId { get; set; }
        public int RestaurantId { get; set; } 

        public virtual Product? Product { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
    }
}
