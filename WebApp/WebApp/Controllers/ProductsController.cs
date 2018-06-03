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
        [Route("Products/type={type}")]
        public ActionResult ByProductType(string type)
        {
            var productype = from p in db.Products
                              select p;
            if(!String.IsNullOrEmpty(type))
                productype = productype.Where(p => p.ProductType.Equals(type));


            return View(productype.ToList());
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
        [Route("products/addToCart/{product}/{user}")]
        public ActionResult addToCart(string product, string user)

        {
            if(!String.IsNullOrEmpty(product)&&!String.IsNullOrEmpty(user))
            {
                bool flag = false;
                var prod = (from p in db.Products
                            where p.ProductName == product
                            select p).SingleOrDefault<Product>();
                var us = (from u in db.Users
                          where u.UserName == user
                          select u).SingleOrDefault<User>();
                if (us != null)
                {
                    foreach(Product p in us.Cart)
                    {
                        if(p.ProductName==product)
                        {
                            p.Amount++;
                            flag = true;
                            db.SaveChanges();
                        }
                    }
                    if (!flag)
                    {
                        us.Cart.Add(prod);
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
