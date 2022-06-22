using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MFAInventorySystem.Models;
using Rotativa;

namespace MFAInventorySystem.Controllers
{
    public class StockHistoryController : Controller
    {
        private db_mfaEntities db = new db_mfaEntities();

        // GET: StockHistory
        
        public ActionResult Index(string startdate = null, string enddate = null)
        {
            var tb_stockhistory = db.tb_stockhistory.Include(t => t.tb_stock).Include(t => t.tb_user).Include(t => t.tb_vendingmachine);

            if (startdate != null && enddate != null)
            {
                //this will default to current date if for whatever reason the date supplied by user did not parse successfully

                DateTime start = Convert.ToDateTime(startdate);

                DateTime end = Convert.ToDateTime(enddate);

                var rangeData = (db.tb_stockhistory.Where(x => x.sh_date >= start && x.sh_date <= end)).ToList();
                if(rangeData!=null)
                {
                    return View(rangeData);

                }
                else
                {
                    return View(tb_stockhistory.ToList());
                }

                   
            }
            else
            {
                return View(tb_stockhistory.ToList());
            }
            
        }


       

        // GET: StockHistory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_stockhistory tb_stockhistory = db.tb_stockhistory.Find(id);
            if (tb_stockhistory == null)
            {
                return HttpNotFound();
            }
            return View(tb_stockhistory);
        }

        // GET: StockHistory/Create
        public ActionResult Create()
        {
            ViewBag.sh_sid = new SelectList(db.tb_stock, "s_id", "s_product");
            ViewBag.sh_uid = new SelectList(db.tb_user, "u_id", "u_name");
            ViewBag.sh_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location");


            return View();
        }

        // POST: StockHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tb_stockhistory tb_stockhistory)
        {
            if (ModelState.IsValid)
            {
                var profitpercan = (from tb_stock in db.tb_stock where tb_stock.s_id == tb_stockhistory.sh_sid select tb_stock.s_untungBersihPerTin).Sum();
                var pricesell = (from tb_stock in db.tb_stock where tb_stock.s_id == tb_stockhistory.sh_sid select tb_stock.s_hargaJualan).Sum();
                var v_cashInSlot = pricesell * tb_stockhistory.sh_qtySlot;
                var v_id = tb_stockhistory.sh_vmID;
                //Stock Storage update
                var s_id = tb_stockhistory.sh_sid;
                var s_qty = (from tb_stock in db.tb_stock where tb_stock.s_id == tb_stockhistory.sh_sid select tb_stock.s_qty).Sum();
                s_qty = s_qty - tb_stockhistory.sh_qtySold;
                if (s_qty >= 0)
                {
                    tb_stockhistory.sh_untungBersih = profitpercan * tb_stockhistory.sh_qtySold;
                    var v_profit = tb_stockhistory.sh_untungBersih;
                    SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-OG65LBU\SQLEXPRESS01;Initial Catalog=db_mfa;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
                    SqlDataAdapter cmd = new SqlDataAdapter();
                    cmd.InsertCommand = new SqlCommand("UPDATE tb_stock SET s_qty = @s_qty WHERE s_id=@s_id;");
                    cmd.InsertCommand.Connection = con;
                    {
                        //command for updating stock storage

                        cmd.InsertCommand.Parameters.Add("s_qty", s_qty.ToString());
                        cmd.InsertCommand.Parameters.Add("s_id", s_id.ToString());


                        con.Open();
                        cmd.InsertCommand.ExecuteNonQuery();

                        con.Close();


                        //command for updating vending machine
                        cmd.InsertCommand = new SqlCommand("UPDATE tb_vendingmachine SET v_cashInSlot=v_cashInSlot+@v_cashInSlot,v_profit=v_profit+@v_profit WHERE v_id=@v_id;");
                        cmd.InsertCommand.Connection = con;
                        {
                            cmd.InsertCommand.Parameters.Add("v_cashInSlot", v_cashInSlot.ToString());
                            cmd.InsertCommand.Parameters.Add("v_profit", v_profit.ToString());
                            cmd.InsertCommand.Parameters.Add("v_id", v_id.ToString());


                            con.Open();
                            cmd.InsertCommand.ExecuteNonQuery();

                            con.Close();
                        }

                    }

                    tb_stockhistory.sh_uid = @Session["uid"].ToString() ;
                    db.tb_stockhistory.Add(tb_stockhistory);
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Stock out record successfully added!";
                    ViewBag.Message = string.Format("Stock History Updated Successfully");

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = string.Format("Fail");
                    return RedirectToAction("Index");
                }
            }

            ViewBag.sh_sid = new SelectList(db.tb_stock, "s_id", "s_product", tb_stockhistory.sh_sid);
            ViewBag.sh_uid = new SelectList(db.tb_user, "u_id", "u_name", tb_stockhistory.sh_uid);
            ViewBag.sh_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location", tb_stockhistory.sh_vmID);

            return View(tb_stockhistory);
        }



