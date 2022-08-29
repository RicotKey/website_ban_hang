using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models;
using ShopAcc.Mail;
using Newtonsoft.Json.Linq;
using ShopAcc.MoMo;
using System.Configuration;

namespace ShopAcc.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        MyDataDataContext data = new MyDataDataContext();
        public List<GioHang> Laygiohang()
        {

            List<GioHang> lstGiohang = new List<GioHang>();
            User user = (User)Session["TaiKhoan"];
            var truycap = data.TruyCaps.Where(p => p.iduser == user.iduser && p.Account.trangthai==true);
            foreach (var item in truycap)
            {
                GioHang sanpham = new GioHang(item.id);
                lstGiohang.Add(sanpham);
            }
            ViewBag.Tongsoluong = lstGiohang.Count;
            ViewBag.Tongtien = lstGiohang.Sum(n => n.giaban);
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            User user = (User)Session["TaiKhoan"];
            var acc = data.TruyCaps.Where(p => p.id == id && p.iduser ==user.iduser).FirstOrDefault();
            if (acc == null)
            {
                List<GioHang> lstGiohang = new List<GioHang>();
                TruyCap truyCap = new TruyCap();
                truyCap.iduser = user.iduser;
                truyCap.id = id;
                truyCap.trangthai = true;
                data.TruyCaps.InsertOnSubmit(truyCap);
                data.SubmitChanges();
                return Redirect(strURL);
            }
            else
                return Redirect(strURL);
        }
       
        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = lstGiohang.Count;
            ViewBag.Tongtien = lstGiohang.Sum(n => n.giaban);
            return View(lstGiohang);
        }
        public ActionResult GioHangPartial()
        {
            return PartialView();
        }
        public ActionResult XoaAcc(int id)
        {
            List<GioHang> lstGiohang = Laygiohang();
            User user = (User)Session["TaiKhoan"];
            var truycap = data.TruyCaps.Where(p => p.id == id && p.iduser==user.iduser).First();
            if (truycap != null)
            {
                data.TruyCaps.DeleteOnSubmit(truycap);
                data.SubmitChanges();
            }

            return RedirectToAction("GioHang");

        }
        public ActionResult XoaAll()
        {

            User user = (User)Session["TaiKhoan"];

            var truycap = data.TruyCaps.Where(p => p.iduser == user.iduser);
            foreach (var item in truycap)
            {
                data.TruyCaps.DeleteOnSubmit(item);
            }
            data.SubmitChanges();

            return RedirectToAction("GioHang");
        }



        [HttpGet]
        public ActionResult DatHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = lstGiohang.Count;
            ViewBag.Tongtien = lstGiohang.Sum(n => n.giaban);
            if (lstGiohang == null )
            {
                return RedirectToAction("HomePage", "Account");
            }
           
            return View(lstGiohang);
        }
       
        //public ActionResult Payment()
        //{
        //    var lstGioHang = Laygiohang();
        //    User user = (User)Session["TaiKhoan"];
        //    var total = lstGioHang.Sum(n => n.giaban);

        //     string detail = "";

        //    foreach (var item in lstGioHang)
        //    {
        //        detail += "Tài khoản:  " + item.taikhoan.ToString() + "" + "Mật khẩu:  " + item.matkhau.ToString() + "=======================";
                
        //    }

        //    string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/neworder.html"))+detail;

        //    content = content.Replace("{{CustomerName}}", user.tenhienthi);
        //    content = content.Replace("{{Phone}}", user.sdt);
        //    content = content.Replace("{{Email}}", user.email);
        //    content = content.Replace("{{Address}}", user.anhdaidien);
        //    content = content.Replace("{{Total}}", total.ToString("N0"));
        //    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            
        //    new MailHelper().SendEmail(user.email, "Xác nhận mua Acc", content);
        //    new MailHelper().SendEmail(toEmail, "Xác nhận mua Acc", content);
        //    return RedirectToAction("GioHang");

        //}
        //public ActionResult Payment()
        //{
        //    var lstGioHang = Laygiohang();
        //    User user = (User)Session["TaiKhoan"];
        //    var total = lstGioHang.Sum(n => n.giaban);

        //    string detail = "";

        //    foreach (var item in lstGioHang)
        //    {
        //        detail += "Tài khoản:  " + item.taikhoan.ToString() + "" + "Mật khẩu:  " + item.matkhau.ToString() + "=======================";

        //    }

        //    string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/neworder.html")) + detail;


        //    var hoadon = new HoaDon();
        //    hoadon.ngaythanhtoan = DateTime.Today;
        //    hoadon.thanhtoan = false;
        //    hoadon.tongtien = (Decimal)total;
        //    try
        //    {
        //        data.HoaDons.InsertOnSubmit(hoadon);
        //        data.SubmitChanges();

        //        foreach (var item in lstGioHang)
        //        {
        //            var cthd = new ChiTietHD();
        //            cthd.iduser = user.iduser;
        //            cthd.id = item.idacc;
        //            cthd.maHD = hoadon.maHD;
        //            cthd.gia = item.giaban;

        //            data.ChiTietHDs.InsertOnSubmit(cthd);
        //            data.SubmitChanges();
        //        }

        //        //var chitiet = data.ChiTietHDs.ToList();
        //        //var acc = data.Accounts.ToList();
        //        content = content.Replace("{{CustomerName}}", user.tenhienthi);
        //        content = content.Replace("{{Phone}}", user.sdt);
        //        content = content.Replace("{{Email}}", user.email);
        //        content = content.Replace("{{Total}}", hoadon.tongtien.ToString());
        //        foreach (var item in lstGioHang)
        //        {
        //            content = content.Replace("{{TaiKhoan}}", item.taikhoan);
        //            content = content.Replace("{{MatKhau}}", item.matkhau);
        //        }
        //        var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();


        //        new MailHelper().SendEmail(user.email, "Xác nhận mua Acc", content);
        //        new MailHelper().SendEmail(toEmail, "Xác nhận mua Acc", content);
        //    }
        //    catch (Exception)
        //    {
        //        return Redirect("/Cart/UnSuccess");
        //    }
        //    return Redirect("/Cart/Success");
        //}
        public ActionResult PaymentMoMo()
        {
            var lstGioHang = Laygiohang();
            User user = (User)Session["TaiKhoan"];
            var total = lstGioHang.Sum(n => n.giaban);

            string detail = "";

            foreach (var item in lstGioHang)
            {
                detail += "Tài khoản:  " + item.taikhoan.ToString() + "------" + "Mật khẩu:  " + item.matkhau.ToString() + "=======================";

            }

            string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/neworder.html")) + detail;
            foreach (var item in lstGioHang)
            {
                var acc = data.Accounts.Where(p => p.id == item.idacc).First();
                if(acc!= null)
                {
                    acc.trangthai = false;
                    UpdateModel(acc);
                    data.SubmitChanges();

                }    
            }
           

            
            data.SubmitChanges();
            var hoadon = new HoaDon();
            hoadon.ngaythanhtoan = DateTime.Today;
            hoadon.thanhtoan = true;
            hoadon.tongtien = (Decimal)total;
            try
            {
                data.HoaDons.InsertOnSubmit(hoadon);
                data.SubmitChanges();

                foreach (var item in lstGioHang)
                {
                    var cthd = new ChiTietHD();
                    cthd.iduser = user.iduser;
                    cthd.id = item.idacc;
                    cthd.maHD = hoadon.maHD;
                    cthd.gia = item.giaban;

                    data.ChiTietHDs.InsertOnSubmit(cthd);
                    data.SubmitChanges();

                }
               
                //var chitiet = data.ChiTietHDs.ToList();
                //var acc = data.Accounts.ToList();
                content = content.Replace("{{CustomerName}}", user.tenhienthi);
                content = content.Replace("{{Phone}}", user.sdt);
                content = content.Replace("{{Email}}", user.email);
                content = content.Replace("{{Total}}", hoadon.tongtien.ToString());
                foreach (var item in lstGioHang)
                {
                    content = content.Replace("{{TaiKhoan}}", item.taikhoan);
                    content = content.Replace("{{MatKhau}}", item.matkhau);
                }
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();


                new MailHelper().SendEmail(user.email, "Xác nhận mua Acc", content);
                new MailHelper().SendEmail(toEmail, "Xác nhận mua Acc", content);
                
            }

            catch (Exception)
            {
                return Redirect("/Cart/UnSuccess");
            }


            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOGZWY20220330";
            string accessKey = "4eqlDd5BfFaIWFPx";
            string serectkey = "V3ADky2O30vGqicMY6mnnqI9jiRcKeaz";
            string orderInfo = "Thanh toán Acc game";
            string returnUrl = "https://localhost:44355/Home/ConfirmPaymentClient";
            string notifyurl = "http://ba1adf48beba.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = lstGioHang.Sum(p=>p.giaban).ToString();
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());

        }


        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i

        public ActionResult ReturnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string secretkey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string signature = crypto.signSHA256(param, secretkey);
            if(signature != Request["signature"].ToString())
            {
                ViewBag.message = "Thông tin request không hợp lệ";
            }
            if (!Request.QueryString["errorCode"].Equals("0")) 
            {
                ViewBag.message = "Thanh toán thành công";

            }
            else
            {
                ViewBag.message = "Thanh toán thành công";
                
            }
            return View();
        }
        

        public ActionResult ConfirmPaymentClient()
        {
            //hiển thị thông báo cho người dùng
            return View();
        }

        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}