using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DNyC.Helpers
{
    /// <summary>
    /// Clase de ayuda para obtener las llaves para OpenPay
    /// </summary>
    public class Payments
    {
        /// <summary>
        /// Regresa si se está utilizando el ambiente de producción de la plataforma actual de pagos.
        /// </summary>
        public static bool IsPaymentProduction
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["PaymentsProd"]);
            }
        }

        /// <summary>
        /// Regresa la URL (development o prod, dependiendo del valor de "PaymentsProd") de la plataforma de pagos.
        /// </summary>
        public static string PaymentURL
        {
            get
            {
                return IsPaymentProduction == true ? ConfigurationManager.AppSettings["PaymentsURLProd"]
                    : ConfigurationManager.AppSettings["PaymentsURLDev"];
            }
        }

        /// <summary>
        /// Regresa la llave pública.
        /// </summary>
        public static String PublicKey
        {
            get
            {
                return IsPaymentProduction == true ? ConfigurationManager.AppSettings["PaymentsPublicKeyProd"]
                  : ConfigurationManager.AppSettings["PaymentsPublicKeyDev"];
            }
        }

        /// <summary>
        /// Regresa la llave privada.
        /// </summary>
        public static String PrivateKey
        {
            get
            {
                return IsPaymentProduction == true ? ConfigurationManager.AppSettings["PaymentsPrivateKeyProd"]
                  : ConfigurationManager.AppSettings["PaymentsPrivateKeyDev"];
            }
        }

        /// <summary>
        /// Regresa el Id.
        /// </summary>
        public static String Id
        {
            get
            {
                return IsPaymentProduction == true ? ConfigurationManager.AppSettings["PaymentsIDProd"]
                  : ConfigurationManager.AppSettings["PaymentsIDDev"];
            }
        }

    }
}