using System;

namespace FacturoPorTi.Api.Cfdi.Entidades
{

    public class UsuarioPeticion
    {
        public UsuarioPeticion()
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
        ~UsuarioPeticion()
        {
            this.Dispose();
        }

        public string IdEmpresa { get; set; }       
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string RFC { get; set; }
        public string Correo { get; set; }
        public string MR { get; set; }
        public string NuevoPassword { get; set; }
        public int IdAplicacion { get; set; }
        public int IdPerfil { get; set; }
    }
}
