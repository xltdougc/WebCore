using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class Role
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
    }

    public class Company
    {
        public int company_id { get; set; }
        public string company_name { get; set; }
    }

    public class Customer
    {
        public int customer_id { get; set; }
        public string customer_name { get; set; }
    }


    public class LoginDA
    {
        public int GetUserID(string username)
        {
            int userID = 0;

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@UserName", (string)username);
                queryParameters.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                const string storedProcedure = "dbo.up_FetchUserID";
                connection.Execute(storedProcedure, queryParameters
                                                    , null
                                                    , DAShared.GetCommandTimeOutInterval()
                                                    , CommandType.StoredProcedure);

                userID = queryParameters.Get<Int32>("@UserId");
            }
            return userID;
        }

        public IEnumerable<Role> GetUserRoleList(int userid)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@UserId", userid);

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchUserRoles";
                return connection.Query<Role>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval() );
            }
        }

        public IEnumerable<Company> GetUserCompanyList(int userid)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@UserId", userid);

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchUserCompanies";
                return connection.Query<Company>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval() );
            }
        }

        public IEnumerable<Customer> GetUserCustomerList(int userid)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@UserId", userid);

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchUserCustomers";
                return connection.Query<Customer>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval() );
            }
        }

        public Guid Login(int userID, string username, string password)
        {
            Guid returnGuid = Guid.NewGuid();

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@UserID", (int)userID);
                queryParameters.Add("@SessionID", returnGuid);
                queryParameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                const string storedProcedure = "dbo.up_InsertLogin";
                connection.Execute(storedProcedure
                                    , queryParameters
                                    , null
                                    , DAShared.GetCommandTimeOutInterval()
                                    , CommandType.StoredProcedure);
                return returnGuid;
               
            }

        }
    }
}