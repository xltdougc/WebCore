using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class InvoiceTransactionDisputeItem
    {
        public int invoice_id { get; set; }
        public int transaction_type_id { get; set; }
        public int? payment_type_id { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }
        public int create_user { get; set; }
    }

    public class InvoiceTransactionDisputeListItem
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
    public class InvoiceTransactionDisputeDA
    {
        public IEnumerable<InvoiceTransactionDisputeListItem> GetCustomerInvoiceList(int invoiceID)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@InvoiceID", invoiceID);
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchInvoiceTransactionsForDisputes";
                return connection.Query<InvoiceTransactionDisputeListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }
    }
}
