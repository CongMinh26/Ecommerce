using Ecommerce.Models;
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
        public ActionResult SanPham(int ? MaLoaiSP, int ? MaNSX)
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
            return View(list_sp);
        }
    }
}