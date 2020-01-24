
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FacturoPorTi.CFDI;
using FacturoPorTi.CFDI.Entidades;
using FacturoPorTi.CFDI.Genericos;

namespace FacturoPorTi.Tester
{
    class Program
    {
        private static string UUID { get; set; }
        static void Main(string[] args)
        {
            TimbrarDocumento();
            CancelarUUID();
            ConsultarEstatusUUID();
            ConsultarTimbresRestantes();
        }

        private static void TimbrarDocumento()
        {
            Archivos manager = new Archivos();

            TimbreFiscalDigital Timbre = new TimbreFiscalDigital();
            CFDIPeticion Peticion = new CFDIPeticion();
            Utilerias utilerias = new Utilerias();
            string NombreArchivo = string.Empty;

            #region "Datos Generales"

            Peticion.DatosGenerales = new DatosGeneralesCFDI();
            Peticion.DatosGenerales.Usuario = "PruebasTimbrado"; // Este usuario se genera desde la pagina de https://cfdi.facturoporti.com.mx/ se debe de registrar para usar el productivo
            Peticion.DatosGenerales.Password = "@Notiene1"; // Es la contraseña del usuario cuando se registró
            Peticion.DatosGenerales.GeneraPDF = "true";
            Peticion.DatosGenerales.EnviaEmail = "false"; // Valores permitidos "true" : "false";
            Peticion.DatosGenerales.ReceptorEmail = "correodestino@midominio.com";

            //Logotipo (opcional) acepta una imagen jpeg o png en base 64 menor a 100 KB
            //Peticion.DatosGenerales.Logotipo = manager.ConvertirByteToBase64(manager.ConvertirStreamToByte(manager.Abrir("Cambiar la ruta de lectura o enviar la imagen en base 64")));

            enumTipoDocumento TipoDocumentoActual = enumTipoDocumento.Factura;

            switch (TipoDocumentoActual)
            {
                case enumTipoDocumento.Factura:
                    Peticion.DatosGenerales.CFDI = "Factura";
                    Peticion.DatosGenerales.TipoCFDI = "Ingreso";
                    break;
                case enumTipoDocumento.NotaCargo:
                    Peticion.DatosGenerales.CFDI = "NotaCargo";
                    Peticion.DatosGenerales.TipoCFDI = "Ingreso";
                    break;
                case enumTipoDocumento.NotaCredito:
                    Peticion.DatosGenerales.CFDI = "NotaCredito";
                    Peticion.DatosGenerales.TipoCFDI = "Egreso";
                    break;
                case enumTipoDocumento.CartaPorte:
                    Peticion.DatosGenerales.CFDI = "CartaPorte";
                    Peticion.DatosGenerales.TipoCFDI = "Traslado";
                    break;
                case enumTipoDocumento.Pago:
                    Peticion.DatosGenerales.CFDI = "Pago";
                    Peticion.DatosGenerales.TipoCFDI = "Pago";
                    break;
                case enumTipoDocumento.ReciboNominaCFDI:
                    Peticion.DatosGenerales.CFDI = "ReciboNomina";
                    Peticion.DatosGenerales.TipoCFDI = "ReciboNomina";
                    break;
            }

            Peticion.DatosGenerales.OpcionDecimales = ((int)enumOpcionDecimales.Redondear).ToString(); // Valores permitidos 1: Truncar (Operaciones exactas) 2: Redondear hacia arriba o hacia abajo las cantidades 
            Peticion.DatosGenerales.NumeroDecimales = "2"; // El valor predeterminado es 2 hasta un máximo de 6 decimales

            #endregion "Datos Generales"

            #region "Encabezado"

            #region "Emisor"

            Peticion.Encabezado = new EncabezadoCFDI();
            Peticion.Encabezado.Emisor = new EmisorCFDI();
            Peticion.Encabezado.Emisor.RFC = "AAA010101AAA"; // Para realizar pruebas solamente se puede usar este RFC AAA010101AAA
            Peticion.Encabezado.Emisor.NombreRazonSocial = "Mi nombre o el nombre de mi empresa";
            Peticion.Encabezado.Emisor.RegimenFiscal = "601"; // Se agrega la clave de acuerdo al catálogo del SAT http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls

            // El domicilio de emision es opcional pero se agrega por peticion del usuario
            DireccionCFDI direccion = new DireccionCFDI();

            direccion.Calle = "Avenida Reforma";
            direccion.NumeroExterior = "1234";
            direccion.NumeroInterior = "XA";
            direccion.Colonia = "Roma Norte";
            direccion.Estado = "Ciudad de México";
            direccion.Municipio = "Benito Juarez";
            direccion.Pais = "México";
            direccion.CodigoPostal = "06470";

            Peticion.Encabezado.Emisor.Direccion = new List<DireccionCFDI>();
            Peticion.Encabezado.Emisor.Direccion.Add(direccion);

            #endregion "Emisor"

            #region "Receptor"

            Peticion.Encabezado.Receptor = new ReceptorCFDI();
            Peticion.Encabezado.Receptor.RFC = "XEXX010101000";
            Peticion.Encabezado.Receptor.NombreRazonSocial = "Nombre del cliente";
            Peticion.Encabezado.Receptor.UsoCFDI = "G03"; // Se agrega la clave de acuerdo al catálogo del SAT http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls

            // El domicilio del receptor es opcional pero se agrega por peticion del usuario
            direccion = new DireccionCFDI();

            direccion.Calle = "Leo";
            direccion.NumeroExterior = "9876";
            direccion.NumeroInterior = "A-34";
            direccion.Colonia = "San Rafael";
            direccion.Estado = "Morelos";
            direccion.Municipio = "Cuernavaca";
            direccion.Pais = "México";
            direccion.CodigoPostal = "62775";

            Peticion.Encabezado.Receptor.Direccion = direccion;

            #endregion "Receptor"

            Peticion.Encabezado.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"); // Se debe de enviar con el formato indicado yyyy-MM-ddTHH:mm:ss
            Peticion.Encabezado.Serie = "A"; // Es el numero de serie es un valor opcional
            Peticion.Encabezado.Folio = "12"; // Es el numero de folio es un valor opcional
            Peticion.Encabezado.MetodoPago = "PUE";// Se agrega la clave de acuerdo al catálogo del SAT http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            Peticion.Encabezado.FormaPago = "99";// Se agrega la clave de acuerdo al catálogo del SAT http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            Peticion.Encabezado.Moneda = "MXN"; // Se agrega la clave de acuerdo al catálogo del SAT http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            Peticion.Encabezado.LugarExpedicion = "06470";
            Peticion.Encabezado.SubTotal = "1500"; // Es la suma de los importes en bruto
            Peticion.Encabezado.Total = "1740"; // Es la suma de los importes + los impuestos trasladados - los impuestos retenidos

            #endregion "Encabezado"

            #region "Conceptos"

            Peticion.Conceptos = new List<ConceptoCFDI>();

            ConceptoCFDI concepto = new ConceptoCFDI();

            concepto.Cantidad = "1";
            concepto.CodigoUnidad = "E48"; // Se agrega la clave de acuerdo al catálogo del SAT http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
                                           //concepto.Unidad = "Pieza"; // Este es un valor opcional 
                                           //concepto.Serie = ""; // Este es un valor opcional se agregan numero de series, partes, etc.
            concepto.CodigoProducto = "53112101";// Se agrega la clave de acuerdo al catálogo del SAT http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            concepto.Producto = "Zapatos de caballero marca patito";
            concepto.PrecioUnitario = "1000";
            concepto.Importe = "1000";

            concepto.Impuestos = new List<ImpuestosCFDI>();
            ImpuestosCFDI impuesto = new ImpuestosCFDI();

            impuesto.TipoImpuesto = ((int)enumTipoImpuesto.Trasladado).ToString(); // Tipo de impuesto se envía la clave 1 traslado 2 retenido
            impuesto.Impuesto = ((int)enumImpuesto.IVA).ToString();
            impuesto.Factor = ((int)enumFactor.Tasa).ToString();
            impuesto.Base = concepto.Importe;
            impuesto.Tasa = "0.160000"; // Se debe de enviar con los 6 decimales la tasa para revisar las tasas actuales vea http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            impuesto.ImpuestoImporte = utilerias.RegresaStringDecimalesXOpcion(Convert.ToDecimal(concepto.Importe) * Convert.ToDecimal("0.160000"), (int)enumOpcionDecimales.Redondear, 2);
            concepto.Impuestos.Add(impuesto);

            // En caso de llevar IEPS se llena esta seccion
            //if (TasaIEPS != null)
            //{
            //    impuesto.TipoImpuesto = ((int)enumTipoImpuesto.Trasladado).ToString(); // Tipo de impuesto se envía la clave 1 traslado 2 retenido
            //    impuesto.Impuesto = ((int)enumImpuesto.IEPS).ToString();
            //    impuesto.Factor = ((int)enumFactor.Tasa).ToString();
            //    impuesto.Base = concepto.Importe;
            //    impuesto.Tasa = "0.08000"; // en el ejemplo la tasa es de 8 porciento Se debe de enviar con los 6 decimales la tasa para revisar las tasas actuales vea http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            //    impuesto.ImpuestoImporte = utilerias.RegresaStringDecimalesXOpcion(Convert.ToDecimal(concepto.Importe) * Convert.ToDecimal("0.080000"), (int)enumOpcionDecimales.Redondear, 2);
            //    concepto.Impuestos.Add(impuesto);
            //}

            //if (RetencionIVA != null)
            //{
            //    impuesto.TipoImpuesto = ((int)enumTipoImpuesto.Retenido).ToString(); // Tipo de impuesto se envía la clave 1 traslado 2 retenido
            //    impuesto.Impuesto = ((int)enumImpuesto.IVA).ToString();
            //    impuesto.Factor = ((int)enumFactor.Tasa).ToString();
            //    impuesto.Base = concepto.Importe;
            //    impuesto.Tasa = "0.106667"; // en el ejemplo la tasa es de 2/3 partes de IVA 10.66667 porciento Se debe de enviar con los 6 decimales la tasa para revisar las tasas actuales vea http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            //    impuesto.ImpuestoImporte = utilerias.RegresaStringDecimalesXOpcion(Convert.ToDecimal(concepto.Importe) * Convert.ToDecimal("0.106667"), (int)enumOpcionDecimales.Redondear, 2);
            //    concepto.Impuestos.Add(impuesto);
            //}

            //if (RetencionISR != null)
            //{
            //    impuesto.TipoImpuesto = ((int)enumTipoImpuesto.Retenido).ToString(); // Tipo de impuesto se envía la clave 1 traslado 2 retenido
            //    impuesto.Impuesto = ((int)enumImpuesto.ISR).ToString();
            //    impuesto.Factor = ((int)enumFactor.Tasa).ToString();
            //    impuesto.Base = concepto.Importe;
            //    impuesto.Tasa = "0.10000"; // en el ejemplo la tasa es de 10 porciento Se debe de enviar con los 6 decimales la tasa para revisar las tasas actuales vea http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            //    impuesto.ImpuestoImporte = utilerias.RegresaStringDecimalesXOpcion(Convert.ToDecimal(concepto.Importe) * Convert.ToDecimal("0.100000"), (int)enumOpcionDecimales.Redondear, 2);
            //    concepto.Impuestos.Add(impuesto);
            //}

            Peticion.Conceptos.Add(concepto);

            concepto = new ConceptoCFDI();
            concepto.Cantidad = "2";
            concepto.CodigoUnidad = "E48"; // Se agrega la clave de acuerdo al catálogo del SAT http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
                                           //concepto.Unidad = "Pieza"; // Este es un valor opcional 
                                           //concepto.Serie = ""; // Este es un valor opcional se agregan numero de series, partes, etc.
            concepto.CodigoProducto = "53112102";// Se agrega la clave de acuerdo al catálogo del SAT http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            concepto.Producto = "Zapatos de mujer  marca patito";
            concepto.PrecioUnitario = "250";
            concepto.Importe = "500";

            concepto.Impuestos = new List<ImpuestosCFDI>();
            impuesto = new ImpuestosCFDI();

            impuesto.TipoImpuesto = ((int)enumTipoImpuesto.Trasladado).ToString(); // Tipo de impuesto se envía la clave 1 traslado 2 retenido
            impuesto.Impuesto = ((int)enumImpuesto.IVA).ToString();
            impuesto.Factor = ((int)enumFactor.Tasa).ToString();
            impuesto.Base = concepto.Importe;
            impuesto.Tasa = "0.160000"; // Se debe de enviar con los 6 decimales la tasa para revisar las tasas actuales vea http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/catCFDI.xls
            impuesto.ImpuestoImporte = utilerias.RegresaStringDecimalesXOpcion(Convert.ToDecimal(concepto.Importe) * Convert.ToDecimal("0.160000"), (int)enumOpcionDecimales.Redondear, 2);
            concepto.Impuestos.Add(impuesto);

            Peticion.Conceptos.Add(concepto);

            #endregion "Conceptos"

            #region "Realiza el Timbrado"

            FacturoPorTi.CFDI.Api.ComprobanteDigital comprobante = new CFDI.Api.ComprobanteDigital();
            comprobante.SandBox = true; // True = pruebas,  False= Productivo

            //Para el ejercicio se usan los certificados de prueba del SAT
            // Tambien se puede enviar un stream o arreglo de bytes
            string RutaCertificado = ObtieneDirectorioAplicacion() + @"\Certificado\AAA010101AAA.cer";
            string RutaLlavePrivada = ObtieneDirectorioAplicacion() + @"\Certificado\AAA010101AAA.key";
            string RutaTimbrados = ObtieneDirectorioAplicacion() + @"\Timbrados\";

            Console.WriteLine("Inicio de Timbrado " + DateTime.Now);
            
            var resultado = comprobante.GeneraCFDI(Peticion, RutaCertificado, RutaLlavePrivada, "12345678a");

            if (resultado == true)
            {
                NombreArchivo = Peticion.Encabezado.Receptor.RFC + AcronimoArchivo(TipoDocumentoActual) + (Peticion.Encabezado.Serie == null ? "" : Peticion.Encabezado.Serie) + Peticion.Encabezado.Folio + "_" + Convert.ToDateTime(Peticion.Encabezado.Fecha).ToString("yyyyMMdd");                
                manager.Guardar(manager.ConvertirBase64ToByte(comprobante.Timbrado.CFDITimbrado.Respuesta.CFDIXML), RutaTimbrados + NombreArchivo + ".xml");
                manager.Guardar(manager.ConvertirBase64ToByte(comprobante.Timbrado.CFDITimbrado.Respuesta.PDF), RutaTimbrados + NombreArchivo + ".pdf");

                Timbre = utilerias.CargaObjetoDeXML<TimbreFiscalDigital>(new TimbreFiscalDigital(), new Archivos().ConvertirBase64ToString(comprobante.Timbrado.CFDITimbrado.Respuesta.TimbreXML));
                UUID = Timbre.UUID;
                Console.WriteLine(comprobante.Mensaje + " UUID " + UUID);
                Console.WriteLine(" Para ver los archivos ingrese a la carpeta " + RutaTimbrados);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine(comprobante.Mensaje);
            }

            Console.WriteLine("Fin de Timbrado");
            Console.WriteLine("");
            //Console.ReadLine();

            #endregion "Realiza el Timbrado"

        }

