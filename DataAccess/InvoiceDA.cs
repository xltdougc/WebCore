using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class InvoiceUpdateItem
    {
        public int id { get; set; }
        public string description { get; set; }
        public int? source_type_id { get; set; }
        public decimal? amount_due { get; set; }
        public int? company_currency_type_id { get; set; }
        public decimal? company_amount { get; set; }
        public int? customer_currency_type_id { get; set; }
        public decimal? customer_amount { get; set; }
        public DateTime? due_date { get; set; }
        public DateTime? expiration_date { get; set; }
        public int currency_calculation_id { get; set; }
        public DateTime? hedge_date { get; set; }
        public DateTime? close_date { get; set; }
        public DateTime? dispute_start_date { get; set; }
        public DateTime? dispute_end_date { get; set; }
        public int update_user { get; set; }
    }

    public class InvoiceAddItem
    {
        public int customer_id { get; set; }
        public string invoice_number { get; set; }
        public string description { get; set; }
        public int? source_type_id { get; set; }
        public decimal? amount_due { get; set; }
        public int? company_currency_type_id { get; set; }
        public decimal? company_amount { get; set; }
        public int? customer_currency_type_id { get; set; }
        public decimal? customer_amount { get; set; }
        public DateTime? due_date { get; set; }
        public DateTime? expiration_date { get; set; }
        public int currency_calculation_id { get; set; }
        public DateTime? hedge_date { get; set; }
        public DateTime? close_date { get; set; }
        public int create_user { get; set; }
    }

    public class InvoiceListItem
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string company_name { get; set; }
        public string invoice_number { get; set; }
        public string description { get; set; }
        public int? source_type_id { get; set; }
        public string source_type_code { get; set; }
        public int? company_currency_type_id { get; set; }
        public string company_currency_type_code { get; set; }
        public decimal? company_amount { get; set; }
        public int? customer_currency_type_id { get; set; }
        public decimal? customer_amount { get; set; }
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

    public class InvoiceDA
    {
        public int CreateInvoice(InvoiceAddItem invoice, out string errormsg)
        {
            int returnID = 0;
            errormsg = null;

            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@CustomerID", invoice.customer_id);
                queryParameters.Add("@InvoiceNumber", invoice.invoice_number);
                if ((invoice.description != null) && (invoice.description.Trim().Length>0))
                {
                    queryParameters.Add("@Description", invoice.description);
                }
                if (invoice.source_type_id != 0)
                {
                    queryParameters.Add("@SourceTypeID", invoice.source_type_id);
                }
                if (invoice.amount_due != null)
                {
                    queryParameters.Add("@AmountDue", invoice.amount_due);
                }
                if (invoice.company_currency_type_id != 0)
                {
                    queryParameters.Add("@CompanyCurrencyTypeID", invoice.company_currency_type_id);
                }
                if (invoice.company_amount != null)
                {
                    queryParameters.Add("@CompanyAmount", invoice.company_amount);
                }
                if (invoice.customer_currency_type_id != 0)
                {
                    queryParameters.Add("@CustomerCurrencyTypeID", invoice.customer_currency_type_id);
                }
                if (invoice.customer_amount != null)
                {
                    queryParameters.Add("@CustomerAmount", invoice.customer_amount);
                }
                if ((invoice.due_date != null) && (invoice.due_date != DateTime.MinValue))
                {
                    queryParameters.Add("@DueDate", (DateTime)invoice.due_date);
                }
                if ((invoice.expiration_date != null) && (invoice.expiration_date != DateTime.MinValue))
                {
                    queryParameters.Add("@ExpirationDate", (DateTime)invoice.expiration_date);
                }
                if (invoice.currency_calculation_id != 0)
                {
                    queryParameters.Add("@CurrencyCalculationID", invoice.currency_calculation_id);
                }
                if ((invoice.hedge_date != null) && (invoice.hedge_date != DateTime.MinValue))
                {
                    queryParameters.Add("@HedgeDate", (DateTime)invoice.hedge_date);
                }
                if ((invoice.close_date != null) && (invoice.close_date != DateTime.MinValue))
                {
                    queryParameters.Add("@CloseDate", (DateTime)invoice.close_date);
                }
                queryParameters.Add("@CreateUser", invoice.create_user);
                queryParameters.Add("@ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_InsertInvoice";
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

        public int UpdateInvoice(InvoiceUpdateItem invoice, out string errormsg)
        {
            int returnID = 0;
            errormsg = null;

            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", invoice.id);
                if ((invoice.description != null) && (invoice.description.Trim().Length > 0))
                {
                    queryParameters.Add("@Description", invoice.description);
                }
                if (invoice.source_type_id != 0)
                {
                    queryParameters.Add("@SourceTypeID", invoice.source_type_id);
                }
                if (invoice.amount_due != null)
                {
                    queryParameters.Add("@AmountDue", invoice.amount_due);
                }
                if (invoice.company_currency_type_id != 0)
                {
                    queryParameters.Add("@CompanyCurrencyTypeID", invoice.company_currency_type_id);
                }
                if (invoice.company_amount != null)
                {
                    queryParameters.Add("@CompanyAmount", invoice.company_amount);
                }
                if (invoice.customer_currency_type_id != 0)
                {
                    queryParameters.Add("@CustomerCurrencyTypeID", invoice.customer_currency_type_id);
                }
                if (invoice.customer_amount != null)
                {
                    queryParameters.Add("@CustomerAmount", invoice.customer_amount);
                }
                if ((invoice.due_date != null) && (invoice.due_date != DateTime.MinValue))
                {
                    queryParameters.Add("@DueDate", (DateTime)invoice.due_date);
                }
                if ((invoice.expiration_date != null) && (invoice.expiration_date != DateTime.MinValue))
                {
                    queryParameters.Add("@ExpirationDate", (DateTime)invoice.expiration_date);
                }
                if (invoice.currency_calculation_id != 0)
                {
                    queryParameters.Add("@CurrencyCalculationID", invoice.currency_calculation_id);
                }
                if ((invoice.hedge_date != null) && (invoice.hedge_date != DateTime.MinValue))
                {
                    queryParameters.Add("@HedgeDate", (DateTime)invoice.hedge_date);
                }
                if ((invoice.close_date != null) && (invoice.close_date != DateTime.MinValue))
                {
                    queryParameters.Add("@CloseDate", (DateTime)invoice.close_date);
                }

                if ((invoice.dispute_start_date != null) && (invoice.dispute_start_date != DateTime.MinValue))
                {
                    queryParameters.Add("@DisputeStartDate", (DateTime)invoice.dispute_start_date);
                }

                if ((invoice.dispute_end_date != null) && (invoice.dispute_end_date != DateTime.MinValue))
                {
                    queryParameters.Add("@DisputeEndDate", (DateTime)invoice.dispute_end_date);
                }


                queryParameters.Add("@Updateuser", invoice.update_user);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_UpdateInvoice";
                    connection.Execute(storedProcedure
                        , queryParameters
                        , null
                        , DAShared.GetCommandTimeOutInterval()
                        , CommandType.StoredProcedure);

                    returnID = invoice.id;
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.Message;
            }
            return returnID;
        }

        public IEnumerable<InvoiceListItem> GetInvoice(int invoiceID)
        {
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchInvoice";
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@InvoiceID", invoiceID);

                return connection.Query<InvoiceListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval() );
            }
        }
    }
}