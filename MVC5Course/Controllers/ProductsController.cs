using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    [計算Action的執行時間]

    public class ProductsController : BaseController
    {
        //private FabricsEntities db = new FabricsEntities();
        // GET: Products
        public ActionResult Index()
        {
            var data = repoProduct.All(false).Take(5);
            return View(data);
            //return View(db.Product.ToList());
        }

        [HttpPost]
        public ActionResult Index(IList<BatchUpdateProduct> data)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var product = repoProduct.Find(item.ProductId);
                    product.Price = item.Price;
                    product.Active = item.Active;
                    product.Stock = item.Stock;
                }
                repoProduct.UnitOfWork.Commit();


                return RedirectToAction("Index");
            }
            ViewData.Model = repoProduct.All().Take(5);
            return View();
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = repoProduct.Find(id.Value);
            //Product product = db.Product.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);


        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                repoProduct.Add(product);
                repoProduct.UnitOfWork.Commit();
                //db.Product.Add(product);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Product product = db.Product.Find(id);
            Product product = repoProduct.Find(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
            
            //Product product = db.Product.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(product);
        }

        // POST: Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form) //會衝突，所以多加一個form參數，但用不到
        {
            var product = repoProduct.Find(id);

            if (TryUpdateModel(product, new string[] {
                "productId", "ProductName", "Price", "Active", "Stock"}))
            {
                //var dbProduct = (FabricsEntities)repoProduct.UnitOfWork.Context;
                //dbProduct.Entry(product).State = EntityState.Modified;
                repoProduct.UnitOfWork.Commit();
                TempData["ProductsEditMsg"] = product.ProductName + "更新成功";

                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repoProduct.Find(id.Value);
            //Product product = db.Product.Find(id);
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
            // Product product = db.Product.Find(id);

            Product product = repoProduct.Find(id);
            repoProduct.Delete(product);
            repoProduct.UnitOfWork.Commit();

            //db.Product.Remove(product);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoProduct.UnitOfWork.Context.Dispose();
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