        private static void CancelarUUID()
        {
            CancelarCFDIPeticion Peticion = new CancelarCFDIPeticion();

            Peticion.Usuario = "PruebasTimbrado"; // Este usuario se genera desde la pagina de https://cfdi.facturoporti.com.mx/ se debe de registrar para usar el productivo
            Peticion.Password = "@Notiene1"; // Es la contraseña del usuario cuando se registró
            Peticion.RFC = "AAA010101AAA";

            Peticion.UUIDs = new List<string>();
            Peticion.UUIDs.Add(UUID);

            #region "Realiza la cancelación"

            FacturoPorTi.CFDI.Api.ComprobanteDigital comprobante = new CFDI.Api.ComprobanteDigital();
            comprobante.SandBox = true; // True = pruebas,  False= Productivo

            //Para el ejercicio se usan los certificados de prueba del SAT
            // Tambien se puede enviar un stream o arreglo de bytes
            string RutaCertificado = ObtieneDirectorioAplicacion() + @"\Certificado\AAA010101AAA.cer";
            string RutaLlavePrivada = ObtieneDirectorioAplicacion() + @"\Certificado\AAA010101AAA.key";
            
            Console.WriteLine("Inicio de cancelación " + DateTime.Now);

            var resultado = comprobante.CancelarCFDI(Peticion, RutaCertificado, RutaLlavePrivada, "12345678a");

            if (resultado == true)
            {
                Console.WriteLine(comprobante.Mensaje + " Respuesta del Folio " + comprobante.Cancelaciones.FoliosRespuesta[0].EstatusCancelacionSAT);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine(comprobante.Mensaje);
            }

            Console.WriteLine("Fin de la cancelación");
            Console.WriteLine("");
            //Console.ReadLine();

            #endregion "Realiza el Timbrado"

        }

