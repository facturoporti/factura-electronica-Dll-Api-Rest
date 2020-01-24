
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturoPorTi.CFDI.Entidades
{
    public class TokenRespuesta : RespuestaBase
    {                
        public string Token { get; set; }
        public string CorreoUsuario { get; set; }
        public string IdEmpresa { get; set; }
        public string IdEmpresaPadre { get; set; }
    }
}
