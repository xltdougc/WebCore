using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess
{
    public static class DAShared
    {
        public static IDbConnection OpenConnection()
        {
            IDbConnection connection = new SqlConnection("data source=prdb01.cr2ya0ljgxjh.us-east-2.rds.amazonaws.com;initial catalog=PayRecs;persist security info=True;user id=sqladmin;password=GoBronco$;MultipleActiveResultSets=True;App=Test;");
            return connection;
        }

        public static int GetCommandTimeOutInterval()
        {
            return 30;
        }
    }
}
