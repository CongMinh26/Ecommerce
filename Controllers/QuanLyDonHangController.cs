using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

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

       // phương thức gửi mail
        public void GuiMail(string title, string ToEmail, string FromEmail, string Pass,string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);
            mail.From = new MailAddress(ToEmail);
            mail.Subject = title;
            mail.Body = Content;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(FromEmail, Pass);
            smtp.EnableSsl = true;
            smtp.Send(mail);
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