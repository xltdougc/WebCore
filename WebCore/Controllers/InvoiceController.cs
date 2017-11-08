using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{
    public class InvoicePostResponse
    {
        public bool success { get; set; }
        public string errormsg { get; set; }
        public int newid { get; set; }
    }

    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {
        [HttpPost]
        public InvoicePostResponse Post([FromBody]InvoiceAddItem invoice)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            int returnID = 0;
            InvoicePostResponse response = new InvoicePostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(invoice);
                EventLogDA.WriteEventLog(1, null, "InvoiceController.Post " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                InvoiceDA ida = new InvoiceDA();
                returnID = ida.CreateInvoice(invoice, out string errormsg);
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
                                            , "InvoiceController.Post " + callGuid
                                            , 0
                                            , 1);
                response.newid = 0;
                response.success = false;
                response.errormsg = ex.Message;
                return response;
            }
        }

        [HttpPut]
        public InvoicePostResponse Put([FromBody]InvoiceUpdateItem invoice)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            int returnID = 0;
            InvoicePostResponse response = new InvoicePostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(invoice);
                EventLogDA.WriteEventLog(1, null, "InvoiceController.Put " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                InvoiceDA ida = new InvoiceDA();
                returnID = ida.UpdateInvoice(invoice, out string errormsg);
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
                                            , "InvoiceController.Put " + callGuid
                                            , 0
                                            , 1);
                response.newid = 0;
                response.success = false;
                response.errormsg = ex.Message;
                return response;
            }
        }

        [HttpGet]
        public IEnumerable<InvoiceListItem> Get(int invoiceID)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = "invoiceid: " + invoiceID.ToString();
                EventLogDA.WriteEventLog(1, null, "InvoiceController.Get " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                InvoiceDA invoice = new InvoiceDA();
                return invoice.GetInvoice(invoiceID);
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
                                            , "InvoiceController.Get " + callGuid
                                            , 0
                                            , 1);
                return new List<InvoiceListItem>();
            }
        }

    }
}
