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
    
    public partial class ALARMAS_PERMANS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ALARMAS_PERMANS()
        {
            this.ALARMAS = new HashSet<ALARMAS>();
        }
    
        public byte ID { get; set; }
        public string DESCRIPCION_LARGA { get; set; }
        public string DESCRIPCION_MED { get; set; }
        public string DESCRIPCION_CORTA { get; set; }
        public string ABREVIACION { get; set; }
        public byte ESTADO_ID { get; set; }
        public string OBSERVACIONES { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALARMAS> ALARMAS { get; set; }
    }
}
