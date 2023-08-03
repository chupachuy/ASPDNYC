using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNyC.Helpers
{
    /// <summary>
    /// Enum con los códigos que puede regresar la BD.
    /// </summary>
    public enum DBResponseCodes
    {
        Success = 200,
        LoginFailed = 300,
        LoginFailedNoUserFound = 301,
        ErrorInData = 499,
        ErrorGeneric = 500
    }

    /// <summary>
    /// Códigos de tipos de usuarios del sistema.
    /// </summary>
    public enum TiposUsuarios
    {
        AGENTE = 1
    }

    /// <summary>
    /// Tipo de guardado para el archivo. Temporal o persistente.
    /// </summary>
    public enum FilePersistentType
    {
        Temporal = 1,
        Persistent = 10,
    }
}