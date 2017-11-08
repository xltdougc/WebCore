
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{
    [Route("api/[controller]")]
    public class CustomerAccountController : Controller
    {
        public class CustomerAccountPostResponse
        {
            public bool success { get; set; }
            public string errormsg { get; set; }
            public int newid { get; set; }
        }

        [HttpPost]
        public CustomerAccountPostResponse Post([FromBody]CustomerAccountAddItem customerAccountItem)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            int returnID = 0;
            CustomerAccountPostResponse response = new CustomerAccountPostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(customerAccountItem);
                EventLogDA.WriteEventLog(1, null, "CustomerAccountController.Post " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                CustomerAccountDA cuda = new CustomerAccountDA();
                returnID = cuda.CreateCustomerAccount(customerAccountItem, out string errormsg);
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
                                            , "CustomerAccountController.Post " + callGuid
                                            , 0
                                            , 1);
                response.newid = 0;
                response.success = false;
                response.errormsg = ex.Message;
                return response;
            }
        }

        [HttpPut]
        public CustomerAccountPostResponse Put([FromBody]CustomerAccountUpdateItem customerAccountItem)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            int returnID = 0;
            CustomerAccountPostResponse response = new CustomerAccountPostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(customerAccountItem);
                EventLogDA.WriteEventLog(1, null, "CustomerAccountController.Put " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                CustomerAccountDA cuda = new CustomerAccountDA();
                returnID = cuda.UpdateCustomerAccount(customerAccountItem, out string errormsg);
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
                                            , "CustomerAccountController.Put " + callGuid
                                            , 0
                                            , 1);
                response.newid = 0;
                response.success = false;
                response.errormsg = ex.Message;
                return response;
            }
        }

        [HttpGet]
        public IEnumerable<CustomerAccountListItem> Get(int customer_id)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = "customer_id: " + customer_id.ToString();
                EventLogDA.WriteEventLog(1, null, "CustomerAccountController.Get " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                CustomerAccountDA customerAccount = new CustomerAccountDA();
                return customerAccount.GetCustomerAccountList(customer_id);
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
                                            , "CustomerAccountController.Get " + callGuid
                                            , 0
                                            , 1);
                return new List<CustomerAccountListItem>();
            }
        }
    }
}