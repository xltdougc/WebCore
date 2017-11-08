using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public static class ErrorLogDA
    {
        public static bool WriteErrorLog(int errorTypeID
                                    , string dataBaseName
                                    , int errorCode
                                    , string errorMessage
                                    , int errorSeverity
                                    , string innerErrorMessage
                                    , string callStack
                                    , string errorSource
                                    , string methodName
                                    , int lineNumber
                                    ,int createuser)
        {
            var queryParameters = new DynamicParameters();

            if (errorTypeID != 0)
            {
                queryParameters.Add("@ErrorTypeID", errorTypeID);
            }
            if (string.IsNullOrEmpty(dataBaseName))
            {
                queryParameters.Add("@DatabaseName", dataBaseName);
            }
            if (errorCode != 0)
            {
                queryParameters.Add("@ErrorCode", errorCode);
            }
            queryParameters.Add("@ErrorMessage", errorMessage);
            if (errorSeverity != 0)
            {
                queryParameters.Add("@ErrorSeverity", errorSeverity);
            }
            queryParameters.Add("@InnerErrorMessage", innerErrorMessage);
            queryParameters.Add("@CallStack", callStack);
            queryParameters.Add("@Source", errorSource);
            queryParameters.Add("@Method", methodName);
            if (lineNumber != 0)
            {
                queryParameters.Add("@LineNumber", lineNumber);
            }
            queryParameters.Add("@CreateUser", createuser);
            queryParameters.Add("@ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_InsertErrorLog";
                connection.Execute(storedProcedure
                    , queryParameters
                    , null
                    , DAShared.GetCommandTimeOutInterval()
                    , CommandType.StoredProcedure);
            }
            return true;
        }
    }
}