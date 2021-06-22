using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataBills
    {
        public int ID { get; set; }
        [Display(Name ="کاربر درخواست کننده")]
        public int UserID { get; set; }
        [Display(Name = "کل دریافتی")]
        public int Stock { get; set; }
        [Display(Name = "آخرین دریافتی")]
        public int EndReceive { get; set; }
        [Display(Name = "زمان آخرین دریافت")]
        public System.DateTime DateEndReceive { get; set; }
        [Display(Name = "مبلغ درخواستی")]
        public int AmountRequested { get; set; }
        public bool Requested { get; set; }
    }

    [MetadataType(typeof(MetaDataBills))]
    public partial class Tbl_Bills
    {

    }
}