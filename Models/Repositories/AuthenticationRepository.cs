using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Security;
using DNyC.Models.Authentication;
//using DNyC.Areas.Login.Models;
using DNyC.Helpers;
using System.Web.Configuration;

namespace DNyC.Models.Repositories
{
    /// <summary>
    /// Repositorio de autenticación.
    /// </summary>
    public class AuthenticationRepository : BaseRepository
    {
        public AuthenticationRepository()
            : base()
        {
        }

        private CustomIdentityUser generateTestUser()
        {
            CustomIdentityUser result = new CustomIdentityUser()
            {
                //UserName = "Test",
                //Id = 1
            };

            //result.Password = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("bbb"));
            //result.VAL_Clave = "bbb";

            return result;
        }

        #region WIF Authentication
        /// <summary>
        /// Crea un usuario y lo inserta en la BD.
        /// Se utiliza el CustomUserStore para hacer todas las transacciones.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="idRole"></param>
        /// <returns></returns>
        internal async Task<bool> CreateUser(CustomIdentityUser user)
        {
            try
            {
                //return await Task.FromResult(true);

                XmlDocument xml = await cGeneral.LlamaSPAsync("perfil.Login$sp_crea_usuario", JsonConvert.SerializeObject(user));
                using (TextReader reader = new StringReader(xml.InnerXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CustomIdentityUser));
                    CustomIdentityUser result = (CustomIdentityUser)serializer.Deserialize(reader);

                    return true;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error al crear usuario -{0}", user.UserName), ex);
            }

            return false;
        }

        /// <summary>
        /// Crea una nueva cuenta de usuario con los datos del formulario.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal async Task<bool> CreateAccount(CreateAccountModel user)
        {
            try
            {
                //return await Task.FromResult(true);

                XmlDocument xml = await cGeneral.LlamaSPAsync("perfil.Login$sp_crea_usuario", JsonConvert.SerializeObject(user));
                using (TextReader reader = new StringReader(xml.InnerXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CustomIdentityUser));
                    CustomIdentityUser result = (CustomIdentityUser)serializer.Deserialize(reader);

                    return true;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error al crear usuario -{0}", user.UserName), ex);
            }

            return false;
        }

        /// <summary>
        /// Busca a un usuario por su username. Para nosotros el username es el email.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal CustomIdentityUser FindUser(string userName)
        {
            try
            {
                //return await Task.FromResult(generateTestUser());

                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$sp_busca_usuario_email", JsonConvert.SerializeObject(new { EMAIL = userName }));
                if (int.Parse(xml.SelectNodes("/Respuesta/status").Item(0).InnerText) == (int)DBResponseCodes.LoginFailedNoUserFound)
                {
                    LOG.Warning(string.Format("Usuario no existente -{0}-", userName));
                    return null;
                }

                using (TextReader reader = new StringReader(xml.SelectSingleNode("/Respuesta/USUARIO").OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CustomIdentityUser));
                    CustomIdentityUser user = (CustomIdentityUser)serializer.Deserialize(reader);

                    return user;
                }

            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error al identificar usuario -{0}-", userName), ex);
            }

            return null;
        }

        /// <summary>
        /// Find a user by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<CustomIdentityUser> FindUserAsync(decimal Id)
        {
            try
            {
                //return await Task.FromResult(generateTestUser());

                // TODO: Ya implementado
                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$sp_busca_usuario_id", JsonConvert.SerializeObject(new { ID = Id }));
                using (TextReader reader = new StringReader(xml.SelectSingleNode("Respuesta/CustomIdentityUser").OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CustomIdentityUser));
                    CustomIdentityUser user = (CustomIdentityUser)serializer.Deserialize(reader);

                    return user;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error al identificar usuario -{0}-", Id), ex);
            }

            return null;
        }

        /// <summary>
        /// Busca a un usuario por su usuario de red social.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal async Task<CustomIdentityUser> FindUserFromSocial(string provider, string userSocialId)
        {
            try
            {
                return await Task.FromResult(generateTestUser());

                // TODO: Ya implementado
                //XmlDocument xml = await cGeneral.LlamaSPAsync("perfil.sp_busca_usuario_social", JsonConvert.SerializeObject(new { userSocialId = userSocialId, provider = provider }));
                //using (TextReader reader = new StringReader(xml.InnerXml))
                //{
                //    XmlSerializer serializer = new XmlSerializer(typeof(CustomIdentityUser));
                //    CustomIdentityUser user = (CustomIdentityUser)serializer.Deserialize(reader);

                //    return user;
                //}
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error al identificar usuario -{0}-", userSocialId), ex);
            }

            return null;
        }

