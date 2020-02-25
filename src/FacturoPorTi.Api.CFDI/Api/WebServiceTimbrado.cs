using System;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using FacturoPorTi.Api.Cfdi.Genericos;
using Newtonsoft.Json;

namespace FacturoPorTi.Api.Cfdi
{
    public class WebServiceFacturoPorTi
    {
        #region "Variables"

        public bool EsSandBox { get; set; }
        private string Url;
        public NameValueCollection Parametros { get; set; }
        public string Token { get; set; }

        public string Mensaje { get; set; }
        public string Codigo { get; set; }
        private const int TiempoEspera = 300000; // 5 minutos de espera para descargar 
        public bool ErrorConexion { get; set; }

        #endregion "Variables"

        #region "Constructor"

        public WebServiceFacturoPorTi(bool _esSandBox)
        {
            EsSandBox = _esSandBox;
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private class WebClientPersonalizado : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest webRequest = base.GetWebRequest(uri);

                webRequest.Timeout = TiempoEspera;
                ((HttpWebRequest)webRequest).ReadWriteTimeout = TiempoEspera;

                return webRequest;
            }            
        }
        
        #endregion "Constructor"

        #region "Metodos públicos"

        public T2 ConsumeServicio<T1, T2>(string controlador, TipoVerboHttp metodo, T1 objeto)
        {            
            #region "Asigna URL"        

#if DEBUG
            //Url = "http://localhost:52860/Servicios.svc/" + controlador;
           Url = "https://wcfpruebas.facturoporti.com.mx/Timbrado/Servicios.svc/" + controlador;
#else
            if (EsSandBox == false)
                Url = "https://wcf.facturoporti.com.mx/Timbrado/Servicios.svc/" + controlador;
            else
                Url = "https://wcfpruebas.facturoporti.com.mx/Timbrado/Servicios.svc/" + controlador;

#endif

            #endregion "Asigna URL"

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            WebClientPersonalizado webClient = new WebClientPersonalizado();
          
            ErrorConexion = false;

            try
            {
                switch (metodo)
                {
                    case TipoVerboHttp.Post:
                    case TipoVerboHttp.Put:
                    case TipoVerboHttp.Delete:

                        webClient.Headers["content-type"] = "application/json";
                        webClient.Headers["accept-encoding"] = "gzip,deflate";
                        webClient.Headers["user-agent"] = "Apache-HttpClient/4.1.1 (java 1.5)";

                        byte[] reqString = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objeto));
                        byte[] resByte = webClient.UploadData(Url, metodo == TipoVerboHttp.Post ? "POST" : metodo == TipoVerboHttp.Put ? "PUT" : "DELETE", reqString);

                        Stream responseStream = new Archivos().ConvertirByteToStream(resByte);
                        
                        StreamReader ReaderResponse = new StreamReader(responseStream, Encoding.UTF8);
                        return JsonConvert.DeserializeObject<T2>(ReaderResponse.ReadToEnd());
                        
                        break;
                    case TipoVerboHttp.Get:
                    
                        if (Parametros != null)
                        {
                            webClient.QueryString = Parametros;
                        }

                        webClient.Encoding = Encoding.UTF8;
                        string resultadoGet = webClient.DownloadString(Url);

                        return JsonConvert.DeserializeObject<T2>(resultadoGet);

                        break;                   
                   
                    case TipoVerboHttp.Patch:
                        break;
                    case TipoVerboHttp.Head:
                        break;
                    case TipoVerboHttp.Connect:
                        break;
                    case TipoVerboHttp.Options:
                        break;
                    case TipoVerboHttp.Trace:
                        break;
                    case TipoVerboHttp.Custom:
                        break;
                    default:
                        break;
                }
            }
            catch (WebException ew)
            {
                if (ew.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ew.Response as HttpWebResponse;
                    if (response != null)
                    {
                        switch ((int)response.StatusCode)
                        {
                            case (int)HttpStatusCode.OK:
                                break;
                            case (int)HttpStatusCode.Created:
                                break;
                            case (int)HttpStatusCode.Accepted:
                                break;
                            case (int)HttpStatusCode.NonAuthoritativeInformation:
                                break;
                            case (int)HttpStatusCode.NoContent:
                                break;
                            case (int)HttpStatusCode.BadRequest:
                                ErrorConexion = true;
                                Mensaje = "Error al recibir los datos";
                                break;
                            case (int)HttpStatusCode.Unauthorized:
                                Mensaje = "Acceso restringuido";
                                break;
                            case (int)HttpStatusCode.PaymentRequired:
                                Mensaje = "Se requiere realizar el pago por el servicio";
                                break;
                            case (int)HttpStatusCode.Forbidden:
                                Mensaje = "No tiene permisos para ingresar a esta opción";
                                break;
                            case (int)HttpStatusCode.NotFound:
                                ErrorConexion = true;
                                Mensaje = "No encontrado";
                                break;
                            case (int)HttpStatusCode.MethodNotAllowed:
                                ErrorConexion = true;
                                Mensaje = "Método no permitido";
                                break;
                            case (int)HttpStatusCode.InternalServerError:
                                ErrorConexion = true;
                                Mensaje = "Error en el servicio, si el problema persiste contacte al equipo de soporte@facturoporti.com.mx";
                                break;
                            case (int)HttpStatusCode.ServiceUnavailable:
                                ErrorConexion = true;
                                Mensaje = "Error en el servicio, si el problema persiste contacte al equipo de soporte@facturoporti.com.mx";
                                break;
                            default:
                                Mensaje = "Error en el servicio, si el problema persiste contacte al equipo de soporte@facturoporti.com.mx";
                                break;
                        }
                    }                    
                }                              
            }
            catch (Exception ex)
            {                
                ex = ex;
            }

            return default(T2);
        }

        #endregion "Metodos públicos"
    }
}
