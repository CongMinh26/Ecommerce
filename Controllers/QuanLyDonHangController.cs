﻿using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        // GET: QuanLyDonHang
        EcommerceEntities db = new EcommerceEntities();
        public ActionResult ChuaThanhToan()
        {
            var listCTT = db.DonDatHang.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(listCTT);
        }
        // đã thanh toán chưa giao
        public ActionResult ChuaGiao()
        {
            var listCG = db.DonDatHang.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan == true).OrderBy(n => n.NgayDat);
            return View(listCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            var listDG_DTT = db.DonDatHang.Where(n => n.DaThanhToan == true && n.TinhTrangGiaoHang == true).OrderBy(n => n.NgayDat);
            return View(listDG_DTT);
        }

        // truyền mã đơn đặt hàng
        [HttpGet]
        public ActionResult DuyetDonHang(int ? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHang.SingleOrDefault(n => n.MaDDH == id);
            if(model == null)
            {
                return HttpNotFound();
            }

            // lấy những sản phẩm đã mua từ đơn đặt hàng ra
            var listCTDDH = db.ChiTietDonDatHang.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = listCTDDH;
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            // truy vấn cập nhật
            DonDatHang ddhUpdate = db.DonDatHang.SingleOrDefault(n => n.MaDDH == ddh.MaDDH);
            // cập nhật lại
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();

            var listCTDDH = db.ChiTietDonDatHang.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = listCTDDH;
            return View(ddhUpdate);
        }
    }
}