        /// <summary>
        /// Obtiene usuario por su email.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal CustomIdentityUser FindUserByEmail(string email)
        {
            try
            {
                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$sp_busca_usuario_email", JsonConvert.SerializeObject(new { EMAIL = email }));

                using (TextReader reader = new StringReader(xml.SelectSingleNode("Respuesta/CustomIdentityUser").OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CustomIdentityUser));
                    CustomIdentityUser user = (CustomIdentityUser)serializer.Deserialize(reader);

                    return user;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error al buscar usuario -{0}-", email), ex);
            }

            return null;
        }

        #region Authentication ROLES
        /// <summary>
        /// Obtiene si un usuario pertenece a un rol.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal async Task<bool> IsInRole(string userName, string roleName)
        {
            try
            {
                if (roleName.ToLower() == "admin")
                    return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error al identificar usuario -{0}- con rol -{1}-", userName, roleName), ex);
            }

            return false;
        }

        /// <summary>
        /// Obtiene los roles de un usuario.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal async Task<List<string>> GetRolesForUser(CustomIdentityUser user)
        {
            try
            {
                return await Task.FromResult(new List<string>() { "admin", "User", });
            }
            catch (Exception ex)
            {
                LOG.Error(string.Format("Error al identificar usuario -{0}-", user.UserName), ex);
            }

            return null;
        }
        #endregion
        #endregion

        #region Forms Authentication
        /// <summary>
        /// Busca a un usuario por su email y password.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal string LoginUser(string email, string pass, out USUARIO user, out string message)
        {
            user = null;
            try
            {
                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$sp_login_usuario", JsonConvert.SerializeObject(new { EMAIL = email, PASSWORD = pass, ID_PROMOTORIA = WebConfigurationManager.AppSettings["PromotoriaID"] }));
                if (int.Parse(xml.SelectNodes("/Respuesta/status").Item(0).InnerText) == (int)DBResponseCodes.LoginFailedNoUserFound)
                {
                    message = string.Format("Usuario no existente ::{0}::", email);
                    LOG.Warning(message);
                    return null;
                }

                message = "OK";

                using (TextReader reader = new StringReader(xml.SelectSingleNode("/Respuesta/USUARIO").OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(USUARIO));
                    USUARIO _user = (USUARIO)serializer.Deserialize(reader);
                    message = "OK";
                    user = _user;
                }

                return JsonConvert.SerializeObject(xml);

            }
            catch (Exception ex)
            {
                message = string.Format("Error al identificar usuario -{0}-", email);
                LOG.Error(message, ex);
            }

            return null;
        }
        /// <summary>
        /// nuevo método
        /// </summary>
        /// <param name="email">correo del usuario</param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal string Obtener_Permisos(string email, out List<OBJETOS> objects, out string message)
        {
            objects = null;
            try
            {
                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$sp_obtener_permisos", JsonConvert.SerializeObject(new { EMAIL = email }));
                if (int.Parse(xml.SelectNodes("/Respuesta/status").Item(0).InnerText) == (int)DBResponseCodes.LoginFailedNoUserFound)
                {
                    message = string.Format("Usuario no existente ::{0}::", email);
                    LOG.Warning(message);
                    return null;
                }

                message = "OK";

                objects = new List<OBJETOS>();
                foreach (XmlNode node in xml.SelectSingleNode("/Respuesta").SelectNodes("OBJETOS"))
                {
                    using (TextReader reader = new StringReader(node.OuterXml))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(OBJETOS));
                        objects.Add((OBJETOS)serializer.Deserialize(reader));
                    }
                }

                //using (TextReader reader = new StringReader(xml.SelectSingleNode("/Respuesta/OBJETOS").OuterXml))
                //{
                //    XmlSerializer serializer = new XmlSerializer(typeof(OBJETO));
                //    objects = (List<OBJETO>)serializer.Deserialize(reader);
                //}

