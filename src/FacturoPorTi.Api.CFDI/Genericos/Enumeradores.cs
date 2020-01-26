
namespace FacturoPorTi.Api.Cfdi.Genericos
{
    public class Enumeradores
    {        
        public const string RutaDescargaCFDI = "http://www.facturoporti.com.mx/DescargaCFDI/Index/";
    }
    
    //Enumerador con el tipo de documento
    public enum enumTipoDocumento
    {
        Factura = 1,
        NotaCargo = 2,
        NotaCredito = 3,
        ReciboHonorarios = 4,
        ReciboArrendamiento = 5,
        CartaPorte = 6,
        ReciboNominaCFDI = 7,
        ReciboDonatario = 8,
        Pago = 9,
    }
    
    public enum enumTazaIVA
    {
        Excento = 1,
        Cero = 2,
        Fronterizo = 3,
        General = 4,
    }
    
    public enum TipoVerboHttp
    {
        Post = 1,
        Get = 2,
        Put = 3,
        Delete = 4,
        Patch = 5,
        Head = 6,
        Connect = 7,
        Options = 8,
        Trace = 9,
        Custom = 10,
    }

    public enum enumOpcionDecimales
    {
        Truncar = 1,
        Redondear = 2,
    }

    public enum enumTipoImpuesto
    {
        Trasladado = 1,
        Retenido = 2,
    }
    public enum enumImpuesto
    {
        ISR = 1,
        IVA = 2,
        IEPS = 3,
    }

    public enum enumFactor
    {
        Tasa = 1,
        Cuota = 2,
        Exento = 3,
    }
}
