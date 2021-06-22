using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataProduct
    {
        public int ID { get; set; }
        [Display(Name = "شناسه کاربری")]
        public int UserID { get; set; }
        [Display(Name = "نام محصول")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="عنوان را وارد نمایید")]
        [StringLength(100,ErrorMessage ="عنوان حداکثر 100 کاراکتر باشد")]
        public string Title { get; set; }
        [Display(Name = "تصویر شاخص")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "تصویر شاخص را وارد نمایید")]
        public string ImageIndex { get; set; }
        [Display(Name = "قیمت فروش")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "قیمت را وارد نمایید")]
        public int Price { get; set; }
        [Display(Name = "وزن")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "وزن را وارد نمایید")]
        public int Weight { get; set; }
        [Display(Name = "تعداد بازدید")]
        public int Visit { get; set; }
        [Display(Name = "زمان ثبت")]
        public System.DateTime Date { get; set; }
        [Display(Name = "دسته محصول")]
        public int TopicID { get; set; }
        [Display(Name = "زمان انقضا")]
        public Nullable<System.DateTime> AuctionDate { get; set; }
        [Display(Name = "توضیحات محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "توضیح را وارد نمایید")]
        [AllowHtml]
        public string Text { get; set; }
        [Display(Name = "توضیح مختصر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "توضیح مختصر را وارد نمایید")]
        [StringLength(100, ErrorMessage = "توضیح مختصر حداکثر 100 کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "موجودی")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "موجودی را وارد نمایید")]
        public int ExitCount { get; set; }
        [Display(Name = "گارانتی")]
        public string Warranty { get; set; }
    }

    [MetadataType(typeof(MetaDataProduct))]
    public partial class Tbl_Products
    {

    }
}