                return JsonConvert.SerializeObject(xml);

            }
            catch (Exception ex)
            {
                message = string.Format("Error al identificar usuario -{0}-", email);
                LOG.Error(message, ex);
            }

            return null;
        }

        /// <summary>
        /// Busca a un usuario por su email. Para proceso de restaurar password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal CustomIdentityUser SearchUserByEmail(string email, out DBResponseCodes status)
        {
            status = DBResponseCodes.Success;
            try
            {
                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$sp_busca_usuario_email", JsonConvert.SerializeObject(new { EMAIL = email }));
                if (int.Parse(xml.SelectNodes("/Respuesta/status").Item(0).InnerText) == (int)DBResponseCodes.LoginFailedNoUserFound)
                {
                    status = DBResponseCodes.LoginFailedNoUserFound;
                    LOG.Warning(string.Format("Usuario no existente ::{0}::", email));
                    return null;
                }

                using (TextReader reader = new StringReader(xml.SelectSingleNode("/Respuesta/CustomIdentityUser").OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CustomIdentityUser));
                    CustomIdentityUser user = (CustomIdentityUser)serializer.Deserialize(reader);
                    return user;
                }

                //return JsonConvert.SerializeObject(xml);
            }
            catch (Exception ex)
            {
                status = DBResponseCodes.ErrorGeneric;
                LOG.Error(string.Format("Error al identificar usuario -{0}-", email), ex);
            }

            return null;
        }

        /// <summary>
        /// Busca a un usuario por su token. Para proceso de restaurar password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal CustomIdentityUser SearchUserByToken(string token, out DBResponseCodes status)
        {
            status = DBResponseCodes.Success;
            try
            {
                token = HttpUtility.UrlEncode(token);
                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$sp_busca_usuario_token", JsonConvert.SerializeObject(new { TOKEN = token }));
                if (int.Parse(xml.SelectNodes("/Respuesta/status").Item(0).InnerText) == (int)DBResponseCodes.LoginFailedNoUserFound)
                {
                    status = DBResponseCodes.LoginFailedNoUserFound;
                    LOG.Warning(string.Format("Usuario no existente ::{0}::", token));
                    return null;
                }

                using (TextReader reader = new StringReader(xml.SelectSingleNode("/Respuesta/CustomIdentityUser").OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CustomIdentityUser));
                    CustomIdentityUser user = (CustomIdentityUser)serializer.Deserialize(reader);
                    return user;
                }

                //return JsonConvert.SerializeObject(xml);
            }
            catch (Exception ex)
            {
                status = DBResponseCodes.ErrorGeneric;
                LOG.Error(string.Format("Error al identificar usuario -{0}-", token), ex);
            }

            return null;
        }

        /// <summary>
        /// Busca a un usuario por su email. Para proceso de restaurar password
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal CustomIdentityUser SearchUserById(decimal id, out DBResponseCodes status)
        {
            status = DBResponseCodes.Success;
            try
            {
                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$sp_busca_usuario_id", JsonConvert.SerializeObject(new { ID = id }));
                if (int.Parse(xml.SelectNodes("/Respuesta/status").Item(0).InnerText) == (int)DBResponseCodes.LoginFailedNoUserFound)
                {
                    status = DBResponseCodes.LoginFailedNoUserFound;
                    LOG.Warning(string.Format("Usuario no existente ::{0}::", id));
                    return null;
                }

                using (TextReader reader = new StringReader(xml.SelectSingleNode("/Respuesta/CustomIdentityUser").OuterXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CustomIdentityUser));
                    CustomIdentityUser user = (CustomIdentityUser)serializer.Deserialize(reader);
                    return user;
                }
            }
            catch (Exception ex)
            {
                status = DBResponseCodes.ErrorGeneric;
                LOG.Error(string.Format("Error al identificar usuario -{0}-", id), ex);
            }

            return null;
        }

        /// <summary>
        /// Se actualiza el password de un usuario.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        internal bool UpdatePassword(string email, string newPassword, out DBResponseCodes status)
        {
            status = DBResponseCodes.Success;
            try
            {
                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$SP_Act_Password", JsonConvert.SerializeObject(new { VAL_Correo = email, VAL_Clave = newPassword }));
                status = (DBResponseCodes)Enum.Parse(typeof(DBResponseCodes), xml.SelectNodes("/Respuesta/status").Item(0).InnerText);

                switch (status)
                {
                    case DBResponseCodes.LoginFailedNoUserFound:
                        LOG.Warning(string.Format("Usuario no existente ::{0}::", email));
                        return false;
                    case DBResponseCodes.Success:
                        LOG.Info(string.Format("Contraseña actualizada para usuario ::{0}::", email));
                        return true;
                }
            }
            catch (Exception ex)
            {
                status = DBResponseCodes.ErrorGeneric;
                LOG.Error(string.Format("Error al identificar usuario -{0}-", email), ex);
            }

            return false;
        }

        /// <summary>
        /// Se actualiza el password de un usuario.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        internal bool UpdateToken(decimal Id, string token, out DBResponseCodes status)
        {
            status = DBResponseCodes.Success;
            try
            {
                XmlDocument xml = cGeneral.LlamaSP("perfiles.Login$sp_act_token", JsonConvert.SerializeObject(new { ID = Id, TOKEN = token }));
                status = (DBResponseCodes)Enum.Parse(typeof(DBResponseCodes), xml.SelectNodes("/Respuesta/status").Item(0).InnerText);

                switch (status)
                {
                    case DBResponseCodes.LoginFailedNoUserFound:
                        LOG.Warning(string.Format("Usuario no existente ::{0}::", Id));
                        return false;
                    case DBResponseCodes.Success:
                        LOG.Info(string.Format("Token actualizado para usuario ::{0}::", Id));
                        return true;
                }
            }
            catch (Exception ex)
            {
                status = DBResponseCodes.ErrorGeneric;
                LOG.Error(string.Format("Error al identificar usuario -{0}-", Id), ex);
            }

            return false;
        }
        #endregion
    }
}