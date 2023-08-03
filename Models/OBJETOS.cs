namespace DNyC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Clase modelo que representa un usuario de la BD
    /// </summary>
    public class OBJETOS
    {
        [Column(TypeName = "numeric")]
        public decimal ID_OBJE { get; set; }

        [Required]
        [StringLength(500)]
        public string GLS_OBJE { get; set; }

        [StringLength(500)]
        public string GLS_NOMB_FANT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal NUM_PRIO_DESP { get; set; }

        [StringLength(500)]
        public string COD_TIPO_OBJE { get; set; }

        [StringLength(500)]
        public string GLS_ICO { get; set; }
        [StringLength(500)]
        public string GLS_LINK_Area { get; set; }
        [StringLength(500)]
        public string GLS_LINK_Controller { get; set; }
        [StringLength(500)]
        public string GLS_LINK_Action { get; set; }
    }
}
