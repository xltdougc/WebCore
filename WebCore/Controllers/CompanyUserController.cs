//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using DataAccess;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace WebCore.Controllers
//{
//    [Route("api/[controller]")]
//    public class CompanyUserController : Controller
//    {
//        [HttpGet]
//        public IEnumerable<CompanyUserListItem> Get(int companyID)
//        {
//            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

//            try
//            {

//                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
//                string json = "companyID: " + companyID.ToString();
//                EventLogDA.WriteEventLog(1, null, "CompanyUserController.Get " + callGuid + ", " + clientIP + " Data:" + json, 1);

//                CompanyInvoiceListDA cil = new CompanyInvoiceListDA();
//                return cil.GetCompanyInvoiceList(companyID, activeOnly);
//            }
//            catch (Exception ex)
//            {
//                ErrorLogDA.WriteErrorLog(1
//                                            , null
//                                            , 1
//                                            , ex.Message
//                                            , 1
//                                            , ex.InnerException?.Message
//                                            , ex.StackTrace
//                                            , ex.Source
//                                            , "CompanyInvoiceListController.Get " + callGuid
//                                            , 0
//                                            , 1);
//                return new List<CompanyInvoiceListItem>();
//            }
//        }
//    }
//}
