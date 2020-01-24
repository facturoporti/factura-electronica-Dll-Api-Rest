using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization; 

namespace FacturoPorTi.CFDI.Entidades
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
