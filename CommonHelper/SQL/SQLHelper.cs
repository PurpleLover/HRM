using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonHelper.SQL
{
    public static class SQLHelper
    {
        public static int BulkInsert<T>(IEnumerable<T> list, string tableName = null) where T : class
        {
            string connectionStr = ConfigurationManager.ConnectionStrings["QLNSContext"].ConnectionString;
            tableName = GetTableName<T>(tableName);
            DataTable table = ConvertToDataTable(list, tableName);
            using (var bulkCopy = new SqlBulkCopy(connectionStr))
            {
                foreach (DataColumn c in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                }
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = list.Count();
                bulkCopy.BulkCopyTimeout = 200;
                bulkCopy.WriteToServer(table);
                return table.Rows.Count;
            }
        }

        private static DataTable ConvertToDataTable<T>(IEnumerable<T> list, string tableName)
        {
            var table = new DataTable(tableName);
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       //.Where(x => x.CanRead && x.PropertyType.Namespace.Equals("System"))
                                       //.Where(y => y.CustomAttributes.Any(z => z.AttributeType.Name == "DataMemberAttribute"))
                                       .ToList();
            //&& typeof(T).Namespace.Equals("System")
            if (!props.Any())
            {
                table.Columns.Add("Id", typeof(T));
                foreach (var item in list)
                {
                    table.Rows.Add(item);
                }

                return table;
            }

            foreach (var x in props)
            {
                var type = Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType;
                table.Columns.Add(x.Name, type);
            }

            var values = new object[props.Count()];
            foreach (var item in list)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }

            return table;
        }

        private static string GetTableName<T>(string tableName)
        {
            TableAttribute tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));
            if (tableAttribute != null && tableName == null)
            {
                tableName = tableAttribute.Name;
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("The Table name was not found in BulkInsert.");
            }
            return tableName;
        }
    }
}
