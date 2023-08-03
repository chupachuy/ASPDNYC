using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using DNyC.Helpers;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace DNyC.Models
{
    public class DNyCGeneal : cEncripta
    {
        public String ApiGoogle()
        {
            String Url = string.Empty;
            if (HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) == "https://prueba.AgenteTop.com")
                Url = "https://maps.googleapis.com/maps/api/js?key=AIzaSyDEA47QK3qkVdmJy36nockocAtJk5gRQYA&callback=initMap&libraries=places&language=es";
            else if (HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) == "https://www.AgenteTop.com")
                Url = "https://maps.googleapis.com/maps/api/js?key=AIzaSyDJhWzHu_NZXi9oMWDDgfH5koGNjd0SPUM&callback=initMap&libraries=places&language=es";
            else
                Url = "https://maps.googleapis.com/maps/api/js?key=AIzaSyDEA47QK3qkVdmJy36nockocAtJk5gRQYA&callback=initMap&libraries=places&language=es";

            return Url;
        }


    }

    public class cGeneral
    {
        private static Logger LOG = new Logger();
        public static HttpRequestBase LastRequest;


        public static Boolean EsProduccion()
        {
            Boolean _EsPro;
            String Url = HttpContext.Current.Request.Url.AbsoluteUri;
            if ((Url.ToLower().IndexOf("localhost") >= 0) || (Url.ToLower().IndexOf("prueba") >= 0))
                _EsPro = false;
            else
                _EsPro = true;

            return _EsPro;
        }
        public static String GetHttpContext()
        {
            String Url = HttpContext.Current.Request.Url.AbsoluteUri;
            return Url;
        }


        public static XmlDocument ObtenerXML2JSON(String pJSON)
        {
            XmlDocument oXMLDatos;
            try { oXMLDatos = JsonConvert.DeserializeXmlNode(pJSON, "JSONDatos"); }
            catch { oXMLDatos = new XmlDocument(); }
            return oXMLDatos;
        }



        #region Inicializa XML y Valida Usuario Password
        public static XmlDocument InicializaXML()
        {
            XmlDocument oXML = new XmlDocument();
            XmlElement root = oXML.CreateElement("Respuesta");
            XmlNode nStatus = oXML.CreateNode(XmlNodeType.Element, "status", null);
            XmlNode nMessage = oXML.CreateNode(XmlNodeType.Element, "message", null);
            root.AppendChild(nStatus);
            root.AppendChild(nMessage);
            oXML.AppendChild(root);
            return oXML;
        }
        public static XmlDocument InicializaXML(String pStatus, String pMessage)
        {
            XmlDocument oXML = new XmlDocument();
            XmlElement root = oXML.CreateElement("Respuesta");
            XmlNode nStatus = oXML.CreateNode(XmlNodeType.Element, "status", null);
            XmlNode nMessage = oXML.CreateNode(XmlNodeType.Element, "message", null);
            root.AppendChild(nStatus);
            root.AppendChild(nMessage);
            oXML.AppendChild(root);
            nMessage.InnerText = pMessage;
            nStatus.InnerText = pStatus;
            return oXML;
        }

        public static String SerializaXML2JSON(XmlDocument pXML)
        {
            String str = JsonConvert.SerializeObject(pXML);
            //JSONEncrypt o = new JSONEncrypt();
            //str = o.OpenSSLEncrypt(str);
            return str;
        }

        public static Object XML2Class(XmlDocument oXML, String rootXML, Object objDestino)
        {
            if (oXML.SelectSingleNode("//" + rootXML) != null)
            {
                StringReader stringReader = new StringReader(oXML.SelectSingleNode("//" + rootXML).OuterXml);
                XmlSerializer serializer = new XmlSerializer(objDestino.GetType(), new XmlRootAttribute(rootXML));
                Object oO = serializer.Deserialize(stringReader);
                if (oO == null)
                {
                    return objDestino;
                }
                else
                {
                    return oO;
                }
            }
            else
            {
                return objDestino;
            }
        }

        // JAO - 2019/12/13
        public static void XMLToClass<T>(XmlDocument oXML, String rootXML, ref T objDestino)
        {
            if (oXML.SelectSingleNode("//" + rootXML) != null)
            {
                using (StringReader stringReader = new StringReader(oXML.SelectSingleNode("//" + rootXML).OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(objDestino.GetType(), new XmlRootAttribute(rootXML));
                    objDestino = (T)serializer.Deserialize(stringReader);
                    if (objDestino == null) { objDestino = (T)new Object (); }
                }
            }
        }

        #endregion

        #region Llamada a BD

        private static String _strStringConnection = "";

        public static SqlConnection ConexionSql()
        {
            return new SqlConnection(_strStringConnection);
        }


        /// <summary>
        /// Llama un Stored procedure convirtiendo los datos Json en un parámetro Xml.
        /// </summary>
        /// <param name="pStoreProcedure">Nombre de SP</param>
        /// <param name="pJSONDatos">Datos json a enviar como parámetros</param>
        /// <returns></returns>
        public static XmlDocument LlamaSP(String pStoreProcedure, String pJSONDatos, Boolean IsProduccion = false)
        {
            if (HttpContext.Current == null)
                return LlamaSP(pStoreProcedure, pJSONDatos, 1, LastRequest.ServerVariables["REMOTE_ADDR"], IsProduccion);

            decimal idUsuario = 1;
            return LlamaSP(pStoreProcedure, pJSONDatos, idUsuario, HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"], IsProduccion);
        }

        public static XmlDocument LlamaSP_SinSessionActiva(String pStoreProcedure, String pJSONDatos, Boolean IsProduccion = false)
        {
            return LlamaSP(pStoreProcedure, pJSONDatos, 0, HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"], IsProduccion);
        }


        /// <summary>
        /// Llama un Stored procedure convirtiendo los datos Json en un parámetro Xml.
        /// </summary>
        /// <param name="pStoreProcedure">Nombre de SP</param>
        /// <param name="pJSONDatos">Datos json a enviar como parámetros</param>
        /// <returns></returns>
        public async static Task<XmlDocument> LlamaSPAsync(String pStoreProcedure, String pJSONDatos, Boolean IsProduccion = false)
        {
            return await LlamaSPAsync(pStoreProcedure, pJSONDatos, 1, HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        }

        public static Boolean ValidaIPBloqueada(String ipBloq)
        {
            bool Valida = false;

            String Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            //Ip Estatica
            if (Ip == ipBloq)
                Valida = true;

            return Valida;
        }

        public static String GetIPPublic()
        {
            String Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            return Ip;
        }

        public static XmlDocument LlamaSP(String pStoreProcedure, String pJSONDatos, Decimal pIdUsua, String pIP, Boolean IsProduccion)
        {
            XmlDocument oXML = null;

            SqlConnection oConn = null;
            IsProduccion = true; // <---------- ¡OJO! Solo se puso para hacer pruebas de producción. ¡OJO!
                oConn = new SqlConnection(_strStringConnection);

            String vJSON = pJSONDatos;
            try
            {
                JSONEncrypt o = new JSONEncrypt();
                vJSON = o.OpenSSLDecrypt(vJSON);
            }
            catch { }
            XmlDocument oXMLDatos = ObtenerXML2JSON(vJSON);
            try
            {
                oConn.Open();
                using (SqlCommand cmd = new SqlCommand(pStoreProcedure, oConn))
                {
                    cmd.CommandTimeout = 99999;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pDatos", SqlDbType.Xml).Value = oXMLDatos.InnerXml;
                    cmd.Parameters.Add("@pIPClie", SqlDbType.VarChar).Value = pIP;
                    cmd.Parameters.Add("@pIdUsua", SqlDbType.Decimal).Value = pIdUsua;
                    cmd.Parameters.Add("@pXML", SqlDbType.Xml).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    oXML = new XmlDocument();
                    oXML.LoadXml(cmd.Parameters["@pXML"].Value.ToString());

                    if (oXML.SelectNodes("/Respuesta/status").Item(0).InnerText != "200")
                    {
                        LOG.Warning("Error sp - Datos envio: " + oXMLDatos.InnerXml.ToString() + " Error: " + ((oXML == null) ? "" : oXML.SelectNodes(" / Respuesta / status").Item(0).InnerText) + ((oXML == null) ? "" : oXML.SelectNodes(" / Respuesta / message").Item(0).InnerText + ")"));
                    }
                }
            }
            catch (Exception oE)
            {
                oXML = InicializaXML("499", "Error Obtener Datos (" + oE.Message + ")");
                LOG.Error("Datos para envio oXMLDatos.InnerText" + oXMLDatos.InnerXml.ToString() + "Error catch (Error:" + ((oXML == null) ? "" : oXML.SelectNodes("/Respuesta/status").Item(0).InnerText), oE);

            }
            finally
            {
                if (oConn.State == System.Data.ConnectionState.Open)
                    oConn.Close();
                oConn.Dispose();
            }
            return oXML;
        }

        public static async Task<XmlDocument> LlamaSPAsync(String pStoreProcedure, String pJSONDatos, Decimal pIdUsua, String pIP)
        {
            XmlDocument oXML = null;
            SqlConnection oConn = null;
            
            bool IsProduccion = true; // <---------- ¡OJO! Solo se puso para hacer pruebas de producción. ¡OJO!
                oConn = new SqlConnection(_strStringConnection);

            String vJSON = pJSONDatos;
            try
            {
                JSONEncrypt o = new JSONEncrypt();
                vJSON = o.OpenSSLDecrypt(vJSON);
            }
            catch { }
            XmlDocument oXMLDatos = ObtenerXML2JSON(vJSON);
            try
            {
                oConn.Open();
                using (SqlCommand cmd = new SqlCommand(pStoreProcedure, oConn))
                {
                    cmd.CommandTimeout = 99999;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pDatos", SqlDbType.Xml).Value = oXMLDatos.InnerXml;
                    cmd.Parameters.Add("@pIPClie", SqlDbType.VarChar).Value = pIP;
                    cmd.Parameters.Add("@pIdUsua", SqlDbType.Decimal).Value = pIdUsua;
                    cmd.Parameters.Add("@pXML", SqlDbType.Xml).Direction = ParameterDirection.Output;
                    await cmd.ExecuteNonQueryAsync();

                    oXML = new XmlDocument();
                    oXML.LoadXml(cmd.Parameters["@pXML"].Value.ToString());

                    if (oXML.SelectNodes("/Respuesta/status").Item(0).InnerText != "200")
                    {
                        LOG.Warning("Error sp - Datos envio: " + oXMLDatos.InnerXml.ToString() + " Error: " + ((oXML == null) ? "" : oXML.SelectNodes(" / Respuesta / status").Item(0).InnerText) + ((oXML == null) ? "" : oXML.SelectNodes(" / Respuesta / message").Item(0).InnerText + ")"));
                    }
                }
            }
            catch (Exception oE)
            {
                oXML = InicializaXML("499", "Error Obtener Datos (" + oE.Message + ")");
                LOG.Error("Datos para envio oXMLDatos.InnerText" + oXMLDatos.InnerXml.ToString() + "Error catch (Error:" + ((oXML == null) ? "" : oXML.SelectNodes("/Respuesta/status").Item(0).InnerText), oE);

            }
            finally
            {
                if (oConn.State == System.Data.ConnectionState.Open)
                    oConn.Close();
                oConn.Dispose();
            }
            return oXML;
        }
        #endregion

        #region General
        public static bool fEnviaCorreo(string pstrTo, string pstrCC, string pstrCCO, string pAsun, string pCuerHTML, System.Net.Mail.Attachment pArch, List<System.Net.Mail.Attachment> pArchivos = null)
        {
            MailMessage oMail = new MailMessage();
            oMail.To.Add(pstrTo);
            if (!String.IsNullOrEmpty(pstrCC))
            {
                oMail.CC.Add(pstrCC);
            }
            if (!String.IsNullOrEmpty(pstrCCO))
            {
                oMail.Bcc.Add(pstrCCO);
            }
            oMail.Subject = pAsun;
            oMail.IsBodyHtml = true;
            if (pArch != null)
            {
                oMail.Attachments.Add(pArch);
            }

            //Caso varios archivos
            if (pArchivos != null && pArchivos.Count > 0)
            {
                oMail.Attachments.Clear();
                foreach (System.Net.Mail.Attachment attachment in pArchivos)
                {
                    oMail.Attachments.Add(attachment);
                }
            }
            oMail.Body = pCuerHTML;

            bool bEnviado = false;
            using (var oSmtp = new SmtpClient())
            {
                string username = "";
                string password = "";
                string host = "";
                int port = 0;
                using (var smtp = new SmtpClient())
                {
                    var credential = (NetworkCredential)smtp.Credentials;
                    username = credential.UserName;
                    password = credential.Password;
                    host = smtp.Host;
                    port = smtp.Port;
                    oMail.From = new MailAddress($"Duraznos, Nectarinas y Ciruelas de California<{username}>");
                }

                oMail.ReplyToList.Add("deliciosamentedecalifornia@gmail.com");

                oSmtp.Host = host;
                oSmtp.Port = port;
                oSmtp.EnableSsl = true;
                oSmtp.UseDefaultCredentials = false;
                oSmtp.Credentials = new NetworkCredential(username, password);
                oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                int RETRY_CEILING = 3;
                String msg = String.Empty;
                for (int i = 0; i < RETRY_CEILING; ++i)
                {
                    try
                    {
                        oSmtp.Send(oMail);
                        bEnviado = true;
                        msg = String.Empty;
                    }
                    catch (Exception e)
                    {
                        msg = e.Message;
                        System.Threading.Thread.Sleep(1000 * 5);
                        continue;
                    }
                    break;
                }
            }
            return bEnviado;
        }

        public static MailMessage ObtenerMail(System.Net.Mail.SmtpClient oSmtp)
        {
            oSmtp = new System.Net.Mail.SmtpClient();
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string username = smtpSection.Network.UserName;
            System.Net.Mail.MailMessage oMail = new System.Net.Mail.MailMessage("Enlace.VET <" + username + ">", username);
            return oMail;
        }

        public static String ObtenerDominio(string host = "", bool quitarTripleW = true)
        {
            if (host == "") { host = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower(); }            
            if (quitarTripleW && host.StartsWith("www."))
            {
                host = host.Substring(4);
            }
            return host;
        }
        public static string ObtenerSubdominio(string host = "")
        {
            if (host == "") { host = ObtenerDominio(); }            
            string subdominio = "";
            int posDNyC = host.IndexOf("crmsalto.com");
            if (posDNyC > 0 ) {
                subdominio = host.Substring(0,posDNyC-1);
            }
            if (subdominio.StartsWith("www.")) {
                subdominio = host.Substring(4);
            }
            return subdominio;
        }
        #endregion

        #region TABGEN

        /// <summary>
        /// Clase Tabgen
        /// </summary>
        [Serializable]
        public class clTabgen
        {
            /// <summary>
            /// Identificador
            /// </summary>
            public String CID_TAB { get; set; }

            /// <summary>
            /// Código
            /// </summary>
            public String COD_INTE { get; set; }

            /// <summary>
            /// Descripción Tipo Proveedor
            /// </summary>
            [Display(Name = "Descripción")]
            public String GLS_DESC { get; set; }

            public List<SelectListItem> lTabgen { set; get; }

            /// <summary>
            /// New
            /// </summary>
            public clTabgen()
            { }

            /// <summary>
            /// Nuevo
            /// </summary>
            /// <param name="pId"></param>
            /// <param name="pGlosa"></param>
            public clTabgen(String pCOD_INTE, String pCID_TAB)
            {
                COD_INTE = pCOD_INTE;
                CID_TAB = pCID_TAB;
            }

            #region métodos
            public static List<SelectListItem> ObtenerTabgen(String pCodInte, Boolean pTodos)
            {
                dynamic oJSON = new JObject();
                oJSON.COD_INTE = pCodInte;
                oJSON.TODOS = pTodos;

                List<SelectListItem> oLista = new List<SelectListItem>();

                XmlDocument oXML = cGeneral.LlamaSP("bo.PQ_PROC_GENE$sp_select_tabgen", oJSON.ToString());

                DataSet dataSet = new DataSet();
                DataTable dataTable = new DataTable("table1");
                dataTable.Columns.Add("COD_INTE", typeof(string));
                dataTable.Columns.Add("GLS_DESC", typeof(string));
                dataSet.Tables.Add(dataTable);

                System.IO.StringReader xmlSR = new System.IO.StringReader(oXML.InnerText);
                dataSet.ReadXml(xmlSR, XmlReadMode.IgnoreSchema);

                if (dataSet.Tables[0] != null)
                    foreach (DataRow item in dataSet.Tables[0].Rows)
                        oLista.Add(new SelectListItem { Value = item["COD_INTE"].ToString(), Text = item["GLS_DESC"].ToString() });
                if (pTodos)
                    oLista.Insert(0, new SelectListItem { Value = "0", Text = "Todos" });
                return oLista;
            }
            #endregion
        }

        #endregion

        #region AgenteTopAnalytics
        public static String GetUserPlatform(HttpRequestBase request)
        {
            var ua = request.UserAgent;

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            //fallback to basic platform:
            return request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
        }

        public static String GetUserPlatform(HttpRequest request)
        {
            var ua = request.UserAgent;

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            //fallback to basic platform:
            return request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
        }

        private static String GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }

        internal static XmlDocument LlamaSP(string v)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region TALENTLMS

        public static Boolean TalentLMS_RegistroCuenta(String CORREO, String CONTRASENA, String NOMBRE, String PATERNO,
            String TIPO_PERFIL, String ID_USUARIO)
        {
            HttpClient client = new HttpClient();

            var webUrl = "https://agentetop.talentlms.com/";
            var parameters = "?email=" + CORREO;
            var uri_users = "api/v1/users" + parameters;

            client.BaseAddress = new Uri(webUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.ConnectionClose = true;

            var user = "mX6WqhYF5wBNaJqyTWXyHg96TyAuBR";
            var password = "$ocialMedia%agentetop";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
            System.Text.ASCIIEncoding.ASCII.GetBytes($"{user}:{password}")));

            HttpResponseMessage response = client.GetAsync(uri_users).Result;

            UsuarioTalentLMS EmpInfo = new UsuarioTalentLMS();
            if (response.IsSuccessStatusCode)
            {
                var EmpResponse = response.Content.ReadAsStringAsync().Result;
                EmpInfo = JsonConvert.DeserializeObject<UsuarioTalentLMS>(EmpResponse);

                return true;
            }
            else
            {
                var uri_usersignup = "api/v1/usersignup";

                HttpClient client2 = new HttpClient();
                client2.BaseAddress = new Uri(webUrl);
                client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client2.DefaultRequestHeaders.ConnectionClose = true;

                client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                System.Text.ASCIIEncoding.ASCII.GetBytes($"{user}:{password}")));

                var values = new Dictionary<string, string>
                {
                    {"first_name", NOMBRE},
                    {"last_name", PATERNO},
                    {"email", CORREO},
                    {"login", CORREO},
                    {"password", CONTRASENA},
                    {"user_type", "Learner-Type"},
                    {"restrict_email", "off"},
                    {"status", "active"},
                    {"language", "es"}
                };

                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response2 = client.PostAsync(uri_usersignup, content).Result;

                UsuarioTalentLMS EmpInfo2 = new UsuarioTalentLMS();
                if (response2.IsSuccessStatusCode)
                {
                    var EmpResponse2 = response2.Content.ReadAsStringAsync().Result;
                    EmpInfo2 = JsonConvert.DeserializeObject<UsuarioTalentLMS>(EmpResponse2);


                    //Registro TalentLMS
                    string jsonTalentLMS = JsonConvert.SerializeObject(new
                    {
                        ID_USUARIO = ID_USUARIO,
                        CORREO = CORREO,
                        CONTRASENA = CONTRASENA,
                        TIPO_PERFIL = TIPO_PERFIL
                    });

                    XmlDocument xml_TalentLMS = cGeneral.LlamaSP("[PERFILES].[PQ_REGISTRO$sp_crear_registro_TalentLMS]", jsonTalentLMS);
                    string json_TalentLMS = JsonConvert.SerializeObject(xml_TalentLMS);

                    return true;
                }
                else
                {
                    return false;
                }
            }

        }


        // GET: Crear Usuario
        public static String TalentLMS_Login(String ID_USUARIO)
        {
            HttpClient client = new HttpClient();

            var webUrl = "https://agentetop.talentlms.com/";
            var uri = "api/v1/userlogin";
            client.BaseAddress = new Uri(webUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.ConnectionClose = true;

            var user = "mX6WqhYF5wBNaJqyTWXyHg96TyAuBR";
            var password = "$ocialMedia%agentetop";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
            System.Text.ASCIIEncoding.ASCII.GetBytes($"{user}:{password}")));

            string json = JsonConvert.SerializeObject(new
            {
                ID_USUARIO = ID_USUARIO
            });

            XmlDocument oXML = cGeneral.LlamaSP("[PERFILES].[PQ_REGISTRO$sp_seleccionar_registro_TalentLMS]", json);

            if (oXML.ChildNodes[0]["Registros"] == null)
            {
                return "";
            }

            var values = new Dictionary<string, string>
            {
              {"login", oXML.ChildNodes[0]["Registros"]["CORREO"].InnerText},
              {"password", oXML.ChildNodes[0]["Registros"]["CONTRASENA"].InnerText}
            };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = client.PostAsync(uri, content).Result;

            UsuarioTalentLMS EmpInfo = new UsuarioTalentLMS();
            if (response.IsSuccessStatusCode)
            {
                var EmpResponse = response.Content.ReadAsStringAsync().Result;
                EmpInfo = JsonConvert.DeserializeObject<UsuarioTalentLMS>(EmpResponse);

                return EmpInfo.login_key;
            }
            else
            {
                return "";
            }
        }


        #endregion

    }

    /// <summary>
    /// Clase que representa un archivo a subir al servidor.
    /// </summary>
    public class ViewDataUploadFilesResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameDB { get; set; }
        public string GUID { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public string Text_Input { get; set; }
        public string Url { get; set; }
        public string DeleteUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string DeleteType { get; set; }
        public string RUTA_WEB { get; set; }
    }


    /// <summary>
    /// Modelo que se regresa a la vista con los archivos que se guardaron.
    /// </summary>
    public class JsonFilesModelView
    {
        public ViewDataUploadFilesResult[] files;
        public string TempFolder { get; set; }
        public JsonFilesModelView(List<ViewDataUploadFilesResult> filesList)
        {
            files = new ViewDataUploadFilesResult[filesList.Count];
            for (int i = 0; i < filesList.Count; i++)
            {
                files[i] = filesList.ElementAt(i);
            }
        }
    }


    public class UsuarioTalentLMS
    {
        public String id { set; get; }
        public String user_id { set; get; }
        public String login { set; get; }
        public String first_name { set; get; }
        public String last_name { set; get; }
        public String email { set; get; }
        public String login_key { set; get; }
    }



}