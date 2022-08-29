using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopAcc.Models;
namespace ShopAcc.Controllers
{
    public class AdminController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();

        // GET: Admin/Admin
        public ActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var all_account = from tt in db.Accounts
                              where tt.trangthai == true
                              select tt;
            return View(all_account.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index1()
        {
            var listacc = db.Accounts.Where(p => p.trangthai == true).ToList();
            ViewBag.tongaccount = listacc.Count;
            var listuser = db.Users.Where(p => p.trangthai == true).ToList();
            ViewBag.tonguser = listuser.Count;
            var listhd = db.HoaDons.Where(p => p.thanhtoan == true).ToList();
            ViewBag.tonghd = listhd.Count;
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            List<LoaiGame> maloai = db.LoaiGames.ToList();
            List<SelectListItem> item = new List<SelectListItem>();

            foreach (var i in maloai)
            {
                item.Add(new SelectListItem
                {
                    Text = i.tengame,
                    Value = i.idGame.ToString()
                });

            }
            ViewBag.idGame = item;
            List<LoaiAcc> loai = db.LoaiAccs.ToList();
            List<SelectListItem> item1 = new List<SelectListItem>();

            foreach (var i in loai)
            {
                item1.Add(new SelectListItem
                {
                    Text = i.tenloai,
                    Value = i.idLoai.ToString()
                });

            }
            ViewBag.idLoai = item1;
            return View(new Account());

        }
        public ActionResult DetailUser(int id)
        {
            var D_sach = db.Users.Where(m => m.iduser == id).First();
            return View(D_sach);
        }
        public bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Account s)
        {
            var E_idGame = collection["idGame"];
            var E_taikhoan = collection["taikhoan"];
            var E_matkhau = collection["matkhau"];
            var E_giaban = collection["giaban"];
            var E_rank = collection["rank"];
            var E_tuong = collection["tuong"];
            var E_trangphuc = collection["trangphuc"];
            var E_hinh = collection["hinh"];
            Account checktendn = db.Accounts.SingleOrDefault(n => n.taikhoan == E_taikhoan);

            if (string.IsNullOrEmpty(E_taikhoan))
            {
                ViewData["Error2"] = "Vui lòng nhập dữ liệu!";
            }
            else if (string.IsNullOrEmpty(E_matkhau))
            {
                ViewData["Error3"] = "Vui lòng nhập dữ liệu!";
            }
            else if (IsNumber(E_giaban) == false || string.IsNullOrEmpty(E_giaban))
            {
                ViewData["Error4"] = "Vui lòng nhập dữ liệu đúng!";
            }
            else if (string.IsNullOrEmpty(E_rank))
            {
                ViewData["Error5"] = "Vui lòng nhập dữ liệu!";
            }
            else if (IsNumber(E_trangphuc) == false || string.IsNullOrEmpty(E_trangphuc))
            {
                ViewData["Error6"] = "Vui lòng nhập dữ liệu đúng!";
            }
            else if (IsNumber(E_tuong) == false || string.IsNullOrEmpty(E_tuong))
            {
                ViewData["Error7"] = "Vui lòng nhập dữ liệu đúng!";
            }
            else
            {
                if (checktendn != null)
                {
                    ViewBag.thongbao = "Tên tài khoản đã tồn tại";
                }
                else
                {
                    s.taikhoan = E_taikhoan.ToString();
                    s.matkhau = E_matkhau.ToString();
                    s.giaban = Convert.ToDecimal(E_giaban);
                    s.ngaycapnhat = DateTime.Today;
                    s.trangthai = true;
                    s.rank = E_rank.ToString();
                    s.trangphuc = Convert.ToInt32(E_trangphuc);
                    s.tuong = Convert.ToInt32(E_tuong);
                    s.hinh = E_hinh.ToString();
                    db.Accounts.InsertOnSubmit(s);
                    db.SubmitChanges();
                    return RedirectToAction("Index1");
                }
            }
            return this.Create();
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Account find = db.Accounts.FirstOrDefault(p => p.id == id);
            if (find == null)
                return HttpNotFound();
            List<LoaiGame> maloai = db.LoaiGames.ToList();
            List<SelectListItem> item = new List<SelectListItem>();
            foreach (var i in maloai)
            {
                item.Add(new SelectListItem
                {
                    Text = i.tengame,
                    Value = i.idGame.ToString()
                });

            }
            List<LoaiAcc> loaiacc = db.LoaiAccs.ToList();
            List<SelectListItem> item1 = new List<SelectListItem>();
            foreach (var i in loaiacc)
            {
                item1.Add(new SelectListItem
                {
                    Text = i.tenloai,
                    Value = i.idLoai.ToString()
                });

            }
            ViewBag.idGame = item;
            ViewBag.idLoai = item1;
            return View(find);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var s = db.Accounts.First(m => m.id == id);

            var E_idGame = collection["idGame"];
            var E_taikhoan = collection["taikhoan"];
            var E_matkhau = collection["matkhau"];
            var E_giaban = collection["giaban"];
            var E_rank = collection["rank"];
            var E_tuong = collection["tuong"];
            var E_trangphuc = collection["trangphuc"];
            var E_hinh = collection["hinh"];
            if (string.IsNullOrEmpty(E_taikhoan))
            {
                ViewData["Error2"] = "Vui lòng nhập dữ liệu!";
            }
            else if (string.IsNullOrEmpty(E_matkhau))
            {
                ViewData["Error3"] = "Vui lòng nhập dữ liệu!";
            }
            else if (IsNumber(E_giaban) == false || string.IsNullOrEmpty(E_giaban))
            {
                ViewData["Error4"] = "Vui lòng nhập dữ liệu đúng!";
            }
            else if (string.IsNullOrEmpty(E_rank))
            {
                ViewData["Error5"] = "Vui lòng nhập dữ liệu!";
            }
            else if (IsNumber(E_trangphuc) == false || string.IsNullOrEmpty(E_trangphuc))
            {
                ViewData["Error6"] = "Vui lòng nhập dữ liệu đúng!";
            }
            else if (IsNumber(E_tuong) == false || string.IsNullOrEmpty(E_tuong))
            {
                ViewData["Error7"] = "Vui lòng nhập dữ liệu đúng!";
            }
            else
            {
                s.taikhoan = E_taikhoan.ToString();
                s.matkhau = E_matkhau.ToString();
                s.giaban = Convert.ToDecimal(E_giaban);
                s.ngaycapnhat = DateTime.Today;
                s.trangthai = true;
                s.rank = E_rank.ToString();
                s.trangphuc = Convert.ToInt32(E_trangphuc);
                s.tuong = Convert.ToInt32(E_tuong);
                s.hinh = E_hinh.ToString();
                UpdateModel(s);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {

            var delete = db.Accounts.First(p => p.id == id);
            return View(delete);
        }


        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var delete = db.Accounts.Where(m => m.id == id).First();
                delete.trangthai = false;
                UpdateModel(delete);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }
        public ActionResult TTUser()
        {
            var all_user = from tt in db.Users where tt.trangthai == true select tt;
            return View(all_user);
        }

        [HttpGet]
        public ActionResult DeleteUser(int? id)
        {

            var delete = db.Users.First(p => p.iduser == id);
            return View(delete);
        }


        [HttpPost]
        public ActionResult DeleteUser(int id, FormCollection collection)
        {
            try
            {

                var userdel = db.Users.Where(m => m.iduser == id).First();
                userdel.trangthai = false;
                UpdateModel(userdel);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("TTUser");
        }

        public ActionResult DetailDH(int? id)
        {
            var listHD = db.HoaDons.Where(m => m.maHD == id).First();
            return View(listHD);
        }
        public ActionResult TTHD()
        {
            List<DonHang> listDH = new List<DonHang>();
            List<HoaDon> hd = db.HoaDons.Where(n => n.thanhtoan == true).ToList();
            foreach (var item in hd)
            {
                DonHang dh = new DonHang(item.maHD);
                listDH.Add(dh);
            }
            ViewBag.Tongtien = TongDoanhThu(listDH);
            return View(listDH);
        }
        private decimal TongDoanhThu(List<DonHang> dh)
        {
            decimal tdt = 0;
            foreach (var item in dh)
            {
                tdt = tdt + item.TongTien;
            }
            return tdt;
        }

        public ActionResult DetailHD(int? id)
        {
            var ListCT = db.ChiTietHDs.Where(n => n.maHD == id).ToList();
            ViewBag.Soluong = ListCT.Count;
            return View(ListCT);
        }

        public ActionResult Media(int id)
        {
            var acc = db.Accounts.Where(p => p.id == id).First();
            Session["Acc"] = acc;
            return View();
        }
        [HttpPost]
        public ActionResult Media(int id, FormCollection collection)
        {
            var link = collection["link_Media"];
            if(string.IsNullOrEmpty(link))
            {
                ViewData["ErroChon"] = "Vui lòng chọn hình!";
            }    
            Media md = new Media();
            md.id = id;
            md.link_Media = link;
            db.Medias.InsertOnSubmit(md);
            db.SubmitChanges();
            return View();


        }
        public ActionResult DetailMedia(int id)
        {

            var lstMedia = db.Medias.Where(p => p.id == id);

            return View(lstMedia);
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "Account");
        }
    }
}