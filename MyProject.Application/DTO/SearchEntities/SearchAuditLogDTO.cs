using MyProject.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.DTO.SearchEntities
{
    public class SearchAuditLogDTO : PageSearch
    {
        public string UserName { get; set; }
        public string UseCaseName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
