using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Globalization;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;
using FacturoPorTi.CFDI.Genericos;
using FacturoPorTi.CFDI.Entidades;
using System.IO;

namespace FacturoPorTi.CFDI.Seguridad
{
    public class Configuracion
    {
        #region "Constructor"

        public Configuracion()
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
        ~Configuracion()
        {
            this.Dispose();
        }

        #endregion "Constructor"

        #region "Propiedades"
       
        public bool Respuesta { get; set; }

        public string Mensaje { get; set; }

        #endregion "Propiedades"

        #region "Metodos Privados"

        public bool CargaCertificado(byte[] _Certificado, byte[] _LlavePrivada, string _Password, string _RFC)
        {
            bool resultado = false;
            var mensaje = string.Empty;

            Archivos archivo = new Archivos();            
            CertificadoUtilerias utilerias = new CertificadoUtilerias();
            
            try
            {                
                resultado = utilerias.ObtieneGenerica(_Certificado);
                Mensaje = utilerias.Mensaje;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo leer el certificado, por favor verifique.";
            }

            if (resultado == true)
            {                
                // Valida que sea un tipo de certificado valido
                // Valida la fecha y que pueda generar cifrar una cadena 
                resultado = false;
                X509Certificate2 cert = new X509Certificate2(_Certificado);
                int indice = -1;

                // Valida que sea un CSD
                try
                {
                    indice = cert.Subject.ToUpper().IndexOf("OU=");
                }
                catch
                {
                }

                if (indice > -1)
                {
                    // Valida el RFC sea identico a la de la empresa configurada configurada
                    indice = -1;
                    indice = cert.Subject.ToUpper().IndexOf(_RFC.ToUpper());

                    if (indice > -1)
                    {
                        CertificateSAT certificate = new CertificateSAT();
                        resultado = certificate.ValidateCertificatePrivateKey(_Certificado, _LlavePrivada, _Password);

                        if (resultado == true)
                        {
                            if (DateTime.Now.CompareTo(utilerias.FechaInicioVigencia) <= 0)
                            {
                                Mensaje = "La fecha y hora de inicio de vigencia del Certificado de Sello Digital es posterior a la fecha y hora actual. Por favor espere hasta que la fecha y hora de inicio de vigencia del Certificado de Sello Digital sea la correcta.";
                            }
                            else
                            {
                                // Se le agrega 30 minutos para la generacion de los CFDI's
                                if (DateTime.Now.AddMinutes(30).CompareTo(utilerias.FechaTerminoVigencia) <= 0)
                                {                                    
                                    resultado = true;
                                }
                                else
                                {
                                    Mensaje = "El Certificado de Sello Digital seleccionado ha caducado." + Environment.NewLine + Environment.NewLine;
                                    Mensaje += "Diríjase a la página del SAT para solicitar otro certificado de sello digital o seleccione otro certificado de sello digital para generar el comprobante.";
                                }
                            }
                        }
                        else
                        {
                            Mensaje = certificate.Mensaje;
                        }
                    }
                    else
                    {
                        Mensaje = "El RFC del certificado de sello digital no es igual al que esta registrado en la informacion fiscal del Contribuyente seleccionado." + Environment.NewLine + Environment.NewLine + "Por favor verifique que el RFC y el certificado de sello digital sean correctos";
                    }
                }
                else
                {
                    Mensaje = "El archivo con extensión .cer no es un certificado de sello digital válido, probablemente pertenece a su clave FIEL." + Environment.NewLine + Environment.NewLine;
                    Mensaje += "Si tiene dudas de como generar el certificado de sello digital por favor revise el manual que se encuentra en el menú Ayuda";
                }
            }

            return resultado;
        }

        #endregion "Metodos Privados"
    }
}
