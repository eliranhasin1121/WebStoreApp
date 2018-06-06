using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class StorageProducts
    {
        [Key]
        public int ProductID { get; set; }
        public string productName { get; set; }
        public DateTime lastOrder { get; set; }
        public virtual Supplier Supplier { get; set; }
        public int amount { get; set; }

        public StorageProducts() {}
       public StorageProducts(int prodId,string productName,Supplier supplier)
        {
            this.ProductID = prodId;
            this.productName = productName;
            this.Supplier = supplier;
            this.amount = 100;
            this.lastOrder = DateTime.Now;
        }

    }
}