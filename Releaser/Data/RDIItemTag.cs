//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Releaser.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class RDIItemTag
    {
        public int RDIItemTagId { get; set; }
        public int TagId { get; set; }
        public int RDIItemId { get; set; }
        public string Note { get; set; }
        public string ins_user { get; set; }
        public Nullable<System.DateTime> ins_date { get; set; }
        public string upd_user { get; set; }
        public Nullable<System.DateTime> upd_date { get; set; }
    
        public virtual RDIItem RDIItem { get; set; }
    }
}
