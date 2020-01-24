using System;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Drawing;

namespace FacturoPorTi.CFDI.Genericos
{
    /// <summary>
    /// Clase que permite realizar operaciones basicas y comunes 
    /// del manejo de archivos como son: salvar, eliminar 
    /// abrir archivos a una ruta especifica
    /// </summary>
    public class Archivos
    {
        public bool Resultado { get; set; }
        public string Mensaje { get; set; }

        /// <summary>
        /// Crea una instancia de Modulo
        /// </summary>
        public Archivos()
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
        ~Archivos()
        {
            this.Dispose();
        }

        #region "Metodos Privados" 

        #endregion "Metodos Privados" 

        #region "Metodos Publicos" 
        
        /// <summary>
        /// Valida si existe un archivo en una ruta especifica
        /// </summary>
        /// <param name="ruta">Es la ruta de los archivos </param>
        /// <returns></returns>
        public bool Existe(string ruta)
        {
            return File.Exists(ruta);
        }

        /// <summary>
        /// Eliminar un archivo de una ruta especifica 
        /// </summary>
        /// <param name="ruta">Es la ruta de los archivos </param>
        /// <returns></returns>
        public bool Eliminar(string ruta)
        {
            try
            {
                if (File.Exists(ruta))
                    File.Delete(ruta);

                Resultado = true; 
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo eliminar el archivo ";
            }

            return Resultado;
        }

        ///// <summary>
        ///// Eliminar los archivos del directorio
        ///// </summary>
        ///// <param name="RutaDirectorio">Ruta de directorio donde se empieza la busqueda</param>
        ///// <param name="extensiones">Nombre de las extensiones de los archivos que se van a buscar</param>
        public void EliminarArchivosDirectorio(string ruta)
        {
            try
            {
                string[] directorios = Directory.GetDirectories(ruta);

                foreach (string directorio in directorios)
                {
                    DirectoryInfo tmp = new DirectoryInfo(directorio);
                    FileInfo[] archivos = tmp.GetFiles();

                    foreach (FileInfo archivo in archivos)
                    {
                        archivo.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo eliminar los archivos del directorio";
            }
        }

        /// <summary>
        /// Eliminar todos los archivos que se encuentran en un directorio
        /// </summary>
        /// <param name="ruta">Es la ruta de los archivos </param>
        /// <returns></returns>
        public bool EliminarArchivoDirectorio(string ruta)
        {
            bool resultado = false;
            string [] archivos;

            try
            {
                archivos = Directory.GetFiles(ruta);

                foreach (string archivo in archivos)
                {
                    if (File.Exists(archivo))
                        File.Delete(archivo);
                }

                resultado = true;

            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo eliminar los archivos del directorio";
            }
            return resultado;
        }

        /// <summary>
        /// Crea un directorio
        /// </summary>
        /// <param name="ruta">Es la ruta de los archivos </param>
        /// <returns></returns>
        public bool CreaDirectorio(string ruta, string nombreDirectorio)
        {
            bool resultado = false;
            
            //Create a new subfolder under the current active folder
            ruta = System.IO.Path.Combine(ruta, nombreDirectorio);

            try
            {
                if (Directory.Exists(ruta) == false)
                {
                    Directory.CreateDirectory(ruta);

                    DirectorySecurity seguridad = Directory.GetAccessControl(ruta);
                    seguridad.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.SetAccessControl(ruta, seguridad);
                }

                resultado = true;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo crear el directorio";
                throw new Exception(ex.Message);
            }
            return resultado;
        }

        public bool CreaDirectorio(string ruta)
        {
            bool resultado = false;

            //Create a new subfolder under the current active folder

            try
            {
                if (Directory.Exists(ruta) == false)
                {
                    Directory.CreateDirectory(ruta);

                    DirectorySecurity seguridad = Directory.GetAccessControl(ruta);
                    SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                    seguridad.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.FullControl, AccessControlType.Allow));
                  
                    everyone = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                    seguridad.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.FullControl, AccessControlType.Allow));
                  
                    everyone = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
                    seguridad.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.FullControl, AccessControlType.Allow));
                    
