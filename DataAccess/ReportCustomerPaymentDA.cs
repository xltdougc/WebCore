using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class ReportCustomerPayment
    {
        public IEnumerable<ReportCustomerPaymentDADetailListItem> detail { get; set; }
        public IEnumerable<ReportCustomerPaymentDASummaryListItem> summary { get; set; }
    }

    public class ReportCustomerPaymentDADetailListItem
    {
        public int company_id { get; set; }
        public string company_name { get; set; }
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string invoice_number { get; set; }
        public DateTime invoice_due_date { get; set; }
        public string customer_invoice_currency { get; set; }
        public decimal invoice_amount_due { get; set; }
        public DateTime payment_date { get; set; }
        public decimal payment_amount { get; set; }
        public decimal remaining_amount_due { get; set; }
        public string payment_method { get; set; }
        public string late { get; set; }
        public string status { get; set; }
    }

    public class ReportCustomerPaymentDASummaryListItem
    {
        public int company_id { get; set; }
        public string company_name { get; set; }
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string invoice_number { get; set; }
        public DateTime invoice_due_date { get; set; }
        public string customer_invoice_currency { get; set; }
        public decimal invoice_amount_due { get; set; }
        public DateTime payment_date { get; set; }
        public decimal payment_amount { get; set; }
        public decimal remaining_amount_due { get; set; }
        public string payment_method { get; set; }
        public string late { get; set; }
        public string status { get; set; }
    }

    public class ReportCustomerPaymentDA
    {
        public ReportCustomerPayment GetCustomerPaymentList(int customerID, DateTime startdate, DateTime enddate)
        {
            ReportCustomerPayment rcp = new ReportCustomerPayment();
            rcp.detail = GetCustomerPaymentDetailList(customerID, startdate, enddate);
            rcp.summary = GetCustomerPaymentSummaryList(customerID, startdate, enddate);
            return rcp;
        }

        private IEnumerable<ReportCustomerPaymentDADetailListItem> GetCustomerPaymentDetailList(int customerID
                                                    , DateTime? startdate
                                                    , DateTime? enddate)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerID", customerID);
            if ((startdate != null) && (startdate != DateTime.MinValue))
            {
                queryParameters.Add("@StartDate", (DateTime)startdate);
            }
            if ((enddate != null) && (enddate != DateTime.MinValue))
            {
                queryParameters.Add("@enddate", (DateTime)enddate);
            }
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_GetCustomerPaymentDetailData";
                return connection.Query<ReportCustomerPaymentDADetailListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }

        private IEnumerable<ReportCustomerPaymentDASummaryListItem> GetCustomerPaymentSummaryList(int customerID
                                            , DateTime? startdate
                                            , DateTime? enddate)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerID", customerID);
            if ((startdate != null) && (startdate != DateTime.MinValue))
            {
                queryParameters.Add("@StartDate", (DateTime)startdate);
            }
            if ((enddate != null) && (enddate != DateTime.MinValue))
            {
                queryParameters.Add("@enddate", (DateTime)enddate);
            }
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_GetCustomerPaymentSummaryData";
                return connection.Query<ReportCustomerPaymentDASummaryListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }
    }
}