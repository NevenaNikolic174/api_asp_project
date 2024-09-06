using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO.SearchEntities
{
    public class SearchByPaymentMethodDTO : PageSearch
    {
        public string Method { get; set; }
        public OrderByFunction? OrderByMethod { get; set; }
    }

    public enum OrderByFunction
    {
        ASC,
        DESC
    }
}
