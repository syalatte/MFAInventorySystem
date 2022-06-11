using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MFAInventorySystem.Models;

namespace MFAInventorySystem.Controllers
{
    public class StockController : Controller
    {
        private db_mfaEntities db = new db_mfaEntities();

        // GET: Stock
        public ActionResult Index()
        {
            return View(db.tb_stock.ToList());
        }

        // GET: Stock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_stock tb_stock = db.tb_stock.Find(id);
            if (tb_stock == null)
            {
                return HttpNotFound();
            }
            return View(tb_stock);
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stock/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "s_id,s_product,s_qty,s_modal,s_hargaJualan,s_untungBersihPerTin")] tb_stock tb_stock)
        {
            if (ModelState.IsValid)
            {
                db.tb_stock.Add(tb_stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tb_stock);
        }

        // GET: Stock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_stock tb_stock = db.tb_stock.Find(id);
            if (tb_stock == null)
            {
                return HttpNotFound();
            }
            return View(tb_stock);
        }

        // POST: Stock/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "s_id,s_product,s_qty,s_modal,s_hargaJualan,s_untungBersihPerTin")] tb_stock tb_stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_stock);
        }

        // GET: Stock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_stock tb_stock = db.tb_stock.Find(id);
            if (tb_stock == null)
            {
                return HttpNotFound();
            }
            return View(tb_stock);
        }

        // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_stock tb_stock = db.tb_stock.Find(id);
            db.tb_stock.Remove(tb_stock);
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
    }
}
