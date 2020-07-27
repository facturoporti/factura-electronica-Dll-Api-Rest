using FacturoPorTi.Api.Cfdi.Genericos;
using FacturoPorTi.Api.Cfdi.Seguridad;
using System;
using System.IO;

namespace FacturoPorTi.Api.Cfdi
{
    public class ComprobanteDigital
    {
        public ComprobanteDigital()
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
        ~ComprobanteDigital()
        {
            this.Dispose();
        }
        /// <summary>
        /// Variable que indica si la operacion fue correcta
        /// </summary>
        public bool Resultado { get; set; }
        /// <summary>
        /// Almacena los mensajes del sistema
        /// </summary>
        public string Mensaje { get; set; }
        /// <summary>
        /// Es el objeto del timbre generado junto con el XML y PDF
        /// </summary>
        public GeneraCFDIApiRespuesta Timbrado { get; set; }

        public CancelarCFDIRespuesta Cancelaciones { get; set; }

        public ConsultaEstatusRespuesta EstatusFolios { get; set; }
        
        /// <summary>
        /// Indica si es SandBox el ambitente True = Pruebas , False = Productivo
        /// </summary>
        public bool SandBox { get; set; }
        
        private Archivos AdministradorArchivos { get; set; }

        /// <summary>
        /// Genera el CFDI a partir de los datos ingresados
        /// </summary>
        /// <param name="Peticion"></param>
        /// <param name="RutaCertificado"></param>
        /// <param name="RutaLlavePrivada"></param>
        /// <param name="ContraseñaCertificado"></param>
        /// <returns></returns>
        public bool GeneraCFDI(CFDIPeticion Peticion, string RutaCertificado, string RutaLlavePrivada, string ContraseñaCertificado)
        {
            AdministradorArchivos = new Archivos();

            var Certificado = AdministradorArchivos.ConvertirStreamToByte(AdministradorArchivos.Abrir(RutaCertificado));
            var LlavePrivada = AdministradorArchivos.ConvertirStreamToByte(AdministradorArchivos.Abrir(RutaLlavePrivada));

            return GeneraCFDI(Peticion, Certificado, LlavePrivada, ContraseñaCertificado);
        }

        /// <summary>
        /// Genera el CFDI a partir de los datos ingresados
        /// </summary>
        /// <param name="Peticion"></param>
        /// <param name="Certificado"></param>
        /// <param name="LlavePrivada"></param>
        /// <param name="ContraseñaCertificado"></param>
        /// <returns></returns>
        public bool GeneraCFDI(CFDIPeticion Peticion, Stream Certificado, Stream LlavePrivada, string ContraseñaCertificado)
        {
            AdministradorArchivos = new Archivos();

            var arregloCertificado = AdministradorArchivos.ConvertirStreamToByte(Certificado);
            var arregloLlavePrivada = AdministradorArchivos.ConvertirStreamToByte(LlavePrivada);

            return GeneraCFDI(Peticion, arregloCertificado, arregloLlavePrivada, ContraseñaCertificado);
        }

