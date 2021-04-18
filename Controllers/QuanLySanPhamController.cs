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

        [HttpGet]
        public ActionResult ChinhSua(int ? id)
        {
            if(id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id); //SingleOrDefault nếu ko có thì = null
            if(sp == null)
            {
              
                return HttpNotFound();
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC",sp.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.Ten), "MaNSX", "Ten",sp.MaNSX);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai",sp.MaSP);

            return View(sp);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham sp, HttpPostedFileBase HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.Ten), "MaNSX", "Ten", sp.MaNSX);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai", sp.MaSP);
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
                    db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            
            return View(sp);
        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id); //SingleOrDefault nếu ko có thì = null
            if (sp == null)
            {

                return HttpNotFound();
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.Ten), "MaNSX", "Ten", sp.MaNSX);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai", sp.MaSP);

            return View(sp);
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