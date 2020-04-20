using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Routing;

namespace LeaveApplication.Validation_Classes
{/// <summary>
/// Check if the user session live otherwise return to login page
/// </summary>
    public class SessionLive : ActionFilterAttribute
    {
        public bool CheckAdmin { get; set; }
        public bool IsJsonResult { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["EmpId"] == null)
            {
                filterContext.Result = new RedirectResult("/Login");
                JsonCheck(filterContext);

            }
            else if (CheckAdmin)
            {
                Models.Employee e1 = (Models.Employee)HttpContext.Current.Session["Employee"];
                if (e1.isAdmin == false)
                {
                    filterContext.Result = new ContentResult() { Content = "Access denied" };
                    JsonCheck(filterContext);

                }
            }


        }
        public void JsonCheck(ActionExecutingContext filterContext)
        {
            if (IsJsonResult)
            {

                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.End();
            }
        }
    }
}