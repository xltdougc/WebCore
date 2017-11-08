using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class CompanyUserListItem
    {
        public int userid { get; set; }
        public string email { get; set; }
        public int companyid { get; set; }
        public string company_name { get; set; }
        public int company_preferred_currency_type_id { get; set; }
        public string company_preferred_currency_type_code { get; set; }
    }
    public class CompanyUserListDA
    {
        public IEnumerable<CompanyUserListItem> GetCompanyUser(int companyID)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@CompanyID", companyID);

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchCompanyUsers";
                return connection.Query<CompanyUserListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }
    }
}