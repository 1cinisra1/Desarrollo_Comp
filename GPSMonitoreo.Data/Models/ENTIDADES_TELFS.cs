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
    
    public partial class ENTIDADES_TELFS
    {
        public int ENTIDAD_ID { get; set; }
        public byte TIPO_ID { get; set; }
        public string TELEFONO { get; set; }
        public short CODIGO_PAIS { get; set; }
        public short CODIGO_AREA { get; set; }
        public int ID { get; set; }
        public Nullable<int> DIRECCION_ID { get; set; }
        public Nullable<int> UBICACION_ID { get; set; }
    
        public virtual ENTIDADES ENTIDAD { get; set; }
        public virtual TELEFONO_TIPOS TIPO { get; set; }
        public virtual ENTIDADES_DIRS DIRECCION { get; set; }
        public virtual ENTIDADES_DIRS_UBICAS UBICACION { get; set; }
    }
}
