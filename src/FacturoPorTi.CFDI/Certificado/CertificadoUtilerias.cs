using System;
using System.Security.Cryptography.X509Certificates;

namespace FacturoPorTi.CFDI.Seguridad
{
    public class CertificadoUtilerias
    {
        public CertificadoUtilerias()
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
        ~CertificadoUtilerias()
        {
            this.Dispose();
        }

        public string Certificado {get; set;}
        public string NumeroSerieCertificado {get; set;}
        public DateTime FechaInicioVigencia {get; set;}
        public DateTime FechaTerminoVigencia {get; set;}
        public string Mensaje { get; set; }

        /// <summary>
        /// Obtiene los valores del certificado
        /// </summary>
        /// <param name="datos">Es el certificado en bytes</param>
        /// <returns></returns>
        public bool ObtieneGenerica(byte[] datos)
        {            
            string strSerial = string.Empty;
            //string mensaje = string.Empty;
            bool bandera = false;
            bool resultado = false; 

            try
            {
                X509Certificate cCert = new X509Certificate(datos);
                FechaInicioVigencia = DateTime.Parse(cCert.GetEffectiveDateString());
                FechaTerminoVigencia = DateTime.Parse(cCert.GetExpirationDateString());

                for (int i = 0; i < cCert.GetSerialNumberString().Length; i++)
                {
                    if (bandera == true)
                    {
                        strSerial = strSerial + cCert.GetSerialNumberString().Substring(i, 1);
                        bandera = false;
                    }
                    else
                        bandera = true;
                }

                NumeroSerieCertificado = strSerial;
                Certificado = Convert.ToBase64String(cCert.GetRawCertData());

                // Se le agrega un minuto para 
                if (DateTime.Now.CompareTo(FechaTerminoVigencia) <= 0)
                {
                    resultado = true;
                }
                else
                {
                    Mensaje = "El Certificado de Sello Digital de " + cCert.Subject + " ha caducado" + Environment.NewLine + Environment.NewLine;
                    Mensaje = Mensaje + "Diríjase a la página del SAT para solicitar otro certificado de sello digital o seleccione otro certificado de sello digital.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message + " - " + ex.StackTrace;
            }

            return resultado; 
        }

        /// <summary>
        /// Valida la clave privada que corresponda
        /// </summary>
        /// <param name="contraseña">es la ingresada por el usuario</param>
        /// <param name="datos">Es el key en bytes</param>
        /// <returns></returns>
        public bool ValidarLLavePrivada(string contraseña, byte[] datos)
        {
            try
            {
                Org.BouncyCastle.Crypto.AsymmetricKeyParameter asp = Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(contraseña.ToCharArray(), datos);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message + " - " + ex.StackTrace != "pad block corrupted")
                    Mensaje = ex.Message + " - " + ex.StackTrace;
                else
                    Mensaje = "La contraseña del certificado es incorrecta";

                return false; 
            }
        }
    }
}
