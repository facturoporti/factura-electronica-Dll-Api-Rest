using System.Collections.Generic;

namespace FacturoPorTi.Api.Cfdi
{
    public class ConsultaEstatusPeticion
    {
        #region "Variables publicas"

        public string Usuario { get; set; }
        public string Password { get; set; }        
        public List<string> UUIDs { get; set; }

        #endregion "Variables publicas"
    }
}
