//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shopping_Website_2020.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_invoice
    {
        public tb_invoice()
        {
            this.tb_order = new HashSet<tb_order>();
        }
    
        public int invoice_id { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<System.DateTime> invoice_date { get; set; }
        public Nullable<int> total_bill { get; set; }
        public Nullable<System.DateTime> entrydate { get; set; }
    
        public virtual tb_user tb_user { get; set; }
        public virtual ICollection<tb_order> tb_order { get; set; }
    }
}