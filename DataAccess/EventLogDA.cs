using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public static class EventLogDA
    {
        public static bool WriteEventLog(int eventTypeID, string dataBaseName, string eventMessage, int createuser)
        {
            var queryParameters = new DynamicParameters();

            if (eventTypeID != 0)
            {
                queryParameters.Add("@EventTypeID", eventTypeID);
            }
            if (string.IsNullOrEmpty(dataBaseName))
            {
                queryParameters.Add("@DatabaseName", dataBaseName);
            }
            queryParameters.Add("@EventMessage", eventMessage);
            queryParameters.Add("@CreateUser", createuser);

            queryParameters.Add("@ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_InsertEventLog";
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