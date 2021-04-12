using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{

    public class GioHangController : Controller
    {
        EcommerceEntities db = new EcommerceEntities();
        // GET: GioHang
        public ActionResult XemGioHang()
        {
            List<CartItem> listGH = LayGioHang();
            return View(listGH);
        }


        // Lấy Giỏ Hàng
        public List<CartItem> LayGioHang()
        {
            //kiểm tra giỏ hàng đã tồn tại trong session chưa
            List<CartItem> listGH = Session["GioHang"] as List<CartItem>;
            if (listGH == null)
            {
                // null thì tạo mới, xong gán lại cho session
                listGH = new List<CartItem>();
                Session["GioHang"] = listGH;

            }
            // có thì trả về thôi
            return listGH;
        }

        // Thêm giỏ hàng
        public ActionResult ThemGioHang(int MaSP, string url)
        {
            // kiểm tra xem sp có hợp lệ ko
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // lấy giỏ hàng
            List<CartItem> listGH = LayGioHang();

            // kiểm tra xem sp trong giỏ hàg đã tồn tại hay chưa
            CartItem spCheck = listGH.SingleOrDefault(n => n.MaSP == MaSP);

            // nếu sp đã có trong gh
            if (spCheck != null)
            {
                // kiểm tra số lượng tồn chứ
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;

                // tinh thanh tien
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(url);
            }

            // nếu chưa có
            // thì tạo đối tượng item new thôi
            CartItem item = new CartItem(MaSP);
            // nếu sp chưa có trông giỏ hàng
            if (sp.SoLuongTon < item.SoLuong)
            {
                return View("Thôngbáo");
            }
            listGH.Add(item);
            return Redirect(url);

        }

        // Tính tổng số lượng của giỏ hàng
        public double TongSoLuong()
        {
            var listGH = Session["GioHang"] as List<CartItem>;
            if (listGH == null)
            {
                return 0;
            }

            return listGH.Sum(n => n.SoLuong);
        }
        // Tính tổng tiền của giỏ hàng
        public decimal TongTien()
        {
            List<CartItem> listGH = Session["GioHang"] as List<CartItem>;
            if (listGH == null)
            {
                return 0;
            }
            return listGH.Sum(n => n.ThanhTien);
        }

        // gán sum soluong va tien cho viewbag truyển qua view
        public ActionResult IconGioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return PartialView();
        }



        // SỬA GIỎ HÀNG
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            // kiểm tra giỏ hàng đã tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiểm tra xem sp có tồn tại trong database ko
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // lấy giỏ hàng
            List<CartItem> listGH = LayGioHang();

            // kiểm tra xem sp trong giỏ hàg đã tồn tại hay chưa
            CartItem spCheck = listGH.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // truyền giỏ hàng ra view
            ViewBag.GioHang = listGH;
                return View(spCheck);
        }

        [HttpPost]
        // nhận vào 1 đối tượng cartitem() thay vì MaSP với soluong
        // MaSP đc gửi từ view qua
        public ActionResult CapNhatGioHang(int ? MaSP, FormCollection f)
        {
            ////// kiểm tra số lượng tồn
            SanPham spcheck = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
          if(spcheck.SoLuongTon < int.Parse(f["SoLuong"].ToString()))
            {
             return View("ThongBao");
             }
            //lấy giỏ hàmh từ session
            var listGH = LayGioHang();
            //lấy sp cần cập nhật trong giỏ hàng ra
            CartItem cartItemUpdate = listGH.Find(n => n.MaSP == MaSP);
            //// cập nhật
            cartItemUpdate.SoLuong = int.Parse(f["SoLuong"].ToString());
           cartItemUpdate.ThanhTien = int.Parse(f["SoLuong"].ToString()) * spcheck.DonGia;

            // cập nhật số lượng
            return RedirectToAction("XemGioHang");
        }


        // Xóa Giỏ Hàng
        public ActionResult XoaGioHang(int? MaSP)
        {
            // kiểm tra giỏ hàng đã tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiểm tra xem sp có tồn tại trong database ko
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // lấy giỏ hàng
            List<CartItem> listGH = LayGioHang();

            // kiểm tra xem sp trong giỏ hàg đã tồn tại hay chưa
            CartItem spCheck = listGH.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            listGH.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }

        //Đặt Hàng
        public ActionResult DatHang()
        {
            // Đảm bảo đã có hàng để đặt
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.UuDai = 0;
            ddh.DaThanhToan = false;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHang.Add(ddh);
            db.SaveChanges();
            // thêm chi tiết đơn hàng
            var listGH = LayGioHang();
            foreach (var item in listGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.DonGia = item.DonGia;
                ctdh.SoLuong = item.SoLuong;
                db.ChiTietDonDatHang.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }



        // Thêm giỏ hàng bằng Ajax
        public ActionResult ThemGioHangAjax(int MaSP, string url)
        {
            // kiểm tra xem sp có hợp lệ ko
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // lấy giỏ hàng
            var listGH = LayGioHang();

            // kiểm tra xem sp trong giỏ hàg đã tồn tại hay chưa
            CartItem spCheck = listGH.SingleOrDefault(n => n.MaSP == MaSP);

            // nếu sp đã có trong gh
            if (spCheck != null)
            {
                // kiểm tra số lượng tồn chứ
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</scrip>");
                }
                spCheck.SoLuong++;

                // tinh thanh tien
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TongTien();
                return PartialView("IconGioHangPartial");
            }

            // nếu chưa có
            // thì tạo đối tượng item new thôi
            CartItem item = new CartItem(MaSP);
            // nếu sp chưa có trông giỏ hàng
            if (sp.SoLuongTon < item.SoLuong)
            {
                return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</scrip>");
            }
            listGH.Add(item);
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView("IconGioHangPartial");
        }
    } 
}