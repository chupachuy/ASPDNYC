using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DNyC.Models.Authentication
{
    /// <summary>
    /// Clase personalizada que representa un usuario para login en el sistema.
    /// </summary>
    public class CustomIdentityUser : IUser<decimal>
    {
        public decimal Id
        {
            get { return ID_Usua; }
            set { ID_Usua = value; }
        }

        public decimal UserId
        {
            get { return ID_Usua; }
            set { ID_Usua = value; }
        }

        public List<int> Roles { get; set; }

        [DataType(DataType.EmailAddress)]
        public string UserName
        {
            get { return VAL_Correo; }
            set { VAL_Correo = value; }
        }

        public decimal ID_Usua { get; set; }

        public decimal ID_AFILIACION { get; set; }

        public decimal ID_PERFIL { get; set; }

        [StringLength(500)]
        public string FOTO_PERFIL { get; set; }

        [Required]
        [StringLength(500)]
        [DataType(DataType.EmailAddress)]
        public string VAL_Correo { get; set; }

        [Required]
        [StringLength(200)]
        public string VAL_Clave { get; set; }

        [Required]
        [StringLength(20)]
        public string ESTADO_USUARIO { get; set; }

        [Required]
        [StringLength(80)]
        public string GLS_Nombre { get; set; }

        [Required]
        [StringLength(60)]
        public string GLS_Paterno { get; set; }

        [Required]
        [StringLength(60)]
        public string GLS_Materno { get; set; }

        public DateTime? CTR_FECH_INGR { get; set; }

        public DateTime? CTR_FECH_MODI { get; set; }

        public decimal? CTR_USUA_INGR { get; set; }

        public decimal? CTR_USUA_MODI { get; set; }

        [StringLength(255)]
        public string SOCIAL_ID { get; set; }

        [StringLength(55)]
        public string SOCIAL_PROVIDER { get; set; }

        public string VAL_GUID { get; set; }

        public string SecurityStamp
        {
            get { return VAL_GUID; }
            set { VAL_GUID = value; }
        }

        public string TOKEN_SEGURIDAD { get; set; }

        public DateTime? FECH_EXP_TOKEN { get; set; }

        public CustomIdentityUser()
        {
            Roles = new List<int>();
        }
    }
}