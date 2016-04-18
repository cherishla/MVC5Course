using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    /// <summary>
    /// Product 資料的API
    /// </summary>
    public class ProductsApiController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        /// <summary>
        /// 取得所有產品
        /// </summary>
        /// <returns></returns>
        // GET: api/ProductsApi
        public IQueryable<Product> GetProduct()
        {
            return db.Product;
        }

        /// <summary>
        /// 取得單一產品
        /// </summary>
        /// <param name="id">產品id</param>
        /// <returns></returns>
        // GET: api/ProductsApi/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// 更新產品
        /// </summary>
        /// <param name="id">產品id</param>
        /// <param name="product">產品資訊</param>
        /// <returns></returns>
        // PUT: api/ProductsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// 新增產品
        /// </summary>
        /// <param name="product">產品資訊</param>
        /// <returns></returns>
        // POST: api/ProductsApi
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Product.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        /// <summary>
        /// 刪除產品
        /// </summary>
        /// <param name="id">產品id</param>
        /// <returns></returns>
        // DELETE: api/ProductsApi/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Product.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 判斷產品是否存在
        /// </summary>
        /// <param name="id">產品id</param>
        /// <returns></returns>
        private bool ProductExists(int id)
        {
            return db.Product.Count(e => e.ProductId == id) > 0;
        }
    }
}