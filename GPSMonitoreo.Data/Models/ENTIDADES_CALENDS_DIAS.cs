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
    
    public partial class ENTIDADES_CALENDS_DIAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ENTIDADES_CALENDS_DIAS()
        {
            this.HORARIOS = new HashSet<ENTIDADES_CALENDS_DIA_HRS>();
        }
    
        public int ID { get; set; }
        public int CALENDARIO_ID { get; set; }
        public byte MES { get; set; }
        public byte DIA { get; set; }
        public byte TIPO_ID { get; set; }
        public string OBSERVACIONES { get; set; }
    
        public virtual DIAS_TIPOS TIPODIA { get; set; }
        public virtual ENTIDADES_CALENDS CALENDARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENTIDADES_CALENDS_DIA_HRS> HORARIOS { get; set; }
    }
}
