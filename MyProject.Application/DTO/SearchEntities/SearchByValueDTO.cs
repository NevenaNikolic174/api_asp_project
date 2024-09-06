using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO.SearchEntities
{
    public class SearchByValueDTO : PageSearch
    {
        public string Value { get; set; }
        public OrderByStatus? OrderByValue { get; set; }
    }

    public enum OrderByStatus
    {
        ASC,
        DESC
    }
}
