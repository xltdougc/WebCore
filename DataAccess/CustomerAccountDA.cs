using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class CustomerAccountUpdateItem
    {
        public int id { get; set; }
        public DateTime inactive_date { get; set; }
        public int update_user { get; set; }
    }

    public class CustomerAccountAddItem
    {
        public int customer_id { get; set; }
        public string card_number { get; set; }
        public DateTime expiration_date { get; set; }
        public int create_user { get; set; }
    }

    public class CustomerAccountListItem
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string card_number { get; set; }
        public DateTime expiration_date { get; set; }
        public DateTime? inactive_date { get; set; }
        public int account_type_id { get; set; }
        public string account_type_description { get; set; }
        public string account_type_code { get; set; }
        public int create_user { get; set; }
        public DateTime create_date { get; set; }
        public int update_user { get; set; }
        public DateTime? update_date { get; set; }
    }
    public class CustomerAccountDA
    {
        public IEnumerable<CustomerAccountListItem> GetCustomerAccountList(int customer_id)
        {
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@CustomerID", customer_id);
                const string storedProcedure = "dbo.up_FetchCustomerAccounts";
                return connection.Query<CustomerAccountListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }

        public int CreateCustomerAccount(CustomerAccountAddItem cit, out string errormsg)
        {
            int returnID = 0;
            errormsg = null;

            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@CustomerID", cit.customer_id);
                queryParameters.Add("@CardNumber", cit.card_number);
                queryParameters.Add("@ExpirationDate", cit.expiration_date);
                queryParameters.Add("@CreateUser", cit.create_user);
                queryParameters.Add("@ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_InsertCustomerAccount";
                    connection.Execute(storedProcedure
                        , queryParameters
                        , null
                        , DAShared.GetCommandTimeOutInterval()
                        , CommandType.StoredProcedure);

                    returnID = queryParameters.Get<Int32>("@ID");
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.Message;
            }
            return returnID;
        }


        public int UpdateCustomerAccount(CustomerAccountUpdateItem cut, out string errormsg)
        {
            int returnID = 0;
            errormsg = null;

            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@ID", cut.id);
                queryParameters.Add("@InactiveDate", cut.inactive_date);
                queryParameters.Add("@UpdateUser", cut.update_user);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_UpdateCustomerAccount";
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
