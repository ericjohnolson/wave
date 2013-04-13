using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WavePoetry.DataAccess;

namespace WavePoetry.Website
{
    public class AuthControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            if (HttpContext.Current.Request.IsAuthenticated && HttpContext.Current.Session["LoggedInUser"] == null)
            {
                string userName = HttpContext.Current.User.Identity.Name;
                AdminData data = new AdminData();
                HttpContext.Current.Session["LoggedInUser"] = data.GetUserByName(userName);
            }

            return base.CreateController(requestContext, controllerName);
        }
    }
}