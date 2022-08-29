using System;
using ShopAcc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ShopAcc.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        MyDataDataContext data = new MyDataDataContext();
        
      
        public ActionResult HomePage(int? page)
        { 
            int pageSize = 8;
            int pageNum = (page ?? 1);

            var all_account = from tt in data.Accounts where tt.trangthai == true
                              select tt;
            return View(all_account.ToPagedList(pageNum,pageSize));
        }
       
        public ActionResult Detail(int id)
        {

            var D_Acc = data.Accounts.Where(m => m.id == id).First();
            return View(D_Acc);

        }
        public ActionResult Media(int id)
        {
            var all_media = data.Medias.Where(p => p.id == id);
            return View(all_media);
        }



    }
}