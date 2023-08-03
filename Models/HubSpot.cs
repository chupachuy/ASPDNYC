using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using ZendeskApi_v2;
using ZendeskApi_v2.Extensions;
//using ZendeskApi_v2.Models.Constants;
using ZendeskApi_v2.Models.Shared;
//using ZendeskApi_v2.Models.Tickets;
//using ZendeskApi_v2.Requests;
using ZendeskApi_v2.Models.Brands;
using System.Globalization;

namespace DNyC.Models
{
    public class HubSpot
    {

        //private ZendeskApi api = new ZendeskApi("AgenteTop.zendesk.com", "miguel@cape-i.net", "96ZOFcriZD6btG8ETuNFcZ0GMFgBsw1Y4Tu5VBQJ","Me Interesa");
        //private TicketSideLoadOptionsEnum ticketSideLoadOptions = TicketSideLoadOptionsEnum.Users | TicketSideLoadOptionsEnum.Organizations | TicketSideLoadOptionsEnum.Groups;

        public Boolean EnviarConacto(String pNombre, String pApellido, String pCorreo, String pEdad, String pCP, String pPregunta, String pPagina)
        {
            // Build dictionary of field names/values (must match the HS field names)
            Dictionary<string, string> dictFormValues = new Dictionary<string, string>();
            dictFormValues.Add("firstname", pNombre);
            dictFormValues.Add("lastname", pApellido);
            dictFormValues.Add("email", pCorreo);

            if (pPregunta != "")
                dictFormValues.Add("preguntas", pPregunta);

            if (pEdad != "")
                dictFormValues.Add("edad", pEdad);

            if (pCP != "")
                dictFormValues.Add("codigo_postal", pCP);

            // Form Variables (from the HubSpot Form Edit Screen)
            int intPortalID = Properties.Settings.Default.HubSpot_PortalID; //place your portal ID here
            string strFormGUID = Properties.Settings.Default.HubSpot_FormGUID_Contacto; //place your form guid here

            // Tracking Code Variables
            string strHubSpotUTK = ""; // System.Web.HttpContext.Current.Cookies["hubspotutk"].Value;
            string strIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            // Page Variables
            string strPageTitle = pPagina;
            string strPageURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;

            // Do the post, returns true/false
            string strError = "";
            bool blnRet = Post_To_HubSpot_FormsAPI(intPortalID, strFormGUID, dictFormValues, strHubSpotUTK, strIpAddress, strPageTitle, strPageURL, ref strError);
            return blnRet;
        }

        /// 
        /// This helper function sends data to the the HubSpot Forms API
        /// 
        /// HubSpot Portal ID, or 'HUB ID'
        /// Unique ID for the form
        /// Dictionary containing all of the field names/values
        /// UserToken from the visitor's browser
        /// IP Address of the visitor
        /// Title of the page they visited
        /// URL of the page they visited
        /// 
        /// 
        public static bool Post_To_HubSpot_FormsAPI(int intPortalID, string strFormGUID, Dictionary<string, string> dictFormValues, string strHubSpotUTK, string strIpAddress, string strPageTitle, string strPageURL, ref string strMessage)
        {
            // Build Endpoint URL
            string strEndpointURL = string.Format("https://forms.hubspot.com/uploads/form/v2/{0}/{1}", intPortalID, strFormGUID);

            // Setup HS Context Object
            Dictionary<string, string> hsContext = new Dictionary<string, string>();
            hsContext.Add("hutk", strHubSpotUTK);
            hsContext.Add("ipAddress", strIpAddress);
            hsContext.Add("pageUrl", strPageURL);
            hsContext.Add("pageName", strPageTitle);

            // Serialize HS Context to JSON (string)
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            string strHubSpotContextJSON = json.Serialize(hsContext);

            // Create string with post data
            string strPostData = "";

            // Add dictionary values
            foreach (var d in dictFormValues)
            {
                strPostData += d.Key + "=" + HttpUtility.UrlEncode(d.Value) + "&";
            }

            // Append HS Context JSON
            strPostData += "hs_context=" + HttpUtility.UrlEncode(strHubSpotContextJSON);

            // Create web request object
            System.Net.HttpWebRequest r = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strEndpointURL);

            // Set headers for POST
            r.Method = "POST";
            r.ContentType = "application/x-www-form-urlencoded";
            r.ContentLength = strPostData.Length;
            r.KeepAlive = false;

            // POST data to endpoint
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(r.GetRequestStream()))
            {
                try
                {
                    sw.Write(strPostData);
                }
                catch (Exception ex)
                {
                    // POST Request Failed
                    strMessage = ex.Message;
                    return false;
                }
            }

            //System.Web.HttpContext.Current.Request.Cookies["hubspotutk"].Expires = DateTime.Now.AddDays(-1);

            return true; //POST Succeeded

        }


    }
}