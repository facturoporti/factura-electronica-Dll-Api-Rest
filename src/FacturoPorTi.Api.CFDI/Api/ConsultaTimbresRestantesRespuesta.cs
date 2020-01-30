using System;

namespace FacturoPorTi.Api.Cfdi
{
    public class ConsultaTimbresRestantesRespuesta : WCFRespuesta
    {
        private string _FechaCompra;
        public string FechaCompra
        {
            get
            {
                return _FechaCompra;
            }
            set
            {
                this._FechaCompra = value;
            }
        }

        private int _TimbresUtilizados;
        public int TimbresUtilizados
        {
            get
            {
                return _TimbresUtilizados;
            }
            set
            {
                this._TimbresUtilizados = value;
            }
        }

        private int _CreditosRestantes;

        public int CreditosRestantes
        {
            get
            {
                return _CreditosRestantes;
            }
            set
            {
                this._CreditosRestantes = value;
            }
        }
    }
}
