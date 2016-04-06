using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ARController : BaseController
    {
        // GET: AR
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 沒有layout的view
        /// </summary>
        /// <returns></returns>
        public ActionResult PartialViewTest()
        {
            //不載入Layout
            return PartialView("Index");
        }

        /// <summary>
        /// 檔案下載(上課範例)
        /// </summary>
        /// <returns></returns>
        //public ActionResult FileTest()
        //{
        //    //加上fileName，就會讓user下載
        //    return File(Server.MapPath("~/Content/Site.css"), "text/css", "我的css.css");
        //}

        /// <summary>
        /// 檔案下載(實作)
        /// </summary>
        /// <returns></returns>
        public ActionResult FileTest(int? dl)
        {
            //因為有些瀏覽器可能會直接下載檔案，所以要判斷一下
            if(dl.HasValue)
                return File(Server.MapPath("~/App_Data/Landscape.jpg"), "image/jpeg", "photo.jpg");
            else
                return File(Server.MapPath("~/App_Data/Landscape.jpg"), "image/jpeg");

        }

        public ActionResult JsonTest(int id)
        {
            //預設沒有get request，會有資安疑慮，所以要加上JsonRequestBehavior.AllowGet
            //避免延遲載入，因為資料序列化導致無窮迴圈
            //repoProduct.UnitOfWork.Context.Configuration.LazyLoadingEnabled = false;
            var product = repoProduct.Find(id);
            return Json(product, JsonRequestBehavior.AllowGet);
            //return Json(new
            //{
            //    id = 1,
            //    name = "Lala"
            //}, JsonRequestBehavior.AllowGet);
        }
    }
}