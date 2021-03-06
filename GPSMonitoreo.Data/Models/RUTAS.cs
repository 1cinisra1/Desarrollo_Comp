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
    
    public partial class RUTAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RUTAS()
        {
            this.RUTA_TRAMOS = new HashSet<RUTAS_TRAMOS>();
            this.VIAJES_RUTAS = new HashSet<VIAJES_RUTAS>();
            this.RUTAS_FASES_REL = new HashSet<RUTAS_FASES_REL>();
            this.TRAYECTOS_RUTAS = new HashSet<TRAYECTOS_RUTAS>();
        }
    
        public int ID { get; set; }
        public string AUXILIAR_ID { get; set; }
        public string DESCRIPCION_LARGA { get; set; }
        public string DESCRIPCION_MED { get; set; }
        public string DESCRIPCION_CORTA { get; set; }
        public string ABREVIACION { get; set; }
        public short CATEGORIA_ID { get; set; }
        public Nullable<short> FRENTE_ID { get; set; }
        public short DISTANCIA { get; set; }
        public short DISTANCIA_CURVAS { get; set; }
        public byte ESTADO_ID { get; set; }
        public Nullable<int> ENTIDAD_ID { get; set; }
        public string OBSERVACIONES { get; set; }
        public bool ES_RETORNO { get; set; }
    
        public virtual RUTAS_CATS CATEGORIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RUTAS_TRAMOS> RUTA_TRAMOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VIAJES_RUTAS> VIAJES_RUTAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RUTAS_FASES_REL> RUTAS_FASES_REL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRAYECTOS_RUTAS> TRAYECTOS_RUTAS { get; set; }
    }
}
