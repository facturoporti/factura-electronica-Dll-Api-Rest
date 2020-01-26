using System;
using System.Collections.Generic;

namespace FacturoPorTi.Api.Cfdi
{
    #region "Nuevas Propiedades Generacion CFDI"
        
    public class CFDIPeticion
    {
        #region "Variables privadas"

        private DatosGeneralesCFDI _datosGeneralesField;
        private EncabezadoCFDI _encabezadoField;
        private List<ConceptoCFDI> _conceptosField;
        private ComplementoCFDI _complementoCFDIField;

        #endregion "Variables privadas"

        #region "Variables publicas"
                
        public DatosGeneralesCFDI DatosGenerales
        {
            get
            {
                return this._datosGeneralesField;
            }
            set
            {
                this._datosGeneralesField = value;
            }
        }            
        public EncabezadoCFDI Encabezado
        {
            get
            {
                return this._encabezadoField;
            }
            set
            {
                this._encabezadoField = value;
            }
        }        
        public List<ConceptoCFDI> Conceptos
        {
            get
            {
                return this._conceptosField;
            }
            set
            {
                this._conceptosField = value;
            }
        }
        public ComplementoCFDI Complemento
        {
            get
            {
                return this._complementoCFDIField;
            }
            set
            {
                this._complementoCFDIField = value;
            }
        }

        #endregion "Variables publicas"
    }

    public class DatosGeneralesCFDI
    {
        #region "Propiedades Publicas"
        
        public string Version { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }        
        public string SellaCFDI { get; set; }        
        public string TimbraCFDI { get; set; }

        /// <summary>
        /// Esta propiedad se llena automáticamente cuando se realizan las validaciones del certificado
        /// No se puede inicializar de manera manual
        /// </summary>
        public string CSD { get; set; }
        /// <summary>
        /// Esta propiedad se llena automáticamente cuando se realizan las validaciones del certificado
        /// No se puede inicializar de manera manual
        /// </summary>
        public string LlavePrivada { get; set; }
        /// <summary>
        /// Esta propiedad se llena automáticamente cuando se realizan las validaciones del certificado
        /// No se puede inicializar de manera manual
        /// </summary>
        public string CSDPassword { get; set; }
        public string GeneraPDF { get; set; }
        public string Logotipo { get; set; }
        public string FormatoImpresion { get; set; }        
        public string CFDI { get; set; }       
        public string OpcionDecimales { get; set; }        
        public string NumeroDecimales { get; set; }        
        public string TipoCFDI { get; set; }
        public string EnviaEmail { get; set; }
        public string ReceptorEmail { get; set; }
        public string ReceptorEmailCC { get; set; }
        public string ReceptorEmailCCO { get; set; }
        public string EmailMensaje { get; set; }

        #endregion "Propiedades Publicas"
    }

    public class EncabezadoCFDI
    {
        #region "Propiedades Publicas"
                
        public EmisorCFDI Emisor { get; set; }        
        public ReceptorCFDI Receptor { get; set; }        
        public string Fecha { get; set; }        
        public string Serie { get; set; }        
        public string Folio { get; set; }        
        public string MetodoPago { get; set; }        
        public string FormaPago { get; set; }        
        public string CondicionesPago { get; set; }        
        public string Moneda { get; set; }        
        public string TipoCambio { get; set; }        
        public string LugarExpedicion { get; set; }        
        public string SubTotal { get; set; }        
        public string Total { get; set; }        
        public string Descuento { get; set; }        
        public string CFDIsRelacionados { get; set; }        
        public string TipoRelacion { get; set; }

        #region "Carta Porte"                
        public string FechaEntrega { get; set; }
        public string Remitente { get; set; }
        public string RemitenteDomicilio { get; set; }
        public string RemitenteLugarRecoger { get; set; }
        public string Destinatario { get; set; }
        public string DestinatarioDomicilio { get; set; }
        public string DestinatarioLugarEntrega { get; set; }
        public string DescripcionMercancia { get; set; }
        public string Peso { get; set; }
        public string MetrosCubicos { get; set; }
        public string Litros { get; set; }
        public string MaterialPeligroso { get; set; }
        public string ValorDeclarado { get; set; }
        public string Indemnizacion { get; set; }
        public string Conductor { get; set; }
        public string Vehiculo { get; set; }
        public string Placas { get; set; }
        public string Kilometros { get; set; }
        public string Observaciones { get; set; }

        #endregion "Carta Porte"

        #region "Adicionales"        
        public string CampoAdicional_1 { get; set; }        
        public string CampoAdicional_2 { get; set; }        
        public string CampoAdicional_3 { get; set; }        
        public string CampoAdicional_4 { get; set; }        
        public string CampoAdicional_5 { get; set; }        
        public string CampoAdicional_6 { get; set; }        
        public string CampoAdicional_7 { get; set; }        
        public string CampoAdicional_8 { get; set; }        
        public string CampoAdicional_9 { get; set; }        
        public string CampoAdicional_10 { get; set; }        
        public string CampoAdicional_11 { get; set; }        
        public string CampoAdicional_12 { get; set; }        
        public string CampoAdicional_13 { get; set; }        
        public string CampoAdicional_14 { get; set; }        
        public string CampoAdicional_15 { get; set; }        
        public string CampoAdicional_16 { get; set; }        
        public string CampoAdicional_17 { get; set; }        
        public string CampoAdicional_18 { get; set; }        
        public string CampoAdicional_19 { get; set; }        
        public string CampoAdicional_20 { get; set; }

