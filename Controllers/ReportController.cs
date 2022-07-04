using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MFAInventorySystem.Models;
using Rotativa;

namespace MFAInventorySystem.Controllers
{
    public class ReportController : Controller
    {
        private db_mfaEntities db = new db_mfaEntities();

        // GET: Report
        public ActionResult Index(string startDate = null, string endDate = null)
        {
            var tb_report = db.tb_report.Include(t => t.tb_vendingmachine).Include(t => t.tb_stock);
            
            if (startDate != null && endDate != null)
            {
                DateTime start = Convert.ToDateTime(startDate);
                DateTime end = Convert.ToDateTime(endDate);

                var data = (db.tb_report.Where(x => x.r_date >= start && x.r_date <= end).ToList());
               
                if (data != null)
                {
                    return View(data);
                }
                else
                {
                    return View(tb_report.ToList());
                }
            }
            else
            {
                return View(tb_report.ToList());
            }
        }

        // GET: Report/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_report tb_report = db.tb_report.Find(id);
            if (tb_report == null)
            {
                return HttpNotFound();
            }
            return View(tb_report);
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            ViewBag.r_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location");
            ViewBag.r_sid = new SelectList(db.tb_stock, "s_id", "s_product");
            return View();
        }
        public ActionResult CreateVending()
        {
            ViewBag.r_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location");
            return View();
        }

        public ActionResult CreateStock()
        {
            ViewBag.r_sid = new SelectList(db.tb_stock, "s_id", "s_product");
            return View();
        }

