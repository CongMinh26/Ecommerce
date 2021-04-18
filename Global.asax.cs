using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Ecommerce
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["PageView"] = 0;
            Application["SoNguoiDangOnline"] = 0;

        }
        protected void Session_Start()
        {
            Application.Lock(); // dùng để đồng bộ hóa
            Application["PageView"] = (int)Application["PageView"] + 1; // đếm số th xuất hiện thôi
            Application["SoNguoiDangOnline"] = (int)Application["SoNguoiDangOnline"] + 1; // đếm số th xuất hiện thôi
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock(); // dùng để đồng bộ hóa
            Application["SoNguoiDangOnline"] = (int)Application["SoNguoiDangOnline"] - 1; // đếm số th xuất hiện thôi
            Application.UnLock();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var TaiKhoanCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if(TaiKhoanCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(TaiKhoanCookie.Value); // giải mã
                var roles = authTicket.UserData.Split(new char[] { ','});
                var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name),roles);
                Context.User = userPrincipal;
            }
        }
    }
}
