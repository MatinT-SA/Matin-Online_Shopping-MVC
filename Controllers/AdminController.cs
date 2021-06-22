using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sourceiran_MVC.Models.Domains;


namespace Sourceiran_MVC.Controllers
{
    public class AdminController : Controller
    {
        DBSI db = new DBSI();

        UtilityFunction ut = new UtilityFunction();
         
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin)).SingleOrDefault();

                if (q == null)
                {
                    return RedirectToAction("Login", "Register");
                }
                else
                {
                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult ManageUser()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var quser = from a in db.Tbl_User
                                where a.UserName != "Admin"
                                select a;

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];
                    return View(quser);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult DetailsUser(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var quser = (from a in db.Tbl_User
                                 where a.ID.Equals(id)
                                 select a).SingleOrDefault();

                    return View(quser);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult DisableUser(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var quser = (from a in db.Tbl_User
                                 where a.Status.Equals(true) && a.ID.Equals(id)
                                 select a).SingleOrDefault();

                    if (quser != null)
                    {

                        quser.Status = false;
                        db.Tbl_User.Attach(quser);
                        db.Entry(quser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        TempData["Message"] = "این کاربر غیر فعال شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageUser");
                    }
                    else
                    {
                        TempData["Message"] = "متاسفانه مشکلی رخ داد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageUser");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult EnableUser(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var quser = (from a in db.Tbl_User
                                 where a.Status.Equals(false) && a.ID.Equals(id)
                                 select a).SingleOrDefault();

                    quser.Status = true;
                    db.Tbl_User.Attach(quser);
                    db.Entry(quser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["Message"] = "این کاربر فعال شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("ManageUser");
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult ChangeAccess(int id, string AccessName)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var quser = (from a in db.Tbl_User
                                 where a.ID.Equals(id)
                                 select a).SingleOrDefault();

                    quser.Access = AccessName;
                    db.Tbl_User.Attach(quser);
                    db.Entry(quser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["Message"] = "وضعیت دسترسی کاربر تغییر پیدا کرد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("ManageUser");
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpGet]
        public ActionResult ManageSlide()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qslide = (from a in db.Tbl_Slider
                                  orderby a.Sort
                                  select a).ToList();

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];
                    return View(qslide);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult DeleteSlide(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qdelslide = (from a in db.Tbl_Slider
                                     where a.ID.Equals(id)
                                     select a).SingleOrDefault();

                    db.Tbl_Slider.Remove(qdelslide);


                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "اسلایدر با موفقیت حذف شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageSlide");
                    }
                    else
                    {
                        TempData["Message"] = "متاسفانه اسلایدر حذف نشد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageSlide");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpGet]
        public ActionResult CreateSlide()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpPost]
        public ActionResult CreateSlide(Tbl_Slider s, HttpPostedFileBase ImageSlide)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        //ViewBag.Message = "فیلدهای لازم را وارد نمایید";
                        //ViewBag.Class = "alert alert-danger";
                        //ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                    else
                    {
                        if (ImageSlide == null)
                        {
                            ViewBag.Message = "تصویر را انتخاب کنید";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }

                        Random rnd = new Random();

                        Tbl_Slider t = new Tbl_Slider();
                        t.Enable = s.Enable;
                        t.Title = s.Title;
                        t.Url = s.Url;
                        t.Sort = s.Sort;

                        if (ImageSlide.ContentType != "image/jpeg")
                        {
                            ViewBag.Message = "به یکی از دلایل زیر خطا رخ داد <br /> 1- پسوند تصویر شما باید jpg باشد <br /> 2- نوع فایل انتخابی شما غیر مجاز است";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }
                        if (ImageSlide.ContentLength >= 100000)
                        {
                            ViewBag.Message = "حجم فایل انتخابی شما بالای 1 مگابایت می باشد";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }

                        t.Image = rnd.Next().ToString() + ".jpg";

                        ImageSlide.SaveAs(Path.Combine(Server.MapPath("~") + "Content/images/Slider/Slid/" + t.Image));

                        db.Tbl_Slider.Add(t);
                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            TempData["Message"] = "اسلایدر با موفقیت ایجاد شد";
                            TempData["Class"] = "alert alert-success";
                            TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                            return RedirectToAction("ManageSlide");
                        }
                        else
                        {
                            ViewBag.Message = "مشکلی رخ داد و اسلایدر ایجاد نشد";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult EditSlide(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }

                var qeditslider = (from a in db.Tbl_Slider
                                   where a.ID.Equals(id)
                                   select a).SingleOrDefault();

                return View(qeditslider);
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpPost]
        public ActionResult EditSlide(Tbl_Slider s, HttpPostedFileBase ImageSlide)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qeditslide2 = (from a in db.Tbl_Slider
                                       where a.ID.Equals(s.ID)
                                       select a).SingleOrDefault();

                    qeditslide2.Enable = s.Enable;
                    if (s.Url == null)
                    {
                        qeditslide2.Url = "";
                    }
                    else
                    {
                        qeditslide2.Url = s.Url;
                    }

                    qeditslide2.Title = s.Title;
                    qeditslide2.Sort = s.Sort;

                    if (ImageSlide != null)
                    {
                        if (ImageSlide.ContentType != "image/jpeg")
                        {
                            ViewBag.Message = "به یکی از دلایل زیر خطا رخ داد <br /> 1- پسوند تصویر شما باید jpg باشد <br /> 2- نوع فایل انتخابی شما غیر مجاز است";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }
                        if (ImageSlide.ContentLength >= 100000)
                        {
                            ViewBag.Message = "حجم فایل انتخابی شما بالای 1 مگابایت می باشد";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }

                        Random rnd = new Random();

                        qeditslide2.Image = rnd.Next().ToString() + ".jpg";

                        ImageSlide.SaveAs(Path.Combine(Server.MapPath("~") + "Content/images/Slider/Slid/" + qeditslide2.Image));
                    }
                    db.Tbl_Slider.Attach(qeditslide2);
                    db.Entry(qeditslide2).State = System.Data.Entity.EntityState.Modified;
                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "اسلایدر با موفقیت ویرایش شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageSlide");
                    }
                    else
                    {
                        ViewBag.Message = "مشکلی رخ داد و ویرایش اسلایدر صورت نگرفت";
                        ViewBag.Class = "alert alert-danger";
                        ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpGet]
        public ActionResult Status()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                var qstat = (from a in db.Tbl_Status
                             orderby a.Sort
                             where a.ID > 1
                             select a).ToList();

                ViewBag.Message = TempData["Message"];
                ViewBag.Style = TempData["Style"];
                ViewBag.Class = TempData["Class"];
                return View(qstat);
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpGet]
        public ActionResult CreateStatus()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpPost]
        public ActionResult CreateStatus(Tbl_Status st)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }
                    Tbl_Status s = new Tbl_Status();
                    s.Title = st.Title;
                    s.Sort = st.Sort;
                    db.Tbl_Status.Add(s);
                    db.SaveChanges();

                    TempData["Message"] = "وضعیت با موفقیت ثبت شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("Status");
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpGet]
        public ActionResult EditStatus(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }

                var qst = (from a in db.Tbl_Status
                           where a.ID.Equals(id)
                           select a).SingleOrDefault();

                return View(qst);
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpPost]
        public ActionResult EditStatus(Tbl_Status st)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }
                    var qst2 = (from a in db.Tbl_Status
                                where a.ID.Equals(st.ID)
                                select a).SingleOrDefault();

                    qst2.Sort = st.Sort;
                    qst2.Title = st.Title;
                    db.Tbl_Status.Attach(qst2);
                    db.Entry(qst2).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["Message"] = "وضعیت با موفقیت ویرایش شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("Status");
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult DeleteStatus(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    var qst = (from a in db.Tbl_Status
                               where a.ID.Equals(id)
                               select a).SingleOrDefault();
                    db.Tbl_Status.Remove(qst);
                    db.SaveChanges();

                    TempData["Message"] = "وضعیت با موفقیت حذف شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("Status");
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult ManageBill()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qbill = (from a in db.Tbl_Bills
                                 where a.Requested.Equals(true) && a.Tbl_User.Status.Equals(true)
                                 select a).ToList();

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];
                    return View(qbill);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpGet]
        public ActionResult DetailsBills(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qbill = (from a in db.Tbl_Bills
                                 where a.Requested.Equals(true) && a.Tbl_User.Status.Equals(true) && a.ID.Equals(id)
                                 select a).SingleOrDefault();

                    if (qbill != null)
                    {
                        ViewBag.Message = TempData["Message"];
                        ViewBag.Style = TempData["Style"];
                        ViewBag.Class = TempData["Class"];
                        return View(qbill);
                    }
                    else
                    {
                        TempData["Message"] = "صورت حسابی موجود نمی باشد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageBill");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult PayRequest(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qbill = (from a in db.Tbl_Bills
                                 where a.Requested.Equals(true) && a.Tbl_User.Status.Equals(true) && a.ID.Equals(id)
                                 select a).SingleOrDefault();

                    if (qbill != null)
                    {
                        //انتقال دادن مدیر بانک به درگاه بانکی برای پرداخت صورت حساب کاربر مربوطه

                        qbill.Requested = false;
                        qbill.Stock += qbill.AmountRequested;
                        qbill.EndReceive = qbill.AmountRequested;
                        qbill.DateEndReceive = DateTime.Now;

                        db.Tbl_Bills.Attach(qbill);
                        db.Entry(qbill).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        var mb = qbill.UserID;
                        var bank = db.Tbl_BankNum.Where(a => a.UserID.Equals(mb)).SingleOrDefault().BankNum;

                        Tbl_Message m = new Tbl_Message();

                        m.Date = DateTime.Now;
                        m.Read = false;
                        m.Text = "مبلغ درخواستی شما به حساب تان واریز شد. <br /> مبلغ " + qbill.AmountRequested + " تومان <br /> به شماره حساب: '" + bank + "'";
                        m.Title = "واریز مبلغ";
                        m.UserGet = qbill.Tbl_User.ID;


                        db.Tbl_Message.Add(m);
                        db.SaveChanges();

                        var uEmail = qbill.Tbl_User.Email;
                        var uUserName = qbill.Tbl_User.UserName;

                        ut.SendEmail(db.Tbl_Setting.FirstOrDefault().SMTP, db.Tbl_Setting.FirstOrDefault().Email, db.Tbl_Setting.FirstOrDefault().EmailPass, uEmail, "ایمیل پرداخت موفقیت آمیز صورت حساب شما در سایت '" + db.Tbl_Setting.FirstOrDefault().Title.ToString() + "'", "<div style='font-size: 20px;float: right;direction:rtl;'>کاربر گرامی " + uUserName + "، وقت شما بخیر. <br /> صورت حساب درخواست شده شما با موفقیت پرداخت شد و مبلغ به حساب شما واریز شده است. <br /> در صورت داشتن هر گونه سوال، به تیم پشتیبانی سایت ما پیام دهید تا در اسرع وقت پاسخگو باشیم. <br /> موفق باشید.</div>");

                        TempData["Message"] = "پیامی مبتنی بر واریز مبلغ از سوی شما به کاربر فروشنده ارسال شد. همچنین این کاربر از طریق ایمیل نیز باخبر خواهد شد.";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageBill");
                    }
                    else
                    {
                        TempData["Message"] = "خطایی رخ داد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageBill");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult ManageFilter()
        {
            if (Session["Admin"] != null)
            {

                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qf = (from a in db.Tbl_GroupFilter
                              select a).ToList();

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];
                    return View(qf);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpGet]
        public ActionResult CreateGFilter(int id = 0)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpPost]
        public ActionResult CreateGFilter(Tbl_GroupFilter tgf)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Tbl_GroupFilter g = new Tbl_GroupFilter();
                    g.ParentID = tgf.ParentID == 0 ? null : tgf.ParentID;
                    g.Title = tgf.Title;

                    db.Tbl_GroupFilter.Add(g);
                    db.SaveChanges();

                    if (g.ParentID == null)
                    {
                        var qs = (from a in db.Tbl_GroupFilter
                                  orderby a.ID descending
                                  select a).FirstOrDefault();

                        qs.ParentID = qs.ID;

                        db.Tbl_GroupFilter.Attach(qs);
                        db.Entry(qs).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        TempData["Message"] = "فیلتر اصلی با موفقیت ثبت شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageFilter");
                    }
                    return RedirectToAction("ManageFilter");
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult SubFilter()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qf = (from a in db.Tbl_GroupFilter
                              orderby a.ParentID descending
                              select a).ToList();

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];
                    return View(qf);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpGet]
        public ActionResult CreateSubFilter()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpPost]
        public ActionResult CreateSubFilter(Tbl_GroupFilter tgf, int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Tbl_GroupFilter g = new Tbl_GroupFilter();
                    g.ParentID = id;
                    g.Title = tgf.Title;

                    db.Tbl_GroupFilter.Add(g);
                    db.SaveChanges();

                    TempData["Message"] = "فیلتر فرعی با موفقیت ثبت شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("SubFilter");
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult Filters(int page = 1)
        {
            try
            {
                if (Session["Admin"] != null)
                {
                    string admin = Session["Admin"].ToString();

                    var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                    if (q == null)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        int Take = 6;
                        int Skip = (page * Take) - Take;

                        var qf = (from a in db.Tbl_Filter
                                  orderby a.GroupFilterID descending
                                  select a).ToList();

                        ViewBag.Count = qf.Count();
                        ViewBag.Take = Take;

                        if (qf.Count() > 0)
                        {
                            ViewBag.Message = TempData["Message"];
                            ViewBag.Style = TempData["Style"];
                            ViewBag.Class = TempData["Class"];
                            return View(qf.Skip(Skip).Take(Take));
                        }
                        else
                        {
                            ViewBag.Message = "فیلتری برای نمایش وجود ندارد";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }

                        ViewBag.Message = TempData["Message"];
                        ViewBag.Style = TempData["Style"];
                        ViewBag.Class = TempData["Class"];
                        return View(qf);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Register");
                }
            }
            catch
            {
                TempData["Message"] = "متاسفانه مشکلی رخ داد";
                TempData["Class"] = "alert alert-dander";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public ActionResult CreateFilters()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpPost]
        public ActionResult CreateFilters(Tbl_Filter tf, int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Tbl_Filter f = new Tbl_Filter();
                    f.GroupFilterID = id;
                    f.Title = tf.Title;
                    db.Tbl_Filter.Add(f);
                    db.SaveChanges();

                    TempData["Message"] = "فیلتر با موفقیت ثبت شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("Filters");
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult DeleteFilters(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qfl = (from a in db.Tbl_Filter
                               where a.ID.Equals(id)
                               select a).SingleOrDefault();

                    db.Tbl_Filter.Remove(qfl);


                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "فیلتر با موفقیت حذف شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("Filters");
                    }
                    else
                    {
                        TempData["Message"] = "متاسفانه فیلتر حذف نشد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("Filters");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult DeleteSub(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qgf = (from a in db.Tbl_GroupFilter
                               where a.ID.Equals(id)
                               select a).SingleOrDefault();

                    db.Tbl_GroupFilter.Remove(qgf);


                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "گروه فیلتر با موفقیت حذف شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("SubFilter");
                    }
                    else
                    {
                        TempData["Message"] = "متاسفانه گروه فیلتر حذف نشد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("SubFilter");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpGet]
        public ActionResult EditFilters(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qef = (from a in db.Tbl_Filter
                               where a.ID.Equals(id)
                               select a).SingleOrDefault();

                    return View(qef);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpPost]
        public ActionResult EditFilters(Tbl_Filter tf)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    var qef2 = (from a in db.Tbl_Filter
                                where a.ID.Equals(tf.ID)
                                select a).SingleOrDefault();

                    qef2.Title = tf.Title;

                    db.Tbl_Filter.Attach(qef2);
                    db.Entry(qef2).State = System.Data.Entity.EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "فیلتر با موفقیت ویرایش شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("Filters");
                    }
                    else
                    {
                        ViewBag.Message = "متاسفانه فیلتر ویرایش نشد";
                        ViewBag.Class = "alert alert-danger";
                        ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpGet]
        public ActionResult EditSub(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qegf = (from a in db.Tbl_GroupFilter
                                where a.ID.Equals(id)
                                select a).SingleOrDefault();

                    return View(qegf);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpPost]
        public ActionResult EditSub(Tbl_GroupFilter tgf)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qegf = (from a in db.Tbl_GroupFilter
                                where a.ID.Equals(tgf.ID)
                                select a).SingleOrDefault();

                    qegf.Title = tgf.Title;

                    db.Tbl_GroupFilter.Attach(qegf);
                    db.Entry(qegf).State = System.Data.Entity.EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "فیلتر فرعی با موفقیت ویرایش شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("SubFilter");
                    }
                    else
                    {
                        ViewBag.Message = "متاسفانه فیلتر فرعی ویرایش نشد";
                        ViewBag.Class = "alert alert-danger";
                        ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpGet]
        public ActionResult EditGFilter(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qeggf = (from a in db.Tbl_GroupFilter
                                 where a.ID.Equals(id)
                                 select a).SingleOrDefault();

                    return View(qeggf);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpPost]
        public ActionResult EditGFilter(Tbl_GroupFilter tgf)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qeggf = (from a in db.Tbl_GroupFilter
                                 where a.ID.Equals(tgf.ID)
                                 select a).SingleOrDefault();

                    qeggf.Title = tgf.Title;

                    db.Tbl_GroupFilter.Attach(qeggf);
                    db.Entry(qeggf).State = System.Data.Entity.EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "فیلتر اصلی با موفقیت ویرایش شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageFilter");
                    }
                    else
                    {
                        ViewBag.Message = "متاسفانه فیلتر اصلی ویرایش نشد";
                        ViewBag.Class = "alert alert-danger";
                        ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpGet]
        public ActionResult EditSettings()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qsetting = (from a in db.Tbl_Setting
                                    select a).FirstOrDefault();

                    return View(qsetting);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }
        [HttpPost]
        public ActionResult EditSettings(Tbl_Setting ts)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qs = (from a in db.Tbl_Setting
                              where a.ID.Equals(ts.ID)
                              select a).FirstOrDefault();

                    qs.Title = ts.Title;
                    qs.KeyWord = ts.KeyWord;
                    qs.SMTP = ts.SMTP;
                    qs.CountPage = ts.CountPage;
                    qs.Email = ts.Email;
                    qs.EmailPass = ts.EmailPass;
                    qs.Description = ts.Description;

                    db.Tbl_Setting.Attach(qs);
                    db.Entry(qs).State = System.Data.Entity.EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "تنظیمات با موفقیت ویرایش شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = "متاسفانه تنظیمات ویرایش نشد";
                        ViewBag.Class = "alert alert-danger";
                        ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(string Password, int ID)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qf = db.Tbl_User.Where(a => a.ID.Equals(ID)).SingleOrDefault();

                    qf.Password = Password;

                    db.Tbl_User.Attach(qf);
                    db.Entry(qf).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["Message"] = "رمز عبور شما با موفقیت تغییر پیدا کرد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult ListComments()
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qcom = (from a in db.Tbl_Comment
                                orderby a.Date ascending
                                where a.Confirm_Comm.Equals(false)
                                select a).ToList();

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];
                    return View(qcom);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult ConfirmComment(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qconfirmCom = (from a in db.Tbl_Comment
                                       where a.ID.Equals(id) && a.Confirm_Comm.Equals(false)
                                       select a).SingleOrDefault();

                    if (qconfirmCom != null)
                    {
                        qconfirmCom.Confirm_Comm = true;

                        db.Tbl_Comment.Attach(qconfirmCom);
                        db.Entry(qconfirmCom).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        TempData["Message"] = "کامنت مربوطه از سوی شما تایید شد و به نمایش در آمد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ListComments");
                    }
                    else
                    {
                        TempData["Message"] = "مشکلی رخ داد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ListComments");
                    }


                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult DetailsComment(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qdetailcomment = (from a in db.Tbl_Comment
                                          where a.ID.Equals(id) && a.Confirm_Comm.Equals(false)
                                          select a).SingleOrDefault();

                    return View(qdetailcomment);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult DeleteComment(int id)
        {
            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(admin) && a.Status.Equals(true) && a.Access.Equals("Admin"));

                if (q == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var qdelcomment = (from a in db.Tbl_Comment
                                       where a.ID.Equals(id) && a.Confirm_Comm.Equals(false)
                                       select a).SingleOrDefault();

                    if (qdelcomment != null)
                    {
                        db.Tbl_Comment.Remove(qdelcomment);
                        db.SaveChanges();

                        TempData["Message"] = "کامنت با موفقیت حذف شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ListComments");
                    }
                    else
                    {
                        TempData["Message"] = "مشکلی رخ داد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ListComments");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

    }//End Controller
}