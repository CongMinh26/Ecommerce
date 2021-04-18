using Ecommerce.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class SanPhamController : Controller
    {
        EcommerceEntities db = new EcommerceEntities();
      
       
        // GET: SanPham

        [ChildActionOnly] // ko cho ng dùng post
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }

        // Chi tiết sản phẩm
        public ActionResult ChiTietSanPham(int ? id)
        {
            // kiểm tra tham số đầu vào
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if(sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }


        // Xây dựng Acction load Sản Phẩm theo Mã Loại SX, Mã nhà sản xuất
        [HttpGet]
        public ActionResult SanPham(int ? MaLoaiSP, int ? MaNSX, int ? page)
        {
            if( MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list_sp = db.SanPham.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if(list_sp.Count() == 0)
            {
                return HttpNotFound();
            }

            // Thực hiện chức năng phân trang
            if(Request.HttpMethod != "GET")
            {
                page = 1;
            }
            // tạo biến sp trên trang
            int pageSize = 6;
            // tạo biến số trang hiện tại
            int pageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;

            return View(list_sp.OrderBy(n=>n.MaSP).ToPagedList(pageNumber, pageSize));
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