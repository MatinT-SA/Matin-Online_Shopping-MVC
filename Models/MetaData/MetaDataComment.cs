using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataComment
    {
        public int ID { get; set; }
        [Display(Name = "شناسه محصول")]
        public int ProductID { get; set; }
        [Display(Name ="نام")]
        public string Name { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "وب سایت")]
        public string Web { get; set; }
        [Display(Name = "متن")]
        public string Text { get; set; }
        [Display(Name = "تاریخ")]
        public System.DateTime Date { get; set; }
        [Display(Name = "IP")]
        public string IP { get; set; }
        [Display(Name = "وضعیت تاییدیه")]
        public bool Confirm_Comm { get; set; }
        public Nullable<int> ParentID { get; set; }
    }

    [MetadataType(typeof(MetaDataComment))]
    public partial class Tbl_Comment
    {
    }
}