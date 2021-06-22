using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sourceiran_MVC.Models.Domains;

namespace Sourceiran_MVC.Models.Repository
{
    public class RepositoryIndex
    {
        DBSI db = new DBSI();

        public IEnumerable<Tbl_Products> GetProductNew()
        {
            var q = (from a in db.Tbl_Products
                     where a.AuctionDate == null && a.ExitCount > 0
                     orderby a.Date descending // جدیدترین محصولات عادی
                     select a).Take(4).Skip(0);
            return q.AsEnumerable();
        }

        public IEnumerable<Tbl_Products> GetProductNewAuction()
        {
            var q = (from a in db.Tbl_Products
                     where a.AuctionDate != null ? a.AuctionDate > DateTime.Now : 1 == 1
                     orderby a.Date descending // جدیدترین محصولات مزایده ای
                     select a).Take(4).Skip(0);
            return q.AsEnumerable();
        }

        public IEnumerable<Tbl_Products> GetProductVisit()
        {
            var q = (from a in db.Tbl_Products
                     where a.AuctionDate == null && a.ExitCount > 0
                     orderby a.Visit descending // پر بازدیدترین محصولات عادی
                     select a).Take(8).Skip(0);
            return q.AsEnumerable();
        }

        public IEnumerable<Tbl_Products> GetProductVisitAuction()
        {
            var q = (from a in db.Tbl_Products
                     where a.AuctionDate != null ? a.AuctionDate > DateTime.Now : 1 == 1
                     orderby a.Visit descending // پر بازدیدترین محصولات مزایده ای
                     select a).Take(8).Skip(0);
            return q.AsEnumerable();
        }
    }
}