using Sourceiran_MVC.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Repository
{
    public class RepositorySlider
    {
        DBSI db = new DBSI();
        public IEnumerable<Tbl_Slider> GetSlider()
        {
            var qslider = from a in db.Tbl_Slider
                          where a.Enable == true
                          orderby a.Sort
                          select a;
            return qslider.AsEnumerable();
        }
    }
}