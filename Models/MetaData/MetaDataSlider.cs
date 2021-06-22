using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataSlider
    {
        public int ID { get; set; }
        [Display(Name ="ترتیب نمایش")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="ترتیب نمایش را وارد نمایید")]
        public int Sort { get; set; }
        [Display(Name = "آدرس لینک")]
        [StringLength(100,ErrorMessage ="تعداد کاراکتر باید کمتر از 100 باشد")]
        public string Url { get; set; }
        [Display(Name = "تصویر")]
        public string Image { get; set; }
        [Display(Name = "عنوان اسلاید")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "عنوان اسلاید را وارد نمایید")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر باید کمتر از 100 باشد")]
        public string Title { get; set; }
        [Display(Name = "وضعیت نمایش")]
        public bool Enable { get; set; }
    }

    [MetadataType(typeof(MetaDataSlider))]
    public partial class Tbl_Slider
    {
    }
}