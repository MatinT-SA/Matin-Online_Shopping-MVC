using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Sourceiran_MVC.Models.Domains;

namespace Sourceiran_MVC.Controllers
{
    public class RegisterController : Controller
    {
        DBSI db = new DBSI();
        UtilityFunction ut = new UtilityFunction();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Tbl_User user)
        {
            //کاربر لاگین کرده باشد

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "لطفا فیلدهای لازم را وارد نمایید";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View();
            }

            Tbl_User u = new Tbl_User();

            //اضافه نمودن تصویر پروفایل
            u.UserName = user.UserName;
            u.Email = user.Email;
            u.Password = user.Password;
            u.NatCode = user.NatCode;
            u.Mobile = user.Mobile;
            u.Access = "User";
            u.Status = false;

            db.Tbl_User.Add(u);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                //بعد از ثبت نام ایمیل تاییدیه ارسال می شود.

                Tbl_ConfirmEmail em = new Tbl_ConfirmEmail();

                em.Date = DateTime.Now;
                em.UserID = db.Tbl_User.OrderByDescending(a => a.ID).FirstOrDefault().ID;
                em.Status = false;
                em.DateEnd = em.Date.AddDays(3);

                db.Tbl_ConfirmEmail.Add(em);

                //تاییدیه شماره همراه



                Tbl_ConfirmMobile mo = new Tbl_ConfirmMobile();

                mo.Date = DateTime.Now;
                mo.UserID = db.Tbl_User.OrderByDescending(a => a.ID).FirstOrDefault().ID;
                mo.Status = false;
                mo.DateEnd = em.Date.AddDays(3);

                db.Tbl_ConfirmMobile.Add(mo);

                db.SaveChanges();//مشترک برای موبایل و ایمیل

                //ایمیل ارسال شود

                ut.SendEmail(db.Tbl_Setting.FirstOrDefault().SMTP, db.Tbl_Setting.FirstOrDefault().Email, db.Tbl_Setting.FirstOrDefault().EmailPass, u.Email, "این ایمیل تاییدیه به کاربر از سوی سایت '" + db.Tbl_Setting.FirstOrDefault().Title + "'", "<div style='font-size: 20px;float: right;direction:rtl;'> لطفا بر روی لینک زیر جهت تایید ایمیل خود کلیک کنید <br /> <a href='https://localhost:44346/Register/ConfirmEmail?code=" + db.Tbl_ConfirmEmail.OrderByDescending(a => a.ID).FirstOrDefault().ID + "'>لینک فعال سازی</a></div>");

                //پیامک ارسال شود

                ut.SendMessage("30002577587745", "Anousheh_2006", u.Mobile, "کاربر گرامی '" + u.UserName + "'، کد فعال سازی شما در سایت'" + db.Tbl_Setting.FirstOrDefault().Title + "' , '" + db.Tbl_ConfirmMobile.OrderByDescending(a => a.ID).FirstOrDefault().ID + "' می باشد.", "09334363774");

