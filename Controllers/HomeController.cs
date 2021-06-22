using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sourceiran_MVC.Models.Repository;
using Sourceiran_MVC.Models.Domains;
using CaptchaMvc;
using CaptchaMvc.HtmlHelpers;


namespace Sourceiran_MVC.Controllers
{
    public class HomeController : Controller
    {
        DBSI db = new DBSI();

        public ActionResult Index()
        {
            //امنیت بالاتر

            //EndAuction.EndMozayede();

            return View();
        }

        public PartialViewResult ProductVisit()
        {
            RepositoryIndex Rep_ProductVisit = new RepositoryIndex();
            return PartialView("P_ProductVisit");
        }

        public PartialViewResult ProductVisitAuction()
        {
            RepositoryIndex Rep_ProductVisitAuction = new RepositoryIndex();
            return PartialView("P_ShowProductVisitAuction");
        }

        public PartialViewResult ProductNew()
        {
            RepositoryIndex Rep_ProductNew = new RepositoryIndex();
            return PartialView("P_ShowProductNew");
        }

        public PartialViewResult ProductNewAuction()
        {
            RepositoryIndex Rep_ProductNewAuction = new RepositoryIndex();
            return PartialView("P_ShowProductNewAuction");
        }

        public PartialViewResult Menu()
        {
            return PartialView("P_Category", MyClassMenu.MenuItem());
        }

        public ActionResult MessageRegister()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Style = TempData["Style"];
            ViewBag.Class = TempData["Class"];
            return View();
        }

        public ActionResult Product(int id)
        {
            var qProduct = (from a in db.Tbl_Products
                            where a.ID == id && (a.AuctionDate != null ? a.AuctionDate > DateTime.Now : 1 == 1) && (a.AuctionDate == null ? a.ExitCount > 0 : 1 == 1)
                            select a).SingleOrDefault();

            if (qProduct == null)
                return RedirectToAction("Index");

            qProduct.Visit = qProduct.Visit + 1;
            db.Tbl_Products.Attach(qProduct);
            db.Entry(qProduct).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            if (Session["User"] != null)
            {
                string users = Session["User"].ToString();
                var quser = db.Tbl_User.Where(a => a.UserName.Equals(users)).SingleOrDefault().ID;

                if (quser != 0)
                {
                    var qvisit = db.Tbl_Visit.Where(a => a.UserID.Equals(quser) && a.ProductID.Equals(id)).SingleOrDefault();

                    if (qvisit != null)
                    {
                        //یعنی این کاربر قبلا بازدید از این محصول داشته و از تکرار آن در پایگاه داده جلوگیری می کنیم. فقط یکبار
                        qvisit.Date = DateTime.Now;
                        db.Tbl_Visit.Attach(qvisit);
                        db.Entry(qvisit).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        //یعنی دفعه اول است که داره از این محصول این کاربر بازدید میکنه
                        Tbl_Visit tv = new Tbl_Visit();
                        tv.Date = DateTime.Now;
                        tv.ProductID = id;
                        tv.UserID = quser;

                        db.Tbl_Visit.Add(tv);
                        db.SaveChanges();
                    }
                }
            }

            ViewBag.Message = TempData["Message"];
            ViewBag.Style = TempData["Style"];
            ViewBag.Class = TempData["Class"];
            return View(qProduct);
        }

