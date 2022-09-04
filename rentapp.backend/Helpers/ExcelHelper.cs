using OfficeOpenXml;
using System.Data;
using System.Reflection;

namespace rentapp.backend.Helpers
{
    public class ExcelHelper
    {
        internal static byte[] SaveExcel(DataTable dt, List<string> filters, List<string> cells, string worksheetName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add(worksheetName);

                var currentRow = 1;
                var column = 1;
                if (filters != null && filters.Count > 0)
                {
                    currentRow++;
                    worksheet.Cells[currentRow, column].Value = "Filtros";
                    worksheet.Cells[currentRow, column].Style.Font.Bold = true;
                    foreach (string item in filters)
                    {
                        currentRow++;
                        worksheet.Cells[currentRow, column].Value = item;
                    }
                }

                currentRow++;
                currentRow++;
                foreach (string cell in cells)
                {
                    worksheet.Cells[currentRow, column].Value = cell;
                    column++;
                }

                currentRow++;
                column = 1;
                worksheet.Cells[currentRow, column].LoadFromDataTable(dt);

                byte[] excelBytes;
                using (MemoryStream stream = new MemoryStream())
                {
                    excelPackage.SaveAs(stream);
                    excelBytes = stream.ToArray();

                    return excelBytes;
                }
            }
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
