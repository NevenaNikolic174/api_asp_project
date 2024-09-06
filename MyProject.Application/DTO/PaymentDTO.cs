using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO
{
    public class PaymentDTO : PageSearch
    {
        public string Method { get; set; }
    }
}
