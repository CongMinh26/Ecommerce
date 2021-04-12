using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;

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
            var ldt = db.SanPham.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.ListDT = ldt;

            // Lấy list LapTop mới nhất
            var llt = db.SanPham.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.ListLT = llt;

            // Lấy list PC mới nhất
            var lpc = db.SanPham.Where(n => n.MaLoaiSP == 3 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.ListPC = lpc;
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
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}