using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models;

namespace ShopAcc.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LMHT()
        {
            var acc = data.Accounts.Where(p => p.idGame == 1 && p.trangthai == true).ToList();

            return View(acc);
        }
        public ActionResult TocChien()
        {
            var acc = data.Accounts.Where(p => p.idGame == 2 && p.trangthai == true).ToList();

            return View(acc);
        }
        public ActionResult LQM()
        {
            var acc = data.Accounts.Where(p => p.idGame == 3 && p.trangthai == true).ToList();

            return View(acc);
        }
    }
}