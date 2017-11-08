using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess;

namespace WebCore.Controllers
{
    [Produces("application/json")]
    [Route("api/ReportInvoiceAging")]
    public class ReportInvoiceAgingController : Controller
    {
        [HttpGet]
        public ReportInvoiceAging Get(int companyID)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = "companyID: " + companyID;
                EventLogDA.WriteEventLog(1, null, "ReportInvoiceAgingController.Get " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                ReportInvoiceAgingDA riada = new ReportInvoiceAgingDA();
                return riada.GetInvoiceAgingList(companyID);
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
                                        , "ReportInvoiceAgingController.Get " + callGuid
                                        , 0
                                        , 1);
                return new ReportInvoiceAging();
            }
        }
    }
}