        public ActionResult Errors()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertComment(Tbl_Comment com)
        {
            try
            {
                //اعتبارسنجی فرم
                //اعتبارسنجی کد کاپچا
                if (!this.IsCaptchaValid("Error"))
                {
                    TempData["Message"] = "کد امنیتی را درست وارد کنید";
                    TempData["Class"] = "alert alert-danger";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("Product", new { id = com.ProductID });
                }
                Tbl_Comment c = new Tbl_Comment();

                c.Date = DateTime.Now;
                c.IP = Request.UserHostAddress.ToString();
                c.Confirm_Comm = false;
                c.ParentID = com.ParentID == 0 ? 1 : com.ParentID;
                c.ProductID = com.ProductID;
                c.Text = com.Text;
                c.Web = com.Web;
                c.Name = com.Name;
                c.Email = com.Email;

                db.Tbl_Comment.Add(c);
                db.SaveChanges();

                //اگر شرط زیر برقرار باشد، یعنی کامنت اصلی است

                if (com.ParentID == 1)
                {
                    var q = (from a in db.Tbl_Comment
                             orderby a.ID descending
                             select a).FirstOrDefault();

                    q.ParentID = q.ID;

                    db.Tbl_Comment.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                TempData["Message"] = "پیام شما در انتظار تاییدیه از سوی مدیر سایت است";
                TempData["Class"] = "alert alert-success";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("Product", new { id = com.ProductID });
            }
            catch
            {
                TempData["Message"] = "مشکلی رخ داد";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("Product", new { id = com.ProductID });
            }
        }

        public ActionResult Search(string Name, int Category)
        {
            if (Name == "")
            {
                return RedirectToAction("Index");
            }
            if (Category == 1)
            {
                //محصول عادی
                var qsearchNormal = (from a in db.Tbl_Products
                                     where a.Title.Contains(Name) && a.AuctionDate == null
                                     select a).OrderBy(a => a.Title);
                //صفحه بندی
                return View(qsearchNormal);
            }
            else
            {
                //محصول مزایده ای
                var qsearchAuction = (from a in db.Tbl_Products
                                      where a.Title.Contains(Name) && a.AuctionDate != null
                                      select a).OrderBy(a => a.Title);
                //صفحه بندی
                return View(qsearchAuction);
            }
        }

        public ActionResult Category(List<int> CheckFilter, decimal minpric = 0, decimal maxpric = 0, int topicid = 0, int page = 1)
        {
            List<Tbl_Products> lstpr = new List<Tbl_Products>();

            if (CheckFilter != null)
            {
                var qcheck = from a in db.Tbl_Products
                             join b in db.Tbl_Filter_Product on a.ID equals b.ProductID
                             where a.TopicID.Equals(topicid)
                             select b;

                foreach (var item in CheckFilter)
                {
                    var u = from a in qcheck
                            where a.FilterID.Equals(item)
                            select a;

                    if (u != null)
                    {
                        foreach (var item1 in u)
                        {
                            if (!lstpr.Contains(item1.Tbl_Products))
                                lstpr.Add(item1.Tbl_Products);
                        }
                    }
                }
            }
            else
            {
                var q = from a in db.Tbl_Products
                        where a.TopicID.Equals(topicid)
                        select a;
                lstpr.AddRange(q);
            }


            ViewBag.Checkfilter = CheckFilter != null ? CheckFilter : new List<int>() { 0 };
            ViewBag.Topic = topicid;
            ViewBag.Minprice = minpric <= 0 ? "1000" : minpric.ToString();
            ViewBag.Maxprice = maxpric <= 0 ? "" : maxpric.ToString();

            if (minpric >= 1000 && maxpric > 1000)
            {
                List<Tbl_Products> listpsroduct = new List<Tbl_Products>();
                listpsroduct.AddRange(from a in lstpr where a.Price >= minpric && a.Price <= maxpric select a);
                lstpr.Clear();
                lstpr.AddRange(listpsroduct);
            }

            int Take = db.Tbl_Setting.Select(a => a.CountPage).FirstOrDefault();
            int Count = lstpr.Count();

            ViewBag.Take = Take;
            ViewBag.Count = Count;

            int Skip = (Take * page) - Take;

            return View(lstpr.OrderBy(a => a.ID).Skip(Skip).Take(Take));
        }

        public ActionResult AddFilter(int TopicID)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            if (Request.IsAjaxRequest())
            {
                Tbl_Categories c = new Tbl_Categories();
                c.ID = TopicID;
                return PartialView("P_Filter", c);
            }
            else
            {
                TempData["Message"] = "شما دسترسی به این قسمت ندارید";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("CreateProduct");
            }
        }

    }//End controller
}