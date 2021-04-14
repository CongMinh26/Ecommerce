using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        EcommerceEntities db = new EcommerceEntities();
        // GET: QuanLySanPham
        public ActionResult Index()
        {

            return View(db.SanPham.Where(n => n.DaXoa == false));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {

            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.Ten), "MaNSX", "Ten");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");

            return View();
        }
        [ValidateInput(false)]
        [HttpPost] // created
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.Ten), "MaNSX", "Ten");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");

            // kiểm tra hình ảnh
            if (HinhAnh.ContentLength > 0)
            {
                // lấy tên hình
                var fileName = Path.GetFileName(HinhAnh.FileName);
                // chuyển hình ảnh vào thư mục images
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình ảnh đã tồn tại";
                }
                else
                {
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = fileName;
                }

            }
            db.SanPham.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}