using DNyC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNyC.Models.Repositories
{
    /// <summary>
    /// Repositorio base del que deben heredar todos los repositorios
    /// que vayan a acceder a datos, o a hacer cálculos o transformaciones
    /// para presentar en el sitio web.
    /// </summary>
    public abstract class BaseRepository : IDisposable
    {
        protected Logger LOG;

        public BaseRepository()
        {
            LOG = new Logger();
        }

        public virtual void Dispose()
        {

        }
    }
}