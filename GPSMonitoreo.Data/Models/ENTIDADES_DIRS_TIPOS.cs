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
    
    public partial class ENTIDADES_DIRS_TIPOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ENTIDADES_DIRS_TIPOS()
        {
            this.ENTIDADES_DIRS_TIPOS_REL = new HashSet<ENTIDADES_DIRS_TIPOS_REL>();
        }
    
        public byte ID { get; set; }
        public string DESCRIPCION_LARGA { get; set; }
        public string DESCRIPCION_MED { get; set; }
        public string DESCRIPCION_CORTA { get; set; }
        public string ABREVIACION { get; set; }
        public byte ESTADO_ID { get; set; }
        public string OBSERVACIONES { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENTIDADES_DIRS_TIPOS_REL> ENTIDADES_DIRS_TIPOS_REL { get; set; }
    }
}
