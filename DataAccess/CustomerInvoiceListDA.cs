using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class CustomerInvoiceList
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string company_name { get; set; }
        public string invoice_number { get; set; }
        public string description { get; set; }
        public int source_type_id { get; set; }
        public string source_type_code { get; set; }
        public int company_currency_type_id { get; set; }
        public string company_currency_type_code { get; set; }
        public decimal company_amount { get; set; }
        public int customer_currency_type_id { get; set; }
        public string customer_currency_type_code { get; set; }
        public decimal customer_amount { get; set; }
        public DateTime? due_date { get; set; }
        public DateTime? expiration_date { get; set; }
        public int currency_calculation_id { get; set; }
        public DateTime? hedge_date { get; set; }
        public DateTime? close_date { get; set; }
        public bool is_prepaid { get; set; }
        public int create_user { get; set; }
        public DateTime? create_date { get; set; }
        public int? update_user { get; set; }
        public DateTime? update_date { get; set; }
    }
    public class CustomerInvoiceListDA
    {
        public IEnumerable<CustomerInvoiceList> GetCustomerInvoiceList(int customerID
                                                    , bool? unpaidonly
                                                    , DateTime? startdate
                                                    , DateTime? enddate 
                                                    , string invoicenumber)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerID", customerID);
            if (unpaidonly == null)
            {
                queryParameters.Add("@UnPaidOnly", false);
            }
            else
            {
                queryParameters.Add("@UnPaidOnly", unpaidonly);
            }
            if ((startdate != null) && (startdate != DateTime.MinValue))
            {
                queryParameters.Add("@StartDate", (DateTime)startdate);
            }
            if ((enddate != null) && (enddate != DateTime.MinValue))
            {
                queryParameters.Add("@enddate", (DateTime)enddate);
            }
            if (invoicenumber != null && invoicenumber.Trim().Length > 0)
            {
                queryParameters.Add("@InvoiceNumber", invoicenumber.Trim());
            }
            
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchCustomerInvoices";
                return connection.Query<CustomerInvoiceList>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }
    }
}