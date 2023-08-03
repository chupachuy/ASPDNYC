using Openpay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DNyC.Helpers
{
    /// <summary>
    /// Creación de instancias y manipulación de objetos OpenPay
    /// </summary>
    public class PaymentOpenPay
    {
        public enum OpenPayChargeTypes
        {
            card,
            store,
            bank_account
        }

        /// <summary>
        /// Crea una nueva instancia con los valores dinámicos de Test o Produccion y las llaves correspondientes.
        /// </summary>
        public static OpenpayAPI CreateInstance()
        {
            return new OpenpayAPI(Payments.PrivateKey, Payments.Id, Payments.IsPaymentProduction);
        }
    }
}