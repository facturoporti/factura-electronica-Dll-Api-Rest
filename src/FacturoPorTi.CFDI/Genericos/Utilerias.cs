using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace FacturoPorTi.CFDI.Genericos
{
    public class Utilerias
    {
        public string Mensaje { get; set; }
        public Utilerias()
        { 
        }

        public bool ValidaCorreos(string cadena)
        {
            bool validos = true;
            int indice = -1;
            string caracterSeparador = ";";

            indice = cadena.IndexOf(caracterSeparador);
            if (indice > 0)
            {
                var lista = cadena.Trim().Split(new String[] { caracterSeparador }, StringSplitOptions.None).ToList();

                for (int contador = 0; contador < lista.Count; contador++)
                {
                    if (!string.IsNullOrEmpty(lista[contador]))
                    {
                        validos = ValidaEmail(lista[contador].Trim());
                        if (validos == false)
                        {
                            Mensaje = "El correo " + lista[contador].Trim() + " no es válido, corrija y vuelva a intentarlo.";
                            break;
                        }
                    }
                    else
                    {
                        Mensaje = "Ingrese el correo correctamente.";
                        break;
                    }
                }

                return validos;
            }

            caracterSeparador = ",";
            indice = cadena.IndexOf(caracterSeparador);
            if (indice > 0)
            {
                var lista = cadena.Trim().Split(new String[] { caracterSeparador }, StringSplitOptions.None).ToList();

                for (int contador = 0; contador < lista.Count; contador++)
                {
                    if (!string.IsNullOrEmpty(lista[contador]))
                    {
                        validos = ValidaEmail(lista[contador].Trim());
                        if (validos == false)
                        {
                            Mensaje = "El correo " + lista[contador].Trim() + " no es válido, corrija y vuelva a intentarlo.";
                            break;
                        }
                    }
                    else
                    {
                        Mensaje = "Ingrese el correo correctamente.";
                        break;
                    }
                }

                return validos;
            }

            validos = ValidaEmail(cadena.Trim());
            if (validos == false)
            {
                Mensaje = "El correo " + cadena.Trim() + " no es válido, corrija y vuelva a intentarlo.";
            }

            return validos;
        }

        public decimal RegresaDecimalesXOpcion(decimal valor, short OpcionDecimales, short NumeroDecimales)
        {          
            decimal tmpValor;

            if (OpcionDecimales == 1) // Truncar los resultados 
            {
                var formula = Math.Pow(10, NumeroDecimales);
                decimal tmpValDecimal = valor * 100;
                tmpValor = Convert.ToDecimal(Math.Truncate(Convert.ToDouble(tmpValDecimal)) / formula);
            }
            else // Redondear al numero de decimales hacia arriba 
            {
                tmpValor = Math.Round(valor, NumeroDecimales, MidpointRounding.AwayFromZero);
            }

            tmpValor = Convert.ToDecimal(string.Format("{0:N" + NumeroDecimales + "}", tmpValor));

            return tmpValor;
        }
        public string RegresaStringDecimalesXOpcion(decimal valor, short OpcionDecimales, short NumeroDecimales)
        {
            return RegresaDecimalesXOpcion(valor, OpcionDecimales, NumeroDecimales).ToString();
        }

        public bool ValidaRFC(string valor)
        {
            bool resultado = false;
            string expresion = String.Empty;

            expresion = @"[A-Za-z,Ñ,ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Za-z,0-9]?[A-Za-z,0-9]?[0-9,A-Za-z]?";

            Regex valorRegex = new Regex(expresion, RegexOptions.Compiled);

            if (string.IsNullOrEmpty(valor))
                valor = "";

            resultado = valorRegex.IsMatch(valor);

            if (resultado == true)
                resultado = valor.Trim().Length < 12 ? false : true;

            return resultado;
        }

        public bool ValidaEmail(string valor)
        {
            bool resultado = false;
            string expresion = String.Empty;
            
            expresion = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            Regex valorRegex = new Regex(expresion, RegexOptions.Compiled);

            if (string.IsNullOrEmpty(valor))
                valor = "";

            resultado = valorRegex.IsMatch(valor);

            return resultado;
        }
        public bool ValidaCURP(string valor)
        {
            bool resultado = false;

            string expresion = String.Empty;

            expresion = @"[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]"; // Version del SAT

            Regex valorRegex = new Regex(expresion, RegexOptions.Compiled);

            if (string.IsNullOrEmpty(valor))
                valor = "";

            resultado = valorRegex.IsMatch(valor);
            return resultado;
        }

        public bool ValidaRegistroPatronal(string valor)
        {
            bool resultado = false;
            string expresion = String.Empty;

            expresion = @"([A-Z]|[a-z]|[0-9]|Ñ|ñ|!|&quot;|%|&|'|´|-|:|;|>|=|<|@|_|,|\{|\}|`|~|á|é|í|ó|ú|Á|É|Í|Ó|Ú|ü|Ü){1,20}";

            Regex valorRegex = new Regex(expresion, RegexOptions.Compiled);

            if (string.IsNullOrEmpty(valor))
                valor = "";

            resultado = valorRegex.IsMatch(valor);

            return resultado;
        }

        public bool ValidaNumeroSeguroSocial(string valor)
        {
            bool resultado = false;
            string expresion = String.Empty;

            expresion = @"[0-9]{1,15}";

            Regex valorRegex = new Regex(expresion, RegexOptions.Compiled);

            if (string.IsNullOrEmpty(valor))
                valor = "";

            resultado = valorRegex.IsMatch(valor);

            return resultado;
        }

        public T CargaObjetoDeXML<T>(T objeto, string XMLCFDI)
        {
            string mensaje = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.InnerXml = XMLCFDI;

                XmlParserContext contexto = new XmlParserContext(null, null, "", XmlSpace.None);
                XmlTextReader leeXML = new XmlTextReader(xmlDoc.InnerXml, XmlNodeType.Document, contexto);
                XmlSerializer XmlDeserializa = new XmlSerializer(objeto.GetType());

                objeto = (T)XmlDeserializa.Deserialize(leeXML);
                leeXML.Close();
            }
            catch (Exception ex)
            {
                //Logger.Error("Error al serializar el archivo " + Environment.NewLine + XMLCFDI);
                Mensaje = "Error al Cargar el XML";
            }

            return objeto;
        }
    }
}
