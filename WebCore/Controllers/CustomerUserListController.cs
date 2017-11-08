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
    public class CustomerUserListController : Controller
    {
        [HttpGet]
        public IEnumerable<CustomerUserListItem> Get(int customerID)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = "customer_id: " + customerID.ToString() ;
                EventLogDA.WriteEventLog(1, null, "CustomerUserListController.Get " + callGuid + ", " + clientIP + " Data:" + json, 1);

                CustomerUserListDA cil = new CustomerUserListDA();
                return cil.GetCustomerUser(customerID);
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
                                            , "CustomerUserListController.Get " + callGuid
                                            , 0
                                            , 1);
                return new List<CustomerUserListItem>();
            }
        }
    }
}
