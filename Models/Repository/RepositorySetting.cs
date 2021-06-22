using Sourceiran_MVC.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Repository
{
    public class RepositorySetting
    {
        DBSI db = new DBSI();
        public string GetSetting()
        {
            var q = (from a in db.Tbl_Setting
                     select a).FirstOrDefault().Title;

            return q;
        }

        public string GetDescription()
        {
            var p = (from a in db.Tbl_Setting
                     select a).FirstOrDefault().Description;
            return p.ToString();
        }

        public string GetKeyWord()
        {
            var q = (from a in db.Tbl_Setting
                     select a).FirstOrDefault().KeyWord;
            return q.ToString();
        }
    }
}