using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FacturoPorTi.CFDI
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
