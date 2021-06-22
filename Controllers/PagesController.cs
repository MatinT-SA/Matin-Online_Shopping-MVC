using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sourceiran_MVC.Models.Domains;

namespace Sourceiran_MVC.Controllers
{
    public class PagesController : Controller
    {
        DBSI db = new DBSI();
        // GET: Pages
        public ActionResult ListPages()
        {
            //دسترسی به این صفحه برای مدیر فروشگاه

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
                    var qs = (from a in db.Tbl_Menu
                              orderby a.Sort
                              select a).ToList();

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];
                    return View(qs);
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }


        }

        [HttpGet]
        public ActionResult CreatePage()
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
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpPost]
        public ActionResult CreatePage(Tbl_Menu tm)
        {
            //دسترسی

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
                    Tbl_Menu m = new Tbl_Menu();
                    m.ContentPage = tm.ContentPage;
                    m.Sort = tm.Sort;
                    m.Enable = true;
                    m.Description = tm.Description;
                    m.Tag = tm.Tag;
                    m.TitlePage = tm.TitlePage;

                    db.Tbl_Menu.Add(m);
                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "صفحه با موفقیت ثبت شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ListPages", "Pages");
                    }
                    else
                    {
                        ViewBag.Message = "نام صفحه تکراری است";
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

        public ActionResult DeletePage(string pagename)
        {
            //دسترسی

            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var qs = db.Tbl_User.Where(a => a.UserName.Equals(admin)).SingleOrDefault();

                if (qs == null)
                {
                    return RedirectToAction("Login", "Register");
                }
                else
                {
                    var q = (from a in db.Tbl_Menu
                             where a.TitlePage.Equals(pagename)
                             select a).SingleOrDefault();

                    if (q != null)
                    {
                        db.Tbl_Menu.Remove(q);
                        db.SaveChanges();

                        TempData["Message"] = "صفحه با موفقیت حذف شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ListPages");
                    }
                    else
                    {
                        TempData["Message"] = "مشکلی رخ داد و حذف صورت نگرفت";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ListPages");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpPost]
        public JsonResult TitleValid(string titlepage)
        {
            try
            {
                var title = (from a in db.Tbl_Menu
                             where a.TitlePage == titlepage
                             select a).SingleOrDefault();

                if (title != null)
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

        [HttpGet]
        public ActionResult EditPage(string pagename)
        {
            //دسترسی

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
                    var editpage = (from a in db.Tbl_Menu
                                    where a.TitlePage == pagename
                                    select a).SingleOrDefault();

                    if (editpage != null)
                    {
                        return View(editpage);
                    }
                    else
                    {
                        TempData["Message"] = "امکان ویرایش صفحه وجود ندارد";
                        TempData["Class"] = "alert alert-danger alert-dismissible";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return RedirectToAction("ListPages", "Pages");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [HttpPost]
        public ActionResult EditPage(Tbl_Menu menu)
        {
            //دسترسی

            if (Session["Admin"] != null)
            {
                string admin = Session["Admin"].ToString();

                var qs = db.Tbl_User.Where(a => a.UserName.Equals(admin)).SingleOrDefault();

                if (qs == null)
                {
                    return RedirectToAction("Login", "Register");
                }
                else
                {
                    var q = (from a in db.Tbl_Menu
                             where a.TitlePage == menu.TitlePage
                             select a).SingleOrDefault();

                    q.Description = menu.Description;
                    q.Sort = menu.Sort;
                    q.Tag = menu.Tag;
                    q.ContentPage = menu.ContentPage;
                    q.Enable = menu.Enable;

                    db.Tbl_Menu.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "ویرایش انجام شد";
                        TempData["Class"] = "alert alert-success alert-dismissible";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return RedirectToAction("ListPages", "Pages");
                    }
                    else
                    {
                        ViewBag.Message = "ویرایش با خطا مواجه شد";
                        ViewBag.Class = "alert alert-danger alert-dismissible";
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

        public ActionResult ShowContent(string pagename)
        {
            var q = (from a in db.Tbl_Menu
                     where a.TitlePage == pagename
                     select a).SingleOrDefault();

            if (q != null)
            {
                return View(q);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }//end controller
}