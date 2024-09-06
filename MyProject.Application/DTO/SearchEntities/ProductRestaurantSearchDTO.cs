using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO.SearchEntities
{
    public class ProductRestaurantSearchDTO : PageSearch
    {
        public string ProductName { get; set; }
        public string RestaurantName { get; set; }
    }
}
