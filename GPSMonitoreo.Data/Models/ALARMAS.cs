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
    
    public partial class ALARMAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ALARMAS()
        {
            this.ALARMAS_NIVELES_REL = new HashSet<ALARMAS_NIVELES_REL>();
            this.GPS_ALARMAS = new HashSet<GPS_ALARMAS>();
        }
    
        public short ID { get; set; }
        public string DESCRIPCION_LARGA { get; set; }
        public string DESCRIPCION_MED { get; set; }
        public string DESCRIPCION_CORTA { get; set; }
        public string ABREVIACION { get; set; }
        public Nullable<byte> ESTADO_ID { get; set; }
        public Nullable<byte> FUENTE_ID { get; set; }
        public Nullable<byte> PERMANENCIA_ID { get; set; }
        public Nullable<byte> PERMANENCIA_RESETEO_ID { get; set; }
        public Nullable<byte> PERMANENCIA_RESETEO_EVENTO_ID { get; set; }
        public bool MEDICION_TIEMPO { get; set; }
        public Nullable<byte> UNIDAD_ID { get; set; }
        public short CATEGORIA_ID { get; set; }
        public string OBSERVACIONES { get; set; }
        public bool REGISTRA_INTERVALOS { get; set; }
        public string ORDENADOR { get; set; }
    
        public virtual ALARMAS_CATS ALARMAS_CATS { get; set; }
        public virtual ALARMAS_UNIDADES ALARMAS_UNIDADES { get; set; }
        public virtual ALARMAS_PERMANS ALARMAS_PERMANS { get; set; }
        public virtual ALARMAS_PERMANS_RESETS ALARMAS_PERMANS_RESETS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALARMAS_NIVELES_REL> ALARMAS_NIVELES_REL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_ALARMAS> GPS_ALARMAS { get; set; }
    }
}
