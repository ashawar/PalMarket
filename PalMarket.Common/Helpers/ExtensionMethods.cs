using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PalMarket.Common.Helpers
{
    internal static class ExtensionMethods
    {
        #region Public Methods

        public static DataTable ToDataTable<T>(this T[] items) where T : class
        {
            var dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DateTime MinTime(this DateTime date)
        {
            return date.Date;
        }

        public static DateTime? MinTime(this DateTime? date)
        {
            return date.HasValue ? date.Value.Date : (DateTime?)null;
        }

        public static DateTime MaxTime(this DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime? MaxTime(this DateTime? date)
        {
            return date.HasValue ? date.Value.Date.AddDays(1).AddTicks(-1) : (DateTime?)null;
        }

        public static DateTime MinMonthTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime? MinMonthTime(this DateTime? date)
        {
            return date.HasValue ? new DateTime(date.Value.Year, date.Value.Month, 1) : (DateTime?)null;
        }

        public static DateTime MaxMonthTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddTicks(-1);
        }

        public static DateTime? MaxMonthTime(this DateTime? date)
        {
            return date.HasValue ? new DateTime(date.Value.Year, date.Value.Month, 1).AddMonths(1).AddTicks(-1) : (DateTime?)null;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static Dictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static Dictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (System.ComponentModel.PropertyDescriptor property in System.ComponentModel.TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        #endregion

        #region Private Methods

        private static void AddPropertyToDictionary<T>(System.ComponentModel.PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }

        #endregion
    }
}