                TempData["Message"] = "ثبت نام کامل شد. ابتدا ایمیل و شماره موبایل خود را تایید کنید و سپس وارد پروفایل کاربری خود شوید.";
                TempData["Class"] = "alert alert-success alert-dismissible";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return RedirectToAction("MessageRegister", "Home", FormMethod.Post);
            }
            else
            {
                ViewBag.Message = "ثبت نام کامل نشد!";
                ViewBag.Class = "alert alert-danger alert-dismissible";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View();
            }
        }

        [HttpPost]
        public JsonResult RegisterValid(string UserName)
        {
            try
            {
                var UName = (from a in db.Tbl_User
                             where a.UserName == UserName
                             select a).SingleOrDefault();
                if (UName != null)
                {
                    //موجود می باشد
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    //موجود نمی باشد
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpPost]
        public JsonResult EmailValid(string Email)
        {
            try
            {
                var email = (from a in db.Tbl_User
                             where a.Email == Email
                             select a).SingleOrDefault();
                if (email != null)
                {
                    //موجود می باشد
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    //موجود نمی باشد
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult NatCodeValid(string NatCode)
        {
            try
            {
                var natcode = (from a in db.Tbl_User
                               where a.NatCode == NatCode
                               select a).SingleOrDefault();
                if (natcode != null)
                {
                    //موجود می باشد
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    //موجود نمی باشد
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult MobileValid(string Mobile)
        {
            try
            {
                var mobile = (from a in db.Tbl_User
                              where a.Mobile == Mobile
                              select a).SingleOrDefault();
                if (mobile != null)
                {
                    //موجود می باشد
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    //موجود نمی باشد
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult ConfirmEmail(int code)
        {
            if (code == 0)
            {
                return View();
            }
            else
            {
                var qConfirmEmailCode = (from a in db.Tbl_ConfirmEmail
                                         join b in db.Tbl_User on a.UserID equals b.ID
                                         where a.ID.Equals(code) && a.Status == false && a.DateEnd > DateTime.Now && b.Status == false
                                         select a).SingleOrDefault();
                if (qConfirmEmailCode != null)
                {
                    qConfirmEmailCode.Status = true;
                    db.Tbl_ConfirmEmail.Attach(qConfirmEmailCode);
                    db.Entry(qConfirmEmailCode).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    //باید قسمت موبایل نیز بررسی شود

                    var qConfirmMobile = (from a in db.Tbl_ConfirmMobile
                                          join b in db.Tbl_User on a.UserID equals b.ID
                                          where a.Status == true && a.UserID.Equals(qConfirmEmailCode.Tbl_User.ID)
                                          select a).SingleOrDefault();

                    if (qConfirmMobile != null)
                    {
                        var qUser = (from a in db.Tbl_User
                                     join b in db.Tbl_ConfirmEmail on a.ID equals b.UserID
                                     where b.ID.Equals(code) && a.Status == false && b.DateEnd > DateTime.Now
                                     select a).SingleOrDefault();

                        qUser.Status = true;
                        db.Tbl_User.Attach(qUser);
                        db.Entry(qUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                    }
                    else
                    {
                        TempData["Message"] = "ایمیل شما با موفقیت تایید شد. اکنون جهت فعالسازی شماره موبایل اقدام کنید";
                        TempData["Class"] = "alert alert-success alert-dismissible";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return RedirectToAction("MessageRegister", "Home", FormMethod.Post);
                    }

                    TempData["Message"] = "اکانت کاربری شما با موفقیت تایید شد. اکنون می توانید وارد فروشگاه شوید.";
                    TempData["Class"] = "alert alert-success alert-dismissible";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return View();

                }
                else
                {
                    TempData["Message"] = "به دلیل یکی از خطاهای زیر امکان تایید ایمیل برای شما وجود ندارد: <br /> 1- ایمیل شما یکبار تایید شده است <br /> 2- زمان انقضای کد تاییدیه شما به اتمام رسیده <br /> 3- اکانت کاربری شما یکبار تایید و فعال شده است <br /> 4- کد ارسالی شما نامعتبر است";
                    TempData["Class"] = "alert alert-danger alert-dismissible";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return RedirectToAction("MessageRegister", "Home", FormMethod.Post);
                }
            }
        }

        [HttpGet]
        public ActionResult ConfirmMobile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmMobile(int codemob = 0)
        {
            if (codemob == 0)
            {
                return View();
            }
            else
            {
                var qConfirmMobileCode = (from a in db.Tbl_ConfirmMobile
                                          join b in db.Tbl_User on a.UserID equals b.ID
                                          where a.ID.Equals(codemob) && a.Status == false && a.DateEnd > DateTime.Now && b.Status == false
                                          select a).SingleOrDefault();

                if (qConfirmMobileCode != null)
                {
                    qConfirmMobileCode.Status = true;
                    db.Tbl_ConfirmMobile.Attach(qConfirmMobileCode);
                    db.Entry(qConfirmMobileCode).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    //باید قسمت ایمیل نیز بررسی شود

                    var qConfirmEmailCode = (from a in db.Tbl_ConfirmEmail
                                             join b in db.Tbl_User on a.UserID equals b.ID
                                             where a.Status == true && a.UserID.Equals(qConfirmMobileCode.Tbl_User.ID)
                                             select a).SingleOrDefault();

                    if (qConfirmEmailCode != null)
                    {
                        var qUser = (from a in db.Tbl_User
                                     join b in db.Tbl_ConfirmEmail on a.ID equals b.UserID
                                     where b.ID.Equals(codemob) && a.Status == false && b.DateEnd > DateTime.Now
                                     select a).SingleOrDefault();

                        qUser.Status = true;
                        db.Tbl_User.Attach(qUser);
                        db.Entry(qUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        TempData["Message"] = "اکانت کاربری شما با موفقیت تایید شد. اکنون می توانید وارد فروشگاه شوید.";
                        TempData["Class"] = "alert alert-success alert-dismissible";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return RedirectToAction("MessageRegister", "Home", FormMethod.Post);

                    }
                    else
                    {
                        TempData["Message"] = "موبایل شما با موفقیت تایید شد. اکنون جهت فعالسازی ایمیل خود اقدام کنید";
                        TempData["Class"] = "alert alert-success alert-dismissible";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return RedirectToAction("MassageRegister", "Home");
                    }

                }
                else
                {
                    TempData["Message"] = "به دلیل یکی از خطاهای زیر امکان تایید موبایل برای شما وجود ندارد: <br /> 1- موبایل شما یکبار تایید شده است <br /> 2- زمان انقضای کد تاییدیه شما به اتمام رسیده <br /> 3- اکانت کاربری شما یکبار تایید و فعال شده است <br /> 4- کد ارسالی شما نامعتبر است";
                    TempData["Class"] = "alert alert-danger alert-dismissible";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return RedirectToAction("MessageRegister", "Home", FormMethod.Post);
                }
            }

        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Style = TempData["Style"];
            ViewBag.Class = TempData["Class"];
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Password, string Email)
        {
            try
            {
                if (Session["User"] == null)
                {
                    var qemail = db.Tbl_User.Where(a => a.Email.Equals(Email) || a.UserName.Equals(Email)).SingleOrDefault();

                    if (qemail != null)
                    {
                        var user = qemail.UserName;
                        if (qemail.Access == "User")
                        {
                            var qlogin = (from a in db.Tbl_User
                                          where a.UserName.Equals(user) && a.Password.Equals(Password) && a.Access.Equals("User") && a.Status.Equals(true)
                                          select a).SingleOrDefault();
                            if (qlogin != null)
                            {
                                Session["User"] = qlogin.UserName;
                                Session["Access"] = qlogin.Access;

                                return RedirectToAction("Index", "User");
                            }
                            else
                            {
                                ViewBag.Message = "به دلیل یکی از خطاهای زیر امکان ورود شما وجود ندارد: <br /> 1- نام کاربری یا ایمیل نادرست است <br /> 2- رمز عبور نادرست است <br /> 3- ورود شما به دلایل امنیتی امکان پذیر نیست <br /> 4- ایمیل و موبایل شما هنوز تایید نشده است";
                                ViewBag.Class = "alert alert-danger";
                                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                                return View();
                            }
                        }
                        else
                        {
                            var qlogin = (from a in db.Tbl_User
                                          where a.UserName.Equals(user) && a.Password.Equals(Password) && a.Access.Equals("Admin") && a.Status.Equals(true)
                                          select a).SingleOrDefault();
                            if (qlogin != null)
                            {
                                Session["Admin"] = qlogin.UserName;
                                Session["Access"] = qlogin.Access;

                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                ViewBag.Message = "به دلیل یکی از خطاهای زیر امکان ورود شما وجود ندارد: <br /> 1- نام کاربری یا ایمیل نادرست است <br /> 2- رمز عبور نادرست است <br /> 3- ورود شما به دلایل امنیتی امکان پذیر نیست <br /> 4- ایمیل و موبایل شما هنوز تایید نشده است";
                                ViewBag.Class = "alert alert-danger";
                                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                                return View();
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "به دلیل یکی از خطاهای زیر امکان ورود شما وجود ندارد: <br /> 1- نام کاربری یا ایمیل نادرست است <br /> 2- رمز عبور نادرست است <br /> 3- ورود شما به دلایل امنیتی امکان پذیر نیست <br /> 4- ایمیل و موبایل شما هنوز تایید نشده است";
                        ViewBag.Class = "alert alert-danger";
                        ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }
            catch
            {
                ViewBag.Message = "مشکلی در ورود رخ داد";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session["User"] = null;
            Session["Access"] = null;

            return RedirectToAction("Index", "Home");
        }

    }//end controller
}