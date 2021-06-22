using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataSales
    {
        public int ID { get; set; }
        [Display(Name = "نام محصول")]
        public int ProductID { get; set; }
        [Display(Name = "کاربر خریدار")]
        public int UserID { get; set; }
        [Display(Name = "مبلغ پرداختی")]
        public int Price { get; set; }
        [Display(Name = "زمان ایجاد فاکتور")]
        public System.DateTime Date { get; set; }
        public bool Payment { get; set; }
        [Display(Name = "وضعیت ارسال")]
        public int Status { get; set; }
        [Display(Name = "شماره گروه")]
        public string GroupNo { get; set; }
        [Display(Name = "تعداد خریداری شده")]
        public int Count { get; set; }
        [Display(Name = "شماره پیگیری")]
        public string TransNo { get; set; }
        [Display(Name = "کد رهگیری")]
        public string CodeRahgiri { get; set; }
        [Display(Name ="شماره بانک")]
        public string BankNo { get; set; }
    }

    [MetadataType(typeof(MetaDataSales))]
    public partial class Tbl_Sales
    {

    }
}