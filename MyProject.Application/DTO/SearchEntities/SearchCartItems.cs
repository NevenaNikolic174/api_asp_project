using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO.SearchEntities
{
    public class SearchCartItems : PageSearch
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
