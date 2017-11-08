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
    public class CurrencyCalculationPostResponse
    {
        public bool success { get; set; }
        public string errormsg { get; set; }
        public int newid { get; set; }
    }

    [Route("api/[controller]")]
    public class CurrencyCalculationController : Controller
    {
        [HttpPost]
        public CurrencyCalculationPostResponse Post([FromBody]CurrencyCalculationAddItem currencyCalculationItem)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            int returnID = 0;
            CurrencyCalculationPostResponse response = new CurrencyCalculationPostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(currencyCalculationItem);
                EventLogDA.WriteEventLog(1, null, "CurrencyCalculationController.Post " + callGuid + ", "+ clientIP + " Data:" + json, 1);

                CurrencyCalculationDA ccda = new CurrencyCalculationDA();
                returnID = ccda.CreateCurrencyCalculation(currencyCalculationItem, out string errormsg);
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
                                        , "CurrencyCalculationController.Post " + callGuid
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
