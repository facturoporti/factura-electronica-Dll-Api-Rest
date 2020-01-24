using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FacturoPorTi.CFDI
{
    public class CancelarCFDIPeticion
    {
        #region "Variables publicas"

        public string Usuario { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// RFC del emisor 
        /// </summary>
        public string RFC { get; set; }
        /// <summary>
        /// Este valor se genera automáticamente no asignar
        /// </summary>
        public string PFX { get; set; }
        /// <summary>
        /// Este valor se asigna autoáticamente no asignar
        /// </summary>
        public string PFXPassword { get; set; }
        public List<string> UUIDs { get; set; }

        #endregion "Variables publicas"
    }
}
