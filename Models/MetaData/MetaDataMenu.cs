using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataMenu
    {
        [Display(Name ="صفحه نمایش")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="عنوان صفحه وارد شود")]
        [StringLength(30,MinimumLength =4,ErrorMessage ="عنوان صفحه باید بین 4 تا 30 کاراکتر باشد")]
        [Remote("TitleValid","Pages",HttpMethod ="Post",ErrorMessage ="نام صفحه تکراری است")]
        public string TitlePage { get; set; }
        [Display(Name = "ترتیب نمایش")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ترتیب نمایش وارد شود")]
        public int Sort { get; set; }
        [Display(Name = "وضعیت فعال سازی")]
        public bool Enable { get; set; }
        [Display(Name = "محتوای صفحه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "محتوای صفحه وارد شود")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ContentPage { get; set; }
        [Display(Name = "متا تگ")]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "متا تگ باید بین 4 تا 25 کاراکتر باشد")]
        public string Tag { get; set; }
        [Display(Name = "توضیحات مختصر")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "توضیحات مختصر باید بین 4 تا 50 کاراکتر باشد")]
        public string Description { get; set; }
    }

    [MetadataType(typeof(MetaDataMenu))]
    public partial class Tbl_Menu
    {

    }

}