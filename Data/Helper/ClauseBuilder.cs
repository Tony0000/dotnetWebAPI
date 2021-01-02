using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Data.Helper
{
    public class ClauseBuilder : IClauseBuilder
    {
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        public ClauseBuilder(string entityName, string propertyName, string propertyValue)
        {
            EntityName = entityName;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string GetClause(PropertyInfo property)
        {
            
            var pType = property.PropertyType;

            if (pType == typeof(string))
            {
                return GetStringLikeClause();
            }
            if (pType == typeof(DateTime) || pType == typeof(DateTime?))
            {
                return GetDateTimeClause(pType);
            }
            if (pType == typeof(bool))
            {
                return GetEqualClause();
            }
            if (pType == typeof(int) || pType == typeof(int?) || 
                pType == typeof(long) || pType == typeof(long?))
            {
                return GetNumberClause<long>();
            }
            if (pType == typeof(double) || pType == typeof(double?))
            {
                return GetNumberClause<double>();
            }
            if (property.PropertyType.IsEnum)
            {
                return GetEnumClause(property);
            }
            return null;
        }

        public string GetStringLikeClause()
        {
            return $"x.{PropertyName}.Contains(\"{PropertyValue}\")";
        }

        public string GetStringEqualClause()
        {
            return $"x.{PropertyName}.Equals(\"{PropertyValue}\")";
        }

        public string GetEqualClause(string value = null)
        {
            value ??= PropertyValue;
            return $"x.{PropertyName} == {value}";
        }
        
        private T GetValue<T>(string str) where T : IComparable<T>
        {
            try
            {
                return (T)Convert.ChangeType(str, typeof(T));
            }
            catch (Exception e)
            {
                throw new InvalidCastException(
                    $"Invalid value for attribute '{PropertyName}' in entity '{EntityName}'");
            }
        }

        private string GetClauseInterval<T>(string property = null, string value = null) where T : IComparable<T>
        {
            var interval = PropertyValue.Split(",");
            var start = GetValue<T>(interval[0]);
            var end = GetValue<T>(interval[1]);
            property ??= PropertyName;
            value ??= "%s";

            if (start.CompareTo(end) > 0)
                throw new InvalidIntervalException(
                    "Start value cannot be greater than end value for attribute " +
                    $"'{PropertyName}' in entity '{EntityName}'");

            return $"x.{property} >= {value.Replace("%s", start.ToString())}" +
                   $" && x.{property} <= {value.Replace("%s", end.ToString())}";
        }

        public string GetDateTimeClause(Type pType)
        {
            var subProperty = pType == typeof(DateTime) ? "Date" : "Value.Date";
            if (PropertyValue.Contains(","))
                return GetClauseInterval<DateTime>($"{PropertyName}.{subProperty}", "((DateTime)(object)\"%s\")");

            var dateTime = GetValue<DateTime>(PropertyValue);
            return $"x.{PropertyName}.{subProperty}.Equals((DateTime)(object)\"{dateTime}\")";
        }

        public string GetNumberClause<T>() where T : IComparable<T>
        {
            if (PropertyValue.Contains(","))
                return GetClauseInterval<T>();

            return GetEqualClause();
        }

        public string GetEnumClause(PropertyInfo property)
        {
            var enumNames = property.PropertyType.GetEnumNames().ToList();
            var idx = enumNames.IndexOf(PropertyValue);
            if (idx == -1)
                throw new InvalidEnumArgumentException(
                    $"Invalid value for attribute '{PropertyName}' in entity '{EntityName}'");

            return GetEqualClause((idx + 1).ToString());
        }
    }

    public class InvalidIntervalException : Exception
    {
        public InvalidIntervalException(string message) : base(message)
        {
        }
    }
}
