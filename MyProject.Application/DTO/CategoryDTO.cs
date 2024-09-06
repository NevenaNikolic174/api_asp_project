using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO
{
    public class CategoryDTO : PageSearch
    {
        public string Name { get; set; }
    }
}
