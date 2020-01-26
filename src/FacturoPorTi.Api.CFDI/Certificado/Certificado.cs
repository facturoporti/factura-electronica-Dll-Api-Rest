
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FacturoPorTi.Api.Cfdi.Seguridad
{
    public class Certificado
    {
        public X509Certificate2 CertificadoX509;
        public string Contraseña { get; set; }
        public static LlavePrivada Llave;
        public string Mensaje;
        public static string Modulo;
        public Properties Propiedades;
        private static string strXML;
        public string TipoCertificado;

        public Certificado()
        {
            this.inicializar();
        }

        public void CargarCertificado(byte[] bufferCertificado)
        {
            try
            {
                this.CertificadoX509 = new X509Certificate2(bufferCertificado);
            }
            catch (Exception exception)
            {
                this.Mensaje = "Error al cargar el buffer del certificado. " + exception.Message;
                return;
            }
            this.cargarPropiedades();
        }

        public byte[] GeneraPFX(Byte[] certificado, Byte[] llavePrivada, string passsword)
        {
            byte[] pfx = null;

            CargarCertificado(certificado);

            if (this.CertificadoX509 == null)
            {
                this.Mensaje = "No ha cargado un certificado.";
            }
            else
            {
                CspParameters Params = new CspParameters();
                Params.KeyContainerName = "KeyContainer";
                RSACryptoServiceProvider Llave = new RSACryptoServiceProvider(Params);
                
                try
                {
                    Llave = Firmado.ObtenerKey(llavePrivada, passsword);
                    this.CertificadoX509.PrivateKey = Llave;
                    pfx = this.CertificadoX509.Export(X509ContentType.Pfx, passsword);
                }
                catch
                {
                    this.Mensaje = "Error de correspondencia entre certificado y llave privada.";
                }
            }
            return pfx;
        }

        private void cargarPropiedades()
        {
            if (this.CertificadoX509 == null)
            {
                this.Mensaje = "No ha cargado un certificado.";
            }
            else
            {
                try
                {
                    strXML = this.certToXML(this.CertificadoX509);
                    this.Propiedades.Version = this.CertificadoX509.Version.ToString();
                    this.Propiedades.Sujeto = this.CertificadoX509.Subject;
                    this.Propiedades.Emisor = this.CertificadoX509.Issuer;
                    this.Propiedades.ValidoDesde = this.CertificadoX509.NotAfter.ToString("s");
                    this.Propiedades.ValidoHasta = this.CertificadoX509.NotBefore.ToString("s");
                    this.Propiedades.RFC = this.CertificadoX509.Subject.Substring(this.CertificadoX509.Subject.IndexOf("OID.2.5.4.45=") + 13, 13).Trim();
                    this.Propiedades.Fiel = this.CertificadoX509.Subject.IndexOf("OU=") < 0;
                    this.Propiedades.NoSerie = this.SerialASCIItoHex(this.CertificadoX509.SerialNumber);
                    int startIndex = this.CertificadoX509.Subject.IndexOf(" OID.2.5.4.41=") + 14;
                    int index = this.CertificadoX509.Subject.IndexOf(", ", startIndex);
                    this.Propiedades.RazonSocial = this.CertificadoX509.Subject.Substring(startIndex, index - startIndex).Trim();
                }
                catch (Exception exception)
                {
                    this.Mensaje = "Error al leer las propiedades del certificado. " + exception.Message;
                }
            }
        }

        private string certToXML(X509Certificate2 cert)
        {
            string str2;
            MemoryStream input = new MemoryStream(cert.GetPublicKey());
            BinaryReader reader = new BinaryReader(input);
            ushort num = 0;
            try
            {
                num = reader.ReadUInt16();
                if (num == 0x8130)
                {
                    reader.ReadByte();
                }
                else if (num == 0x8230)
                {
                    reader.ReadInt16();
                }
                else
                {
                    return null;
                }
                num = reader.ReadUInt16();
                byte num2 = 0;
                byte num3 = 0;
                if (num == 0x8102)
                {
                    num2 = reader.ReadByte();
                }
                else if (num == 0x8202)
                {
                    num3 = reader.ReadByte();
                    num2 = reader.ReadByte();
                }
                else
                {
                    return null;
                }
                byte[] buffer5 = new byte[4];
                buffer5[0] = num2;
                buffer5[1] = num3;
                byte[] buffer4 = buffer5;
                int count = BitConverter.ToInt32(buffer4, 0);
                if (reader.PeekChar() == 0)
                {
                    reader.ReadByte();
                    count--;
                }
                byte[] data = reader.ReadBytes(count);
                if (reader.ReadByte() != 2)
                {
                    return null;
                }
                int num6 = reader.ReadByte();
                byte[] buffer2 = reader.ReadBytes(num6);
                if (reader.PeekChar() != -1)
                {
                    return null;
                }
                this.showBytes("\nExponent", buffer2);
                this.showBytes("\nModulus", data);
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                RSAParameters parameters = new RSAParameters {
                    Modulus = data
                };
                Modulo = Encoding.UTF8.GetString(data);
                parameters.Exponent = buffer2;
                provider.ImportParameters(parameters);
                str2 = provider.ToXmlString(false);
            }
            catch (Exception)
            {
                str2 = null;
            }
            finally
            {
                reader.Close();
            }
            return str2;
        }
        
        private void inicializar()
        {
            this.Mensaje = "";
            this.TipoCertificado = "";
            Modulo = "";
            Contraseña = "";
            this.Propiedades.ValidoDesde = "";
            this.Propiedades.Version = "";
            this.Propiedades.Emisor = "";
            this.Propiedades.Sujeto = "";
            this.Propiedades.ValidoDesde = "";
            this.Propiedades.ValidoHasta = "";
            this.Propiedades.NoSerie = "";
            this.Propiedades.RFC = "";
            this.Propiedades.RazonSocial = "";
            this.Propiedades.Fiel = false;
        }

        private string SerialASCIItoHex(string serialAscci)
        {
            string str = "";
            for (int i = 0; i <= (serialAscci.Length - 1); i++)
            {
                if ((i % 2) == 1)
                {
                    str = str + serialAscci.Substring(i, 1);
                }
            }
            return str;
        }

        private void showBytes(string info, byte[] data)
        {
            for (int i = 1; i <= data.Length; i++)
            {
                if ((i % 0x10) == 0)
                {
                }
            }
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct Properties
        {
            public string Version;
            public bool Fiel;
            public string Emisor;
            public string Sujeto;
            public string ValidoDesde;
            public string ValidoHasta;
            public string NoSerie;
            public string RFC;
            public string RazonSocial;
        }
    }
}
