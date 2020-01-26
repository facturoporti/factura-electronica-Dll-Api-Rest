using System;

namespace FacturoPorTi.Api.Cfdi.Entidades
{    
    public class RespuestaBase
    {
        public RespuestaBase()
        {        
        }
       
        /// <summary>
        /// Libera los recursos de la memoria
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Es el destructor de la clase
        /// </summary>
        ~RespuestaBase()
        {
            this.Dispose();
        }
              
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
    }
}
