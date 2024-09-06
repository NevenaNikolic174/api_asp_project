using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO.SearchEntities
{
    public class SearchByUserUsername : PageSearch
    {
        public string Username { get; set; }
        public OrderByFunction? OrderByUsername { get; set; }

        public enum OrderByFunction
        {
            ASC,
            DESC
        }
    }
}
