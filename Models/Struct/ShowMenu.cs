using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sourceiran_MVC.Models.Domains;
using Sourceiran_MVC.Models.Struct;

namespace Sourceiran_MVC.Models.Struct
{
    public class ShowMenu
    {
        public IEnumerable<Tbl_Categories> MenuList { get; set; }
    }
}