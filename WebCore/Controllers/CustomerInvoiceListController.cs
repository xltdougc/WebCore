using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{
    [Route("api/[controller]")]
    public class CustomerInvoiceListController : Controller
    {
        [HttpGet] 
        public IEnumerable<CustomerInvoiceList> Get(int customerID
                                                        , bool? unpaidonly
                                                        , DateTime? startdate
                                                        , DateTime? enddate
                                                        , string invoiceNumber)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = "customer_id: " + customerID.ToString()+", unpaidonly:"+unpaidonly.ToString()+", startdate: "+startdate.ToString()+", enddate: "+enddate.ToString()+", invoice_number: "+invoiceNumber;
                EventLogDA.WriteEventLog(1, null, "CustomerInvoiceListController.Get " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                CustomerInvoiceListDA cil = new CustomerInvoiceListDA();
                return cil.GetCustomerInvoiceList(customerID
                                            , unpaidonly
                                            , startdate
                                            , enddate
                                            , invoiceNumber);
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
                                            , "CustomerInvoiceListController.Get " + callGuid
                                            , 0
                                            , 1);
                return new List<CustomerInvoiceList>();
            }
        }
    }
}
