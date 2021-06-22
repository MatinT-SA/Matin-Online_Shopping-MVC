using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sourceiran_MVC.Models.Domains;

namespace Sourceiran_MVC.Models.Repository
{
    public class RepositoryMenu
    {
        DBSI db = new DBSI();
        public IEnumerable<Tbl_Menu> Get_MenuTop()
        {
            var qtop = (from a in db.Tbl_Menu
                        orderby a.Sort
                        where a.Enable == true
                        select a).ToList();

            return qtop.Take(4).Skip(0);
        }

        public IEnumerable<Tbl_Menu> Get_MenuBottom()
        {
            var qbottom = (from a in db.Tbl_Menu
                           orderby a.Sort
                           where a.Enable == true
                           select a).ToList();

            return qbottom.Take(6).Skip(0);
        }

    }
}