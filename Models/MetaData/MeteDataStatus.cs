using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MeteDataStatus
    {
        public int ID { get; set; }
        [Display(Name ="عنوان وضعیت")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="عنوان وضعیت را وارد کنید")]
        [StringLength(100,ErrorMessage ="عنوان وضعیت نمی تواند بیشتتر از 100 کاراکتر باشد")]
        public string Title { get; set; }
        [Display(Name = "ترتیب نمایش")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ترتیب نمایش را وارد کنید")]
        [DataType(DataType.Text)]
        public int Sort { get; set; }
    }
    
    [MetadataType(typeof(MeteDataStatus))]
    public partial class Tbl_Status
    {

    }

}