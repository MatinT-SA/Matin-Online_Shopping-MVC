//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sourceiran_MVC.Models.Domains
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_ShoppingCarts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_ShoppingCarts()
        {
            this.Tbl_TempShoppingCart = new HashSet<Tbl_TempShoppingCart>();
        }
    
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public System.DateTime Date { get; set; }
        public string CookieNo { get; set; }
        public bool Status { get; set; }
        public Nullable<int> Count { get; set; }
    
        public virtual Tbl_Products Tbl_Products { get; set; }
        public virtual Tbl_User Tbl_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_TempShoppingCart> Tbl_TempShoppingCart { get; set; }
    }
}