using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            ViewBag.MaNCC = db.NhaCungCap;
            ViewBag.ListSP = db.SanPham;

            // thêm phiếu nhập
            model.DaXoa = false;
            db.PhieuNhap.Add(model);
            db.SaveChanges();


            SanPham sp;
            // thêm chi tiết phiếu nhập
            foreach(var item in listModel)
            {
                item.MaPN = model.MaPN;

                // cập nhật số lượng tồn cho sản phẩm
                sp = db.SanPham.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;
            }
            db.ChiTietPhieuNhap.AddRange(listModel);
            db.SaveChanges();
            return View();
        }

        public ActionResult ListSanPhamGanHet()
        {
            var listSPHH = db.SanPham.Where(n => n.DaXoa == false && n.SoLuongTon <= 10);

            return View(listSPHH);
        }


        // Chi tiết Sản Phẩm gần hết hàng
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        // nhập hàng cho sp gần hết
        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, ChiTietPhieuNhap chiTietPhieuNhap)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n=>n.TenNCC),"MaNCC","TenNCC");
            model.NgayNhap = DateTime.Now;
            model.DaXoa = false;
            db.PhieuNhap.Add(model);
            db.SaveChanges();

            chiTietPhieuNhap.MaPN = model.MaPN;

            // cập nhật số lượng tồn cho sản phẩm
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == chiTietPhieuNhap.MaSP);
            sp.SoLuongTon += chiTietPhieuNhap.SoLuongNhap;

            db.ChiTietPhieuNhap.Add(chiTietPhieuNhap);
            db.SaveChanges();


            return View(sp);
        }


        // giải phòng vùng nhớ
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