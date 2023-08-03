using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNyC.Helpers
{
    /// <summary>
    /// Realiza log tomando la configuración del archivo NLog.config
    /// </summary>
    public class Logger
    {
        private NLog.Logger logger;

        #region Properties
        /// <summary>
        /// Get set si ha sido inicializado.
        /// </summary>
        public bool IsInitialized { get; set; }
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor.
        /// </summary>
        public Logger()
        {
            logger = LogManager.GetLogger(GetType().FullName);
            IsInitialized = true;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Hace un log si el compilador está en modo Debug.
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        public void Debug(string text)
        {
#if DEBUG
            logger.Log(LogLevel.Debug, text);
#endif
        }

        /// <summary>
        /// Hace un log si el compilador está en modo Debug.
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        /// <param name="values">Valores que se asignaran en runtime usando string.format</param>
        public void Debug(string text, params object[] values)
        {
#if DEBUG
            logger.Log(LogLevel.Debug, text, values);
#endif
        }

        /// <summary>
        /// Hace log en nivel Info.
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        public void Info(string text)
        {
            logger.Log(LogLevel.Info, text);
        }

        /// <summary>
        /// Hace log en nivel Info.
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        /// <param name="values">Valores que se asignaran en runtime usando string.format</param>
        public void Info(string text, params object[] values)
        {
            logger.Log(LogLevel.Info, text, values);
        }

        /// <summary>
        /// Hace log en nivel Warning.
        /// </summary>
        /// <param name="values">Valores que se asignaran en runtime usando string.format</param>
        public void Warning(string text)
        {
            logger.Log(LogLevel.Warn, text);
        }

        /// <summary>
        /// Hace log en nivel Warning.
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        /// <param name="values">Valores que se asignaran en runtime usando string.format</param>
        public void Warning(string text, params object[] values)
        {
            logger.Log(LogLevel.Warn, text, values);
        }

        /// <summary>
        /// Hace log en nivel Warning.
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        /// <param name="ex"></param>
        public void Warning(string text, Exception ex)
        {
            logger.Log(LogLevel.Warn, ex, text);
        }

        /// <summary>
        /// Hace log en nivel Error.
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        public void Error(string text)
        {
            logger.Log(LogLevel.Error, text);
        }

        /// <summary>
        /// Hace log en nivel Error.
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        /// <param name="values">Valores que se asignaran en runtime usando string.format</param>
        public void Error(string text, params object[] values)
        {
            logger.Log(LogLevel.Error, text, values);
        }

        /// <summary>
        /// Hace log en nivel Error.
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        /// <param name="ex"></param>
        public void Error(string text, Exception ex)
        {
            logger.Log(LogLevel.Error, ex, text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">Texto a poner en log.</param>
        /// <param name="ex"></param>
        /// <param name="values"></param>
        public void Error(string text, Exception ex, params object[] values)
        {
            logger.Log(LogLevel.Error, ex, text, values);
        }

        #endregion
    }
}