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
    
    public partial class ENTIDADES_EMAILS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ENTIDADES_EMAILS()
        {
            this.PROPOSITOS = new HashSet<ENTIDADES_EMAILS_PROPS>();
            this.ENTIDADES_NOTIF_ALARMAS_EMS = new HashSet<ENTIDADES_NOTIF_ALARMAS_EMS>();
        }
    
        public int ID { get; set; }
        public int ENTIDAD_ID { get; set; }
        public byte TIPO_ID { get; set; }
        public string EMAIL { get; set; }
        public Nullable<byte> ESTADO_ID { get; set; }
    
        public virtual EMAIL_TIPOS TIPO { get; set; }
        public virtual ENTIDADES ENTIDAD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENTIDADES_EMAILS_PROPS> PROPOSITOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENTIDADES_NOTIF_ALARMAS_EMS> ENTIDADES_NOTIF_ALARMAS_EMS { get; set; }
    }
}
