using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class CustomerUpdateItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int update_user { get; set; }
    }

    public class CustomerAddItem
    {
        public int company_id { get; set; }
        public string name { get; set; }
        public int preferred_currency_type_id { get; set; }
        public int create_user { get; set; }
    }

    public class CustomerListItem
    {
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public int preferred_currency_type_id { get; set; }
    }
    public class CustomerDA
    {

        public IEnumerable<CustomerListItem> GetCustomerList()
        {
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchCustomer";
                return connection.Query<CustomerListItem>
                    (sql: storedProcedure
                    , param: null
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }

        public int CreateCustomer(CustomerAddItem cit, out string errormsg)
        {
            int returnID = 0;
            errormsg = null;

            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@CompanyID", cit.company_id);
                queryParameters.Add("@Name", cit.name);

                if (cit.preferred_currency_type_id != 0)
                {
                    queryParameters.Add("@PreferredCurrencyTypeID", cit.preferred_currency_type_id);
                }
                queryParameters.Add("@CreateUser", cit.create_user);
                queryParameters.Add("@ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_InsertCustomer";
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


        public int UpdateCustomer(CustomerUpdateItem cut, out string errormsg)
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
                    const string storedProcedure = "dbo.up_UpdateCustomer";
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
