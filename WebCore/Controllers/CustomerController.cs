using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        public class CustomerPostResponse
        {
            public bool success { get; set; }
            public string errormsg { get; set; }
            public int newid { get; set; }
        }

        [HttpPost]
        public CustomerPostResponse Post([FromBody]CustomerAddItem customerItem)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            int returnID = 0;
            CustomerPostResponse response = new CustomerPostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(customerItem);
                EventLogDA.WriteEventLog(1, null, "CustomerController.Post " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                CustomerDA cuda = new CustomerDA();
                returnID = cuda.CreateCustomer(customerItem, out string errormsg);
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
                                        , "CustomerController.Post " + callGuid
                                        , 0
                                        , 1);
                response.newid = 0;
                response.success = false;
                response.errormsg = ex.Message;
                return response;
            }
        }

        [HttpPut]
        public CustomerPostResponse Put([FromBody]CustomerUpdateItem customerItem)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            int returnID = 0;
            CustomerPostResponse response = new CustomerPostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(customerItem);
                EventLogDA.WriteEventLog(1, null, "CustomerController.Put " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                CustomerDA cuda = new CustomerDA();
                returnID = cuda.UpdateCustomer(customerItem, out string errormsg);
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
                                            , "CustomerController.Put " + callGuid
                                            , 0
                                            , 1);
                response.newid = 0;
                response.success = false;
                response.errormsg = ex.Message;
                return response;
            }
        }

        [HttpGet]
        public IEnumerable<CustomerListItem> Get()
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                EventLogDA.WriteEventLog(1, null, "CustomerController.Get " + callGuid + ", "+ clientIP,1);

                CustomerDA customer = new CustomerDA();
                return customer.GetCustomerList();
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
                                            , "CustomerController.Get " + callGuid
                                            , 0
                                            , 1);
                return new List<CustomerListItem>();
            }
        }


    }
}