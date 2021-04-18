using Ecommerce.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class TimKiemController : Controller
    {
        EcommerceEntities db = new EcommerceEntities();

       
        [HttpGet] // Reload lại 
        public ActionResult KetQuaTimKiem(string TuKhoa, int ? page)
        {
            // phân trang

            if(Request.HttpMethod != "GET") {
                page = 1;
            }
            var PageSize = 6;
            var PageNumber = (page ?? 1);


            // tìm kiếm theo tên sp
            var listSP = db.SanPham.Where(n => n.TenSP.Contains(TuKhoa));
            // lưu lại từ khóa
            ViewBag.TuKhoa = TuKhoa;
            return View(listSP.OrderBy(n=>n.TenSP).ToPagedList(PageNumber,PageSize));
        }
        [HttpPost] // là lúc submit đưa từ khóa lên
        public ActionResult LayTuKhoa(string TuKhoa)
        {
            // gọi đến action KetQuaTimKiem 
            return RedirectToAction("KetQuaTimKiem", new { @TuKhoa = TuKhoa});
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