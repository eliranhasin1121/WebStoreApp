using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderID { get; set; }
        public string userName { get; set; }
        public ICollection<Product> Cart { get; set; }
        public DateTime orderTime { get; set; }
        public double total { get; set; }
    }
}