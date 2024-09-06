using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Domain.Entities
{
    public class Producer : MyEntity
    {
        public string Name { get; set; }
        public string? Phone { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
