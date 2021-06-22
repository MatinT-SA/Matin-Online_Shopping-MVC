using Sourceiran_MVC.Models.Domains;
using Sourceiran_MVC.Models.Struct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc;
using CaptchaMvc.HtmlHelpers;

namespace Sourceiran_MVC.Controllers
{
    public class UserController : Controller
    {
        DBSI db = new DBSI();
        UtilityFunction ut = new UtilityFunction();

        // GET: User
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Style = TempData["Style"];
            ViewBag.Class = TempData["Class"];

            if (Session["User"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }

        }

        public ActionResult ConfirmForgotPass()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RecoveryPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecoveryPass(string Email)
        {
            var qFindPass = (from a in db.Tbl_User
                             where a.Email.Equals(Email)
                             select a.Password).SingleOrDefault();

            ut.SendEmail(db.Tbl_Setting.FirstOrDefault().SMTP, db.Tbl_Setting.FirstOrDefault().Email, db.Tbl_Setting.FirstOrDefault().EmailPass, Email, "بازیابی رمز عبور", "<div style='direction:rtl;font-size: 18px;color:#000;'> کاربر گرامی: '" + Email + "'<br /> سلام <br /> ایمیل به درخواست شما برای بازیابی رمز عبور سایت متین برای شما ارسال شده است. <br /> عبارتی که در خط پایین می بینید، رمز عبور همان ایمیلی است که وارد کردید: <br /> '" + qFindPass + "' <br /> موفق باشید. </div>");

            return RedirectToAction("ConfirmForgotPass", "User");
        }

