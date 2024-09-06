using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UseCaseName { get; set; }
        public string ActionStatus { get; set; }
        public DateTime ExecutedAt { get; set; }
    }

}
