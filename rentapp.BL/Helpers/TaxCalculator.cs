using System;
using System.Collections.Generic;
using System.Text;

namespace rentapp.BL.Helpers
{
    public class TaxCalculator
    {
        public const decimal TAX_RATE = 21M;



        /// <summary>
        /// Retorna el subtotal (sin IVA); totalPaid bruto
        /// </summary>
        /// <param name="totalPaid">Total neto</param>
        /// <param name="alicuota">Alicuota (en porcentaje)</param>
        /// <returns>Total Bruto o subtotal</returns>
        public static decimal GetAmountWithNoTax(decimal total, decimal alicuota)
        {
            return Math.Round(total / (1 + (alicuota) / 100), 5);
        }

        /// <summary>
        /// Retorna el totalPaid (con IVA); totalPaid neto
        /// </summary>
        /// <param name="totalPaid">Total bruto</param>
        /// <param name="alicuota">Alicuota (en porcentaje)</param>
        /// <returns>Total Neto o totalPaid</returns>
        public static decimal GetAmountWithTax(decimal totalWithNoTax, decimal alicuota)
        {
            return Math.Round(totalWithNoTax * (1 + (alicuota) / 100), 5);
        }

        /// <summary>
        /// Retorna el IVA
        /// </summary>
        /// <param name="totalPaid">Total neto</param>
        /// <param name="alicuota">Alicuota (en porcentaje)</param>
        /// <returns>IVA</returns>
        public static decimal GetTax(decimal total, decimal alicuota)
        {
            return total - GetAmountWithNoTax(total, alicuota);
        }
    }
}