        // GET: StockHistory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_stockhistory tb_stockhistory = db.tb_stockhistory.Find(id);
            if (tb_stockhistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.sh_sid = new SelectList(db.tb_stock, "s_id", "s_product", tb_stockhistory.sh_sid);
            ViewBag.sh_uid = new SelectList(db.tb_user, "u_id", "u_name", tb_stockhistory.sh_uid);
            ViewBag.sh_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location", tb_stockhistory.sh_vmID);
            ViewBag.sh_price = new SelectList(db.tb_stock, "s_id", "s_hargajualan", tb_stockhistory.sh_sid);

            return View(tb_stockhistory);
        }

        // POST: StockHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tb_stockhistory tb_Stockhistory)
        {
            if (ModelState.IsValid)
            {
                var v_id = tb_Stockhistory.sh_vmID;
                var profitbefore = (from tb_stockhistory in db.tb_stockhistory where tb_stockhistory.sh_id == tb_Stockhistory.sh_id select tb_stockhistory.sh_untungBersih).Sum();
                var pricesell = (from tb_stock in db.tb_stock where tb_stock.s_id == tb_Stockhistory.sh_sid select tb_stock.s_hargaJualan).Sum();
                var quantitysoldbefore = (from tb_stockhistory in db.tb_stockhistory where tb_stockhistory.sh_id == tb_Stockhistory.sh_id select tb_stockhistory.sh_qtySold).Sum();
                var quantityslotbefore = (from tb_stockhistory in db.tb_stockhistory where tb_stockhistory.sh_id == tb_Stockhistory.sh_id select tb_stockhistory.sh_qtySlot).Sum();
                var cashinslotbefore = quantityslotbefore * pricesell;
                var cashinslotnow = tb_Stockhistory.sh_qtySlot * pricesell;
                var v_cashInSlot = (from tb_vendingmachine in db.tb_vendingmachine where tb_vendingmachine.v_id == v_id select tb_vendingmachine.v_cashInSlot).Sum();
                v_cashInSlot = (v_cashInSlot - cashinslotbefore) + cashinslotnow;

                var v_profit = (from tb_vendingmachine in db.tb_vendingmachine where tb_vendingmachine.v_id == v_id select tb_vendingmachine.v_profit).Sum();

                var s_id = tb_Stockhistory.sh_sid;
                var s_qty = (from tb_stock in db.tb_stock where tb_stock.s_id == tb_Stockhistory.sh_sid select tb_stock.s_qty).Sum();
                var s_qty1 = (from tb_stockhistory in db.tb_stockhistory where tb_stockhistory.sh_id == tb_Stockhistory.sh_id select tb_stockhistory.sh_qtySold).Sum();
                var profitpercan = (from tb_stock in db.tb_stock where tb_stock.s_id == tb_Stockhistory.sh_sid select tb_stock.s_untungBersihPerTin).Sum();

                s_qty = (s_qty + s_qty1) - tb_Stockhistory.sh_qtySold;
                if (s_qty >= 0)
                {
                    tb_Stockhistory.sh_untungBersih = profitpercan * tb_Stockhistory.sh_qtySold;
                    var profitnow = tb_Stockhistory.sh_untungBersih;
                    v_profit = (v_profit - profitbefore) + tb_Stockhistory.sh_untungBersih;
                    SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-OG65LBU\SQLEXPRESS01;Initial Catalog=db_mfa;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
                    SqlDataAdapter cmd = new SqlDataAdapter();
                    //stock storage update
                    cmd.InsertCommand = new SqlCommand("UPDATE tb_stock SET s_qty = @s_qty WHERE s_id=@s_id;");
                    cmd.InsertCommand.Connection = con;
                    {


                        cmd.InsertCommand.Parameters.Add("s_qty", s_qty.ToString());
                        cmd.InsertCommand.Parameters.Add("s_id", s_id.ToString());


                        con.Open();
                        cmd.InsertCommand.ExecuteNonQuery();

                        con.Close();

                        //command for updating vending machine
                        cmd.InsertCommand = new SqlCommand("UPDATE tb_vendingmachine SET v_cashInSlot=@v_cashInSlot,v_profit=@v_profit WHERE v_id=@v_id;");
                        cmd.InsertCommand.Connection = con;
                        {
                            cmd.InsertCommand.Parameters.Add("v_cashInSlot", v_cashInSlot.ToString());
                            cmd.InsertCommand.Parameters.Add("v_profit", v_profit.ToString());
                            cmd.InsertCommand.Parameters.Add("v_id", v_id.ToString());


                            con.Open();
                            cmd.InsertCommand.ExecuteNonQuery();

                            con.Close();
                        }

                    }

                    db.Entry(tb_Stockhistory).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Stock out record successfully modified!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = string.Format("Fail");
                    return RedirectToAction("Index");
                }
            }
            ViewBag.sh_sid = new SelectList(db.tb_stock, "s_id", "s_product", tb_Stockhistory.sh_sid);
            ViewBag.sh_uid = new SelectList(db.tb_user, "u_id", "u_name", tb_Stockhistory.sh_uid);
            ViewBag.sh_vmID = new SelectList(db.tb_vendingmachine, "v_id", "v_location", tb_Stockhistory.sh_vmID);
            return View(tb_Stockhistory);
        }

