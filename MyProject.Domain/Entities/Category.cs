using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Domain.Entities
{
    public class Category : MyEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>(); 
        
    }
}