        // POST: Report/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "r_id,r_name,r_desc,r_date,r_profits,r_capitals,r_vmID,r_sid")] tb_report tb_report)
        {
            if (ModelState.IsValid)
            {

                db.tb_report.Add(tb_report);
                db.SaveChanges();
                TempData["AlertMessage"] = "Report successfully created!";
                return RedirectToAction("Index");
            }

            ViewBag.r_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location", tb_report.r_vmID);
            ViewBag.r_sid = new SelectList(db.tb_stock, "s_id", "s_product", tb_report.r_sid);
            return View(tb_report);
        }
        //vending machine report create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVending(tb_report tb_report)
        {
            if (ModelState.IsValid)
            {
                //variable assign for report
                var v_id = tb_report.r_vmID;
                var r_name = tb_report.r_name;
                var r_date = DateTime.Now;
                var r_sid = tb_report.r_sid;
                 
                //data connection
                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-NKBL84N\SQLEXPRESS;Initial Catalog=db_mfa;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
                SqlDataAdapter cmd = new SqlDataAdapter();

                //select total capital from table stock based on stock history by comparing vending machine id
                cmd.InsertCommand = new SqlCommand("select sum(tb_stock.s_modal) from tb_stock,tb_stockhistory where (tb_stockhistory.sh_vmID=@v_id) AND (tb_stockhistory.sh_sid=tb_stock.s_id);");
                cmd.InsertCommand.Connection = con;
                {


                    cmd.InsertCommand.Parameters.Add("v_id", v_id.ToString());


                    con.Open();
                    //assign the sql into variable
                    var r_capitals = cmd.InsertCommand.ExecuteScalar();

                    con.Close();

                    //Select the total profit for the vending machine
                    cmd.InsertCommand = new SqlCommand("select v_profit from tb_vendingmachine where v_id=@v_id;");
                    cmd.InsertCommand.Connection = con;
                    cmd.InsertCommand.Parameters.Add("v_id", v_id.ToString());


                    con.Open();
                    //Save the result in corresponding variable
                    var r_profits = (double)cmd.InsertCommand.ExecuteScalar();
                    var r_desc = "";
                    var profit = r_profits;
                    if (profit <= 0)
                    {
                        r_desc = "This Vending Machine is not making a profit.";
                    }
                    else
                    {
                        r_desc = "This Vending Machine is making a profit.";
                    }
                    con.Close();

                    
                    
                    
                    //process of inserting the data into tb_report
                    cmd.InsertCommand = new SqlCommand("Insert into tb_report(r_sid,r_name,r_date,r_desc,r_profits,r_capitals,r_vmID) Values(@r_sid,@r_name,@r_date,@r_desc,@r_profits,@r_capitals,@v_id);");
                    cmd.InsertCommand.Connection = con;
                    cmd.InsertCommand.Parameters.Add("r_sid", r_sid.ToString());
                    cmd.InsertCommand.Parameters.Add("r_name", r_name.ToString());
                    cmd.InsertCommand.Parameters.Add("r_date", r_date);
                    cmd.InsertCommand.Parameters.Add("r_desc", r_desc.ToString());
                    cmd.InsertCommand.Parameters.Add("r_profits", r_profits.ToString());
                    cmd.InsertCommand.Parameters.Add("r_capitals", r_capitals.ToString());
                    cmd.InsertCommand.Parameters.Add("v_id", v_id.ToString());


                    con.Open();
                    cmd.InsertCommand.ExecuteNonQuery();

                    con.Close();
                }
                TempData["AlertMessage"] = "Report for vending machine successfully created!";
                return RedirectToAction("Index");
            }

            ViewBag.r_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location", tb_report.r_vmID);
            ViewBag.r_sid = new SelectList(db.tb_stock, "s_id", "s_product", tb_report.r_sid);
            return View(tb_report);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStock(tb_report tb_report)
        {
            if (ModelState.IsValid)
            {
                //variable assign for report
                var v_id = tb_report.r_vmID;
                var r_name = tb_report.r_name;
                var r_date = DateTime.Now;
                
                var r_sid = tb_report.r_sid;
                //data connection
                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-NKBL84N\SQLEXPRESS;Initial Catalog=db_mfa;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
                SqlDataAdapter cmd = new SqlDataAdapter();

                //select total capital from table stock based on stock history by comparing vending machine id
                cmd.InsertCommand = new SqlCommand("select sum(tb_stock.s_modal) from tb_stock,tb_stockhistory where (tb_stockhistory.sh_sid=@r_sid) AND (tb_stockhistory.sh_sid=tb_stock.s_id);");
                cmd.InsertCommand.Connection = con;
                {


                    cmd.InsertCommand.Parameters.Add("r_sid", r_sid.ToString());


                    con.Open();
                    //assign the sql into variable
                    var r_capitals = cmd.InsertCommand.ExecuteScalar();

                    con.Close();

                    //Select the total profit for the vending machine
                    cmd.InsertCommand = new SqlCommand("select sum(tb_stockhistory.sh_untungbersih) from tb_stockhistory where tb_stockhistory.sh_sid=@r_sid;");
                    cmd.InsertCommand.Connection = con;
                    cmd.InsertCommand.Parameters.Add("r_sid", r_sid.ToString());


                    con.Open();
                    //Save the result in corresponding variable
                    var r_profits = (double)cmd.InsertCommand.ExecuteScalar();
                    var profit = r_profits;
                    var r_desc = "";
                    if (profit <= 0)
                    {
                         r_desc = "This Stock is not making a profit.";
                    }
                    else
                    {
                        r_desc = "This Stock is making a profit.";
                    }

                    con.Close();

                    //process of inserting the data into tb_report
                    cmd.InsertCommand = new SqlCommand("Insert into tb_report(r_sid,r_name,r_date,r_desc,r_profits,r_capitals,r_vmID) Values(@r_sid,@r_name,@r_date,@r_desc,@r_profits,@r_capitals,@v_id);");
                    cmd.InsertCommand.Connection = con;
                    cmd.InsertCommand.Parameters.Add("r_sid", r_sid.ToString());
                    cmd.InsertCommand.Parameters.Add("r_name", r_name.ToString());
                    cmd.InsertCommand.Parameters.Add("r_date", r_date);
                    cmd.InsertCommand.Parameters.Add("r_desc", r_desc.ToString());
                    cmd.InsertCommand.Parameters.Add("r_profits", r_profits.ToString());
                    cmd.InsertCommand.Parameters.Add("r_capitals", r_capitals.ToString());
                    cmd.InsertCommand.Parameters.Add("v_id", v_id.ToString());


                    con.Open();
                    cmd.InsertCommand.ExecuteNonQuery();

                    con.Close();
                }
                TempData["AlertMessage"] = "Report for stock successfully created!";
                return RedirectToAction("Index");
            }

            ViewBag.r_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location", tb_report.r_vmID);
            ViewBag.r_sid = new SelectList(db.tb_stock, "s_id", "s_product", tb_report.r_sid);
            return View(tb_report);
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_report tb_report = db.tb_report.Find(id);
            if (tb_report == null)
            {
                return HttpNotFound();
            }
            ViewBag.r_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location", tb_report.r_vmID);
            ViewBag.r_sid = new SelectList(db.tb_stock, "s_id", "s_product", tb_report.r_sid);
            return View(tb_report);
        }

        // POST: Report/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "r_id,r_name,r_desc,r_date,r_profits,r_capitals,r_vmID,r_sid")] tb_report tb_report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_report).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Report successfully modified!";
                return RedirectToAction("Index");
            }
            ViewBag.r_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location", tb_report.r_vmID);
            ViewBag.r_sid = new SelectList(db.tb_stock, "s_id", "s_product", tb_report.r_sid);
            return View(tb_report);
        }

        // GET: Report/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_report tb_report = db.tb_report.Find(id);
            if (tb_report == null)
            {
                return HttpNotFound();
            }
            return View(tb_report);
        }

        // POST: Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_report tb_report = db.tb_report.Find(id);
            db.tb_report.Remove(tb_report);
            db.SaveChanges();
            TempData["AlertMessage"] = "Report successfully deleted!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult GetAll()
        {
            var report = db.tb_report.ToList();
            return View(report);
        }

        public ActionResult PrintAll()
        {
            var q = new ActionAsPdf("GetAll");
            return q;
        }

        public ActionResult GetOne(int? id)
        {
            var report = (from tb_report in db.tb_report where tb_report.r_id == id select tb_report).ToList();
            return View(report);
        }

        public ActionResult PrintOne(int? id)
        {
            var q = new ActionAsPdf("GetOne");
            return q;
        }
    }
}