                    Directory.SetAccessControl(ruta, seguridad);
                }

                resultado = true;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo crear el directorio";
                throw new Exception(ex.Message);
            }
            return resultado;
        }

        /// <summary>
        /// Abrir un archivo de una ruta especifica en forma de Strea,
        /// </summary>
        /// <param name="ruta">Es la ruta de los archivos </param>
        /// <returns></returns>
        public FileStream Abrir(string ruta)
        {
            FileStream resultado = null;
            
            try
            {
                resultado = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo abrir el archivo";
            }
            return resultado;
        }

        /// <summary>
        /// Abrir un archivo y devolver un string con el contenido
        /// </summary>
        /// <param name="ruta">Es la ruta de los archivos </param>
        /// <returns></returns>
        public string AbrirModoTexto(string ruta)
        {
            string cadena = string.Empty;
            FileStream resultado = null;

            try
            {

                resultado = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (StreamReader contenidoArchivo = new StreamReader(resultado))
                {
                    cadena = contenidoArchivo.ReadToEnd();
                    contenidoArchivo.Close();
                }
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo abrir el archivo";
            }
           
            return cadena;
        }

        /// Guarda un archivo en una ubicacion que se especifica como parametro
        /// La condicion es que debe de tener permisos el usuario de asp net para poderlo realizar
        /// </summary>
        public bool Guardar(string contenido, string ruta)
        {
            bool resultado = false;

            try
            {
                using (FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete))
                {
                    StreamWriter tmpArchivo = new StreamWriter(fs);

                    tmpArchivo.Write(contenido);
                    tmpArchivo.Flush();
                    tmpArchivo.Close();
                }

                resultado = true;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo guardar el archivo";
            }
            return resultado;
        }


        /// <summary>
        /// Guarda un archivo en una ubicacion que se especifica como parametro
        /// La condicion es que debe de tener permisos el usuario de asp net para poderlo realizar
        /// </summary>
        public bool Guardar(Stream archivo, string ruta)
        {
            bool resultado = false;

            try
            {
                long tamaño = archivo.Length;
                byte[] archivoBytes = new byte[tamaño];
                
                using (FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete))
                {
                    archivo.Read(archivoBytes, 0, archivoBytes.Length);
                    fs.Write(archivoBytes, 0, archivoBytes.Length);
                }
                resultado = true;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo guardar el archivo";
            }
            return resultado;
        }


        /// <summary>
        /// Guarda un archivo en una ubicacion que se especifica como parametro
        /// La condicion es que debe de tener permisos el usuario de asp net para poderlo realizar
        /// </summary>
        public bool Guardar(byte[] archivo, string ruta)
        {
            bool resultado = false;

            try
            {
                using (FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete))
                {
                    fs.Write(archivo, 0, archivo.Length);
                }
                resultado = true;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo guardar el archivo";
            }
            return resultado;
        }

        /// <summary>
        /// Guarda un archivo en una ubicacion que se especifica como parametro
        /// La condicion es que debe de tener permisos el usuario de asp net para poderlo realizar
        /// </summary>
        public byte[] ConvertirStreamToByte(Stream archivo)
        {
            byte[] archivoBytes = new byte[archivo.Length];

            try
            {
                archivo.Read(archivoBytes, 0, archivoBytes.Length);
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo convertir el objeto fuente al final, valide que tenga informacion el objeto fuente";
            }
            return archivoBytes;
        }

        public byte[] ConvertirBase64ToByte(String file)
        {
            byte[] archivoBytes = null;

            try
            {
                archivoBytes = Convert.FromBase64String(file);
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo convertir el objeto fuente al final, valide que tenga informacion el objeto fuente";
            }
            return archivoBytes;
        }

        public String ConvertirBase64ToString(string valor)
        {
            String file = string.Empty;

            try
            {
                byte[] data = System.Convert.FromBase64String(valor);
                file = Encoding.ASCII.GetString(data);
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo convertir el objeto fuente al final, valide que tenga informacion el objeto fuente";
            }
            return file;
        }

        /// <summary>
        /// Guarda un archivo en una ubicacion que se especifica como parametro
        /// La condicion es que debe de tener permisos el usuario de asp net para poderlo realizar
        /// </summary>
        public byte[] ConvertirImageToByte(Image imagen)
        {           
            try
            {
                ImageConverter converter = new ImageConverter();
                return (byte[])converter.ConvertTo(imagen, typeof(byte[]));    
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo convertir el objeto fuente al final, valide que tenga informacion el objeto fuente";
                return new byte[0];
            }
        }
        public String ConvertirByteToBase64(byte[] valor)
        {
            String file = string.Empty;

            try
            {
                file = Convert.ToBase64String(valor);
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo convertir el objeto fuente al final, valide que tenga informacion el objeto fuente";
            }
            return file;
        }

        public Stream ConvertirByteToStream(byte[] valor)
        {
            Stream file = new MemoryStream();

            try
            {
                file = new MemoryStream(valor);
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo convertir el objeto fuente al final, valide que tenga informacion el objeto fuente";
            }
            return file;
        }

        /// <summary>
        /// Convierte un archivo Unix a Widnows
        /// </summary>
        /// <param name="contenido"></param>
        /// <returns></returns>
        public string UnixWindows(String contenido)
        {
            if (contenido.IndexOf('\r') != -1) return contenido;

            Char[] retornoCarro = { '\n' };
            String[] lineas = contenido.Split(retornoCarro, StringSplitOptions.None);
            return String.Join("\r\n", lineas);
        }

        /// <summary>
        /// Mueve un archivo de una ubicacion a otroa 
        /// </summary>
        /// <returns></returns>
        public bool CopiarArchivo(string fuente, string destino)
        {
            bool resultado = false;

            try
            {
                System.IO.File.Copy(fuente, destino, true);
                resultado = true;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo copiar el archivo";
            }
            return resultado;
        }
        
        /// <summary>
        /// Mueve un archivo de una ubicacion a otroa 
        /// </summary>
        /// <returns></returns>
        public bool MoverArchivo(string fuente, string destino)
        {
            bool resultado = false;

            try
            {
                try
                {
                   if (Existe(destino))
                   {
                        Eliminar(destino);
                   }
                }
                catch
                {
                }

                System.IO.File.Move(fuente, destino);
                resultado = true;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo mover el archivo";
            }
            return resultado;
        }
        
        #endregion
    }
}
