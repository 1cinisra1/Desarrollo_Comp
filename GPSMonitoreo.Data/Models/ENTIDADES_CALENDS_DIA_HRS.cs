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
    
    public partial class ENTIDADES_CALENDS_DIA_HRS
    {
        public int DIA_ID { get; set; }
        public byte ORDEN { get; set; }
        public string DESDE { get; set; }
        public string HASTA { get; set; }
        public string OBSERVACIONES { get; set; }
    
        public virtual ENTIDADES_CALENDS_DIAS CALENDARIO_DIA { get; set; }
    }
}
