using System;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class 計算Action的執行時間Attribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.dtStart = DateTime.Now;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.dtSpan = DateTime.Now - filterContext.Controller.ViewBag.dtStart;
            base.OnActionExecuted(filterContext);
        }
      
    }
}