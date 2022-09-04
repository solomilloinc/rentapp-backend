using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace rentapp.BL.Helpers
{
    public static class CUITHelper
    {
        public static int dniStc, digitoStc, xyStc;

        public static bool Validate(string cuit)
        {
            String xyStr, dniStr, digitoStr;
            int digitoTmp;
            int n = cuit.LastIndexOf("-");
            if (n == -1)
            {
                return false;
            }

            xyStr = cuit.Substring(0, 2);
            int firstCharDNIIndex = cuit.IndexOf("-") + 1;
            if (firstCharDNIIndex <= 0)
            {
                return false;
            }

            dniStr = string.Empty;
            if (firstCharDNIIndex < (n - firstCharDNIIndex))
            {
                dniStr = cuit.Substring(firstCharDNIIndex, n - firstCharDNIIndex);
            }

            digitoStr = cuit.Substring(n + 1);
            if (xyStr.Length != 2 || dniStr.Length > 8 || digitoStr.Length != 1)
            {
                return false;
            }

            try
            {
                if (!Int32.TryParse(xyStr, out xyStc))
                {
                    return false;
                }

                if (!Int32.TryParse(dniStr, out dniStc))
                {
                    return false;
                }

                if (!Int32.TryParse(digitoStr, out digitoTmp))
                {
                    return false;
                }
            }
            catch (InvalidCastException e)
            {
                return false;
            }

            if (xyStc != 20 && xyStc != 23 && xyStc != 24 && xyStc != 27 && xyStc != 30 && xyStc != 33 && xyStc != 34)
            {
                return false;
            }

            Calcular();

            int val = 0;
            if (!Int32.TryParse(xyStr, out val))
            {
                return false;
            }

            if (digitoStc == digitoTmp && xyStc == val)
            {
                return true;
            }

            return false;
        }

        private static void Calcular()
        {
            long tmp1, tmp2;
            long acum = 0;
            int n = 2; tmp1 = xyStc * 100000000L + dniStc;

            for (int i = 0; i < 10; i++)
            {
                tmp2 = tmp1 / 10;
                acum += (tmp1 - tmp2 * 10L) * n;
                tmp1 = tmp2;
                if (n < 7)
                {
                    n++;
                }
                else
                {
                    n = 2;
                }
            }
            n = (int)(11 - acum % 11);
            if (n == 10)
            {
                if (xyStc == 20 || xyStc == 27 || xyStc == 24)
                {
                    xyStc = 23;
                }
                else
                {
                    xyStc = 33;
                }
                /*No es necesario hacer la llamada recursiva a calcular(), 	  	 
                  se puede poner el digito en 9 si el prefijo original era 	  	 
                  23 o 33 o poner el digito en 4 si el prefijo era 27*/
                Calcular();
            }
            else
            {
                if (n == 11)
                {
                    digitoStc = 0;
                }
                else
                {
                    digitoStc = n;
                }
            }
        }

        public static string GetPlainCUIT(string cuitValue)
        {
            string value = cuitValue.Replace("-", "");
            value = Regex.Replace(cuitValue, "-", "");
            return value;
        }
    }
}
