using System;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPI.ActionResult.Model;

namespace WebAPI.Extesions
{
    public static class SqlExceptionHandler
    {
        private const int SqlServerViolationOfUniqueIndex = 2601;
        private const int SqlServerViolationOfUniqueConstraint = 2627;
        private static readonly Regex UniqueConstraintRegex =
            new Regex("'IX_([a-zA-Z0-9]*)_([a-zA-Z0-9]*)'", RegexOptions.Compiled);
        
        public  static ValidationError ValidateUniqueConstraint(Exception ex)
        {
            var dbUpdateEx = ex as DbUpdateException;
            if (!(dbUpdateEx?.InnerException is SqlException sqlEx)) return null;

            if (sqlEx.Number == SqlServerViolationOfUniqueIndex ||
                sqlEx.Number == SqlServerViolationOfUniqueConstraint)
            {
                var valError = UniqueErrorFormatter(sqlEx);
                return valError;
            }

            return null;
        }

        private static ValidationError UniqueErrorFormatter(SqlException ex)
        {
            var message = ex.Errors[0].Message;
            var matches = UniqueConstraintRegex.Matches(message);
 
            if (matches.Count == 0)
                return null;

            var attribute = matches[0].Groups[2].Value;
            var errorMessage = $"Cannot have a duplicate {attribute}.";
 
            var openingBadValue = message.IndexOf("(", StringComparison.Ordinal);
            if (openingBadValue > 0)
            {
                var dupPart = message.Substring(openingBadValue + 1,
                    message.Length - openingBadValue - 3);
                errorMessage += $" Duplicate value was '{dupPart}'.";
            }

            
            return new ValidationError(attribute, errorMessage);
        }
    }
}