using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class Test1Controller : Controller
    {
        //Object Service
        FabricsEntities db = new FabricsEntities();

        // GET: Test1
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 顯示Create資料
        /// </summary>
        /// <returns></returns>
        public ActionResult EDE()
        {
            return View();
        }

        /// <summary>
        /// 當Create後，再把資料帶回表單
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EDE(EdeViewModel data)
        {
            //當傳入資料時，表單會帶回輸入資料
            return View(data);
        }

        /// <summary>
        /// 建立資料(Create)
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateProduct()
        {
            //將 product 以物件方式進行塞資料
            var product = new Product()
            {
                ProductId = 99999,
                ProductName = "Tercel",
                Active = true,
                Price = 1999,
                Stock = 5
            };

            //寫入DB
            db.Product.Add(product);
            db.SaveChanges();
            return View(product);
        }

        /// <summary>
        /// 取得資料(List)
        /// </summary>
        /// <returns></returns>
        public ActionResult ReadProduct(bool? Active)
        {
            //取得所有資料
            //var data = db.Product;
            //將查詢結果先轉為IQueryable，會更便利於查詢
            var data = db.Product.AsQueryable();
            //篩選資料並排序
            data = data.Where(p=> p.ProductId >1550).OrderByDescending(p=> p.Price);
            //如果Active 有值，就在加入一個判斷
            if (Active.HasValue)
            {
                data = data.Where(p => p.Active == Active);
            }
            return View(data);
        }

        /// <summary>
        /// 顯示明細(Details)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OneProduct(int id)
        {
            //要有PK
            var data = db.Product.Find(id);
            //其他寫法
            //var data = db.Product.FirstOrDefault(p => p.ProductId == id);
            //var data = db.Product.Where(p => p.ProductId == id).FirstOrDefault();
            return View(data);
        }

        /// <summary>
        /// 將金額更新為金額x2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UpdateProduct(int id)
        {
            var one = db.Product.FirstOrDefault(p => p.ProductId == id);

            if (one == null)
            {
                return HttpNotFound();
            }

            one.Price = one.Price * 2;

            //每一個SaveChanges都是一個交易，當其中一筆發生例外的時候，會全部rollback
            //所以SaveChanges做一次就好
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityError in ex.EntityValidationErrors)
                {
                    foreach (var err in entityError.ValidationErrors)
                    {
                        return Content(err.PropertyName +": " + err.ErrorMessage);
                    }
                }
            }
            return RedirectToAction("ReadProduct");
        }

        /// <summary>
        /// 刪除一筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var one = db.Product.Find(id);
            //因為可能會有foreign key，所以要刪掉關聯
            //寫法一
            //foreach (var item in one.OrderLine.ToList())
            //{
            //    db.OrderLine.Remove(item);
            //}
            //寫法二
            //會一筆一筆刪掉
            //db.OrderLine.RemoveRange(one.OrderLine);
            //寫法三
            //直接將資料全部刪掉
            //@p0為避免SQL 攻擊，固定要用@p0、@p1..........
            db.Database.ExecuteSqlCommand(@"DELETE FROM dbo.OrderLine WHERE ProductId=@p0)", id);

            //刪除資料

            db.Product.Remove(one);

            //只要寫一次就好
            db.SaveChanges();

            return RedirectToAction("ReadProduct");
        }

        /// <summary>
        /// SQL 查詢(為了效能調教)
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductView()
        {
            //使用這個方式會不能用導覽屬性
            var data = db.Database.SqlQuery<ProductViewModel>(
                @"SELECT * FROM dbo.Product WHERE Active = @p0 
                    AND ProductName like @p1", true, "%Yellow%");

            return View(data);
        }

        /// <summary>
        /// 使用預存程序來抓資料
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductSP()
        {
            //直接呼叫預存程序即可
            var data = db.GetProduct(true, "%Yellow%");
            return View(data);
        }

        public ActionResult Edit(int id)
        {
            var one = db.Product.FirstOrDefault(p => p.ProductId == id);

            if (one == null)
            {
                return HttpNotFound();
            }
            return View(one);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ReadProduct");
            }
            return View(product);
        }
    }
    
}