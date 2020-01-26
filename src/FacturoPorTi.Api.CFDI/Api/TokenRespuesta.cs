namespace FacturoPorTi.Api.Cfdi.Entidades
{
    public class TokenRespuesta : RespuestaBase
    {                
        public string Token { get; set; }
        public string CorreoUsuario { get; set; }
        public string IdEmpresa { get; set; }
        public string IdEmpresaPadre { get; set; }
    }
}
