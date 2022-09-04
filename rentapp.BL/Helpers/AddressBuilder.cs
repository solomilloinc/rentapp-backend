using rentapp.BL.Entities;
using System.Text;

namespace rentapp.BL.Helpers
{
    public class AddressBuilder
    {
        public static string GetCustomerAddress(Customer obj)
        {
            if (obj.Street.Trim().Length == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();

            sb.Append(obj.Street);

            if (!string.IsNullOrWhiteSpace(obj.Number))
            {
                sb.Append(" ");
                sb.Append(obj.Number);
            }

            if (!string.IsNullOrWhiteSpace(obj.Floor))
            {
                sb.Append("'");
                sb.Append(obj.Floor);
            }

            if (!string.IsNullOrWhiteSpace(obj.Unit))
            {
                sb.Append("'");
                sb.Append(obj.Unit);
            }

            // si se indico la localidad
            if (!string.IsNullOrWhiteSpace(obj.City))
            {
                sb.Append(", ");
                sb.Append(obj.City);
            }

            if (!string.IsNullOrWhiteSpace(obj.State))
            {
                sb.Append(", ");
                sb.Append(obj.State);
            }

            // si se indico alguna calle adyacente, entonces se la muestra
            if (!string.IsNullOrWhiteSpace(obj.AdjacentStreet1) || !string.IsNullOrWhiteSpace(obj.AdjacentStreet2))
            {
                sb.AppendLine();
                if (!string.IsNullOrWhiteSpace(obj.AdjacentStreet1) && !string.IsNullOrWhiteSpace(obj.AdjacentStreet2))
                {
                    sb.AppendLine($" (Entre {obj.AdjacentStreet1} y {obj.AdjacentStreet2})");
                }
                else if (!string.IsNullOrWhiteSpace(obj.AdjacentStreet1))
                {
                    sb.AppendLine($" (Esq. {obj.AdjacentStreet1})");
                }
                else if (!string.IsNullOrWhiteSpace(obj.AdjacentStreet2))
                {
                    sb.AppendLine($" (Esq. {obj.AdjacentStreet2})");
                }
            }

            return sb.ToString();
        }

        public static string GetSupplierAddress(Customer obj)
        {
            if (obj.Street.Trim().Length == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();

            sb.Append(obj.Street);

            if (!string.IsNullOrWhiteSpace(obj.Number))
            {
                sb.Append(" ");
                sb.Append(obj.Number);
            }

            if (!string.IsNullOrWhiteSpace(obj.Floor))
            {
                sb.Append("'");
                sb.Append(obj.Floor);
            }

            if (!string.IsNullOrWhiteSpace(obj.Unit))
            {
                sb.Append("'");
                sb.Append(obj.Unit);
            }

            // si se indico la localidad
            if (!string.IsNullOrWhiteSpace(obj.City))
            {
                sb.Append(", ");
                sb.Append(obj.City);
            }

            if (!string.IsNullOrWhiteSpace(obj.State))
            {
                sb.Append(", ");
                sb.Append(obj.State);
            }

            // si se indico alguna calle adyacente, entonces se la muestra
            if (!string.IsNullOrWhiteSpace(obj.AdjacentStreet1) || !string.IsNullOrWhiteSpace(obj.AdjacentStreet2))
            {
                sb.AppendLine();
                if (!string.IsNullOrWhiteSpace(obj.AdjacentStreet1) && !string.IsNullOrWhiteSpace(obj.AdjacentStreet2))
                {
                    sb.AppendLine($" (Entre {obj.AdjacentStreet1} y {obj.AdjacentStreet2})");
                }
                else if (!string.IsNullOrWhiteSpace(obj.AdjacentStreet1))
                {
                    sb.AppendLine($" (Esq. {obj.AdjacentStreet1})");
                }
                else if (!string.IsNullOrWhiteSpace(obj.AdjacentStreet2))
                {
                    sb.AppendLine($" (Esq. {obj.AdjacentStreet2})");
                }
            }

            return sb.ToString();
        }

        public static Customer BuildAddress(string text, string defaultCity, string defaultState, string defaultCountry)
        {
            Customer address = new Customer();

            string[] mainParts = text.Trim().Split(new char[] { '*' });

            string firstPart = string.Empty, secondPart = string.Empty;
            if (mainParts.Length > 0)
            {
                firstPart = mainParts[0];
            }
            if (mainParts.Length > 1)
            {
                secondPart = mainParts[1];
            }

            string[] firstParts = firstPart.Split(new char[] { ',', ';' });
            string[] secondParts = secondPart.Split(new char[] { ',', ';' });

            address.Street = firstParts[0].Trim();
            if (firstParts.Length > 1)
            {
                address.City = firstParts[1].Trim();
            }
            else
            {
                if (defaultCity != null)
                {
                    address.City = defaultCity.Trim();
                }
                else
                {
                    address.City = string.Empty;
                }
            }
            if (firstParts.Length > 2)
            {
                address.State = firstParts[2].Trim();
            }
            else
            {
                if (defaultState != null)
                {
                    address.State = defaultState.Trim();
                }
                else
                {
                    address.State = string.Empty;
                }
            }

            if (firstParts.Length > 3)
            {
                address.Country = firstParts[3].Trim();
            }
            else
            {
                if (defaultCountry != null)
                {
                    address.Country = defaultCountry.Trim();
                }
                else
                {
                    address.Country = string.Empty;
                }
            }

            string nearStreet1 = string.Empty, nearStreet2 = string.Empty;
            if (secondParts.Length > 0)
            {
                nearStreet1 = secondParts[0];
                if (secondParts.Length > 1)
                {
                    nearStreet2 = secondParts[1];
                }
            }
            address.AdjacentStreet1 = nearStreet1.Trim();
            address.AdjacentStreet2 = nearStreet2.Trim();

            address.Street = StringProcessor.TruncateIfNeeded(address.Street, 50);
            address.City = StringProcessor.TruncateIfNeeded(address.City, 50);
            address.State = StringProcessor.TruncateIfNeeded(address.State, 50);
            address.Country = StringProcessor.TruncateIfNeeded(address.Country, 50);
            address.AdjacentStreet1 = StringProcessor.TruncateIfNeeded(address.AdjacentStreet1, 50);
            address.AdjacentStreet2 = StringProcessor.TruncateIfNeeded(address.AdjacentStreet2, 50);

            return address;
        }
    }
}
