//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GPSMonitoreo.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PERMISOS_REQUERIDOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PERMISOS_REQUERIDOS()
        {
            this.ACCIONES = new HashSet<PERMISOS_REQUERIDOS_ACCS>();
            this.ROLES = new HashSet<PERMISOS_REQUERIDOS_ROLES>();
        }
    
        public GPSMonitoreo.Core.Authorization.PermissionElementType ELEMENTO_TIPO_ID { get; set; }
        public int ELEMENTO_ID { get; set; }
        public Nullable<byte> DUMMY { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERMISOS_REQUERIDOS_ACCS> ACCIONES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERMISOS_REQUERIDOS_ROLES> ROLES { get; set; }
    }
}
