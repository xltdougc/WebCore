using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess
{
    public class InvoiceAttachmentAddItem
    {
        public int invoice_id { get; set; }
        public string path { get; set; }
        public string file_name { get; set; }
        public int create_user { get; set; }
    }

    public class InvoiceAttachmentListItem
    {
        public int id { get; set; }
        public int invoice_id { get; set; }
        public string path { get; set; }
        public string file_name { get; set; }
        public int create_user { get; set; }
        public DateTime? create_date { get; set; }
        public int? update_user { get; set; }
        public DateTime? update_date { get; set; }
    }
    public class InvoiceAttachmentDA
    {
        public IEnumerable<InvoiceAttachmentListItem> GetInvoiceAttachmentList(int invoiceID)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@InvoiceID", invoiceID);
            using (IDbConnection connection = DAShared.OpenConnection())
            {
                const string storedProcedure = "dbo.up_FetchInvoiceAttachments";
                return connection.Query<InvoiceAttachmentListItem>
                    (sql: storedProcedure
                    , param: queryParameters
                    , transaction: null
                    , commandType: CommandType.StoredProcedure
                    , commandTimeout: DAShared.GetCommandTimeOutInterval());
            }
        }

        public int CreateInvoiceAttachment(InvoiceAttachmentAddItem iti, out string errormsg)
        {
            int returnID = 0;
            errormsg = null;

            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@InvoiceID", iti.invoice_id);
                queryParameters.Add("@Path", iti.path);
                queryParameters.Add("@FileName", iti.file_name);
                queryParameters.Add("@CreateUser", iti.create_user);
                queryParameters.Add("@ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (IDbConnection connection = DAShared.OpenConnection())
                {
                    const string storedProcedure = "dbo.up_InsertInvoiceAttachment";
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
