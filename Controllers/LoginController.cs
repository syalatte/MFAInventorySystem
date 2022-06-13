using MFAInventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MFAInventorySystem.Controllers
{
    
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(MFAInventorySystem.Models.tb_user userModel)
        {
            using (db_mfaEntities db = new db_mfaEntities())
            {
                var userDetails = db.tb_user.Where(x => x.u_id == userModel.u_id).FirstOrDefault();
                var PasswordCorrect = VerifyHashedPassword(userDetails.u_pw, userModel.u_pw);

                if (PasswordCorrect == false)
                {
                    userModel.LoginErrorMessage = "User Not Found";
                    return View("Index", userModel);
                }
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                else
                {
                    //Session["uid"] = userDetails.u_id;
                    
                    

                    if (userDetails.u_type == 1)
                    {
                        Session["uid"] = userDetails.u_id;
                        Session["utype"] = userDetails.u_type;
                        Session["uname"] = userDetails.u_name;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Session["uid"] = userDetails.u_id;
                        Session["utype"] = userDetails.u_type;
                        Session["uname"] = userDetails.u_name;
                        int total = db.tb_user.Count();
                        return RedirectToAction("Index2", "Home");
                    }


                }
            }

        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }


        //public ActionResult Logout()
        //{
        //    FormsAuthentication.SignOut();
        //    //Session["uid"] = null;
        //    ////Session["utype"] = null;
        //    //Session["uname"] = null;
        //    Session.Abandon();
        //    return RedirectToAction("Index", "Login");

        //}
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["uid"] = null;
            Session["uname"] = null;
            Session["utype"] = null;
            return RedirectToAction("Index", "Login");

        }

    }
}