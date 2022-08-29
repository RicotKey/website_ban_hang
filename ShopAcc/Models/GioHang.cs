using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopAcc.Models
{
    public class GioHang
    {
        MyDataDataContext data = new MyDataDataContext();
        public int idacc { get; set; }
        public string idGame { get; set; }
        public string taikhoan   { get; set; }
        public string matkhau { get; set; }
        public Boolean? trangthai { get; set; }
        public DateTime? ngaycapnhat { get; set; }
        public decimal giaban { get; set; }
        public string rank { get; set; }
        public int? tuong { get; set; }
        public int? trangphuc { get; set; }
        public string hinh { get; set; }
        public string idLoai { get; set; }




        public GioHang(int id)
        {
            idacc = id;
            Account acc = data.Accounts.Single(n => n.id == id);
            idGame = acc.LoaiGame.tengame;
            taikhoan = acc.taikhoan;
            matkhau = acc.matkhau;
            trangthai =  acc.trangthai;
            ngaycapnhat = acc.ngaycapnhat;
            giaban = decimal.Parse(acc.giaban.ToString());
            rank = acc.rank;
            tuong = acc.tuong;
            trangphuc = acc.trangphuc;
            hinh = acc.hinh;
            idLoai = acc.LoaiAcc.tenloai;
        }







    }
}