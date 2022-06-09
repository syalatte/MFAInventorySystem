using MFAInventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                var userDetails = db.tb_user.Where(x => x.u_id == userModel.u_id && x.u_pw == userModel.u_pw).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["uid"] = userDetails.u_id;
                    
                    

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

        public ActionResult Logout()
        {
            string uid = (string)Session["uid"];
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

    }
}