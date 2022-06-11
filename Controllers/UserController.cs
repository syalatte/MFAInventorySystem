using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using System.Web.Mvc;
using MFAInventorySystem.Models;

namespace MFAInventorySystem.Controllers
{

    public class UserController : Controller
    {
        private db_mfaEntities db = new db_mfaEntities();

        // GET: User
        public ActionResult Index() 
        {
            var tb_user = db.tb_user.Include(t => t.tb_usertype);
            return View(tb_user.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            return View(tb_user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.u_type = new SelectList(db.tb_usertype, "ut_id", "ut_desc");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "u_id,u_name,u_contact,u_email,u_pw,u_type")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                var unhashedPass = tb_user.u_pw;
                tb_user.u_pw = HashPassword(unhashedPass);
                db.tb_user.Add(tb_user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.u_type = new SelectList(db.tb_usertype, "ut_id", "ut_desc", tb_user.u_type);
            return View(tb_user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            ViewBag.u_type = new SelectList(db.tb_usertype, "ut_id", "ut_desc", tb_user.u_type);
            return View(tb_user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "u_id,u_name,u_contact,u_email,u_pw,u_type")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.u_type = new SelectList(db.tb_usertype, "ut_id", "ut_desc", tb_user.u_type);
            return View(tb_user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            return View(tb_user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tb_user tb_user = db.tb_user.Find(id);
            db.tb_user.Remove(tb_user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: User/EditProfile/5
        public ActionResult EditProfile(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            ViewBag.u_type = new SelectList(db.tb_usertype, "ut_id", "ut_desc", tb_user.u_type);
            return View(tb_user);
        }

        // POST: User/EditProfile/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "u_id,u_name,u_contact,u_email,u_pw,u_type")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditProfile");
            }
            ViewBag.u_type = new SelectList(db.tb_usertype, "ut_id", "ut_desc", tb_user.u_type);
            return View(tb_user);
        }

        // GET: User/ChangePassword/5
        public ActionResult ChangePassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            ViewBag.u_type = new SelectList(db.tb_usertype, "ut_id", "ut_desc", tb_user.u_type);
            return View(tb_user);
        }

        // POST: User/ChangePassword/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "u_id,u_name,u_contact,u_email,u_pw,u_type")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ChangePassword");
            }
            ViewBag.u_type = new SelectList(db.tb_usertype, "ut_id", "ut_desc", tb_user.u_type);
            return View(tb_user);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }


    }
}
