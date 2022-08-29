using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopAcc.Models
{
    public class DonHang
    {
        MyDataDataContext db = new MyDataDataContext();
        public int maHD { get; set; }
        public string TenUser { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public DonHang(int id)
        {
            ChiTietHD ct = db.ChiTietHDs.First(n => n.maHD == id);
            HoaDon hd = db.HoaDons.First(n => n.maHD == id);
            maHD = id;
            NgayLap = hd.ngaythanhtoan.Value;
            TenUser = ct.TruyCap.User.tendn;
            TongTien = hd.tongtien.Value;
        }
    }
}