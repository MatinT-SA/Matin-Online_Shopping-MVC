using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataBank
    {
        public int ID { get; set; }
        [Display(Name = "نام بانک")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="حتما نام بانک وارد شود")]
        [StringLength(100,ErrorMessage ="حداکثر برای نام بانک 100 کاراکتر می توانید وارد کنید")]
        public string BankName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "حتما شماره کارت وارد شود")]
        [Display(Name = "شماره کارت")]
        [StringLength(30, ErrorMessage = "حداکثر برای شماره کارت 30 کاراکتر می توانید وارد کنید")]
        public string CartNum { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "حتما شماره حساب وارد شود")]
        [Display(Name = "شماره حساب")]
        [StringLength(100, ErrorMessage = "حداکثر برای شماره حساب 100 کاراکتر می توانید وارد کنید")]
        public string BankNum { get; set; }
        [Display(Name = "شناسه کاربری")]
        public int UserID { get; set; }
    }

    [MetadataType(typeof(MetaDataBank))]
    public partial class Tbl_BankNum
    {
    }
}