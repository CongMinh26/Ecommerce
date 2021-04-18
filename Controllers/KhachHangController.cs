using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class KhachHangController : Controller
    {
        EcommerceEntities db = new EcommerceEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            var listkh = db.KhachHang;
            return View(listkh);
        }
        // giải phòng vùng nhớ, biến nào ko dùng thì ...
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();

                }
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}