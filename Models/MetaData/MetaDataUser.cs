using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sourceiran_MVC.Models.Domains
{
    internal class MetaDataUser
    {
        public int ID { get; set; }

        [Display(Name ="نام کاربری")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="نام کاربری را وارد کنید")]
        [StringLength(100,MinimumLength =3,ErrorMessage ="نام کاربری باید بین 3 تا 100 کاراکتر باشد")]
        [Remote("RegisterValid","Register",HttpMethod ="Post",ErrorMessage ="نام کاربری تکراری می  باشد")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [DataType("Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور را وارد کنید")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید بین 6 تا 100 کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ایمیل را وارد کنید")]
        [StringLength(100, ErrorMessage = "ایمیل باید حداکثر 100 کاراکتر باشد")]
        [Remote("EmailValid", "Register", HttpMethod = "Post", ErrorMessage = "ایمیل تکراری می  باشد")]
        [RegularExpression((@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$"),ErrorMessage ="ایمیل را به درستی وارد نمایید")]
        public string Email { get; set; }

        [Display(Name = "دسترسی")]
        public string Access { get; set; }

        [Display(Name ="نام و نام خانوادگی")]
        [StringLength(100, ErrorMessage = "نام و نام خانوادگی باید حداکثر 100 کاراکتر باشد")]
        public string Name { get; set; }

        [Display(Name = "کد ملی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "کد ملی را وارد کنید")]
        [StringLength(15, ErrorMessage = "کد ملی باید حداکثر 15 کاراکتر باشد")]
        [Remote("NatCodeValid", "Register", HttpMethod = "Post", ErrorMessage = "کد ملی تکراری می  باشد")]
        public string NatCode { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "تلفن همراه را وارد کنید")]
        [StringLength(100, ErrorMessage = "تلفن همراه باید حداکثر 100 کاراکتر باشد")]
        [Remote("MobileValid", "Register", HttpMethod = "Post", ErrorMessage = "تلفن همراه تکراری می  باشد")]
        [RegularExpression("09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}",ErrorMessage ="تلفن همراه را به درستی وارد نمایید")]
        public string Mobile { get; set; }

        [Display(Name = "شماره تماس")]
        [StringLength(15, ErrorMessage = "شماره تماس باید حداکثر 15 کاراکتر باشد")]
        public string Phone { get; set; }

        [Display(Name = "آدرس کامل")]
        [StringLength(100, ErrorMessage = "آدرس کامل باید حداکثر 100 کاراکتر باشد")]
        public string Address { get; set; }

        [Display(Name = "استان")]
        [StringLength(100, ErrorMessage = "استان باید حداکثر 100 کاراکتر باشد")]
        public string Shire { get; set; }

        [Display(Name = "شهرستان")]
        [StringLength(100, ErrorMessage = "شهرستان باید حداکثر 100 کاراکتر باشد")]
        public string City { get; set; }

        [Display(Name = "کد پستی")]
        [StringLength(15, ErrorMessage = "کد پستی باید حداکثر 15 کاراکتر باشد")]
        public string PostalCode { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }
    }

    [MetadataType(typeof(MetaDataUser))]
    public partial class Tbl_User
    {

    }
}