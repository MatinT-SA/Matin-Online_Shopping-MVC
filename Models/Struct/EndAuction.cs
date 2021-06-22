using Sourceiran_MVC.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public static class EndAuction
{
    public static void EndMozayede()
    {
        DBSI db = new DBSI();

        var q = (from a in db.Tbl_Products
                 where a.AuctionDate != null
                 select a).ToList();

        List<Tbl_Sales> lstsales = new List<Tbl_Sales>();
        List<Tbl_Message> lstMessage = new List<Tbl_Message>();
        List<Tbl_Auction> lstAuc = new List<Tbl_Auction>();

        foreach (var item in q.Where(a => a.AuctionDate < DateTime.Now))
        {
            var max = item.Tbl_Auction.OrderByDescending(a => a.Price).FirstOrDefault();


            if (max != null)
            {
                int maliat = 1200;
                int send = 10000;

                Tbl_Sales s = new Tbl_Sales();
                s.CodeRahgiri = "";
                s.Count = 1;
                s.Date = DateTime.Now;
                s.GroupNo = "";
                s.Payment = false;
                s.Price = max.Price + send + maliat;
                s.ProductID = max.ProductID;
                s.Status = 1;
                s.TransNo = "";
                s.UserID = max.UserID;

                lstsales.Add(s);


                foreach (var itemTemp in item.Tbl_Auction)
                {
                    Tbl_Message m = new Tbl_Message();

                    if (itemTemp.ID == max.ID)
                        m.Text = "سلام <hr /> <br /> کاربرگرامی شما در مزایده " + item.Title + " پیروز شدید سریعا جهت پرداخت هزینه اقدام کنید";

                    else
                        m.Text = "سلام <hr /> <br /> کاربرگرامی شما در مزایده " + item.Title + " پیروز نشدید";
                    m.Date = DateTime.Now;
                    m.Read = false;
                    m.Title = "پیام سیستمی";
                    m.UserSend = null;
                    m.UserGet = itemTemp.UserID;


                    lstMessage.Add(m);

                    lstAuc.Add(itemTemp);


                    //db.SaveChanges();
                }

                Tbl_Message ms = new Tbl_Message();

                ms.Text = "سلام <hr /> <br /> کاربرگرامی مزایده محصول " + item.Title + " شما به اتمام رسید و برنده ان مشخص گردید";
                ms.Date = DateTime.Now;
                ms.Read = false;
                ms.Title = "پیام سیستمی";
                ms.UserSend = null;
                ms.UserGet = item.UserID;
                db.Tbl_Message.Add(ms);

                db.Tbl_Sales.AddRange(lstsales.AsEnumerable());
                db.Tbl_Message.AddRange(lstMessage.AsEnumerable());
                db.Tbl_Auction.RemoveRange(lstAuc.AsEnumerable());
                db.SaveChanges();
            }
            else
            {

                Tbl_Message m = new Tbl_Message();

                m.Text = "سلام <hr /> <br /> کاربرگرامی مزایده محصول " + item.Title + " شما به اتمام رسید و برنده ای نداشت";
                m.Date = DateTime.Now;
                m.Read = false;
                m.Title = "پیام سیستمی";
                m.UserSend = null;
                m.UserGet = item.UserID;

                db.Tbl_Message.Add(m);
                db.SaveChanges();
            }


        }

        db = null;
        q = null;
    }
}
