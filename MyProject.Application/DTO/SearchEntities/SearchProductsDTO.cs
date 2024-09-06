using MyProject.Api.DTO;
using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO.SearchEntities
{
    public class SearchProductsDTO : PageSearch
    {
        public string Name { get; set; }
        public double? PriceMin { get; set; }
        public double? PriceMax { get; set; }
        public OrderByFunc? OrderByName { get; set; }
        public OrderByFunc? OrderByPrice { get; set; }
        public OrderByFunc? OrderByYear { get; set; }
    }


    public enum OrderByFunc
    {
        ASC,
        DESC
    }
}
