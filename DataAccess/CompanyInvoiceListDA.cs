using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class CompanyInvoiceListItem
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
        public string due_date_time_zone { get; set; }
        public DateTime? expiration_date { get; set; }
        public string expiration_time_zone { get; set; }
        public DateTime? hedge_date { get; set; }
        public DateTime? close_date { get; set; }
        public bool is_prepaid { get; set; }
        public DateTime? dispute_start_date { get; set; }
        public DateTime? dispute_end_date { get; set; }
        public int create_user { get; set; }
        public DateTime? create_date { get; set; }
        public int? update_user { get; set; }
        public DateTime? update_date { get; set; }
    }
    public class CompanyInvoiceListDA
    {

        public IEnumerable<CompanyInvoiceListItem> GetCompanyInvoiceList(int companyID, bool? activeOnly)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@CompanyID", companyID);
            if (activeOnly == null)
            {
                queryParameters.Add("@ActiveOnly", true);
            }
            else
            {
                queryParameters.Add("@ActiveOnly", activeOnly);
            }

            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchCompanyInvoices";
                return connection.Query<CompanyInvoiceListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }
    }
}