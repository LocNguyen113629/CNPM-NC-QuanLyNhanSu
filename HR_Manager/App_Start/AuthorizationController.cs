using HR_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HR_Manager.App_Start
{
    public class AuthorizationController : AuthorizeAttribute
    {
        HRManagerEntities4 db = new HRManagerEntities4();
        // GET: Authorization
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            NhanVien nvSession = (NhanVien)HttpContext.Current.Session["user"];
            if (nvSession == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Login" }));
            }
            return;
        }
    }
}