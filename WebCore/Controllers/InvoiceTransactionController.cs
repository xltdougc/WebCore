using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{

    public class InvoiceTransactionPostResponse
    {
        public bool success { get; set; }
        public string errormsg { get; set; }
        public int newid { get; set; }
    }

    [Route("api/[controller]")]
    public class InvoiceTransactionController : Controller
    {
        [HttpGet]
        public IEnumerable<InvoiceTransactionListItem> Get(int invoiceid)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = "invoiceid: " + invoiceid.ToString();
                EventLogDA.WriteEventLog(1, null, "InvoiceTransactionController.Get " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                InvoiceTransactionDA invoicetransaction = new InvoiceTransactionDA();
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
                                            , "InvoiceTransactionController.Get " + callGuid
                                            , 0
                                            , 1);
                return new List<InvoiceTransactionListItem>();
            }
        }

        [HttpPost]
        public InvoiceTransactionPostResponse Post([FromBody]InvoiceTransactionAddItem invoiceTransactionItem)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            int returnID = 0;
            InvoiceTransactionPostResponse response = new InvoiceTransactionPostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(invoiceTransactionItem);
                EventLogDA.WriteEventLog(1, null, "InvoiceTransactionController.Post " + callGuid + ", " + clientIP + " Data:" + json, 1);

                InvoiceTransactionDA itda = new InvoiceTransactionDA();
                returnID = itda.CreateInvoiceTransaction(invoiceTransactionItem, out string errormsg);
                response.newid = returnID;
                response.success = true;
                response.errormsg = errormsg;
                return response;
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
                                            , "InvoiceTransactionController.Post " + callGuid
                                            , 0
                                            , 1);
                response.newid = 0;
                response.success = false;
                response.errormsg = ex.Message;
                return response;
            }
        }

    }
}