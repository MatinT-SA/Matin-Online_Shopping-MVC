using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataFilter
    {
        public int ID { get; set; }
        [Display(Name ="گروه فیلتر فرعی")]
        public int GroupFilterID { get; set; }
        [Display(Name = "عنوان")]
        [StringLength(100,ErrorMessage ="عنوان باید حداکثر 100 کاراکتر باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "عنوان باید وارد شود")]
        public string Title { get; set; }
    }

    [MetadataType(typeof(MetaDataFilter))]
    public partial class Tbl_Filter
    {
    }
}