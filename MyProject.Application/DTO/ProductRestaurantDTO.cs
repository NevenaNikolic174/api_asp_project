using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO
{
    public class ProductRestaurantDTO : PageSearch
    {
        public string ProductName { get; set; }   
        public string RestaurantName { get; set; }

        public int ProductId { get; set; }
        public int RestaurantId { get; set; }
    }
}
