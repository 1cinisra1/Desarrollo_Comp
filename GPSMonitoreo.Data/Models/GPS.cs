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
    
    public partial class GPS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GPS()
        {
            this.GPS_ALARMAS_OCURRS = new HashSet<GPS_ALARMAS_OCURRS>();
        }
    
        public int ID { get; set; }
        public string DESCRIPCION_LARGA { get; set; }
        public string DESCRIPCION_MED { get; set; }
        public string DESCRIPCION_CORTA { get; set; }
        public string ABREVIACION { get; set; }
        public string IMEI { get; set; }
        public short MARCA_ID { get; set; }
        public short MODELO_ID { get; set; }
        public Nullable<int> EQUIPO_ID { get; set; }
        public byte ESTADO_ID { get; set; }
        public string OBSERVACIONES { get; set; }
    
        public virtual EQUIPOS EQUIPO { get; set; }
        public virtual MARCAS MARCA { get; set; }
        public virtual MODELOS MODELO { get; set; }
        public virtual GPS_MONI_ESTADO MONITOREO_ESTADO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_ALARMAS_OCURRS> GPS_ALARMAS_OCURRS { get; set; }
    }
}
