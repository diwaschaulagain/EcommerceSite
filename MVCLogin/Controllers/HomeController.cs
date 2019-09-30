using MVCLogin.Models;
using MVCLogin.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication32;

namespace MVCLogin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        BhaiDBEntities db = new BhaiDBEntities();
        public ActionResult ViewItem(int id)
        {
            return PartialView("_ViewItem", db.tblProducts.Find(id));
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult ProductList(string search, int? page, int id = 0)
        {

            if (id != 0)
            {

                return View(db.tblProducts.Where(p => p.CategoryId == id).ToList().ToPagedList(page ?? 1, 4));
            }
            else
            {
                if (search != "")
                {
                    return View(db.tblProducts.Where(x => x.Description.Contains(search) || x.ProductName.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, 4));
                }
                else
                {
                    return View(db.tblProducts.ToList().ToPagedList(page ?? 1, 4));
                }

            }

        }
        public ActionResult ForgetPassword()
        {
            return View();


        }
        [ValidateOnlyIncomingValuesAttribute]
        [HttpPost]

        public ActionResult ForgetPassword(UserViewModel uv)
        {

            if (ModelState.IsValid)
            {
                //https://www.google.com/settings/security/lesssecureapps
                //Make Access for less secure apps=true

                string from = "seventythreeplusten@gmail.com";
                using (MailMessage mail = new MailMessage(from, uv.Email))
                {
                    try
                    {
                        tblUser tb = db.tblUsers.Where(u => u.Email == uv.Email).FirstOrDefault();
                        if (tb != null)
                        {
                            mail.Subject = "Password Recovery";
                            mail.Body = "Your Password is:" + tb.Password;

                            mail.IsBodyHtml = false;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential networkCredential = new NetworkCredential(from, "vbproject123");
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = networkCredential;
                            smtp.Port = 587;
                            smtp.Send(mail);
                            ViewBag.Message = "Your Password Is Sent to your email";
                        }
                        else
                        {
                            ViewBag.Message = "email Doesnot Exist in Database";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }

                }

            }
            return View();


            //return RedirectToAction("Index", "Home");
        }
        public ActionResult Signup()
        {
            return View();
        }
            [HttpPost]
            public ActionResult Signup(UserViewModel uv)
            {
                tblUser tbl = db.tblUsers.Where(u => u.Username == uv.Username).FirstOrDefault();
                if (tbl != null)
                {
                    return Json(new { success = false, message = "User Already Registerd" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tblUser tb = new tblUser();
                    tb.Username = uv.Username;
                    tb.Password = uv.Password;
                    db.tblUsers.Add(tb);
                    db.SaveChanges();

                    tblUserRole ud = new tblUserRole();
                    ud.UserId = tb.UserId;
                    ud.UserRoleId = 1003;
                    db.tblUserRoles.Add(ud);
                    db.SaveChanges();
                    return Json(new { success = true, message = "User Registered Successfully" }, JsonRequestBehavior.AllowGet);
                }



            }
    }
}