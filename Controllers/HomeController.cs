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
                var profits = db.tb_vendingmachine.Sum(a => a.v_profit).Value;
                var capitals = db.tb_stock.Sum(a => a.s_modal).Value;

                ViewBag.numEmployer = numEmployer;
                ViewBag.numEmployee = numEmployee;
                ViewBag.numVM = numVM;
                ViewBag.numStock = numStock;
                ViewBag.profits = profits.ToString("0.00");
                ViewBag.capitals = capitals.ToString("0.00");

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
        public ActionResult Viewpdf()
        {
            return View();
        }
        public ActionResult Viewpdf2()
        {
            return View();
        }

        public PartialViewResult PDFPartialView()
        {
            ViewBag.pdf = @Url.Content("~/Content/AD_WBL_Project.pdf");
            //if you don't have url get the url from database here by passing id in the ActionMethod
            // and pass it in Partial View using ViewBag
            return PartialView();
        }
    }
}