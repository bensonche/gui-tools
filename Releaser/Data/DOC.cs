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
    
    public partial class DOC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOC()
        {
            this.DOC_METADATA = new HashSet<DOC_METADATA>();
            this.ItemFiles = new HashSet<ItemFile>();
        }
    
        public int DOC_ID { get; set; }
        public byte[] DOC1 { get; set; }
        public string ins_user { get; set; }
        public Nullable<System.DateTime> ins_date { get; set; }
        public string upd_user { get; set; }
        public Nullable<System.DateTime> upd_date { get; set; }
        public Nullable<int> parent_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOC_METADATA> DOC_METADATA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemFile> ItemFiles { get; set; }
    }
}