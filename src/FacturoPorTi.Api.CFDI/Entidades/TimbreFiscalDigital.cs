namespace FacturoPorTi.Api.Cfdi.Entidades
{
    
    using System.Xml.Serialization;
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.18020")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital", IsNullable = false)]
    public partial class TimbreFiscalDigital
    {

        private string versionField;

        private string uUIDField;

        private System.DateTime fechaTimbradoField;

        private string rfcProvCertifField;

        private string leyendaField;

        private string selloCFDField;

        private string noCertificadoSATField;

        private string selloSATField;

        public TimbreFiscalDigital()
        {
            this.versionField = "1.1";
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string UUID
        {
            get
            {
                return this.uUIDField;
            }
            set
            {
                this.uUIDField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime FechaTimbrado
        {
            get
            {
                return this.fechaTimbradoField;
            }
            set
            {
                this.fechaTimbradoField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RfcProvCertif
        {
            get
            {
                return this.rfcProvCertifField;
            }
            set
            {
                this.rfcProvCertifField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Leyenda
        {
            get
            {
                return this.leyendaField;
            }
            set
            {
                this.leyendaField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SelloCFD
        {
            get
            {
                return this.selloCFDField;
            }
            set
            {
                this.selloCFDField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string NoCertificadoSAT
        {
            get
            {
                return this.noCertificadoSATField;
            }
            set
            {
                this.noCertificadoSATField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SelloSAT
        {
            get
            {
                return this.selloSATField;
            }
            set
            {
                this.selloSATField = value;
            }
        }
    }

}