using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataMessage
    {
        public int ID { get; set; }
        [Display(Name = "زمان پیام")]
        public System.DateTime Date { get; set; }
        [Display(Name = "خوانده شده")]
        public bool Read { get; set; }
        [Display(Name = "عنوان پیام")]
        [StringLength(50, ErrorMessage = "عنوان پیام حداکثر 50 کاراکتر می باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "عنوان پیام باید وارد شود")]
        public string Title { get; set; }
        [Display(Name = "متن")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "متن باید وارد شود")]
        [AllowHtml]
        public string Text { get; set; }
        [Display(Name = "گیرنده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "گیرنده باید وارد شود")]
        [Remote("ValidUserGet", "User", ErrorMessage = "گیرنده نامعتبر است", HttpMethod = "POST")]
        public int UserGet { get; set; }
        [Display(Name = "فرستنده")]
        public Nullable<int> UserSend { get; set; }
    }

    [MetadataType(typeof(MetaDataMessage))]
    public partial class Tbl_Message
    {

    }
}