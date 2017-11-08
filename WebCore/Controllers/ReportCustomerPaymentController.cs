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
    [Route("api/ReportCustomerPayment")]
    public class ReportCustomerPaymentController : Controller
    {
        [HttpGet]
        public ReportCustomerPayment Get(int customerID
                                                        , DateTime startdate
                                                        , DateTime enddate)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = "customerID: " + customerID + ", startdate: " + startdate.ToString() + ", enddate: " + enddate.ToString();
                EventLogDA.WriteEventLog(1, null, "ReportCustomerPaymentController.Get " + callGuid + ", "+ clientIP + " Data:" + json, 1);


                ReportCustomerPaymentDA rcpda = new ReportCustomerPaymentDA();
                return rcpda.GetCustomerPaymentList(customerID, startdate, enddate);
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
                        , "ReportCustomerPaymentController.Get " + callGuid
                        , 0
                        , 1);

                return new ReportCustomerPayment();
            }
        }
    }
}