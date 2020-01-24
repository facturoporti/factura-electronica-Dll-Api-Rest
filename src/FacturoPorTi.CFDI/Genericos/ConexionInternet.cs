
using System.Runtime.InteropServices;

namespace FacturoPorTi.CFDI.Genericos
{
    public class ConexionInternet
    {
        [DllImport("wininet.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool HayConexion()
        {
            int num;
            bool flag = ConexionInternet.InternetGetConnectedState(out num, 0);
            return flag;
        }
    }
}