        #endregion "Adicionales"

        #endregion "Propiedades Publicas"
    }

    public class EmisorCFDI
    {
        #region "Propiedades Publicas"        
        public string RFC { get; set; }        
        public string NombreRazonSocial { get; set; }        
        public string RegimenFiscal { get; set; }        
        public List<DireccionCFDI> Direccion { get; set; }

        #endregion "Propiedades Publicas"
    }

    public class DireccionCFDI
    {
        #region "Propiedades Publicas"        

        public string Calle { get; set; }        
        public string NumeroExterior { get; set; }        
        public string NumeroInterior { get; set; }        
        public string Colonia { get; set; }        
        public string Localidad { get; set; }        
        public string Referencias { get; set; }              
        public string Municipio { get; set; }        
        public string Estado { get; set; }        
        public string Pais { get; set; }        
        public string CodigoPostal { get; set; }        
        public string ClavePais { get; set; }        

        #endregion "Propiedades Publicas"
    }

    public class ReceptorCFDI
    {
        #region "Propiedades Publicas"        
        public string RFC { get; set; }        
        public string NombreRazonSocial { get; set; }        
        public string UsoCFDI { get; set; }        
        public DireccionCFDI Direccion { get; set; }

        #endregion "Propiedades Publicas"
    }

    public class ConceptoCFDI
    {
        #region "Propiedades Publicas"
        
        public string Cantidad { get; set; }        
        public string CodigoUnidad { get; set; }        
        public string Unidad { get; set; }        
        public string Serie { get; set; }        
        public string CodigoProducto { get; set; }        
        public string Producto { get; set; }        
        public string PrecioUnitario { get; set; }        
        public string Importe { get; set; }        
        public string Descuento { get; set; }        
        public string CuentaPredial { get; set; }        
        public string Adicional_1 { get; set; }        
        public string Adicional_2 { get; set; }        
        public string Adicional_3 { get; set; }        
        public string Adicional_4 { get; set; }        
        public string Adicional_5 { get; set; }        
        public string Adicional_6 { get; set; }        
        public string Adicional_7 { get; set; }        
        public string Adicional_8 { get; set; }        
        public string Adicional_9 { get; set; }        
        public string Adicional_10 { get; set; }
        
        public List<ImpuestosCFDI> Impuestos { get; set; }

        #endregion "Propiedades Publicas"        
    }

    public class ImpuestosCFDI
    {
        #region "Propiedades Publicas"
        
        public string TipoImpuesto { get; set; }        
        public string Impuesto { get; set; }        
        public string Factor { get; set; }        
        public string Base { get; set; }        
        public string Tasa { get; set; }        
        public string ImpuestoImporte { get; set; }

        #endregion "Propiedades Publicas"
    }

    public class ComplementoCFDI
    {
        #region "Propiedades Publicas"
                
        public List<Pagos> Pagos { get; set; }        
        public Donativos Donativos { get; set; }
       
        #endregion "Propiedades Publicas"
    }

    public class Pagos
    {
        #region "Propiedades Publicas"

        
        public string FechaPago { get; set; }

        
        public string FormaPago { get; set; }

        
        public string Moneda { get; set; }

        
        public Nullable<decimal> TipoCambio { get; set; }

        
        public string BancoExtranjero { get; set; }

        
        public string NumeroOperacion { get; set; }

        
        public string RFCCtaOrdenante { get; set; }

        
        public string CuentaOrdenante { get; set; }

        
        public string RFCCtaBeneficiario { get; set; }

        
        public string CuentaBeneficiario { get; set; }

        
        public List<DocumentoRelacionadosPagos> DocumentosRelacionados { get; set; }

        
        public string Adicional_1 { get; set; }

        
        public string Adicional_2 { get; set; }

        
        public string Adicional_3 { get; set; }

        
        public string Adicional_4 { get; set; }

        
        public string Adicional_5 { get; set; }

        
        public string Adicional_6 { get; set; }

        
        public string Adicional_7 { get; set; }

        
        public string Adicional_8 { get; set; }

        
        public string Adicional_9 { get; set; }

        
        public string Adicional_10 { get; set; }

        #endregion "Propiedades Publicas"        
    }

    public class DocumentoRelacionadosPagos
    {
        #region "Propiedades Publicas"        
        public string UUID { get; set; }        
        public string Serie { get; set; }        
        public string Folio { get; set; } 
        public string Moneda { get; set; }        
        public string TipoCambio { get; set; }        
        public string MetodoPago { get; set; }        
        public string NumeroParcialidad { get; set; }        
        public string ImporteSaldoAnterior { get; set; }       
        public string ImportePagado { get; set; }        
        public string ImporteSaldoInsoluto { get; set; }          
        public string Adicional_1 { get; set; }        
        public string Adicional_2 { get; set; }        
        public string Adicional_3 { get; set; }        
        public string Adicional_4 { get; set; }        
        public string Adicional_5 { get; set; }
      
        #endregion "Propiedades Publicas"        
    }

    public class Donativos
    {
        #region "Propiedades Publicas"        
        public string Leyenda { get; set; }        
        public string FechaAutorizacion { get; set; }        
        public string NumeroAutorizacion { get; set; }
        
        #endregion "Propiedades Publicas"        
    }

    #endregion "Nuevas Propiedades Generacion CFDI"

}