        private static void ConsultarEstatusUUID()
        {
            ConsultaEstatusPeticion Peticion = new ConsultaEstatusPeticion();

            Peticion.Usuario = "PruebasTimbrado"; // Este usuario se genera desde la pagina de https://cfdi.facturoporti.com.mx/ se debe de registrar para usar el productivo
            Peticion.Password = "@Notiene1"; // Es la contraseña del usuario cuando se registró

            Peticion.UUIDs = new List<string>();
            Peticion.UUIDs.Add(UUID);

            #region "Realiza la cancelación"

            FacturoPorTi.CFDI.Api.ComprobanteDigital comprobante = new CFDI.Api.ComprobanteDigital();
            comprobante.SandBox = true; // True = pruebas,  False= Productivo
            
            Console.WriteLine("Inicio de consulta " + DateTime.Now);

            var resultado = comprobante.ConsultaEstatusCFDI(Peticion);

            if (resultado == true)
            {
                Console.WriteLine(comprobante.Mensaje + " " + comprobante.EstatusFolios.FoliosRespuesta[0].EstatusCancelacionSAT);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine(comprobante.Mensaje);
            }

            Console.WriteLine("Fin de la consulta");
            Console.WriteLine("");
            //Console.ReadLine();

            #endregion "Realiza el Timbrado"

        }

