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
    
    public partial class ItemType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ItemType()
        {
            this.RDIItems = new HashSet<RDIItem>();
        }
    
        public int ItemTypeId { get; set; }
        public string ItemType1 { get; set; }
        public string ins_user { get; set; }
        public Nullable<System.DateTime> ins_date { get; set; }
        public string upd_user { get; set; }
        public Nullable<System.DateTime> upd_date { get; set; }
        public int ApplicationId { get; set; }
        public string TypeDesc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RDIItem> RDIItems { get; set; }
    }
}