
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace FacturoPorTi.Api.Cfdi.Seguridad
{
    public class LlavePrivada
    {
        public string Contraseña;
        public RSACryptoServiceProvider llaveRSA;
        public string Mensaje;
        public string Modulo;
        private const string pemp8encfooter = "-----END ENCRYPTED PRIVATE KEY-----";
        private const string pemp8encheader = "-----BEGIN ENCRYPTED PRIVATE KEY-----";
        private const string pemp8footer = "-----END PRIVATE KEY-----";
        private const string pemp8header = "-----BEGIN PRIVATE KEY-----";
        private const string pemprivfooter = "-----END RSA PRIVATE KEY-----";
        private const string pemprivheader = "-----BEGIN RSA PRIVATE KEY-----";
        private const string pempubfooter = "-----END PUBLIC KEY-----";
        private const string pempubheader = "-----BEGIN PUBLIC KEY-----";
        public string TipoCertificado;
        private static bool verbose = false;

        public LlavePrivada()
        {
            this.inicializar();
        }

        public LlavePrivada(string rutaLlave, string contraseña)
        {
            this.inicializar();
            this.CargarLlave(rutaLlave, contraseña);
        }

        public void CargarLlave(string rutaLlave, string contraseña)
        {
            RSACryptoServiceProvider provider = null;
            byte[] buffer;
            try
            {
                buffer = File.ReadAllBytes(rutaLlave);
                if (buffer == null)
                {
                    this.Mensaje = "Archivo de llave vacído.";
                    return;
                }
            }
            catch (Exception exception)
            {
                this.Mensaje = "Error al leer el archivo de llave. " + exception.Message;
                return;
            }
            provider = this.DecodeX509PublicKey(buffer);
            if (provider != null)
            {
                this.llaveRSA = provider;
                this.TipoCertificado = "Llave p\x00fablica X509";
            }
            else
            {
                provider = this.DecodeRSAPrivateKey(buffer);
                if (provider != null)
                {
                    this.llaveRSA = provider;
                    this.TipoCertificado = "Llave privada RSA";
                }
                else
                {
                    provider = this.DecodePrivateKeyInfo(buffer);
                    if (provider != null)
                    {
                        this.llaveRSA = provider;
                        this.TipoCertificado = "Llave privada PKCS #8 sin encriptaci\x00f3n";
                    }
                    else
                    {
                        SecureString pass = this.ToSecureString(contraseña);
                        if (pass == null)
                        {
                            this.Mensaje = "La contrase\x00f1a de la llave no puede ser vacída.";
                        }
                        else
                        {
                            provider = this.DecodeEncryptedPrivateKeyInfo(buffer, pass);
                            if (provider != null)
                            {
                                this.llaveRSA = provider;
                                this.TipoCertificado = "Llave privada PKCS #8 encriptada";
                            }
                        }
                    }
                }
            }
        }

        public void CargarLlavePrivadaBytes(byte[] llavePrivada, string contraseña)
        {
            RSACryptoServiceProvider provider = null;
            
            try
            {
                if (llavePrivada == null)
                {
                    this.Mensaje = "Archivo de llave vacído.";
                    return;
                }
            }
            catch (Exception exception)
            {
                this.Mensaje = "Error al leer el archivo de llave. " + exception.Message;
                return;
            }


            provider = this.DecodeX509PublicKey(llavePrivada);
            if (provider != null)
            {
                this.llaveRSA = provider;
                this.TipoCertificado = "Llave p\x00fablica X509";
            }
            else
            {
                provider = this.DecodeRSAPrivateKey(llavePrivada);
                if (provider != null)
                {
                    this.llaveRSA = provider;
                    this.TipoCertificado = "Llave privada RSA";
                }
                else
                {
                    provider = this.DecodePrivateKeyInfo(llavePrivada);
                    if (provider != null)
                    {
                        this.llaveRSA = provider;
                        this.TipoCertificado = "Llave privada PKCS #8 sin encriptaci\x00f3n";
                    }
                    else
                    {
                        SecureString pass = this.ToSecureString(contraseña);
                        if (pass == null)
                        {
                            this.Mensaje = "La contrase\x00f1a de la llave no puede ser vacída.";
                        }
                        else
                        {
                            provider = this.DecodeEncryptedPrivateKeyInfo(llavePrivada, pass);
                            if (provider != null)
                            {
                                this.llaveRSA = provider;
                                this.TipoCertificado = "Llave privada PKCS #8 encriptada";
                            }
                        }
                    }
                }
            }
        }

        public void CargarLlaveBase64(string llavePrivadaBase64, string contraseña)
        {
            RSACryptoServiceProvider provider = null;
            byte[] buffer;
            try
            {
                buffer = Convert.FromBase64String(llavePrivadaBase64);
                if (buffer == null)
                {
                    this.Mensaje = "Archivo de llave vacído.";
                    return;
                }
            }
            catch (Exception exception)
            {
                this.Mensaje = "Error al leer el archivo de llave. " + exception.Message;
                return;
            }
            provider = this.DecodeX509PublicKey(buffer);
            if (provider != null)
            {
                this.llaveRSA = provider;
                this.TipoCertificado = "Llave p\x00fablica X509";
            }
            else
            {
                provider = this.DecodeRSAPrivateKey(buffer);
                if (provider != null)
                {
                    this.llaveRSA = provider;
                    this.TipoCertificado = "Llave privada RSA";
                }
                else
                {
                    provider = this.DecodePrivateKeyInfo(buffer);
                    if (provider != null)
                    {
                        this.llaveRSA = provider;
                        this.TipoCertificado = "Llave privada PKCS #8 sin encriptaci\x00f3n";
                    }
                    else
                    {
                        SecureString pass = this.ToSecureString(contraseña);
                        if (pass == null)
                        {
                            this.Mensaje = "La contrase\x00f1a de la llave no puede ser vacída.";
                        }
                        else
                        {
                            provider = this.DecodeEncryptedPrivateKeyInfo(buffer, pass);
                            if (provider != null)
                            {
                                this.llaveRSA = provider;
                                this.TipoCertificado = "Llave privada PKCS #8 encriptada";
                            }
                        }
                    }
                }
            }
        }

        private bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            int index = 0;
            foreach (byte num2 in a)
            {
                if (num2 != b[index])
                {
                    return false;
                }
                index++;
            }
            return true;
        }

        private RSACryptoServiceProvider DecodeEncryptedPrivateKeyInfo(byte[] encpkcs8, SecureString pass)
        {
            RSACryptoServiceProvider provider2;
            if (pass == null)
            {
                return null;
            }
            byte[] b = new byte[] { 6, 9, 0x2a, 0x86, 0x48, 0x86, 0xf7, 13, 1, 5, 13 };
            byte[] buffer2 = new byte[] { 6, 9, 0x2a, 0x86, 0x48, 0x86, 0xf7, 13, 1, 5, 12 };
            byte[] buffer3 = new byte[] { 6, 8, 0x2a, 0x86, 0x48, 0x86, 0xf7, 13, 3, 7 };
            byte[] a = new byte[10];
            byte[] buffer5 = new byte[11];
            MemoryStream input = new MemoryStream(encpkcs8);
            int length = (int) input.Length;
            BinaryReader reader = new BinaryReader(input);
            byte num6 = 0;
            ushort num7 = 0;
            try
            {
                byte[] buffer6;
                int num3;
                int num4;
                num7 = reader.ReadUInt16();
                if (num7 == 0x8130)
                {
                    reader.ReadByte();
                }
                else if (num7 == 0x8230)
                {
                    reader.ReadInt16();
                }
                else
                {
                    return null;
                }
                switch (reader.ReadUInt16())
                {
                    case 0x8130:
                        reader.ReadByte();
                        break;

                    case 0x8230:
                        reader.ReadInt16();
                        break;
                }
                buffer5 = reader.ReadBytes(11);
                if (this.CompareBytearrays(buffer5, b))
                {
                    num7 = reader.ReadUInt16();
                    switch (num7)
                    {
                        case 0x8130:
                            reader.ReadByte();
                            break;

                        case 0x8230:
                            reader.ReadInt16();
                            break;
                    }
                    num7 = reader.ReadUInt16();
                    if (num7 == 0x8130)
                    {
                        reader.ReadByte();
                    }
                    else if (num7 == 0x8230)
                    {
                        reader.ReadInt16();
                    }
                    buffer5 = reader.ReadBytes(11);
                    if (!this.CompareBytearrays(buffer5, buffer2))
                    {
                        return null;
                    }
                    num7 = reader.ReadUInt16();
                    if (num7 == 0x8130)
                    {
                        reader.ReadByte();
                    }
                    else if (num7 == 0x8230)
                    {
                        reader.ReadInt16();
                    }
                    if (reader.ReadByte() == 4)
                    {
                        int num = reader.ReadByte();
                        buffer6 = reader.ReadBytes(num);
                        if (verbose)
                        {
                            this.showBytes("Salt for pbkd", buffer6);
                        }
                        if (reader.ReadByte() != 2)
                        {
                            return null;
                        }
                        switch (reader.ReadByte())
                        {
                            case 1:
                                num4 = reader.ReadByte();
                                goto Label_02C5;

                            case 2:
                                num4 = (0x100 * reader.ReadByte()) + reader.ReadByte();
                                goto Label_02C5;
                        }
                    }
                }
                return null;
            Label_02C5:
                if (verbose)
                {
                }
                num7 = reader.ReadUInt16();
                if (num7 == 0x8130)
                {
                    reader.ReadByte();
                }
                else if (num7 == 0x8230)
                {
                    reader.ReadInt16();
                }
                a = reader.ReadBytes(10);
                if (!this.CompareBytearrays(a, buffer3))
                {
                    return null;
                }
                if (reader.ReadByte() != 4)
                {
                    return null;
                }
                int count = reader.ReadByte();
                byte[] data = reader.ReadBytes(count);
                if (verbose)
                {
                    this.showBytes("IV for des-EDE3-CBC", data);
                }
                if (reader.ReadByte() != 4)
                {
                    return null;
                }
                num6 = reader.ReadByte();
                if (num6 == 0x81)
                {
                    num3 = reader.ReadByte();
                }
                else if (num6 == 130)
                {
                    num3 = (0x100 * reader.ReadByte()) + reader.ReadByte();
                }
                else
                {
                    num3 = num6;
                }
                byte[] edata = reader.ReadBytes(num3);
                byte[] buffer9 = this.DecryptPBDK2(edata, buffer6, data, pass, num4);
                if (buffer9 == null)
                {
                    this.Mensaje = "Contrase\x00f1a de llave incorrecta.";
                    return null;
                }
                provider2 = this.DecodePrivateKeyInfo(buffer9);
            }
            catch (Exception)
            {
                provider2 = null;
            }
            finally
            {
                reader.Close();
            }
            return provider2;
        }

        private byte[] DecodeOpenSSLPrivateKey(string instr, SecureString pass)
        {
            string str = instr.Trim();
            if (str.StartsWith("-----BEGIN RSA PRIVATE KEY-----") && str.EndsWith("-----END RSA PRIVATE KEY-----"))
            {
                byte[] buffer;
                StringBuilder builder = new StringBuilder(str);
                builder.Replace("-----BEGIN RSA PRIVATE KEY-----", "");
                builder.Replace("-----END RSA PRIVATE KEY-----", "");
                string s = builder.ToString().Trim();
                try
                {
                    return Convert.FromBase64String(s);
                }
                catch (FormatException)
                {
                }
                StringReader reader = new StringReader(s);
                if (!reader.ReadLine().StartsWith("Proc-Type: 4,ENCRYPTED"))
                {
                    return null;
                }
                string str3 = reader.ReadLine();
                if (!str3.StartsWith("DEK-Info: DES-EDE3-CBC,"))
                {
                    return null;
                }
                string str4 = str3.Substring(str3.IndexOf(",") + 1).Trim();
                byte[] salt = new byte[str4.Length / 2];
                for (int i = 0; i < salt.Length; i++)
                {
                    salt[i] = Convert.ToByte(str4.Substring(i * 2, 2), 0x10);
                }
                if (!(reader.ReadLine() == ""))
                {
                    return null;
                }
                string str5 = reader.ReadToEnd();
                try
                {
                    buffer = Convert.FromBase64String(str5);
                }
                catch (FormatException)
                {
                    return null;
                }
                byte[] desKey = this.GetOpenSSL3deskey(salt, pass, 1, 2);
                if (desKey == null)
                {
                    return null;
                }
                byte[] buffer4 = this.DecryptKey(buffer, desKey, salt);
                if (buffer4 != null)
                {
                    return buffer4;
                }
            }
            return null;
        }

        private byte[] DecodeOpenSSLPublicKey(string instr)
        {
            byte[] buffer;
            string str = instr.Trim();
            if (!(str.StartsWith("-----BEGIN PUBLIC KEY-----") && str.EndsWith("-----END PUBLIC KEY-----")))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(str);
            builder.Replace("-----BEGIN PUBLIC KEY-----", "");
            builder.Replace("-----END PUBLIC KEY-----", "");
            string s = builder.ToString().Trim();
            try
            {
                buffer = Convert.FromBase64String(s);
            }
            catch (FormatException)
            {
                return null;
            }
            return buffer;
        }

        private void DecodePEMKey(string pemstr, string password)
        {
            SecureString pass = this.ToSecureString(password);
            if (pemstr.StartsWith("-----BEGIN PUBLIC KEY-----") && pemstr.EndsWith("-----END PUBLIC KEY-----"))
            {
                byte[] data = this.DecodeOpenSSLPublicKey(pemstr);
                if (data != null)
                {
                    if (verbose)
                    {
                        this.showBytes("\nRSA public key", data);
                    }
                    string str2 = this.DecodeX509PublicKey(data).ToXmlString(false);
                }
            }
            else
            {
                string str3;
                if (pemstr.StartsWith("-----BEGIN RSA PRIVATE KEY-----") && pemstr.EndsWith("-----END RSA PRIVATE KEY-----"))
                {
                    byte[] buffer2 = this.DecodeOpenSSLPrivateKey(pemstr, pass);
                    if (buffer2 != null)
                    {
                        if (verbose)
                        {
                            this.showBytes("\nRSA private key", buffer2);
                        }
                        str3 = this.DecodeRSAPrivateKey(buffer2).ToXmlString(true);
                    }
                }
                else
                {
                    RSACryptoServiceProvider provider;
                    if (pemstr.StartsWith("-----BEGIN PRIVATE KEY-----") && pemstr.EndsWith("-----END PRIVATE KEY-----"))
                    {
                        byte[] buffer3 = this.DecodePkcs8PrivateKey(pemstr);
                        if (buffer3 != null)
                        {
                            if (verbose)
                            {
                                this.showBytes("\nPKCS #8 PrivateKeyInfo", buffer3);
                            }
                            provider = this.DecodePrivateKeyInfo(buffer3);
                            if (provider != null)
                            {
                                str3 = provider.ToXmlString(true);
                            }
                        }
                    }
                    else if (pemstr.StartsWith("-----BEGIN ENCRYPTED PRIVATE KEY-----") && pemstr.EndsWith("-----END ENCRYPTED PRIVATE KEY-----"))
                    {
                        byte[] buffer4 = this.DecodePkcs8EncPrivateKey(pemstr);
                        if (buffer4 != null)
                        {
                            if (verbose)
                            {
                                this.showBytes("\nPKCS #8 EncryptedPrivateKeyInfo", buffer4);
                            }
                            provider = this.DecodeEncryptedPrivateKeyInfo(buffer4, pass);
                            if (provider != null)
                            {
                                str3 = provider.ToXmlString(true);
                            }
                        }
                    }
                }
            }
        }

        private byte[] DecodePkcs8EncPrivateKey(string instr)
        {
            byte[] buffer;
            string str = instr.Trim();
            if (!(str.StartsWith("-----BEGIN ENCRYPTED PRIVATE KEY-----") && str.EndsWith("-----END ENCRYPTED PRIVATE KEY-----")))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(str);
            builder.Replace("-----BEGIN ENCRYPTED PRIVATE KEY-----", "");
            builder.Replace("-----END ENCRYPTED PRIVATE KEY-----", "");
            string s = builder.ToString().Trim();
            try
            {
                buffer = Convert.FromBase64String(s);
            }
            catch (FormatException)
            {
                return null;
            }
            return buffer;
        }

        private byte[] DecodePkcs8PrivateKey(string instr)
        {
            byte[] buffer;
            string str = instr.Trim();
            if (!(str.StartsWith("-----BEGIN PRIVATE KEY-----") && str.EndsWith("-----END PRIVATE KEY-----")))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(str);
            builder.Replace("-----BEGIN PRIVATE KEY-----", "");
            builder.Replace("-----END PRIVATE KEY-----", "");
            string s = builder.ToString().Trim();
            try
            {
                buffer = Convert.FromBase64String(s);
            }
            catch (FormatException)
            {
                return null;
            }
            return buffer;
        }

        private RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            RSACryptoServiceProvider provider2;
            byte[] b = new byte[] { 0x30, 13, 6, 9, 0x2a, 0x86, 0x48, 0x86, 0xf7, 13, 1, 1, 1, 5, 0 };
            byte[] a = new byte[15];
            MemoryStream input = new MemoryStream(pkcs8);
            int length = (int) input.Length;
            BinaryReader reader = new BinaryReader(input);
            ushort num3 = 0;
            try
            {
                num3 = reader.ReadUInt16();
                if (num3 == 0x8130)
                {
                    reader.ReadByte();
                }
                else if (num3 == 0x8230)
                {
                    reader.ReadInt16();
                }
                else
                {
                    return null;
                }
                if (reader.ReadByte() != 2)
                {
                    return null;
                }
                if (reader.ReadUInt16() != 1)
                {
                    return null;
                }
                a = reader.ReadBytes(15);
                if (!this.CompareBytearrays(a, b))
                {
                    return null;
                }
                if (reader.ReadByte() != 4)
                {
                    return null;
                }
                switch (reader.ReadByte())
                {
                    case 0x81:
                        reader.ReadByte();
                        break;

                    case 130:
                        reader.ReadUInt16();
                        break;
                }
                byte[] privkey = reader.ReadBytes(length - ((int) input.Position));
                provider2 = this.DecodeRSAPrivateKey(privkey);
            }
            catch (Exception)
            {
                provider2 = null;
            }
            finally
            {
                reader.Close();
            }
            return provider2;
        }

        private RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            RSACryptoServiceProvider provider2;
            MemoryStream input = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(input);
            ushort num2 = 0;
            int count = 0;
            try
            {
                num2 = binr.ReadUInt16();
                if (num2 == 0x8130)
                {
                    binr.ReadByte();
                }
                else if (num2 == 0x8230)
                {
                    binr.ReadInt16();
                }
                else
                {
                    return null;
                }
                if (binr.ReadUInt16() != 0x102)
                {
                    return null;
                }
                if (binr.ReadByte() != 0)
                {
                    return null;
                }
                count = this.GetIntegerSize(binr);
                byte[] data = binr.ReadBytes(count);
                count = this.GetIntegerSize(binr);
                byte[] buffer2 = binr.ReadBytes(count);
                count = this.GetIntegerSize(binr);
                byte[] buffer3 = binr.ReadBytes(count);
                count = this.GetIntegerSize(binr);
                byte[] buffer4 = binr.ReadBytes(count);
                count = this.GetIntegerSize(binr);
                byte[] buffer5 = binr.ReadBytes(count);
                count = this.GetIntegerSize(binr);
                byte[] buffer6 = binr.ReadBytes(count);
                count = this.GetIntegerSize(binr);
                byte[] buffer7 = binr.ReadBytes(count);
                count = this.GetIntegerSize(binr);
                byte[] buffer8 = binr.ReadBytes(count);
                if (verbose)
                {
                    this.showBytes("\nModulus", data);
                    this.showBytes("\nExponent", buffer2);
                    this.showBytes("\nD", buffer3);
                    this.showBytes("\nP", buffer4);
                    this.showBytes("\nQ", buffer5);
                    this.showBytes("\nDP", buffer6);
                    this.showBytes("\nDQ", buffer7);
                    this.showBytes("\nIQ", buffer8);
                }
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                RSAParameters parameters = new RSAParameters();
                this.Modulo = Encoding.UTF8.GetString(data);
                parameters.Modulus = data;
                parameters.Exponent = buffer2;
                parameters.D = buffer3;
                parameters.P = buffer4;
                parameters.Q = buffer5;
                parameters.DP = buffer6;
                parameters.DQ = buffer7;
                parameters.InverseQ = buffer8;
                provider.ImportParameters(parameters);
                provider2 = provider;
            }
            catch (Exception)
            {
                provider2 = null;
            }
            finally
            {
                binr.Close();
            }
            return provider2;
        }

        private RSACryptoServiceProvider DecodeX509PublicKey(byte[] x509key)
        {
            RSACryptoServiceProvider provider2;
            byte[] b = new byte[] { 0x30, 13, 6, 9, 0x2a, 0x86, 0x48, 0x86, 0xf7, 13, 1, 1, 1, 5, 0 };
            byte[] a = new byte[15];
            MemoryStream input = new MemoryStream(x509key);
            BinaryReader reader = new BinaryReader(input);
            ushort num2 = 0;
            try
            {
                num2 = reader.ReadUInt16();
                if (num2 == 0x8130)
                {
                    reader.ReadByte();
                }
                else if (num2 == 0x8230)
                {
                    reader.ReadInt16();
                }
                else
                {
                    return null;
                }
                a = reader.ReadBytes(15);
                if (this.CompareBytearrays(a, b))
                {
                    switch (reader.ReadUInt16())
                    {
                        case 0x8103:
                            reader.ReadByte();
                            goto Label_00DC;

                        case 0x8203:
                            reader.ReadInt16();
                            goto Label_00DC;
                    }
                }
                return null;
            Label_00DC:
                if (reader.ReadByte() == 0)
                {
                    switch (reader.ReadUInt16())
                    {
                        case 0x8130:
                            reader.ReadByte();
                            goto Label_013D;

                        case 0x8230:
                            reader.ReadInt16();
                            goto Label_013D;
                    }
                }
                return null;
            Label_013D:
                num2 = reader.ReadUInt16();
                byte num3 = 0;
                byte num4 = 0;
                if (num2 == 0x8102)
                {
                    num3 = reader.ReadByte();
                }
                else if (num2 == 0x8202)
                {
                    num4 = reader.ReadByte();
                    num3 = reader.ReadByte();
                }
                else
                {
                    return null;
                }
                byte[] buffer6 = new byte[4];
                buffer6[0] = num3;
                buffer6[1] = num4;
                byte[] buffer3 = buffer6;
                int count = BitConverter.ToInt32(buffer3, 0);
                byte num6 = reader.ReadByte();
                reader.BaseStream.Seek(-1L, SeekOrigin.Current);
                if (num6 == 0)
                {
                    reader.ReadByte();
                    count--;
                }
                byte[] data = reader.ReadBytes(count);
                if (reader.ReadByte() != 2)
                {
                    return null;
                }
                int num7 = reader.ReadByte();
                byte[] buffer5 = reader.ReadBytes(num7);
                this.showBytes("\nExponent", buffer5);
                this.showBytes("\nModulus", data);
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                RSAParameters parameters = new RSAParameters {
                    Modulus = data,
                    Exponent = buffer5
                };
                provider.ImportParameters(parameters);
                provider2 = provider;
            }
            catch (Exception)
            {
                provider2 = null;
            }
            finally
            {
                reader.Close();
            }
            return provider2;
        }

        private byte[] DecryptKey(byte[] cipherData, byte[] desKey, byte[] IV)
        {
            MemoryStream stream = new MemoryStream();
            TripleDES edes = TripleDES.Create();
            edes.Key = desKey;
            edes.IV = IV;
            try
            {
                CryptoStream stream2 = new CryptoStream(stream, edes.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(cipherData, 0, cipherData.Length);
                stream2.Close();
            }
            catch (Exception exception)
            {
                this.Mensaje = "Error al desencriptar la llave. " + exception.Message;
                return null;
            }
            return stream.ToArray();
        }

        private byte[] DecryptPBDK2(byte[] edata, byte[] salt, byte[] IV, SecureString secpswd, int iterations)
        {
            CryptoStream stream = null;
            IntPtr zero = IntPtr.Zero;
            byte[] destination = new byte[secpswd.Length];
            zero = Marshal.SecureStringToGlobalAllocAnsi(secpswd);
            Marshal.Copy(zero, destination, 0, destination.Length);
            Marshal.ZeroFreeGlobalAllocAnsi(zero);
            try
            {
                Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(destination, salt, iterations);
                TripleDES edes = TripleDES.Create();
                edes.Key = bytes.GetBytes(0x18);
                edes.IV = IV;
                MemoryStream stream2 = new MemoryStream();
                stream = new CryptoStream(stream2, edes.CreateDecryptor(), CryptoStreamMode.Write);
                stream.Write(edata, 0, edata.Length);
                stream.Flush();
                stream.Close();
                return stream2.ToArray();
            }
            catch (Exception exception)
            {
                this.Mensaje = "Error al desencriptar la llave. " + exception.Message;
                return null;
            }
        }

        public string ExportaraXML()
        {
            if (this.llaveRSA == null)
            {
                this.Mensaje = "No ha cargado una llave.";
                return null;
            }
            return this.llaveRSA.ToXmlString(true);
        }

        private int GetIntegerSize(BinaryReader binr)
        {
            byte num = 0;
            byte num2 = 0;
            byte num3 = 0;
            int num4 = 0;
            if (binr.ReadByte() != 2)
            {
                return 0;
            }
            num = binr.ReadByte();
            if (num == 0x81)
            {
                num4 = binr.ReadByte();
            }
            else if (num == 130)
            {
                num3 = binr.ReadByte();
                num2 = binr.ReadByte();
                byte[] buffer2 = new byte[4];
                buffer2[0] = num2;
                buffer2[1] = num3;
                byte[] buffer = buffer2;
                num4 = BitConverter.ToInt32(buffer, 0);
            }
            else
            {
                num4 = num;
            }
            while (binr.ReadByte() == 0)
            {
                num4--;
            }
            binr.BaseStream.Seek(-1L, SeekOrigin.Current);
            return num4;
        }

        private byte[] GetOpenSSL3deskey(byte[] salt, SecureString secpswd, int count, int miter)
        {
            IntPtr zero = IntPtr.Zero;
            int num = 0x10;
            byte[] destinationArray = new byte[num * miter];
            byte[] destination = new byte[secpswd.Length];
            zero = Marshal.SecureStringToGlobalAllocAnsi(secpswd);
            Marshal.Copy(zero, destination, 0, destination.Length);
            Marshal.ZeroFreeGlobalAllocAnsi(zero);
            byte[] buffer3 = new byte[destination.Length + salt.Length];
            Array.Copy(destination, buffer3, destination.Length);
            Array.Copy(salt, 0, buffer3, destination.Length, salt.Length);
            MD5 md = new MD5CryptoServiceProvider();
            byte[] sourceArray = null;
            byte[] buffer5 = new byte[num + buffer3.Length];
            for (int i = 0; i < miter; i++)
            {
                if (i == 0)
                {
                    sourceArray = buffer3;
                }
                else
                {
                    Array.Copy(sourceArray, buffer5, sourceArray.Length);
                    Array.Copy(buffer3, 0, buffer5, sourceArray.Length, buffer3.Length);
                    sourceArray = buffer5;
                }
                for (int j = 0; j < count; j++)
                {
                    sourceArray = md.ComputeHash(sourceArray);
                }
                Array.Copy(sourceArray, 0, destinationArray, i * num, sourceArray.Length);
            }
            byte[] buffer6 = new byte[0x18];
            Array.Copy(destinationArray, buffer6, buffer6.Length);
            Array.Clear(destination, 0, destination.Length);
            Array.Clear(buffer3, 0, buffer3.Length);
            Array.Clear(sourceArray, 0, sourceArray.Length);
            Array.Clear(buffer5, 0, buffer5.Length);
            Array.Clear(destinationArray, 0, destinationArray.Length);
            return buffer6;
        }

        private void inicializar()
        {
            this.Mensaje = "";
            this.TipoCertificado = "";
            this.Modulo = "";
            this.Contraseña = "";
            this.llaveRSA = null;
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

        private void showRSAProps(RSACryptoServiceProvider rsa)
        {
            CspKeyContainerInfo cspKeyContainerInfo = rsa.CspKeyContainerInfo;
        }

        private SecureString ToSecureString(string Source)
        {
            if (string.IsNullOrEmpty(Source))
            {
                return null;
            }
            SecureString str = new SecureString();
            foreach (char ch in Source.ToCharArray())
            {
                str.AppendChar(ch);
            }
            return str;
        }

        public RSACryptoServiceProvider LlavePrivadaRSA
        {
            get
            {
                return this.llaveRSA;
            }
        }

        public string Modulus
        {
            get
            {
                return this.Modulo;
            }
        }
    }
}
