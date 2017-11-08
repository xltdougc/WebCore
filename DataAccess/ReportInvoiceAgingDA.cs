using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class ReportInvoiceAging
    {
        public IEnumerable<ReportInvoiceAgingDADetailListItem> detail { get; set; }
        public IEnumerable<ReportInvoiceAgingDASummaryListItem> summary { get; set; }
    }

    public class ReportInvoiceAgingDADetailListItem
    {
        public int company_id { get; set; }
        public string company_name { get; set; }
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public DateTime invoice_date { get; set; }
        public string invoice_number { get; set; }
        public string company_invoice_currency { get; set; }
        public decimal company_invoice_amount { get; set; }
        public string customer_invoice_currency { get; set; }
        public decimal customer_invoice_amount_0_30 { get; set; }
        public decimal customer_invoice_amount_31_60 {get;set;}
        public decimal customer_invoice_amount_61_90 { get; set; }
        public decimal customer_invoice_amount_Over_90 { get; set; }
    }

    public class ReportInvoiceAgingDASummaryListItem
    {
        public int company_id { get; set; }
        public string company_name { get; set; }
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string company_invoice_currency { get; set; }
        public decimal company_invoice_amount { get; set; }
        public string customer_invoice_currency { get; set; }
        public decimal customer_invoice_amount_0_30 { get; set; }
        public decimal customer_invoice_amount_31_60 { get; set; }
        public decimal customer_invoice_amount_61_90 { get; set; }
        public decimal customer_invoice_amount_Over_90 { get; set; }
    }

    public class ReportInvoiceAgingDA
    {
        public ReportInvoiceAging GetInvoiceAgingList(int companyID)
        {
            ReportInvoiceAging rcp = new ReportInvoiceAging();
            rcp.detail = GetInvoiceAgingDetailList(companyID);
            rcp.summary = GetInvoiceAgingSummaryList(companyID);
            return rcp;
        }

        private IEnumerable<ReportInvoiceAgingDADetailListItem> GetInvoiceAgingDetailList(int companyID)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@CompanyID", companyID);
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_GetInvoiceAgingDetailData";
                return connection.Query<ReportInvoiceAgingDADetailListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }

        private IEnumerable<ReportInvoiceAgingDASummaryListItem> GetInvoiceAgingSummaryList(int companyID)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@CompanyID", companyID);
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_GetInvoiceAgingSummaryData";
                return connection.Query<ReportInvoiceAgingDASummaryListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval() );
            }
        }
    }
}