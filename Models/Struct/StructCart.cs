using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Struct
{
    public class StructCart
    {
        public int SumPrice { get; set; }// جمع هزینه کل سبد
        public int AllCount { get; set; }// تعداد کل محصول داخل سبد
        public int SumWeight { get; set; }// وزن کل سبد
        public List<Tbl_Cart> lsCart { get; set; }
    }

    public class Tbl_Cart
    {
        public int id { get; set; }//شناسه محصول
        public string namecart { get; set; }//نام محصول
        public int price { get; set; }//قیمت واحد محصول
        public string CountCart { get; set; }//تعداد محصول
    }
    
}