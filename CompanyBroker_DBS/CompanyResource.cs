//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CompanyBroker_DBS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class CompanyResource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanyResource()
        {
            this.ResourceDescriptions = new HashSet<ResourceDescription>();
        }

        [Key]
        public int ResourceId { get; set; }
        public int CompanyId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool Active { get; set; }
    
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResourceDescription> ResourceDescriptions { get; set; }
    }
}