        [HttpPost]
        public JsonResult AddCart(int ProductID = 0, int CountProc = 1)
        {
            try
            {
                var qc = db.Tbl_Products.Where(a => a.ID.Equals(ProductID)).SingleOrDefault().ExitCount;
                if (CountProc > qc || CountProc <= 0)
                {
                    return Json("", JsonRequestBehavior.DenyGet);
                }

                if (Session["User"] != null)
                {
                    if (ProductID == 0)
                    {
                        return Json("", JsonRequestBehavior.DenyGet);
                    }

                    string username = Session["User"].ToString();

                    Tbl_ShoppingCarts sc = new Tbl_ShoppingCarts();

                    sc.ProductID = ProductID;
                    sc.Status = false;
                    sc.Date = DateTime.Now;
                    sc.UserID = db.Tbl_User.Where(a => a.UserName.Equals(username)).SingleOrDefault().ID;
                    sc.Count = CountProc;

                    db.Tbl_ShoppingCarts.Add(sc);
                    db.SaveChanges();

                    var q = (from a in db.Tbl_ShoppingCarts
                             join b in db.Tbl_Products on a.ProductID equals b.ID
                             where a.Status.Equals(false) && a.Tbl_User.UserName.Equals(username)
                             select a).ToList();

                    if (q.Count() > 0)
                    {
                        int w = 0;
                        int pr = 0;

                        StructCart c = new StructCart();

                        c.AllCount = q.Count();

                        foreach (var item in q)
                        {
                            pr += (int)(item.Tbl_Products.Price * item.Count);
                        }
                        if (pr != 0)
                        {
                            //اضافه کردن مبلغ ارسال
                            c.SumPrice = pr;
                        }

                        List<Tbl_Cart> lsCart = new List<Tbl_Cart>();

                        foreach (var item1 in q)
                        {
                            Tbl_Cart cart = new Tbl_Cart();
                            cart.CountCart = item1.Count.ToString();
                            cart.namecart = item1.Tbl_Products.Title;
                            cart.price = (int)(item1.Tbl_Products.Price * item1.Count);
                            cart.id = item1.ID;

                            lsCart.Add(cart);
                        }

                        c.lsCart = lsCart;

                        foreach (var item2 in q)
                        {
                            w += (int)(item2.Tbl_Products.Weight * item2.Count);
                        }

                        c.SumWeight = w;

                        return Json(c, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("", JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    return Json("", JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult GetUpdate()
        {
            try
            {
                if (Session["User"] != null)
                {
                    string username = Session["User"].ToString();

                    var q = (from a in db.Tbl_ShoppingCarts
                             join b in db.Tbl_Products on a.ProductID equals b.ID
                             where a.Status.Equals(false) && a.Tbl_User.UserName.Equals(username)
                             select a).ToList();

                    if (q.Count() > 0)
                    {
                        int w = 0;
                        int pr = 0;

                        StructCart c = new StructCart();

                        c.AllCount = q.Count();

                        foreach (var item in q)
                        {
                            pr += (int)(item.Tbl_Products.Price * item.Count);
                        }
                        if (pr != 0)
                        {
                            //اضافه کردن مبلغ ارسال
                            c.SumPrice = pr;
                        }

                        List<Tbl_Cart> lsCart = new List<Tbl_Cart>();

                        foreach (var item1 in q)
                        {
                            Tbl_Cart cart = new Tbl_Cart();
                            cart.CountCart = item1.Count.ToString();
                            cart.namecart = item1.Tbl_Products.Title;
                            cart.price = (int)(item1.Tbl_Products.Price * item1.Count);
                            cart.id = item1.ID;

                            lsCart.Add(cart);
                        }

                        c.lsCart = lsCart;

                        foreach (var item2 in q)
                        {
                            w += (int)(item2.Tbl_Products.Weight * item2.Count);
                        }

                        c.SumWeight = w;

                        return Json(c, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("", JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    return Json("", JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult GetCount()
        {
            try
            {
                if (Session["User"] != null)
                {
                    string username = Session["User"].ToString();

                    var q = (from a in db.Tbl_ShoppingCarts
                             join b in db.Tbl_Products on a.ProductID equals b.ID
                             where a.Status.Equals(false) && a.Tbl_User.UserName.Equals(username)
                             select a).ToList();

                    if (q.Count() > 0)
                    {
                        StructCart c = new StructCart();

                        c.AllCount = q.Count();

                        return Json(c, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("", JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    return Json("", JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult RemoveShoppingCart(int id)
        {
            try
            {
                if (Session["User"] != null)
                {
                    string username = Session["User"].ToString();

                    var q = (from a in db.Tbl_ShoppingCarts
                             join b in db.Tbl_Products on a.ProductID equals b.ID
                             where a.ID.Equals(id) && a.Tbl_User.UserName.Equals(username)
                             select a).SingleOrDefault();

                    if (q == null)
                    {
                        return Json("", JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        db.Tbl_ShoppingCarts.Remove(q);
                        db.SaveChanges();
                    }

                    var qUpdate = (from a in db.Tbl_ShoppingCarts
                                   join b in db.Tbl_Products on a.ProductID equals b.ID
                                   where a.Status.Equals(false) && a.Tbl_User.UserName.Equals(username)
                                   select a).ToList();

                    if (qUpdate.Count() > 0)
                    {
                        int w = 0;
                        int pr = 0;

                        StructCart c = new StructCart();

                        c.AllCount = qUpdate.Count();

                        foreach (var item in qUpdate)
                        {
                            pr += (int)(item.Tbl_Products.Price * item.Count);
                        }
                        if (pr != 0)
                        {
                            //اضافه کردن مبلغ ارسال
                            c.SumPrice = pr;
                        }

                        List<Tbl_Cart> lsCart = new List<Tbl_Cart>();

                        foreach (var item1 in qUpdate)
                        {
                            Tbl_Cart cart = new Tbl_Cart();
                            cart.CountCart = item1.Count.ToString();
                            cart.namecart = item1.Tbl_Products.Title;
                            cart.price = (int)(item1.Tbl_Products.Price * item1.Count);
                            cart.id = item1.ID;

                            lsCart.Add(cart);
                        }

                        c.lsCart = lsCart;

                        foreach (var item2 in qUpdate)
                        {
                            w += (int)(item2.Tbl_Products.Weight * item2.Count);
                        }

                        c.SumWeight = w;

                        return Json(c, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        int w = 0;
                        int pr = 0;

                        StructCart cEmpty = new StructCart();

                        cEmpty.AllCount = 0;

                        foreach (var item in qUpdate)
                        {
                            pr += 0;
                        }
                        if (pr != 0)
                        {
                            //اضافه کردن مبلغ ارسال
                            cEmpty.SumPrice = pr;
                        }

                        List<Tbl_Cart> lsCart = new List<Tbl_Cart>();

                        foreach (var item1 in qUpdate)
                        {
                            Tbl_Cart cart = new Tbl_Cart();
                            cart.CountCart = null;
                            cart.namecart = null;
                            cart.price = 0;
                            cart.id = 0;

                            lsCart.Add(cart);
                        }

                        cEmpty.lsCart = lsCart;

                        foreach (var item2 in qUpdate)
                        {
                            w += 0;
                        }

                        cEmpty.SumWeight = w;
                        return Json(cEmpty, JsonRequestBehavior.DenyGet);
                    }

                }
                else
                {
                    return Json("", JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult PayShopping()
        {
            try
            {
                if (Session["User"] != null)
                {
                    string username = Session["User"].ToString();

                    var q = (from a in db.Tbl_ShoppingCarts
                             join b in db.Tbl_Products on a.ProductID equals b.ID
                             where a.Status.Equals(false) && a.Tbl_User.UserName.Equals(username)
                             select a).ToList();

                    double price = 0;
                    foreach (var item in q)
                    {
                        price += (int)item.Count * item.Tbl_Products.Price;
                    }

                    if (price >= 1000)
                    {
                        PayLine Pay = new PayLine();
                        double amount = (price) * 10;
                        string result = Pay.Send("http://payline.ir/payment-test/gateway-send", "adxcv-zzadq-polkjsad-opp13opoz-1sdf455aadzmck1244567", amount, "https://localhost:44346/User/CompletePay");
                        if (int.Parse(result) > 0)
                        {
                            List<Tbl_TempShoppingCart> lstshop = new List<Tbl_TempShoppingCart>();
                            foreach (var item in q)
                            {
                                Tbl_TempShoppingCart t = new Tbl_TempShoppingCart();
                                t.BankNo = result;
                                t.Date = DateTime.Now;
                                t.ShoppingProductID = item.ID;
                                t.Status = false;

                                lstshop.Add(t);
                            }

                            db.Tbl_TempShoppingCart.AddRange(lstshop.AsEnumerable());
                            db.SaveChanges();

                            return Redirect("http://payline.ir/payment-test/gateway-" + result);
                        }
                        else
                        {
                            //if -1 or -2 or -3 or -4 
                            //Can use swich case For Reports
                            // Response.Write("Not Valid");
                            ViewBag.Error = "خطا رخ داد";
                        }
                    }
                    else
                    {
                        return RedirectToAction("Errors");
                    }

                }
                else
                {
                    return RedirectToAction("Login", "Register");
                }
            }
            catch
            {
                return RedirectToAction("Errors", "User");
            }
            return View();
        }

        public ActionResult CompletePay(string id_get, string trans_id)
        {
            if (Session["User"] != null)
            {
                string username = Session["User"].ToString();

                PayLine GetPayLine = new PayLine();

                string result = GetPayLine.Get("http://payline.ir/payment-test/gateway-result-second", "adxcv-zzadq-polkjsad-opp13opoz-1sdf455aadzmck1244567", trans_id, id_get);

                if (int.Parse(result) > 0)
                {
                    var q = (from a in db.Tbl_TempShoppingCart
                             where a.BankNo.Equals(id_get)
                             select a).ToList();

                    foreach (var item in q)
                    {
                        item.Status = true;
                        db.Tbl_TempShoppingCart.Attach(item);
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();

                    List<Tbl_Sales> lstsales = new List<Tbl_Sales>();
                    foreach (var item in q)
                    {
                        Tbl_Sales t = new Tbl_Sales();
                        t.Date = DateTime.Now;
                        t.GroupNo = "";
                        t.Payment = true;
                        t.Price = (int)(item.Tbl_ShoppingCarts.Tbl_Products.Price * item.Tbl_ShoppingCarts.Count);
                        t.ProductID = item.Tbl_ShoppingCarts.ProductID;
                        t.Status = 1;
                        t.UserID = item.Tbl_ShoppingCarts.UserID;
                        t.Count = (int)item.Tbl_ShoppingCarts.Count;
                        t.TransNo = trans_id;
                        t.CodeRahgiri = "";
                        t.TransNo = trans_id;
                        lstsales.Add(t);
                    }
                    //var sumprice = lstsales.Sum(a => a.Price);

                    db.Tbl_Sales.AddRange(lstsales.AsEnumerable());
                    db.SaveChanges();

                    List<int> lstshop = new List<int>();
                    foreach (var item1 in q)
                    {
                        lstshop.Add(item1.ShoppingProductID);
                    }
                    foreach (var item2 in lstshop)
                    {
                        db.Tbl_ShoppingCarts.Remove(db.Tbl_ShoppingCarts.Where(a => a.ID.Equals(item2)).SingleOrDefault());
                        db.SaveChanges();
                    }

                    foreach (var item in lstsales)
                    {
                        var qs = (from a in db.Tbl_Products
                                  where a.ID.Equals(item.ProductID)
                                  select a).SingleOrDefault();

                        qs.ExitCount = qs.ExitCount - item.Count;//کمتر از صفر نشود
                        db.Tbl_Products.Attach(qs);
                        db.Entry(qs).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();

                    var su = db.Tbl_Sales.Where(a => a.TransNo.Equals(trans_id));
                    return View(su);
                }
                else
                {
                    return View();
                }

            }
            else
            {
                return View();
            }

        }

        public ActionResult Errors()
        {
            return View();
        }

        public JsonResult RecordPrice(int PriceAuc, int ProductID)
        {
            if (Session["User"] != null)
            {
                string user = Session["User"].ToString();

                var result = (from a in db.Tbl_Auction
                              where a.ProductID == ProductID && a.Tbl_User.UserName == user
                              select a).SingleOrDefault();

                if (result != null)
                {
                    //قبلا ثبت قیمت پیشنهادی دارد یا خیر
                    var Max = db.Tbl_Auction.Where(a => a.ProductID.Equals(ProductID)).OrderByDescending(a => a.Price).FirstOrDefault().Price;

                    if (Max < PriceAuc)
                    {
                        if ((PriceAuc - Max) >= 1000)
                        {
                            result.Price = PriceAuc;
                            db.Tbl_Auction.Attach(result);
                            db.Entry(result).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            string td = "";

                            var q = (from a in db.Tbl_Auction
                                     where a.ProductID.Equals(ProductID)
                                     orderby a.Price descending
                                     select a).ToList().Skip(0).Take(5);

                            foreach (var item in q)
                            {
                                td += "<tr>";
                                td += "<td>" + item.Tbl_User.UserName + "</td>";
                                td += "<td>" + item.Price + " تومان </td>";
                                td += "</tr>";
                            }
                            return Json(new { txt = td, success = "" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("", JsonRequestBehavior.DenyGet);
                        }
                    }
                    else
                    {
                        string td = "";

                        var q = (from a in db.Tbl_Auction
                                 where a.ProductID.Equals(ProductID)
                                 orderby a.Price descending
                                 select a).ToList().Skip(0).Take(5);

                        foreach (var item in q)
                        {
                            td += "<tr>";

                            td += "<td>" + item.Tbl_User.UserName + "</td>";
                            td += "<td>" + item.Price + " تومان </td>";

                            td += "</tr>";
                        }
                        return Json(new { txt = td, error = "" }, JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    var resultPrice = db.Tbl_Products.Where(a => a.ID.Equals(ProductID)).SingleOrDefault();
                    if (PriceAuc > resultPrice.Price)
                    {
                        Tbl_Auction t = new Tbl_Auction();
                        t.Price = PriceAuc;
                        t.ProductID = ProductID;
                        t.UserID = db.Tbl_User.Where(a => a.UserName.Equals(user)).SingleOrDefault().ID;
                        t.ConfirmPrice = false;
                        db.Tbl_Auction.Add(t);
                        db.SaveChanges();

                        string td = "";

                        var q1 = (from a in db.Tbl_Auction
                                  where a.ProductID.Equals(ProductID)
                                  orderby a.Price descending
                                  select a).ToList().Skip(0).Take(5);

                        foreach (var item in q1)
                        {

                            td += "<tr>";

                            td += "<td>" + item.Tbl_User.UserName + "</td>";
                            td += "<td>" + item.Price + " تومان </td>";

                            td += "</tr>";


                        }

                        return Json(new { txt = td, success = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string td = "";

                        var q = (from a in db.Tbl_Auction
                                 where a.ProductID.Equals(ProductID)
                                 orderby a.Price descending
                                 select a).ToList().Skip(0).Take(5);

                        foreach (var item in q)
                        {
                            td += "<tr>";

                            td += "<td>" + item.Tbl_User.UserName + "</td>";
                            td += "<td>" + item.Price + " تومان </td>";

                            td += "</tr>";
                        }
                        return Json(new { txt = td, error = "" }, JsonRequestBehavior.DenyGet);
                    }
                }
            }
            else
            {
                return Json("", JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult ListAuction(int page = 1)
        {
            if (Session["User"] != null)
            {
                string user = Session["User"].ToString();

                int take = 3;
                int skip = (page * take) - take;

                var q = (from a in db.Tbl_Sales
                         where a.Tbl_User.UserName.Equals(user) && a.Payment.Equals(false)
                         select a).ToList().OrderByDescending(a => a.Date);

                if (q.Count() > 0)
                {
                    //پیام رسانی برای به اتمام رسیدن مهلت پرداخت هزینه

                    ViewBag.Take = take;
                    ViewBag.Count = q.Count();

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];

                    return View(q.Skip(skip).Take(take));
                }
                else
                {
                    ViewBag.Message = "شما تاکنون مزایده ای برنده نشده اید";
                    ViewBag.Class = "alert alert-danger alert-dismissible";
                    ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public ActionResult PaymentAuction(int id)
        {
            try
            {
                if (Session["User"] != null)
                {
                    string user = Session["User"].ToString();

                    var q = (from a in db.Tbl_Sales
                             where a.Tbl_User.UserName.Equals(user) && a.ID.Equals(id)
                             select a).SingleOrDefault();

                    if (q == null)
                    {
                        TempData["Message"] = "مزایده شما قابل پرداخت نمی باشد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ListAuction", "User");
                    }

                    double price = q.Price;
                    if (price >= 1000)
                    {
                        PayLine Pay = new PayLine();
                        double amount = price * 10;
                        string result = Pay.Send("http://payline.ir/payment-test/gateway-send", "adxcv-zzadq-polkjsad-opp13opoz-1sdf455aadzmck1244567", amount, "https://localhost:44346/User/CompleteAuction");
                        if (int.Parse(result) > 0)
                        {
                            q.BankNo = result;
                            db.Tbl_Sales.Attach(q);
                            db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            return Redirect("http://payline.ir/payment-test/gateway-" + result);
                        }
                        else
                        {
                            TempData["Message"] = "در عملیات پرداخت مشکلی رخ داده";
                            TempData["Class"] = "alert alert-danger";
                            TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                            return RedirectToAction("ListAuction", "User");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "مبلغ شما برای پرداخت کمتر از حد مجاز می باشد";
                        TempData["Class"] = "alert alert-danger";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ListAuction", "User");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Register");
                }
            }
            catch
            {
                TempData["Message"] = "در عملیات پرداخت مشکلی رخ داده";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("ListAuction", "User");
            }
        }

        public ActionResult CompleteAuction(string id_get, string trans_id)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                var q = (from a in db.Tbl_Sales
                         where a.Tbl_User.UserName.Equals(user) && a.Payment.Equals(false) && a.BankNo.Equals(id_get)
                         select a).SingleOrDefault();

                if (q == null)
                {
                    TempData["Message"] = "در عملیات پرداخت مشکلی رخ داده";
                    TempData["Class"] = "alert alert-danger";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("ListAuction", "User");
                }
                else
                {
                    q.Payment = true;
                    q.TransNo = trans_id;
                    db.Tbl_Sales.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    int proid = q.ProductID;

                    var userid = db.Tbl_Products.Where(a => a.ID.Equals(proid)).SingleOrDefault().UserID;

                    var email = db.Tbl_User.Where(a => a.ID.Equals(userid)).SingleOrDefault().Email;

                    var name = db.Tbl_User.Where(a => a.ID.Equals(userid)).SingleOrDefault().UserName;

                    UtilityFunction u = new UtilityFunction();

                    string title = "خرید محصول شما در سایت " + db.Tbl_Setting.FirstOrDefault().Title + "";

                    string text = "کاربر گرامی " + name + " محصول شما در سایت " + db.Tbl_Setting.FirstOrDefault().Title + " به فروش رسید. لطفا اقدام لازم جهت ارسال محصول را انجام دهید";

                    //ایمیل به فروشنده
                    u.SendEmail(db.Tbl_Setting.FirstOrDefault().SMTP, db.Tbl_Setting.FirstOrDefault().Email, db.Tbl_Setting.FirstOrDefault().EmailPass, email, title, text);

                    int iduser = q.UserID;

                    var emailbuyer = db.Tbl_User.Where(a => a.ID.Equals(iduser)).SingleOrDefault().Email;

                    var namebuyer = db.Tbl_User.Where(a => a.ID.Equals(iduser)).SingleOrDefault().UserName;

                    string titlebuyer = "خرید موفق در سایت " + db.Tbl_Setting.FirstOrDefault().Title + "";

                    string textbuyer = "کاربر گرامی " + namebuyer + " خرید شما در سایت " + db.Tbl_Setting.FirstOrDefault().Title + " با موفقیت انجام شد. ارسال محصول شما در دست اقدام می باشد";
                    //ایمیل به خریدار
                    u.SendEmail(db.Tbl_Setting.FirstOrDefault().SMTP, db.Tbl_Setting.FirstOrDefault().Email, db.Tbl_Setting.FirstOrDefault().EmailPass, emailbuyer, titlebuyer, textbuyer);

                    return RedirectToAction("Index", "User");
                }
            }
            catch
            {
                TempData["Message"] = "در عملیات پرداخت مشکلی رخ داده";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("ListAuction", "User");
            }
        }

        public ActionResult HistoryBuy()
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                var q = (from a in db.Tbl_Sales
                         where a.Payment.Equals(true) && a.Tbl_User.UserName.Equals(user)
                         select a).ToList().OrderByDescending(a => a.Date);

                if (q.Count() > 0)
                {
                    return View(q);
                }
                else
                {
                    //یعنی کاربر من تاکنون خریدی را انجام نداده است
                    ViewBag.Message = "شما تاکنون خریدی  نداشته اید";
                    ViewBag.Class = "alert alert-danger";
                    ViewBag.Style = "padding-top: 10px;margin-top: 15px;";
                    return View();
                }
            }
            catch
            {
                ViewBag.Message = "مشکلی رخ داده است";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "padding-top: 10px;margin-top: 15px;";
                return View();
            }
        }

        public ActionResult DetailsBuy(int id)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "User");

                string user = Session["User"].ToString();

                var q = (from a in db.Tbl_Sales
                         where a.ID.Equals(id) && a.Tbl_User.UserName.Equals(user)
                         select a).SingleOrDefault();

                if (q != null)
                {
                    return View(q);
                }
                else
                {
                    TempData["Message"] = "مشکلی رخ داد!";
                    TempData["Class"] = "alert alert-danger";
                    TempData["Style"] = "padding-top: 10px;margin-top: 15px;";
                    return RedirectToAction("HistoryBuy", "User");
                }
            }
            catch
            {
                TempData["Message"] = "مشکلی رخ داد!";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "padding-top: 10px;margin-top: 15px;";
                return RedirectToAction("HistoryBuy", "User");
            }
        }

        public ActionResult HistorySales()
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                var q = (from a in db.Tbl_Products
                         join b in db.Tbl_Sales on a.ID equals b.ProductID
                         where a.Tbl_User.UserName.Equals(user) && b.Payment.Equals(true)
                         select b).ToList().OrderByDescending(a => a.Date);

                if (q.Count() > 0)
                {
                    ViewBag.Message = TempData["Message"];
                    ViewBag.Class = TempData["Class"];
                    ViewBag.Style = TempData["Style"];

                    return View(q);
                }
                else
                {
                    ViewBag.Message = "شما تاکنون فروشی نداشته اید";
                    ViewBag.Class = "alert alert-danger";
                    ViewBag.Style = "padding-top: 10px;margin-top: 15px;";
                    return View();
                }
            }
            catch
            {
                ViewBag.Message = "مشکلی رخ داده است";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "padding-top: 10px;margin-top: 15px;";
                return View();
            }
        }

        public ActionResult DetailsSales(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index", "User");
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                var q = (from a in db.Tbl_Products
                         join b in db.Tbl_Sales on a.ID equals b.ProductID
                         where a.Tbl_User.UserName.Equals(user) && b.Payment.Equals(true) && b.ID.Equals(id)
                         select b).SingleOrDefault();

                if (q != null)
                {
                    return View(q);
                }
                else
                {
                    TempData["Message"] = "مشکلی رخ داد!";
                    TempData["Class"] = "alert alert-danger";
                    TempData["Style"] = "padding-top: 10px;margin-top: 15px;";
                    return RedirectToAction("HistorySales", "User");
                }
            }
            catch
            {
                TempData["Message"] = "مشکلی رخ داد!";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "padding-top: 10px;margin-top: 15px;";
                return RedirectToAction("HistorySales", "User");
            }
        }

        public ActionResult ChangeStatus(int id, int StID, string Code)
        {

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            var q = (from a in db.Tbl_Products
                     join b in db.Tbl_Sales on a.ID equals b.ProductID
                     where a.Tbl_User.UserName.Equals(user) && b.Payment.Equals(true) && b.ID.Equals(id)
                     select b).SingleOrDefault();

            q.Status = StID;
            q.CodeRahgiri = Code;

            db.Tbl_Sales.Attach(q);
            db.Entry(q).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return View("DetailsSales", q);
        }

        public ActionResult Bills()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            var q = from a in db.Tbl_Sales
                    where a.Tbl_Products.Tbl_User.UserName.Equals(user) && a.Payment.Equals(true)
                    select a;

            List<Tbl_Sales> lstsales = new List<Tbl_Sales>();
            foreach (var item in q)
            {
                lstsales.Add(item);
            }

            if (lstsales.Count() > 0)
            {
                var qbill = (from a in db.Tbl_Bills
                             where a.Tbl_User.UserName.Equals(user)
                             select a).SingleOrDefault();

                if (qbill != null)
                {
                    //دفعات مکرر
                    Sourceiran_MVC.Models.Struct.StructBills b = new StructBills();
                    UtilityFunction u = new UtilityFunction();

                    b.AllReceive = qbill.Stock;
                    b.EndReceive = qbill.EndReceive;
                    b.EndDateReceive = u.Shamsi(qbill.DateEndReceive).ToShortDateString();
                    b.Stock = q.Sum(a => a.Price) - qbill.Stock;
                    b.RealStock = qbill.Stock - 2000;

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Class = TempData["Class"];
                    ViewBag.Style = TempData["Style"];

                    return View(b);
                }
                else
                {
                    //دفعه اول برای ایجاد صورت حساب
                    Sourceiran_MVC.Models.Struct.StructBills b = new StructBills();
                    UtilityFunction u = new UtilityFunction();

                    b.AllReceive = qbill != null ? qbill.Stock : 0;
                    b.EndReceive = qbill != null ? qbill.EndReceive : 0;
                    b.EndDateReceive = qbill != null ? u.Shamsi(qbill.DateEndReceive).ToShortDateString() : 0.ToString();
                    b.Stock = qbill != null ? (q.Sum(a => a.Price) - (qbill.Stock != 0 ? qbill.Stock : 0)) : 0;
                    b.RealStock = qbill != null ? (qbill.Stock != 0 ? qbill.Stock : 0) : 0;

                    ViewBag.Message = TempData["Message"];
                    ViewBag.Class = TempData["Class"];
                    ViewBag.Style = TempData["Style"];

                    return View(b);
                }
            }
            else
            {
                ViewBag.Message = "شما تاکنون فروشی  نداشته اید";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "padding-top: 10px;margin-top: 15px;";
                return View();
            }
        }

        public ActionResult RequestPrice(int PriceRequest)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            //شرط اینکه اطلاعات بانکی ثبت شده باشد

            var us = db.Tbl_User.Where(a => a.UserName.Equals(user)).SingleOrDefault().ID;

            var bank = db.Tbl_BankNum.Where(a => a.UserID.Equals(us)).SingleOrDefault();

            if (bank == null)
            {
                TempData["Message"] = "اطلاعات بانکی شما ثبت نشده است";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "padding-top: 10px;margin-top: 15px;";
                return RedirectToAction("Bills");
            }
            else
            {
                var qbill = (from a in db.Tbl_Bills
                             where a.Tbl_User.UserName.Equals(user)
                             select a).SingleOrDefault();

                if (qbill != null)
                {
                    var q = (from a in db.Tbl_Sales
                             where a.Tbl_Products.Tbl_User.UserName.Equals(user) && a.Payment.Equals(true)
                             select a).ToList();

                    List<Tbl_Sales> lstsales = new List<Tbl_Sales>();
                    foreach (var item in q)
                    {
                        lstsales.Add(item);
                    }

                    if (lstsales.Count() > 0)
                    {
                        Sourceiran_MVC.Models.Struct.StructBills b = new StructBills();

                        b.Stock = q.Sum(a => a.Price) - (qbill.Stock != 0 ? qbill.Stock : 0);
                        int f = b.RealStock = b.Stock != 0 ? b.Stock - 2000 : 0;

                        if (PriceRequest <= f)
                        {
                            qbill.AmountRequested = PriceRequest;
                        }
                        else
                        {
                            TempData["Message"] = "مبلغ درخواستی شما بیش از موجودی قابل پرداخت می باشد";
                            TempData["Class"] = "alert alert-danger";
                            TempData["Style"] = "padding-top: 10px;margin-top: 15px;";
                            return RedirectToAction("Bills", "User");
                        }

                        qbill.Requested = true;

                        db.Tbl_Bills.Attach(qbill);
                        db.Entry(qbill).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        TempData["Message"] = "درخواست شما به ثبت رسید";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "padding-top: 10px;margin-top: 15px;";
                        return RedirectToAction("Bills", "User");
                    }
                    else
                    {
                        ViewBag.Message = "شما تاکنون فروشی  نداشته اید";
                        ViewBag.Class = "alert alert-danger";
                        ViewBag.Style = "padding-top: 10px;margin-top: 15px;";
                        return RedirectToAction("Bills", "User");
                    }
                }
                else
                {
                    //برای کاربری که تا الان صورت حساب نداشته ولی دفعه اول داره درخواست میده
                    Tbl_Bills b = new Tbl_Bills();
                    b.Requested = true;
                    b.EndReceive = 0;
                    b.DateEndReceive = DateTime.Now;
                    b.Stock = 0;
                    b.UserID = db.Tbl_User.Where(a => a.UserName.Equals(user)).SingleOrDefault().ID;
                    b.AmountRequested = PriceRequest;

                    db.Tbl_Bills.Add(b);
                    db.SaveChanges();

                    TempData["Message"] = "درخواست شما با موفقیت ثبت شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "padding-top: 10px;margin-top: 15px;";
                    return RedirectToAction("Bills", "User");
                }
            }


        }

        [HttpGet]
        public ActionResult Addresses()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            var q = (from a in db.Tbl_User
                     where a.UserName.Equals(user) && a.Status.Equals(true) && a.Access.Equals("User")
                     select a).SingleOrDefault();

            if (q != null)
            {
                return View(q);
            }
            else
            {
                TempData["Message"] = "متاسفانه مشکلی رخ داد";
                TempData["Class"] = "alert alert-dander";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Addressesedit()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            var q = (from a in db.Tbl_User
                     where a.UserName.Equals(user) && a.Status.Equals(true) && a.Access.Equals("User")
                     select a).SingleOrDefault();

            if (q != null)
            {
                return View(q);
            }
            else
            {
                TempData["Message"] = "متاسفانه مشکلی رخ داد";
                TempData["Class"] = "alert alert-dander";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return RedirectToAction("Addresses");
            }
        }

        [HttpPost]
        public ActionResult Addressesedit(Tbl_User u)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(user)).SingleOrDefault();
                var pass = db.Tbl_User.Where(a => a.UserName.Equals(user)).SingleOrDefault().Password;

                Tbl_User us = new Tbl_User();
                q.Name = u.Name;
                q.Address = u.Address;
                q.City = u.City;
                q.Shire = u.Shire;
                q.PostalCode = u.PostalCode;
                q.Phone = u.Phone;
                q.Password = u.Password;

                db.Tbl_User.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    if (q.Password != pass)
                    {
                        TempData["Message"] = "ویرایش با موفقیت انجام شد. با رمز عبور جدید، مجدد وارد شوید";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return RedirectToAction("Login", "Register");
                    }
                    else
                    {
                        ViewBag.Message = "ویرایش شما با موفقیت انجام شد";
                        ViewBag.Class = "alert alert-success";
                        ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "ویرایش شما با خطا مواجه شد";
                    ViewBag.Class = "alert alert-danger";
                    ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return View();
                }
            }

            catch
            {
                ViewBag.Message = "ویرایش شما با خطا مواجه شد";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditPass()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            var q = (from a in db.Tbl_User
                     where a.UserName.Equals(user) && a.Access.Equals("User") && a.Status.Equals(true)
                     select a).SingleOrDefault();

            if (q != null)
            {
                return View(q);
            }
            else
            {
                TempData["Message"] = "متاسفانه مشکلی رخ داد";
                TempData["Class"] = "alert alert-dander";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult EditPass(int id)
        {
            return View();
        }

        public ActionResult ManageProducts(int page = 1)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                var q = db.Tbl_User.Where(a => a.UserName.Equals(user)).SingleOrDefault();

                int Take = 3;
                int Skip = (page * Take) - Take;

                var getpro = (from a in db.Tbl_Products
                              where a.UserID.Equals(q.ID)
                              select a).ToList().OrderByDescending(a => a.Date);

                ViewBag.Count = getpro.Count();
                ViewBag.Take = Take;

                if (getpro.Count() > 0)
                {
                    ViewBag.Message = TempData["Message"];
                    ViewBag.Style = TempData["Style"];
                    ViewBag.Class = TempData["Class"];
                    return View(getpro.Skip(Skip).Take(Take));
                }
                else
                {
                    ViewBag.Message = "شما تاکنون محصولی برای فروش نداشته اید";
                    ViewBag.Class = "alert alert-danger";
                    ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return View();
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

        public ActionResult Start(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();
            DateTime? dt = DateTime.Now;
            var q = db.Tbl_Products.Where(a => a.Tbl_User.UserName.Equals(user) && a.ID.Equals(id) && (a.AuctionDate != null ? a.AuctionDate < dt : 1 == 1)).SingleOrDefault();

            if (q != null)
            {
                q.Date = DateTime.Now;
                q.AuctionDate = q.Date.AddDays(3);

                db.Tbl_Products.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "شروع دوباره فعال شد";
                TempData["Class"] = "alert alert-success";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return RedirectToAction("ManageProducts");
            }
            else
            {
                TempData["Message"] = "مشکلی در شروع دوباره رخ داد";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return RedirectToAction("ManageProducts");
            }

        }

        public ActionResult DeleteProduct(int id)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                var qdelete = (from a in db.Tbl_Products
                               where a.Tbl_User.UserName.Equals(user) && a.ID.Equals(id)
                               select a).SingleOrDefault();

                if (qdelete != null)
                {
                    db.Tbl_Products.Remove(qdelete);

                    //var qdeletecolor = db.Tbl_Color.Where(a => a.ProductID.Equals(id)).SingleOrDefault();
                    //db.Tbl_Color.Remove(qdeletecolor);

                    db.SaveChanges();
                    TempData["Message"] = "با موفقیت حذف شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return RedirectToAction("ManageProducts");
                }
                else
                {
                    TempData["Message"] = "مشکلی در حذف رخ داد";
                    TempData["Class"] = "alert alert-danger";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return RedirectToAction("ManageProducts");
                }
            }
            catch
            {
                TempData["Message"] = "خطایی رخ داد";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return RedirectToAction("ManageProducts");
            }
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            if (Session["Access"].ToString() != "User")
            {
                TempData["Message"] = "شما دسترسی به این قسمت ندارید";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("ManageProducts");
            }
            else
            {
                return View();
            }


        }

        [HttpPost]
        public ActionResult CreateProduct(Tbl_Products p, int TopicID, HttpPostedFileBase ImageIndex, HttpPostedFileBase[] GalleryProduct, int[] checkfilter, int rcolor, int AuctionDate = 0)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            if (Session["Access"].ToString() != "User")
            {
                ViewBag.Message = "شما اجازه دسترسی به این بخش را ندارید";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (ImageIndex == null)
                    {
                        ViewBag.Message = "حتما یک تصویر شاخص باید انتخاب شود";
                        ViewBag.Class = "alert alert-danger";
                        ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                    else
                    {
                        p.Date = DateTime.Now;

                        if (TopicID == 1)
                        {
                            ViewBag.Message = "حتما دسته ای را انتخاب نمایید!!";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }

                        p.TopicID = TopicID;
                        if (AuctionDate == 1)
                            p.AuctionDate = p.Date.AddDays(3);

                        p.Visit = 0;
                        p.UserID = db.Tbl_User.Where(a => a.UserName.Equals(user)).SingleOrDefault().ID;

                        if (ImageIndex.ContentType != "image/jpeg")
                        {
                            ViewBag.Message = "به یکی از دلایل زیر خطا رخ داد <br /> 1- پسوند تصویر شما باید jpg باشد <br /> 2- نوع فایل انتخابی شما غیر مجاز است";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }
                        if (ImageIndex.ContentLength >= 512000)
                        {
                            ViewBag.Message = "حجم فایل انتخابی شما بالای 512 مگابایت می باشد";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }

                        Random rnd = new Random();
                        p.ImageIndex = rnd.Next().ToString() + ".jpg";
                        ImageIndex.SaveAs(Server.MapPath("~") + "Content/images/PicCategory/" + p.ImageIndex);

                        db.Tbl_Products.Add(p);
                        db.SaveChanges();

                        int proid = db.Tbl_Products.OrderByDescending(a => a.ID).FirstOrDefault().ID;

                        if (GalleryProduct == null)
                        {
                            ViewBag.Message = "یک تصویر شاخص باید انتخاب شود";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }

                        if (GalleryProduct[0] == null)
                        {
                            ViewBag.Message = "حداقل یک تصویر برای گالری باید انتخاب شود";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }
                        else
                        {
                            List<Tbl_Gallery> lstgallery = new List<Tbl_Gallery>();


                            Tbl_Gallery g = new Tbl_Gallery();
                            foreach (var item in GalleryProduct)
                            {
                                if (lstgallery.Count() == 0)
                                {
                                    g.DefaultImage = true;
                                }
                                else
                                {
                                    g.DefaultImage = false;
                                }

                                g.NameImage = rnd.Next().ToString();//نوع فایل و حجم باید بررسی شود
                                item.SaveAs(Path.Combine(Server.MapPath("~") + "Content/images/PicCategory/" + g.NameImage));
                                g.ProductID = proid;
                                lstgallery.Add(g);
                            }
                            db.Tbl_Gallery.AddRange(lstgallery.AsEnumerable());
                        }

                        if (checkfilter != null)
                        {
                            List<Tbl_Filter_Product> lstfp = new List<Tbl_Filter_Product>();
                            foreach (var item in checkfilter)
                            {
                                Tbl_Filter_Product tp = new Tbl_Filter_Product();
                                tp.ProductID = proid;
                                tp.FilterID = item;

                                lstfp.Add(tp);
                            }
                            db.Tbl_Filter_Product.AddRange(lstfp.AsEnumerable());
                        }
                        else
                        {
                            ViewBag.Message = "حتما فیلترها را انتخاب نمایید";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }

                        if (Convert.ToString(rcolor) == null)
                        {
                            ViewBag.Message = "رنگ انتخاب کنید!!";
                            ViewBag.Class = "alert alert-danger";
                            ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                            return View();
                        }
                        else
                        {
                            Tbl_Color c = new Tbl_Color();
                            if (rcolor == 1)
                            {
                                c.CodeColor = "#fff";
                                c.DefaultColor = true;
                                c.NameColor = "سفید";
                                c.ProductID = proid;
                                db.Tbl_Color.Add(c);
                            }
                            else if (rcolor == 2)
                            {
                                c.CodeColor = "#ff0000";
                                c.DefaultColor = false;
                                c.NameColor = "قرمز";
                                c.ProductID = proid;
                                db.Tbl_Color.Add(c);
                            }
                            else if (rcolor == 3)
                            {
                                c.CodeColor = "#0000ff";
                                c.DefaultColor = false;
                                c.NameColor = "آبی";
                                c.ProductID = proid;
                                db.Tbl_Color.Add(c);
                            }
                            else if (rcolor == 4)
                            {
                                c.CodeColor = "#000";
                                c.DefaultColor = false;
                                c.NameColor = "مشکی";
                                c.ProductID = proid;
                                db.Tbl_Color.Add(c);
                            }
                        }

                    }
                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Message"] = "با موفقیت انجام شد";
                        TempData["Class"] = "alert alert-success";
                        TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                        return RedirectToAction("ManageProducts");
                    }
                    else
                    {
                        ViewBag.Message = "متاسفانه با موفقیت انجام نشد";
                        ViewBag.Class = "alert alert-danger";
                        ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "فیلدهای لازم را وارد نمایید";
                    ViewBag.Class = "alert alert-danger";
                    ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                var q = db.Tbl_Products.Where(a => a.ID.Equals(id) && a.Tbl_User.UserName.Equals(user)).SingleOrDefault();

                if (q != null)
                {
                    return View(q);
                }
                else
                {
                    TempData["Message"] = "شما محصولی برای ویرایش ندارید";
                    TempData["Class"] = "alert alert-danger";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("ManageProducts");
                }
            }
            catch
            {
                TempData["Message"] = "مشکلی رخ داد";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("ManageProducts");
            }
        }

        [HttpPost]
        public string DeleteFilter(int id, int ProductID)
        {
            try
            {
                if (Session["User"] == null)
                    return "";

                if (Session["Access"].ToString() != "User")
                    return "";

                string user = Session["User"].ToString();

                var q = db.Tbl_Products.Where(a => a.Tbl_User.UserName.Equals(user) && a.ID.Equals(ProductID)).SingleOrDefault();

                if (q != null)
                {
                    var q2 = q.Tbl_Filter_Product.Where(a => a.FilterID.Equals(id) && a.ProductID.Equals(ProductID)).SingleOrDefault();

                    db.Tbl_Filter_Product.Remove(q2);
                    db.SaveChanges();

                    //برای آپدیت شدن قسمت مورد نظر
                    string Template = "<ul class='list-group'>";
                    foreach (var item in q.Tbl_Filter_Product)
                    {
                        Template += "<li class='list-group-item list-group-item-success'><span style='margin-right:10px; float:right; color:red; font-size:14px; cursor:pointer;'><a href='/User/DeleteFilter/" + item.FilterID + "?ProductID=" + ProductID + "' data-ajax-update='#DivFilter' data-ajax-mode='replace' data-ajax-method='POST' data-ajax-confirm='ایا مطمعا به حذف هستید؟' data-ajax='true'>X</a></span>&nbsp;&nbsp;&nbsp;&nbsp;<span>" + item.Tbl_Filter.Title + "</span></li>";
                    }
                    Template += "</ul>";
                    return Template;
                }
                else
                {
                    return "مشکلی در اجرا رخ داد";
                }
            }
            catch
            {
                return "مشکلی در اجرا رخ داد";
            }
        }

        public ActionResult EditProduct(Tbl_Products p, HttpPostedFileBase ImageIndex, int[] checkfilter)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "باید تمامی فیلدها را وارد کنید";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View();
            }

            var q = db.Tbl_Products.Where(a => a.ID.Equals(p.ID) && a.Tbl_User.UserName.Equals(user)).SingleOrDefault();

            Random rnd = new Random();

            if (q != null)
            {
                q.Description = p.Description;
                q.Text = p.Text;
                q.Title = p.Title;
                q.Price = p.Price;
                q.Weight = p.Weight;
                q.Warranty = p.Warranty;
                q.ExitCount = p.ExitCount;
                q.TopicID = p.TopicID;
            }

            if (ImageIndex != null)
            {
                if (ImageIndex.ContentType != "image/jpeg")
                {
                    ViewBag.Message = "به یکی از دلایل زیر خطا رخ داد <br /> 1- پسوند تصویر شما باید jpg باشد <br /> 2- نوع فایل انتخابی شما غیر مجاز است";
                    ViewBag.Class = "alert alert-danger";
                    ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return View();
                }
                if (ImageIndex.ContentLength >= 512000)
                {
                    ViewBag.Message = "حجم فایل انتخابی شما بالای 512 مگابایت می باشد";
                    ViewBag.Class = "alert alert-danger";
                    ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return View();
                }

                q.ImageIndex = rnd.Next().ToString() + ".jpg";
                ImageIndex.SaveAs(Server.MapPath("~") + "Content/images/PicProduct/" + q.ImageIndex);
            }

            if (checkfilter != null)
            {
                List<Tbl_Filter_Product> lstfil = new List<Tbl_Filter_Product>();
                foreach (var item in checkfilter)
                {
                    Tbl_Filter_Product tfp = new Tbl_Filter_Product();
                    tfp.FilterID = item;
                    tfp.ProductID = q.ID;

                    lstfil.Add(tfp);
                }
                db.Tbl_Filter_Product.AddRange(lstfil.AsEnumerable());
            }

            db.Tbl_Products.Attach(q);
            db.Entry(q).State = System.Data.Entity.EntityState.Modified;
            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["Message"] = "با موفقیت ویرایش شد";
                TempData["Class"] = "alert alert-success";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("ManageProducts");
            }
            else
            {
                ViewBag.Message = "متاسفانه ویرایش نشد";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View(p);
            }
        }

        [HttpGet]
        public ActionResult Infobank()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            return View();
        }
        [HttpPost]
        public ActionResult Infobank(Tbl_BankNum b)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            var us = db.Tbl_User.Where(a => a.UserName.Equals(user)).SingleOrDefault().ID;

            //var BankNumInitial = db.Tbl_BankNum.Where(a => a.UserID.Equals(us)).SingleOrDefault().BankNum;
            //var CartNumInitial = db.Tbl_BankNum.Where(a => a.UserID.Equals(us)).SingleOrDefault().CartNum;

            Tbl_BankNum tbn = new Tbl_BankNum();
            tbn.UserID = us;
            tbn.CartNum = b.CartNum;
            tbn.BankNum = b.BankNum;
            tbn.BankName = b.BankName;

            db.Tbl_BankNum.Add(tbn);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                ViewBag.Message = "اطلاعات بانکی با موفقیت ثبت شد";
                ViewBag.Class = "alert alert-success";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";

                ViewBag.Message = TempData["Message"];
                ViewBag.Style = TempData["Style"];
                ViewBag.Class = TempData["Class"];

                return View();
            }
            else
            {
                ViewBag.Message = "شما قبلا اطلاعات بانکی در سیستم ما وارد کرده اید. در صورت تمایل می توانید همان را ویرایش کنید";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View();
            }
        }

        public ActionResult ManageMessage()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            var q = (from a in db.Tbl_Message
                     where a.Tbl_User.UserName.Equals(user)
                     orderby a.ID descending
                     select a).OrderBy(a => a.Read);

            if (q != null)
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.Style = TempData["Style"];
                ViewBag.Class = TempData["Class"];
                return View(q);
            }
            else
            {
                ViewBag.Message = "مشکلی رخ داد";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View();
            }
        }

        public ActionResult DeleteMessage(int id)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                var qdel = (from a in db.Tbl_Message
                            where a.ID.Equals(id) && a.Tbl_User.UserName.Equals(user)
                            select a).SingleOrDefault();

                if (qdel != null)
                {
                    db.Tbl_Message.Remove(qdel);
                    db.SaveChanges();

                    TempData["Message"] = "پیام با موفقیت حذف شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("ManageMessage");
                }
                else
                {
                    TempData["Message"] = "مشکلی رخ داد";
                    TempData["Class"] = "alert alert-danger";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("ManageMessage");
                }
            }
            catch
            {
                TempData["Message"] = "مشکلی رخ داد";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("ManageMessage");
            }
        }
        [HttpGet]
        public ActionResult CreateMessage()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            return View();
        }
        [HttpPost]
        public ActionResult CreateMessage(Tbl_Message tm, string UserGets)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (!this.IsCaptchaValid("Error"))
                {
                    ViewBag.Message = "کد امنیتی نادرست است";
                    ViewBag.Class = "alert alert-danger";
                    ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                    return View();
                }

                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string user = Session["User"].ToString();

                Tbl_Message m = new Tbl_Message();
                m.Read = false;
                m.Title = tm.Title;
                m.Text = tm.Text;
                m.UserSend = db.Tbl_User.Where(a => a.UserName.Equals(user)).SingleOrDefault().ID;
                m.Date = DateTime.Now;
                m.UserGet = db.Tbl_User.Where(a => a.UserName.Equals(UserGets)).SingleOrDefault().ID;

                db.Tbl_Message.Add(m);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["Message"] = "ارسال با موفقیت انجام شد";
                    TempData["Class"] = "alert alert-success";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("ManageMessage");
                }
                else
                {
                    TempData["Message"] = "متاسفانه ارسال با موفقیت انجام نشد";
                    TempData["Class"] = "alert alert-danger";
                    TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                    return RedirectToAction("ManageMessage");
                }
            }
            catch
            {
                TempData["Message"] = "مشکلی رخ داد";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("ManageMessage");
            }
        }
        [HttpPost]
        public JsonResult ValidUserGet(string UserGets)
        {
            try
            {
                var q = db.Tbl_User.Where(a => a.UserName.Equals(UserGets)).SingleOrDefault().ID;
                if (q != 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult DetailsMessage(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            var q = (from a in db.Tbl_Message
                     where a.ID.Equals(id) && a.Tbl_User.UserName.Equals(user)
                     select a).SingleOrDefault();

            if (q != null)
            {
                q.Read = true;
                db.Tbl_Message.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return View(q);
            }
            else
            {
                TempData["Message"] = "مشکلی رخ داد";
                TempData["Class"] = "alert alert-danger";
                TempData["Style"] = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;padding-top: 10px;";
                return RedirectToAction("ManageMessage");
            }
        }

        public ActionResult ReadMessage()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string user = Session["User"].ToString();

            var q = (from a in db.Tbl_Message
                     where a.Tbl_User1.UserName.Equals(user)
                     orderby a.ID descending
                     select a).OrderByDescending(a => a.Read);

            if (q != null)
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.Style = TempData["Style"];
                ViewBag.Class = TempData["Class"];
                return View(q);
            }
            else
            {
                ViewBag.Message = "پیامی موجود نمی باشد";
                ViewBag.Class = "alert alert-danger";
                ViewBag.Style = "margin: 15px 50px 15px 10px; text-align: center; width: 90%;";
                return View();
            }
        }

    }//End Controller
}
