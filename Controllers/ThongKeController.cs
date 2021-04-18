using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class ThongKeController : Controller
    {
        EcommerceEntities db = new EcommerceEntities();
        // GET: ThongKe
        public ActionResult Index()
        {
            // số người truy cập
            ViewBag.SoNguoiTruyCap = HttpContext.Application["PageView"].ToString() as string;

            // số lượng người đang online 
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();
            return View();
        }

        public decimal TongDoanhThu()
        {
            // từ đầu đến hiện tại
            decimal tongtien = db.ChiTietDonDatHang.Sum(n => n.SoLuong * n.DonGia).Value;
            return tongtien;
        }
        public decimal TongDoanhThuTheoThang(int thang, int nam)
        {
            // tổng doanh thu theo tháng
            // list ra những đơn đặt hàng nào có tháng, năm tương ứng
            var listDDH = db.DonDatHang.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam);
            decimal tongtien = 0;
            // duyệt chi tiết của đơn hàng đó
            foreach(var item in listDDH)
            {
                tongtien = db.ChiTietDonDatHang.Sum(n => n.SoLuong * n.DonGia).Value;
            }
            return tongtien;
        }

        // Thống kê đơn hàng
        public double TongDonHang()
        {
            double SumDonHang = db.DonDatHang.Count();
            return SumDonHang;
        }

        // Thống kê Thành viên
        public double TongThanhVien()
        {
            double SumTV = db.ThanhVien.Count();
            return SumTV;
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