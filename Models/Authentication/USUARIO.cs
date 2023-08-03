namespace DNyC.Models.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Clase modelo que representa un usuario de la BD
    /// </summary>
    public class USUARIO
    {
        [Key]
        [Column(TypeName = "numeric")]
        public decimal ID_Usua { get; set; }

        [Required]
        [StringLength(500)]
        public string VAL_Correo { get; set; }

        [Required]
        [StringLength(200)]
        public string VAL_Clave { get; set; }

        [Required]
        [StringLength(20)]
        public string COD_ESTA_USUA { get; set; }

        [Required]
        [StringLength(80)]
        public string GLS_Nombre { get; set; }

        [Required]
        [StringLength(60)]
        public string GLS_Paterno { get; set; }

        [Required]
        [StringLength(60)]
        public string GLS_Materno { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CTR_FECH_INGR { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CTR_FECH_MODI { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CTR_USUA_INGR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CTR_USUA_MODI { get; set; }

        [StringLength(500)]
        public string VAL_GUID { get; set; }

        [StringLength(255)]
        public string SOCIAL_ID { get; set; }

        [StringLength(55)]
        public string SOCIAL_PROVIDER { get; set; }

        [StringLength(55)]
        public string COD_TIPO_USUA { get; set; }

        public string SecurityStamp { get; set; }

        public string TOKEN_SEGURIDAD { get; set; }

        public DateTime? FECH_EXP_TOKEN { get; set; }

        public decimal ID_AFILIACION { get; set; }

        public decimal ID_PERFIL { get; set; }

        [StringLength(500)]
        public string FOTO_PERFIL { get; set; }

        [Column(TypeName = "numeric")]
        public string ID_AGENTE { get; set; }

        [Column(TypeName = "numeric")]
        public string ID_PROMOTORIA { get; set; }

        [Column(TypeName = "numeric")]
        public string ID_TIPO_USUARIO { get; set; }

        public string TIPO_USUARIO { get; set; }

        public string TELEFONO_USUARIO { get; set; }

        public string CORREO_USUARIO { get; set; }
    }
}
