using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{

    public class CompanyAddItem
    {
        public string name { get; set; }
        public int add_user { get; set; }
    }

    public class CompanyUpdateItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int update_user { get; set; }
    }

    public class CompanyListItem
    {
        public int company_id { get; set; }
        public string company_name { get; set; }
        public int preferred_currency_type_id { get; set; }
        public string preferred_currency_type_code { get; set; }
    }

    public class CompanyDA
    {
        public IEnumerable<CompanyListItem> GetCompanyList()
        {
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchCompany";
                return connection.Query<CompanyListItem>
                    (sql: storedProcedure
                    , param: null
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }

        public int CreateCompany(string companyname, int userid, out string errorMsg)
        {
            errorMsg = null;
            int newID = 0;
            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Name", companyname);
                queryParameters.Add("@CreateUser", userid);
                queryParameters.Add("@ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_InsertCompany";
                    connection.Execute(storedProcedure
                        , queryParameters
                        , null
                        , DAShared.GetCommandTimeOutInterval()
                        , CommandType.StoredProcedure);

                    newID = queryParameters.Get<Int32>("@ID");
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return newID;
        }

        public int UpdateCompany(CompanyUpdateItem cut, out string errormsg)
        {
            int returnID = 0;
            errormsg = null;

            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@ID", cut.id);
                queryParameters.Add("@Name", cut.name);
                queryParameters.Add("@UpdateUser", cut.update_user);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_UpdateCompany";
                    connection.Execute(storedProcedure
                        , queryParameters
                        , null
                        , DAShared.GetCommandTimeOutInterval()
                        , CommandType.StoredProcedure);

                    returnID = cut.id;
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.Message;
            }
            return returnID;
        }
    }
}
