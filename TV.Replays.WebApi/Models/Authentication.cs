using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TV.Replays.WebApi.Models
{
    public class AuthenticationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies == null || filterContext.HttpContext.Request.Cookies["Account"] == null)
                filterContext.Result = new RedirectResult("~/account/index");
        }
    }
}