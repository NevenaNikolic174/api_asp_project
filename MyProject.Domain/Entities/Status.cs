using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Domain.Entities
{
    public class Status : MyEntity
    {
        public string Value { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
