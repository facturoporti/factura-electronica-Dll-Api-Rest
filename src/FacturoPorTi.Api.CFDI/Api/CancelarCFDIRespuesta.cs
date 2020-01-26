using System.Collections.Generic;
using FacturoPorTi.Api.Cfdi.Entidades;

namespace FacturoPorTi.Api.Cfdi
{
    public class CancelarCFDIRespuesta : WCFRespuesta
    {
        private List<FoliosRespuesta> FoliosRespuestaField;
        public List<FoliosRespuesta> FoliosRespuesta
        {
            get
            {
                return this.FoliosRespuestaField;
            }
            set
            {
                this.FoliosRespuestaField = value;
            }
        }
    }
}
