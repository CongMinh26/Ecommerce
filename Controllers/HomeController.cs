using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        EcommerceEntities db = new EcommerceEntities();
        // GET: Home
        public ActionResult Index()
        {
            // Lấy dữ liệu gán cho viewbag.
            // Lấy list điện thoại mới nhất
            ViewBag.ListDT = db.SanPham.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false).Take(6);
        
            
            // Lấy list LapTop mới nhất
            ViewBag.ListLT = db.SanPham.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false).Take(4);
             

            // Lấy list PC mới nhất
           /* var lpc = db.SanPham.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.ListPC = lpc;*/
            return View();
        }

        // Menu 
        public ActionResult MenuPartial()
        {
            var listMenu = db.SanPham;
            return PartialView(listMenu);
        }

        // Đăng ký. chạy lên đầu tiên
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauhoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauhoi());
            // kiểm tra cú pháp captcha hợp lệ
            if (this.IsCaptchaValid("Captcha is not Valid"))
            {
                ViewBag.ThongBao = "Đăng ký thành viên thành công";
                db.ThanhVien.Add(tv);
                db.SaveChanges();
                return View();
            }
            ViewBag.ThongBao = "Sai Mã CaptCha";
            return View();
        }


        // load câu hỏi bí mật
        public List<string> LoadCauhoi()
        {
            var listCH = new List<string>();
            listCH.Add("Bạn thích con gì?");
            listCH.Add("Bạn có thích bóng đá không?");
            listCH.Add("Thu nhập 1 tháng của bạn có trên 100 triệu chưa?");
            return listCH;
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
            var tk = form["TaiKhoan"].ToString();
            var mk = form["MatKhau"].ToString();
            ThanhVien tv = db.ThanhVien.SingleOrDefault(n => n.TaiKhoan == tk && n.MatKhau == mk);
            if(tv != null)
            {
                Session["TaiKhoan"] = tv;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");

           /* var tk = form["TaiKhoan"].ToString();
            var mk = form["MatKhau"].ToString();
            ThanhVien tv = db.ThanhVien.SingleOrDefault(n => n.TaiKhoan == tk && n.MatKhau == mk);
            if (tv != null)
            {
                // lấy ra list quyên của thành viên
                var listQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                string Quyen = "";
                foreach(var item in listQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                // cắt list quyên của thành viên(bỏ dấu ,)
                Quyen = Quyen.Substring(0,Quyen.Length-1);
                // phân quyền
                PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
            }


                return RedirectToAction("Index", "Home");*/
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        // Phân Quyền
        public void PhanQuyen(string TaiKhoan,string Quyen)
        {
            FormsAuthentication.Initialize(); // khởi tạo

            var ticket = new FormsAuthenticationTicket(1,
                TaiKhoan, // user
                DateTime.Now, // begin
                DateTime.Now.AddHours(5), // time out
                false, // remember
                Quyen, // permission
                FormsAuthentication.FormsCookiePath
                );
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,FormsAuthentication.Encrypt(ticket));
            Response.Cookies.Add(cookie);
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

            //Response là gửi từ client đến server
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