        /// <summary>
        /// Genera el CFDI a partir de los datos ingresados
        /// </summary>
        /// <param name="Peticion"></param>
        /// <param name="Certificado"></param>
        /// <param name="LlavePrivada"></param>
        /// <param name="ContraseñaCertificado"></param>
        /// <returns></returns>
        public bool GeneraCFDI(CFDIPeticion Peticion, Byte[] Certificado, Byte[] LlavePrivada, string ContraseñaCertificado)
        {
            #region "Variables"

            Mensaje = string.Empty;
            Resultado = false;
            AdministradorArchivos = new Archivos();
            Utilerias utilerias = new Utilerias();

            #endregion "Variables"

            #region "Valores Predeterminados"

            Peticion.DatosGenerales.Version = "3.3";
            Peticion.DatosGenerales.SellaCFDI = "true";
            Peticion.DatosGenerales.TimbraCFDI = "true";

            if (string.IsNullOrEmpty(Peticion.DatosGenerales.OpcionDecimales))
                Peticion.DatosGenerales.OpcionDecimales = "2"; // Valores permitidos 1: Truncar (Operaciones exactas) 2: Redondear hacia arriba o hacia abajo las cantidades 

            if (string.IsNullOrEmpty(Peticion.DatosGenerales.NumeroDecimales))
                Peticion.DatosGenerales.NumeroDecimales = "2";

            var configuracion = new Configuracion();

            Resultado = configuracion.CargaCertificado(Certificado, LlavePrivada, ContraseñaCertificado, Peticion.Encabezado.Emisor.RFC);

            if (Resultado == true)
            {
                Peticion.DatosGenerales.CSD = AdministradorArchivos.ConvertirByteToBase64(Certificado);
                Peticion.DatosGenerales.LlavePrivada = AdministradorArchivos.ConvertirByteToBase64(LlavePrivada);
                Peticion.DatosGenerales.CSDPassword = ContraseñaCertificado;
            }
            else
            {
                Mensaje = configuracion.Mensaje;
            }

            if (Peticion.Encabezado.Emisor != null)
            {
                Resultado = utilerias.ValidaRFC(Peticion.Encabezado.Emisor.RFC);
                if (Resultado == false)
                {
                    Mensaje = "El RFC del emisor es incorrecto";
                    return false;
                }
            }
            else
            {
                Mensaje = "El RFC del emisor es incorrecto";
                return false;
            }
            
            if (Peticion.Encabezado.Receptor != null)
            {
                Resultado = utilerias.ValidaRFC(Peticion.Encabezado.Receptor.RFC);
                if (Resultado == false)
                {
                    Mensaje = "El RFC del receptor es incorrecto";
                    return false;
                }
            }
            else
            {
                Mensaje = "El RFC del receptor es incorrecto";
                return false;
            }

            if (!string.IsNullOrEmpty(Peticion.DatosGenerales.ReceptorEmail))
            {
                Resultado = utilerias.ValidaCorreos(Peticion.DatosGenerales.ReceptorEmail);
                if (Resultado == false)
                {
                    Mensaje = "El email del destinatario es incorrecto";
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(Peticion.DatosGenerales.ReceptorEmailCC))
            {
                Resultado = utilerias.ValidaCorreos(Peticion.DatosGenerales.ReceptorEmailCC);
                if (Resultado == false)
                {
                    Mensaje = "El email de la copia es incorrecto";
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(Peticion.DatosGenerales.ReceptorEmailCCO))
            {
                Resultado = utilerias.ValidaCorreos(Peticion.DatosGenerales.ReceptorEmailCCO);
                if (Resultado == false)
                {
                    Mensaje = "El email de la copia oculta es incorrecto";
                    return false;
                }
            }

            #endregion "Valores Predeterminados"

            Resultado = false;
            WebServiceFacturoPorTi Api = new WebServiceFacturoPorTi(SandBox);
            Timbrado = Api.ConsumeServicio<CFDIPeticion, GeneraCFDIApiRespuesta>("ApiTimbrarCFDI", FacturoPorTi.Api.Cfdi.Genericos.TipoVerboHttp.Post, Peticion);

            if (Timbrado != null)
            {
                if (Timbrado.Estatus.Codigo == "000")
                {                    
                    Resultado = true;
                    Mensaje = "Factura generada correctamente";
                }
                else
                {
                    Mensaje = Timbrado.Estatus.Descripcion + Environment.NewLine + Environment.NewLine + Timbrado.Estatus.InformacionTecnica;
                }
            }
            else
            {
                Mensaje = "No se pudo realizar el timbrado si el problema persiste comuniquese a soporte@facturoporti.com.mx";
            }

            return Resultado;
        }

        /// <summary>
        /// Se genera la cancelacion de los UUID por medio del certificado
        /// </summary>
        /// <param name="Peticion"></param>
        /// <param name="RutaCertificado"></param>
        /// <param name="RutaLlavePrivada"></param>
        /// <param name="ContraseñaCertificado"></param>
        /// <returns></returns>
        public bool CancelarCFDI(CancelarCFDIPeticion Peticion, string RutaCertificado, string RutaLlavePrivada, string ContraseñaCertificado)
        {
            AdministradorArchivos = new Archivos();

            var Certificado = AdministradorArchivos.ConvertirStreamToByte(AdministradorArchivos.Abrir(RutaCertificado));
            var LlavePrivada = AdministradorArchivos.ConvertirStreamToByte(AdministradorArchivos.Abrir(RutaLlavePrivada));

            return CancelarCFDI(Peticion, Certificado, LlavePrivada, ContraseñaCertificado);
        }

        /// <summary>
        /// Se genera la cancelacion de los UUID por medio del certificado
        /// </summary>
        /// <param name="Peticion"></param>
        /// <param name="Certificado"></param>
        /// <param name="LlavePrivada"></param>
        /// <param name="ContraseñaCertificado"></param>
        /// <returns></returns>
        public bool CancelarCFDI(CancelarCFDIPeticion Peticion, Stream Certificado, Stream LlavePrivada, string ContraseñaCertificado)
        {
            AdministradorArchivos = new Archivos();

            var arregloCertificado = AdministradorArchivos.ConvertirStreamToByte(Certificado);
            var arregloLlavePrivada = AdministradorArchivos.ConvertirStreamToByte(LlavePrivada);

            return CancelarCFDI(Peticion, arregloCertificado, arregloLlavePrivada, ContraseñaCertificado);
        }

        public bool CancelarCFDI(CancelarCFDIPeticion Peticion, Byte[] Certificado, Byte[] LlavePrivada, string ContraseñaCertificado)
        {
            #region "Variables"

            Mensaje = string.Empty;
            Resultado = false;
            AdministradorArchivos = new Archivos();
            Utilerias utilerias = new Utilerias();

            #endregion "Variables"

            #region "Valores Predeterminados"

            Resultado = utilerias.ValidaRFC(Peticion.RFC);
            if (Resultado == false)
            {
                Mensaje = "Ingrese el RFC correctamente";
                return false;
            }

            var configuracion = new Configuracion();

            Resultado = configuracion.CargaCertificado(Certificado, LlavePrivada, ContraseñaCertificado, Peticion.RFC);

            if (Resultado == true)
            {
                FacturoPorTi.Api.Cfdi.Seguridad.Certificado certifica = new FacturoPorTi.Api.Cfdi.Seguridad.Certificado();
                Peticion.PFX = AdministradorArchivos.ConvertirByteToBase64(certifica.GeneraPFX(Certificado, LlavePrivada, ContraseñaCertificado));
                Peticion.PFXPassword = ContraseñaCertificado;
            }
            else
            {
                Mensaje = configuracion.Mensaje;
            }
            
            #endregion "Valores Predeterminados"

            Resultado = false;
            WebServiceFacturoPorTi Api = new WebServiceFacturoPorTi(SandBox);
            Cancelaciones = Api.ConsumeServicio<CancelarCFDIPeticion, CancelarCFDIRespuesta>("ApiCancelarCFDI", FacturoPorTi.Api.Cfdi.Genericos.TipoVerboHttp.Post, Peticion);

            if (Cancelaciones != null)
            {
                if (Cancelaciones.Estatus.Codigo == "000")
                {
                    Resultado = true;
                    Mensaje = Cancelaciones.Estatus.Descripcion; 
                }
                else
                {
                    Mensaje = Cancelaciones.Estatus.Descripcion + Environment.NewLine + Environment.NewLine + Cancelaciones.Estatus.InformacionTecnica;
                }
            }
            else
            {
                Mensaje = "No se pudo realizar la cancelación si el problema persiste comuniquese a soporte@facturoporti.com.mx";
            }

            return Resultado;
        }

        public bool ConsultaEstatusCFDI(ConsultaEstatusPeticion Peticion)
        {
            #region "Variables"

            Mensaje = string.Empty;
            Resultado = false;
            
            #endregion "Variables"
         
            WebServiceFacturoPorTi Api = new WebServiceFacturoPorTi(SandBox);
            EstatusFolios = Api.ConsumeServicio<ConsultaEstatusPeticion, ConsultaEstatusRespuesta>("ApiConsultaEstatusCFDI", FacturoPorTi.Api.Cfdi.Genericos.TipoVerboHttp.Post, Peticion);

            if (EstatusFolios != null)
            {
                if (EstatusFolios.Estatus.Codigo == "000")
                {
                    Resultado = true;
                    Mensaje = EstatusFolios.Estatus.Descripcion;
                }
                else
                {
                    Mensaje = EstatusFolios.Estatus.Descripcion + Environment.NewLine + Environment.NewLine + EstatusFolios.Estatus.InformacionTecnica;
                }
            }
            else
            {
                Mensaje = "No se pudo realizar la consulta del estatus de CFDI's si el problema persiste comuniquese a soporte@facturoporti.com.mx";
            }

            return Resultado;
        }

        public ConsultaTimbresRestantesRespuesta ConsultaTimbresRestantes(ConsultaTimbresRestantesPeticion Peticion)
        {
            #region "Variables"

            Mensaje = string.Empty;
            Resultado = false;

            #endregion "Variables"

            WebServiceFacturoPorTi Api = new WebServiceFacturoPorTi(SandBox);
            var respuesta = Api.ConsumeServicio<ConsultaTimbresRestantesPeticion, ConsultaTimbresRestantesRespuesta>("ApiConsultaTimbresRestantes", FacturoPorTi.Api.Cfdi.Genericos.TipoVerboHttp.Post, Peticion);

            if (respuesta != null)
            {
                if (respuesta.Estatus.Codigo == "000")
                {
                    Resultado = true;
                    Mensaje = respuesta.Estatus.Descripcion;
                }
                else
                {
                    Mensaje = respuesta.Estatus.Descripcion + Environment.NewLine + Environment.NewLine + respuesta.Estatus.InformacionTecnica;
                }
            }
            else
            {
                Mensaje = "No se pudo realizar la consulta de los timbres restantes si el problema persiste comuniquese a soporte@facturoporti.com.mx";
            }

            return respuesta;
        }

    }
}
