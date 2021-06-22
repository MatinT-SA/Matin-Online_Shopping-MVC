using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Struct
{
    public class StructBills
    {
        [Display(Name ="موجودی")]
        public int Stock { get; set; }//موجودی کل
        [Display(Name = "موجودی قابل برداشت")]
        public int RealStock { get; set; }//موجودی قابل برداشت
        [Display(Name = "آخرین دریافتی")]
        public int EndReceive { get; set; }//آخرین دریافتی
        [Display(Name = "زمان آخرین دریافتی")]
        public string EndDateReceive { get; set; }//زمان آخرین دریافتی
        [Display(Name = "کل دریافتی")]
        public int AllReceive { get; set; }//جمع کل دریافتی ها
    }
}