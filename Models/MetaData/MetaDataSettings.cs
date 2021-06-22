using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataSettings
    {
        public int ID { get; set; }
        [Display(Name ="عنوان")]
        [StringLength(100, ErrorMessage = "عنوان باید حداکثر 100 کاراکتر باشد")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="عنوان باید وارد شود")]
        public string Title { get; set; }
        [StringLength(100,ErrorMessage ="توضیحات باید حداکثر 100 کاراکتر باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "توضیحات باید وارد شود")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [StringLength(100, ErrorMessage = "ایمیل باید حداکثر 100 کاراکتر باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ایمیل باید وارد شود")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [StringLength(100, ErrorMessage = "رمز عبور ایمیل باید حداکثر 100 کاراکتر باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور ایمیل باید وارد شود")]
        [Display(Name = "رمز عبور ایمیل")]
        public string EmailPass { get; set; }
        [StringLength(100, ErrorMessage = "SMTP باید حداکثر 100 کاراکتر باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "SMTP باید وارد شود")]
        [Display(Name = "SMTP")]
        public string SMTP { get; set; }
        [Display(Name = "تعداد صفحات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "تعداد صفحات باید وارد شود")]
        public int CountPage { get; set; }
        [Display(Name = "کلمه کلیدی")]
        [StringLength(200, ErrorMessage = "کلمه کلیدی باید حداکثر 200 کاراکتر باشد")]
        public string KeyWord { get; set; }
    }

    [MetadataType(typeof(MetaDataSettings))]
    public partial class Tbl_Setting
    {
    }
}