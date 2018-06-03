using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MySingleProject.Models
{
    [Table("Products")]
    public class Product
    {

       
        [Key]
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public double cost { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
