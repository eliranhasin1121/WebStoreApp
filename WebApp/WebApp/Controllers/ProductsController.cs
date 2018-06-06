using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private WebAppContext db = new WebAppContext();
        //private User usersController = new User();
        static int orderId = 1;
        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Supplier);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "CompanyName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductName,ProductType,cost,ImageURL,Description,Amount,SupplierId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                StorageProducts storage = new StorageProducts(product.ID,product.ProductName,product.Supplier);
                db.StorageProducts.Add(storage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "CompanyName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "CompanyName", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductName,ProductType,cost,ImageURL,Description,Amount,SupplierId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "CompanyName", product.SupplierId);
            return View(product);
        }
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Route("Products/getHotProducts")]
        public ActionResult getHotProducts()
        {
            var hot = (from p in db.Products
                       orderby p.Amount descending
                       select p).Take(6);

            return View(hot.ToList());
          
        }
        [Route("Products/type={type}")]
        public ActionResult ByProductType(string type)
        {
            var productype = from p in db.Products
                              select p;
            if(!String.IsNullOrEmpty(type))
                productype = productype.Where(p => p.ProductType.Equals(type));


            return View(productype.ToList());
        }
        //POST: Product/getHotProducts
        [HttpPost,ActionName("getHotProducts")]
        [ValidateAntiForgeryToken]
        public ActionResult getHotProductsConfirmed()
        {
            var hot = (from p in db.Products
                       orderby p.Amount descending
                       select p).Take(6);
            return RedirectToAction("Index");

        }
        [Route("products/addToCart/{productID}/{user}")]
        public ActionResult addToCart(int productID, string user)

        {
            if(productID>0&&!String.IsNullOrEmpty(user))
            {
                bool flag = false;
                Product product = this.getProductFromDB(productID);
                StorageProducts storage = (from s in db.StorageProducts
                                           where s.productName == product.ProductName
                                           select s).SingleOrDefault<StorageProducts>();
                storage.amount--;

                User us =  (from u in db.Users
                                      where u.UserName == user
                                      select u).SingleOrDefault<User>();//user.getUserFromDB(user);
                if (us != null)
                {
                    foreach(Product p in us.Cart)
                    {
                        if(p.ID==productID)
                        {
                            p.Amount++;
                            flag = true;
                            db.SaveChanges();
                        }
                    }
                    if (!flag)
                    {
                        us.Cart.Add(product);
                        db.SaveChanges();
                    }
                }

                return View(us.Cart.ToList());
                  
            }
           
            return null;
        }

        public double getTotalPrice(User user)
        {
            double total = 0;
            if (user != null)
            {
                foreach(Product p in user.Cart)
                {
                    total += (p.Amount * p.cost);
                }
                return total;
            }
            return 0;
        }
        [Route("products/sendOrder/{userName}")]
        public ActionResult sendOrder(string userName) {
         
            if (!String.IsNullOrEmpty(userName))
            {
                User user = (from u in db.Users
                                        where u.UserName == userName
                                        select u).SingleOrDefault<User>();

                    OrderDetails ord = new OrderDetails();
                ord.Cart = user.Cart;
                ord.OrderID = orderId++;
                ord.userName = user.UserName;
                ord.orderTime=DateTime.Now;
                    ord.total = this.getTotalPrice(user);
                    db.OrderDetails.Add(ord);

                    foreach(Product p in user.Cart.ToList())
                    {
                        user.Cart.Remove(p);
                    }
                 db.SaveChanges();

                return View();
                
            }
             return HttpNotFound();

        }
        public Product getProductFromDB(int productID)
        {
           return (from p in db.Products
             where p.ID == productID
             select p).SingleOrDefault<Product>();
        } 
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
      
    }
}
