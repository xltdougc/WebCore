using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccess;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{

    public class InvoiceTransactionDisputePostResponse
    {
        public bool success { get; set; }
        public string errormsg { get; set; }
        public int newid { get; set; }
    }

    [Route("api/[controller]")]
    public class InvoiceTransactionDisputeController : Controller
    {
        [HttpGet]
        public IEnumerable<InvoiceTransactionDisputeListItem> Get(int invoiceid)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = "invoiceid: " + invoiceid.ToString();
                EventLogDA.WriteEventLog(1, null, "InvoiceTransactionDisputeController.Get " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                InvoiceTransactionDisputeDA invoicetransaction = new InvoiceTransactionDisputeDA();
                return invoicetransaction.GetCustomerInvoiceList(invoiceid);
            }
            catch (Exception ex)
            {
                ErrorLogDA.WriteErrorLog(1
                                        , null
                                        , 1
                                        , ex.Message
                                        , 1
                                        , ex.InnerException?.Message
                                        , ex.StackTrace
                                        , ex.Source
                                        , "InvoiceTransactionDisputeController.Get " + callGuid
                                        , 0
                                        , 1);
                return new List<InvoiceTransactionDisputeListItem>();
            }
        }
    }
}