        // GET: StockHistory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_stockhistory tb_stockhistory = db.tb_stockhistory.Find(id);
            if (tb_stockhistory == null)
            {
                return HttpNotFound();
            }
            return View(tb_stockhistory);
        }

        // POST: StockHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_stockhistory tb_Stockhistory = db.tb_stockhistory.Find(id);
            var sh_id = tb_Stockhistory.sh_id;
            var v_id = tb_Stockhistory.sh_vmID;
            var profitpercan = (from tb_stock in db.tb_stock where tb_stock.s_id == tb_Stockhistory.sh_sid select tb_stock.s_untungBersihPerTin).Sum();
            var pricesell = (from tb_stock in db.tb_stock where tb_stock.s_id == tb_Stockhistory.sh_sid select tb_stock.s_hargaJualan).Sum();

            var s_id = tb_Stockhistory.sh_sid;
            var s_qty = (from tb_stock in db.tb_stock where tb_stock.s_id == tb_Stockhistory.sh_sid select tb_stock.s_qty).Sum();
            var s_qty1 = (from tb_stockhistory in db.tb_stockhistory where tb_stockhistory.sh_id == tb_Stockhistory.sh_id select tb_stockhistory.sh_qtySold).Sum();
            s_qty = s_qty + s_qty1;
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-OG65LBU\SQLEXPRESS01;Initial Catalog=db_mfa;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
            SqlDataAdapter cmd = new SqlDataAdapter();
            //stock storage update

            cmd.InsertCommand = new SqlCommand("UPDATE tb_stock SET s_qty = s_qty+tb_stockhistory.sh_qtySold FROM tb_stock,tb_stockhistory WHERE (s_id=@s_id AND tb_stockhistory.sh_id=@sh_id);");
            cmd.InsertCommand.Connection = con;
            {


                cmd.InsertCommand.Parameters.Add("sh_id", sh_id.ToString());
                cmd.InsertCommand.Parameters.Add("s_id", s_id.ToString());


                con.Open();
                cmd.InsertCommand.ExecuteNonQuery();

                con.Close();

                //command for updating vending machine
                cmd.InsertCommand = new SqlCommand("UPDATE tb_vendingmachine SET v_cashInSlot=v_cashInSlot-(tb_stockhistory.sh_qtySlot*(CONVERT(float, @pricesell))),v_profit=v_profit-(tb_stockhistory.sh_qtySold*(CONVERT(float, @profitpercan))) FROM tb_vendingmachine,tb_stockhistory WHERE (v_id=@v_id AND tb_stockhistory.sh_id=@sh_id);");
                cmd.InsertCommand.Connection = con;
                {
                    cmd.InsertCommand.Parameters.Add("pricesell", pricesell.ToString());
                    cmd.InsertCommand.Parameters.Add("profitpercan", profitpercan.ToString());
                    cmd.InsertCommand.Parameters.Add("v_id", v_id.ToString());
                    cmd.InsertCommand.Parameters.Add("sh_id", sh_id.ToString());



                    con.Open();
                    cmd.InsertCommand.ExecuteNonQuery();

                    con.Close();
                }

            }
            db.tb_stockhistory.Remove(tb_Stockhistory);
            db.SaveChanges();
            TempData["AlertMessage"] = "Stock out record successfully deleted!";
            return RedirectToAction("Index");
        }
        public ActionResult GetAll()
        {
            var so = db.tb_stockhistory.ToList();
            return View(so);
        }

        public ActionResult PrintAll()
        {
            var q = new ActionAsPdf("GetAll");
            return q;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
