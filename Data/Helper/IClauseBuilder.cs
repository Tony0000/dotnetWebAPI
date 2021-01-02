using System;
using System.Reflection;

namespace Data.Helper
{
    public interface IClauseBuilder
    {
        public string GetClause(PropertyInfo property);
        public string GetStringLikeClause();
        public string GetStringEqualClause();
        public string GetEqualClause(string value = null);
        public string GetDateTimeClause(Type pType);
        public string GetNumberClause<T>() where T : IComparable<T>;
        public string GetEnumClause(PropertyInfo property);
    }
}
