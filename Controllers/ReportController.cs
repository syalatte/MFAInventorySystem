using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        public ActionResult Index()
        {
            var tb_report = db.tb_report.Include(t => t.tb_vendingmachine).Include(t => t.tb_stock);
            return View(tb_report.ToList());
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
