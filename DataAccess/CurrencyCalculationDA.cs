using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class CurrencyCalculationAddItem
    {
        public int currency_type_id { get; set; }
        public DateTime calculation_date { get; set; }
        public decimal amount { get; set; }
        public int create_user { get; set; }
    }

    public class CurrencyCalculationDA
    {
        public int CreateCurrencyCalculation(CurrencyCalculationAddItem ccitem, out string errormsg)
        {
            int returnID = 0;
            errormsg = null;

            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@CurrencyTypeID", ccitem.currency_type_id);
                queryParameters.Add("@CalculationDate", ccitem.calculation_date);
                queryParameters.Add("@Amount", ccitem.amount);
                queryParameters.Add("@CreateUser", ccitem.create_user);
                queryParameters.Add("@ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_InsertCurrencyCalculation";
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
    }
}