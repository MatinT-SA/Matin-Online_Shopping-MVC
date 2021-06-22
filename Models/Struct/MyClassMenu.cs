using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sourceiran_MVC.Models.Domains;
using Sourceiran_MVC.Models.Struct;

public class MyClassMenu
{
    public static ShowMenu MenuItem()
    {
        using (DBSI db = new DBSI())
        {
            var menu = db.Tbl_Categories.ToList();
            return new ShowMenu()
            {
                MenuList = menu
            };
        }
    }
}
