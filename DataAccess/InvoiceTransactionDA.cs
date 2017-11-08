using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class InvoiceTransactionAddItem
    {
        public int invoice_id { get; set; }
        public int transaction_type_id { get; set; }
        public int? payment_type_id { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }
        public int create_user { get; set; }
    }

    public class InvoiceTransactionListItem
    {
        public string invoice_number { get; set; }
        public int invoice_transaction_id { get; set; }
        public int invoice_id { get; set; }
        public int transaction_type_id { get; set; }
        public string transaction_type { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int create_user { get; set; }
        public DateTime? create_date { get; set; }
        public int? update_user { get; set; }
        public DateTime? update_date { get; set; }
    }
    public class InvoiceTransactionDA
    {
        public IEnumerable<InvoiceTransactionListItem> GetCustomerInvoiceList(int invoiceID)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@InvoiceID", invoiceID);
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchInvoiceTransactions";
                return connection.Query<InvoiceTransactionListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval() );
            }
        }

        public int CreateInvoiceTransaction(InvoiceTransactionAddItem iti, out string errormsg)
        {
            int returnID = 0;
            errormsg = null;

            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@InvoiceID", iti.invoice_id);
                queryParameters.Add("@TransactionTypeID", iti.transaction_type_id);
                if (iti.payment_type_id != 0)
                {
                    queryParameters.Add("@PaymentTypeID", iti.payment_type_id);
                }
                queryParameters.Add("@Amount", iti.amount);
                if (iti.description != null)
                {
                    queryParameters.Add("@Description", iti.description);

                }
                queryParameters.Add("@CreateUser", iti.create_user);
                queryParameters.Add("@ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_InsertInvoiceTransaction";
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
