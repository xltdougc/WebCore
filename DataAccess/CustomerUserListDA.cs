using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class CustomerUserListItem
    {
        public int userid { get; set; }
        public string email { get; set; }
        public int customer_preferred_currency_type_id { get; set; }
        public string customer_preferred_currency_type_code { get; set; }
        public int companyid { get; set; }
        public string company_name { get; set; }
        public int company_preferred_currency_type_id { get; set; }
        public string company_preferred_currency_type_code { get; set; }
    }
    public class CustomerUserListDA
    {
        public IEnumerable<CustomerUserListItem> GetCustomerUser(int customerID)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerID", customerID);

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchCustomerUsers";
                return connection.Query<CustomerUserListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }
    }
}