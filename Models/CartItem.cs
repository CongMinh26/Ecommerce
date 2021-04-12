using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class CartItem
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }

        // Hàm Tạp cart_item
        public CartItem(int iMaSP)
        {
            using(EcommerceEntities db = new EcommerceEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp  = db.SanPham.Single(n => n.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
            }
        }

        public CartItem(int iMaSP, int soluong)
        {
            using (EcommerceEntities db = new EcommerceEntities())
            {
                MaSP = iMaSP;
                SanPham sp = db.SanPham.Single(n => n.MaSP == iMaSP);
                TenSP = sp.TenSP;
                HinhAnh = sp.HinhAnh;
                DonGia = sp.DonGia;
                SoLuong = soluong;
                ThanhTien = DonGia * SoLuong;
            }
        }

    }
}