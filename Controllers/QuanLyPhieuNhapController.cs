using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        EcommerceEntities db = new EcommerceEntities();
        // GET: QuanLyPhieuNhap
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCap;
            ViewBag.ListSP = db.SanPham;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> listModel )
        {
            return View();
        }
    }
}