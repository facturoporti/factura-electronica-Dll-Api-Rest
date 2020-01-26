namespace FacturoPorTi.Api.Cfdi.Entidades
{
    public class Folios
    {
        public string[] UUID;
    }

    public class FoliosRespuesta
    {
        public string UUID;
        public Estatus Estatus;
        public string EstatusCancelacionSAT;
    }
}
