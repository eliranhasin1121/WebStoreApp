using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySingleProject.Models
{
    public class Supplier
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