        private static void ConsultarTimbresRestantes()
        {
            ConsultaTimbresRestantesPeticion Peticion = new ConsultaTimbresRestantesPeticion();

            Peticion.Usuario = "PruebasTimbrado"; // Este usuario se genera desde la pagina de https://cfdi.facturoporti.com.mx/ se debe de registrar para usar el productivo
            Peticion.Password = "@Notiene1"; // Es la contraseña del usuario cuando se registró

            #region "Realiza la cancelación"

            FacturoPorTi.CFDI.Api.ComprobanteDigital comprobante = new CFDI.Api.ComprobanteDigital();
            comprobante.SandBox = true; // True = pruebas,  False= Productivo

            Console.WriteLine("Inicio de consulta de timbres restantes " + DateTime.Now);

            var resultado = comprobante.ConsultaTimbresRestantes(Peticion);

            if (comprobante.Resultado == true)
            {
                Console.WriteLine(comprobante.Mensaje);
                Console.WriteLine(" Fecha de Compra = " + resultado.FechaCompra + " Timbres Utilizados= " + resultado.TimbresUtilizados + " CreditosRestantes = " + resultado.CreditosRestantes);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine(comprobante.Mensaje);
            }

            Console.WriteLine("Fin de la consulta de timbres restantes");
            Console.ReadLine();

            #endregion "Realiza el Timbrado"
        }

