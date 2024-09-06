using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Domain.Entities
{
    public class UseCaseUser
    {
        public int UserId { get; set; }
        public int UseCaseUserId { get; set; }

        public virtual User User { get; set; }
    }
}
