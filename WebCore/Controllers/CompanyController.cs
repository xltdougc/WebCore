using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{
    public class CompanyPostResponse
    {
        public bool success { get; set; }
        public string errormsg { get; set; }
        public int newid { get; set; }
    }

    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        [HttpGet]
        public IEnumerable<CompanyListItem> Get()
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";
            try
            {
                string clientIP = ("(ClientIP:"+  HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown")+")";
                EventLogDA.WriteEventLog(1,null,"CompanyController.Get "+ callGuid + ", "+clientIP,1);

                CompanyDA company = new CompanyDA();
                return company.GetCompanyList();
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
                                            , "CompanyController.Get "+ callGuid
                                            , 0
                                            , 1);
                return new List<CompanyListItem>();
            }
        }

        [HttpPost]
        public CompanyPostResponse Post([FromBody]CompanyAddItem companyItem)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";
            CompanyPostResponse response = new CompanyPostResponse();
            response.success = true;
            response.errormsg = null;
            response.newid = 0;

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(companyItem);
                EventLogDA.WriteEventLog(1, null, "CompanyController.Post " + callGuid+", "+clientIP +" Data:"+ json, 1);
                CompanyDA ida = new CompanyDA();
                response.newid = ida.CreateCompany(companyItem.name, companyItem.add_user, out string errorMsg);
                if (response.newid == 0)
                {
                    response.errormsg = string.Format("Failed to add company [{0}], Exception={1}", companyItem.name, errorMsg);
                }
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
                                        , "CompanyController.Post " + callGuid
                                        , 0
                                        , 1);
                response.success = false;
                response.errormsg = string.Format("Failed to add company [{0}], Exception={1}", companyItem.name, ex.Message);
                return response;
            }
        }

        [HttpPut]
        public CompanyPostResponse Put([FromBody]CompanyUpdateItem companyItem)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";
            int returnID = 0;
            CompanyPostResponse response = new CompanyPostResponse();

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                string json = JsonConvert.SerializeObject(companyItem);
                EventLogDA.WriteEventLog(1, null, "CompanyController.Put " + callGuid + ", " + clientIP + " Data:" + json, 1);

                CompanyDA cuda = new CompanyDA();
                returnID = cuda.UpdateCompany(companyItem, out string errormsg);
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
                                            , "CompanyController.Put " + callGuid
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