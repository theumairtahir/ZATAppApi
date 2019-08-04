using System;
using System.Data.SqlClient;

namespace ZATAppApi.Models.Exceptions
{
    /// <summary>
    /// Exception will be thrown whenever Database query or stored procedure calling process fails.
    /// </summary>
    public sealed class DbQueryProcessingFailedException:Exception
    {
        SqlException sqlException;
        public DbQueryProcessingFailedException(string path, SqlException sqlException):base("Error occured while processing SQL Query or Stored Procedure. Path: " + path, sqlException)
        {
            this.sqlException = sqlException;
        }
        /// <summary>
        /// Original SQL Exception caused the problem
        /// </summary>
        public SqlException InnerSQLException
        {
            get
            {
                return sqlException;
            }
        }
    }
}