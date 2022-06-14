namespace WebShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("CREDENTIAL_S")]
    [Serializable]
    public partial class CREDENTIAL_S
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string usergroup_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string role_id { get; set; }

        [StringLength(50)]
        public string note { get; set; }

        public virtual ROLE_S ROLE_S { get; set; }

        public virtual USER_GROUP_S USER_GROUP_S { get; set; }
    }
}
