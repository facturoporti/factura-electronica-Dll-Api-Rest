using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FacturoPorTi.CFDI
{
    #region "Respuestas Api Generacion"

    public class WCFRespuesta
    {
        /// <summary>
        /// Objeto que contiene el detalle del estatus de la respuesta 
        /// en caso de error se envía el mensaje correspondiente y el codigo de respuesta
        /// </summary>
        private Estatus estatusField;
        public Estatus Estatus
        {
            get
            {
                return this.estatusField;
            }
            set
            {
                this.estatusField = value;
            }
        }
    }
    public class Estatus
    {
        private string codigoField;
        public string Codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }

        private string descripcionField;
        public string Descripcion
        {
            get
            {
                return this.descripcionField;
            }
            set
            {
                this.descripcionField = value;
            }
        }

        private string informacionTecnicaField;
        public string InformacionTecnica
        {
            get
            {
                return this.informacionTecnicaField;
            }
            set
            {
                this.informacionTecnicaField = value;
            }
        }

        /// <summary>
        /// Fecha  y hora de la respuesta 
        /// </summary>
        private string fechaField;
        public string Fecha
        {
            get
            {
                return this.fechaField;
            }
            set
            {
                this.fechaField = value;
            }
        }

        /// <summary>
        /// Fecha  y hora de la respuesta 
        /// </summary>
        private bool DetieneEjecucionProveedorField;
        public bool DetieneEjecucionProveedor
        {
            get
            {
                return this.DetieneEjecucionProveedorField;
            }
            set
            {
                this.DetieneEjecucionProveedorField = value;
            }
        }
    }

    public class GeneraCFDIApiRespuesta : WCFRespuesta
    {
        public CFDITimbradoRespuesta CFDITimbrado { get; set; }
    }

    
    public class CFDITimbradoRespuesta
    {
        private CFDITimbreRespuesta timbreField;       
        public CFDITimbreRespuesta Respuesta
        {
            get
            {
                return this.timbreField;
            }
            set
            {
                this.timbreField = value;
            }
        }
    }


    
    public class CFDITimbreRespuesta
    { 
        public string IdVersionTimbrado { get; set; }        
        public string Fecha { get; set; }        
        public string CadenaOriginal { get; set; }        
        public string CadenaOriginalCFD { get; set; }        
        public string NoCertificado { get; set; }        
        public string RfcProvCertif { get; set; }        
        public string SelloCFD { get; set; }        
        public string SelloSAT { get; set; }        
        public string TimbreXML { get; set; }        
        public string UUID { get; set; }        
        public string CFDIXML { get; set; }        
        public string EmailEnviado { get; set; }        
        public string PDF { get; set; }      
    }

   
    #endregion "Respuestas Api Generacion"
}
