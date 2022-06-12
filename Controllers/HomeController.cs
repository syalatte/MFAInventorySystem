using MFAInventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFAInventorySystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (db_mfaEntities db = new db_mfaEntities())
            {
                var numEmployer = db.tb_user.Where(a => a.u_type == 1).Count();
                var numEmployee = db.tb_user.Where(a => a.u_type == 2).Count();
                var numVM = db.tb_vendingmachine.Count();
                var numStock = db.tb_stock.Count();
                var profits = db.tb_vendingmachine.Sum(a => a.v_profit);
                var capitals = db.tb_stock.Sum(a => a.s_modal);

                ViewBag.numEmployer = numEmployer;
                ViewBag.numEmployee = numEmployee;
                ViewBag.numVM = numVM;
                ViewBag.numStock = numStock;
                ViewBag.profits = profits;
                ViewBag.capitals = capitals;

                return View();
            }
        }
        public ActionResult Index2()
        {
            using (db_mfaEntities db = new db_mfaEntities())
            {
                var numVM = db.tb_vendingmachine.Count();
                var numStock = db.tb_stock.Count();
                ViewBag.numVM = numVM;
                ViewBag.numStock = numStock;
                return View();
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}