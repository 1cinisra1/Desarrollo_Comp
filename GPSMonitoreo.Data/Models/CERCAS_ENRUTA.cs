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
    
    public partial class CERCAS_ENRUTA
    {
        public int CERCA_ID { get; set; }
        public byte TRAZADO_ID { get; set; }
        public Nullable<byte> CURVA_TIPO_ID { get; set; }
        public Nullable<byte> CURVA_GRADO_ID { get; set; }
        public byte CALZADA_TIPO_ID { get; set; }
        public byte CALZADA_ESTADO_ID { get; set; }
        public byte CARRILES_IDA { get; set; }
        public byte CARRILES_RETORNO { get; set; }
        public byte TRAFICO_ID { get; set; }
        public Nullable<byte> PENDIENTE_IDA_ID { get; set; }
        public Nullable<byte> PENDIENTE_IDA_GRADO_ID { get; set; }
        public Nullable<byte> PENDIENTE_RETORNO_ID { get; set; }
        public Nullable<byte> PENDIENTE_RETORNO_GRADO_ID { get; set; }
        public byte VELOCIDAD_IDA_MOP { get; set; }
        public byte VELOCIDAD_IDA_MOPREST { get; set; }
        public byte VELOCIDAD_IDA_EMPREST { get; set; }
        public byte VELOCIDAD_RETORNO_MOP { get; set; }
        public byte VELOCIDAD_RETORNO_MOPREST { get; set; }
        public byte VELOCIDAD_RETORNO_EMPREST { get; set; }
        public byte CURVA_CANTIDAD { get; set; }
        public double DISTANCIA { get; set; }
        public byte VELOCIDAD_OPTIMA { get; set; }
        public double TIEMPO_OPTIMO { get; set; }
    
        public virtual CERCAS CERCA { get; set; }
        public virtual CERCAS_CALZADAS CALZADA { get; set; }
        public virtual CERCAS_CALZADAS_ESTADOS CALZADA_ESTADO { get; set; }
        public virtual CERCAS_CURVAS_GRADO CURVA_GRADO { get; set; }
        public virtual CERCAS_CURVAS_TIPOS CURVA { get; set; }
        public virtual CERCAS_TRAZADO_VIA TRAZADO { get; set; }
        public virtual CERCAS_TRAFICO TRAFICO { get; set; }
        public virtual CERCAS_PENDIENTES PENDIENTE_IDA { get; set; }
        public virtual CERCAS_PENDIENTES_GRADO PENDIENTE_IDA_GRADO { get; set; }
        public virtual CERCAS_PENDIENTES PENDIENTE_RETORNO { get; set; }
        public virtual CERCAS_PENDIENTES_GRADO PENDIENTE_RETORNO_GRADO { get; set; }
    }
}