        public static string AcronimoArchivo(enumTipoDocumento TipoDocumentoActual)
        {
            string nombre = string.Empty;

            switch (TipoDocumentoActual)
            {
                case enumTipoDocumento.Factura:
                    nombre = "_FAC_";
                    break;
                case enumTipoDocumento.NotaCargo:
                    nombre = "_NCA_";
                    break;
                case enumTipoDocumento.NotaCredito:
                    nombre = "_NCE_";
                    break;
                case enumTipoDocumento.ReciboHonorarios:
                    nombre = "_RHO_";
                    break;
                case enumTipoDocumento.ReciboArrendamiento:
                    nombre = "_RARR_";
                    break;
                case enumTipoDocumento.CartaPorte:
                    nombre = "_CPT_";
                    break;
                case enumTipoDocumento.ReciboNominaCFDI:
                    nombre = "_RNOM_";
                    break;
                case enumTipoDocumento.ReciboDonatario:
                    nombre = "_RDON_";
                    break;
                case enumTipoDocumento.Pago:
                    nombre = "_PAGO_";
                    break;
            }

            return nombre;
        }

        private static string ObtieneDirectorioAplicacion()
        {
            string cmdLine = Environment.CommandLine;
            string DirectorioInstalacionAplicacion = string.Empty;

            try
            {
                cmdLine = cmdLine.Replace("console", " ");
                cmdLine = cmdLine.Replace("\"", "");
                DirectorioInstalacionAplicacion = Path.GetDirectoryName(cmdLine);

                int indice = DirectorioInstalacionAplicacion.ToUpper().IndexOf("BIN");
                DirectorioInstalacionAplicacion = DirectorioInstalacionAplicacion.Substring(0, indice - 1);
            }
            catch (Exception)
            {
                cmdLine = cmdLine.Replace("console", " ");
                cmdLine = cmdLine.Replace("\"", "");
                DirectorioInstalacionAplicacion = Path.GetDirectoryName(cmdLine);
            }

            return DirectorioInstalacionAplicacion;
        }
    }
}
