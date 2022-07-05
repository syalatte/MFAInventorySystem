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
    public class VendingMachineController : Controller
    {
        private db_mfaEntities db = new db_mfaEntities();

        // GET: VendingMachine
        public ActionResult Index()
        {
            return View(db.tb_vendingmachine.ToList());
        }

        // GET: VendingMachine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_vendingmachine tb_vendingmachine = db.tb_vendingmachine.Find(id);
            if (tb_vendingmachine == null)
            {
                return HttpNotFound();
            }
            return View(tb_vendingmachine);
        }

        // GET: VendingMachine/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VendingMachine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tb_vendingmachine tb_vendingmachine)
        {
            if (ModelState.IsValid)
            {
                var v_location = tb_vendingmachine.v_location;

                SqlConnection con = new SqlConnection(@"Data Source=LATTE-LAPTOP\SQLEXPRESS01;Initial Catalog=db_mfa;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
                SqlDataAdapter cmd = new SqlDataAdapter();
                cmd.InsertCommand = new SqlCommand("Insert into tb_vendingmachine Values(@v_location,0,0);");
                cmd.InsertCommand.Connection = con;
                {
                    //command for updating stock storage

                    cmd.InsertCommand.Parameters.Add("v_location", v_location.ToString());
                   


                    con.Open();
                    cmd.InsertCommand.ExecuteNonQuery();

                    con.Close();


                    
                }
                TempData["AlertMessage"] = "Vending machine successfully added!";
                return RedirectToAction("Index");
            }

            return View(tb_vendingmachine);
        }

        // GET: VendingMachine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_vendingmachine tb_vendingmachine = db.tb_vendingmachine.Find(id);
            if (tb_vendingmachine == null)
            {
                return HttpNotFound();
            }
            return View(tb_vendingmachine);
        }

        // POST: VendingMachine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( tb_vendingmachine tb_Vendingmachine)
        {
            if (ModelState.IsValid)
            {
                tb_Vendingmachine.v_cashInSlot = (from tb_vendingmachine in db.tb_vendingmachine where tb_vendingmachine.v_id == tb_Vendingmachine.v_id select tb_vendingmachine.v_cashInSlot).Sum();
                tb_Vendingmachine.v_profit = (from tb_vendingmachine in db.tb_vendingmachine where tb_vendingmachine.v_id == tb_Vendingmachine.v_id select tb_vendingmachine.v_profit).Sum();

                db.Entry(tb_Vendingmachine).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Vending machine successfully modified!";
                return RedirectToAction("Index");
            }
            return View(tb_Vendingmachine);
        }

        // GET: VendingMachine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_vendingmachine tb_vendingmachine = db.tb_vendingmachine.Find(id);
            if (tb_vendingmachine == null)
            {
                return HttpNotFound();
            }
            return View(tb_vendingmachine);
        }

        // POST: VendingMachine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_vendingmachine tb_vendingmachine = db.tb_vendingmachine.Find(id);

            var v_id = tb_vendingmachine.v_id;
            SqlConnection con = new SqlConnection(@"Data Source=LATTE-LAPTOP\SQLEXPRESS01;Initial Catalog=db_mfa;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
            SqlDataAdapter cmd = new SqlDataAdapter();


            cmd.InsertCommand = new SqlCommand("DELETE FROM tb_stockhistory WHERE sh_vmID=@v_id;");
            cmd.InsertCommand.Connection = con;
            cmd.InsertCommand.Parameters.Add("v_id", v_id.ToString());

            con.Open();
            cmd.InsertCommand.ExecuteNonQuery();

            con.Close();
            db.tb_vendingmachine.Remove(tb_vendingmachine);
            db.SaveChanges();
            TempData["AlertMessage"] = "Vending machine successfully deleted!";
            return RedirectToAction("Index");
        }
        public ActionResult GetAll()
        {
            var vm = db.tb_vendingmachine.ToList();
            return View(vm);
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
