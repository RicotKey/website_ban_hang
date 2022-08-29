using ShopAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopAcc.Controllers
{
    public class UserController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, User us)
        {
            var tendn = collection["tendn"];
            var matkhau = collection["matkhau"];
            var email = collection["email"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var sdt = collection["sdt"];
            var tenhienthi = collection["tenhienthi"];
            var anhdaidien = collection["anhdaidien"];
            var trangthai = Convert.ToBoolean(collection["trangthai"]);
            User checktendn = data.Users.SingleOrDefault(n => n.tendn == tendn);
            Admin checkadmin = data.Admins.SingleOrDefault(n => n.tenDangNhap == tendn && n.matkhau == matkhau);
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập!";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu!";
            }

            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu!";
            }
            else if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
            }

            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi3"] = "Phải nhập email!";
            }
            else if (String.IsNullOrEmpty(sdt))
            {
                ViewData["Loi4"] = "Phải nhập số điện thoại!";
            }
            else if (String.IsNullOrEmpty(tenhienthi))
            {
                ViewData["Loi5"] = "Phải nhập tên hiển thị!";
            }
            else if (String.IsNullOrEmpty(anhdaidien))
            {
                ViewData["Loi6"] = "Phải nhập ảnh đại diện!";
            }

            else
            {
                if (checkadmin != null)
                {
                    ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
                }
                else if (checktendn != null)
                {
                    ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
                }
                else
                {
                    if (!matkhau.Equals(MatKhauXacNhan))
                    {
                        ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                    }
                    else
                    {
                        us.tendn = tendn;
                        us.matkhau = matkhau;
                        us.email = email;
                        us.sdt = sdt;
                        us.tenhienthi = tenhienthi;
                        us.anhdaidien = anhdaidien.ToString();
                        us.trangthai = true;

                        data.Users.InsertOnSubmit(us);
                        data.SubmitChanges();
                        return RedirectToAction("DangNhap");
                    }
                }
            }

            return this.DangKy();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["tendn"];
            var matkhau = collection["matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập!";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu!";
            }
            User us = data.Users.SingleOrDefault(n => n.tendn == tendn && n.matkhau == matkhau && n.trangthai==true);
            Admin checkadmin = data.Admins.SingleOrDefault(n => n.tenDangNhap == collection["tendn"] && n.matkhau == collection["matkhau"]);
            if (us != null)
            {
                
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["TaiKhoan"] = us;

                return RedirectToAction("HomePage", "Account");
            }
            else if(checkadmin != null)
            {

                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["TaiKhoan"] = checkadmin;

                return RedirectToAction("Index1", "Admin");
                
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("HomePage","Account");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            User find = data.Users.FirstOrDefault(p => p.iduser == id);
            return View(find);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection, int id)
        {
            var s = data.Users.First(m => m.iduser == id);

            var matkhau = collection["matkhau"];
            var email = collection["email"];
           
            var sdt = collection["sdt"];
            var tenhienthi = collection["tenhienthi"];
            var anhdaidien = collection["anhdaidien"];
            
          

            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu!";
            }
           

            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi3"] = "Phải nhập email!";
            }
            else if (String.IsNullOrEmpty(sdt))
            {
                ViewData["Loi4"] = "Phải nhập số điện thoại!";
            }
            else if (String.IsNullOrEmpty(tenhienthi))
            {
                ViewData["Loi5"] = "Phải nhập tên hiển thị!";
            }
            else if (String.IsNullOrEmpty(anhdaidien))
            {
                ViewData["Loi6"] = "Phải nhập ảnh đại diện!";
            }
                else
                {
                    s.matkhau = matkhau;
                    s.email = email;
                    s.sdt = sdt;
                    s.tenhienthi = tenhienthi;
                    s.anhdaidien = anhdaidien.ToString();
                        UpdateModel(s);
                        data.SubmitChanges();
                        return RedirectToAction("DangNhap");
                }
            return View(id);
            }
    }
}
