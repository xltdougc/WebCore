using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{

    public class InvoiceAttachmentPostResponse
    {
        public bool success { get; set; }
        public string errormsg { get; set; }
        public int newid { get; set; }
    }

    [Route("api/[controller]")]
    public class InvoiceAttachmentController : Controller
    {
        [HttpGet]
        public IEnumerable<InvoiceAttachmentListItem> Get(int invoiceid)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = "invoiceid: " + invoiceid.ToString();
                EventLogDA.WriteEventLog(1, null, "InvoiceAttachmentController.Get " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                InvoiceAttachmentDA invoiceAttachment = new InvoiceAttachmentDA();
                return invoiceAttachment.GetInvoiceAttachmentList(invoiceid);
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
                                            , "InvoiceAttachmentController.Get " + callGuid
                                            , 0
                                            , 1);
                return new List<InvoiceAttachmentListItem>();
            }
        }

        [HttpPost]
        public InvoiceAttachmentPostResponse Post([FromBody]InvoiceAttachmentAddItem invoiceAttachmentItem)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            int returnID = 0;
            InvoiceAttachmentPostResponse response = new InvoiceAttachmentPostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(invoiceAttachmentItem);
                EventLogDA.WriteEventLog(1, null, "InvoiceAttachmentController.Post " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                InvoiceAttachmentDA itda = new InvoiceAttachmentDA();
                returnID = itda.CreateInvoiceAttachment(invoiceAttachmentItem, out string errormsg);
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
                                        , "InvoiceAttachmentController.Post " + callGuid
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