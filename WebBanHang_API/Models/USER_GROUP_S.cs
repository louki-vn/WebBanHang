namespace WebBanHang_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USER_GROUP_S
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER_GROUP_S()
        {
            CREDENTIAL_S = new HashSet<CREDENTIAL_S>();
            MEMBERs = new HashSet<MEMBER>();
        }

        [Key]
        [StringLength(20)]
        public string usergroup_id { get; set; }

        [StringLength(50)]
        public string usergroup_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CREDENTIAL_S> CREDENTIAL_S { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEMBER> MEMBERs { get; set; }
    }
}
