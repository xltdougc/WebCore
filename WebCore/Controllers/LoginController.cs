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
    public class LoginController : Controller
    {

        // GET api/login/5
        [HttpGet("{username}/{password}")]
        public LoginResponse Get(string username, string password)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
            string json = "username: " + username + " Login Attempt";
            EventLogDA.WriteEventLog(1, null, "LoginController.Get " + callGuid + ", "+ clientIP + " Data:" + json, 1);

            LoginResponse lr = new LoginResponse();
            Guid returnGuid = new Guid();
            lr.roleList = new List<IDNamePair>();
            lr.companyList = new List<IDNamePair>();
            lr.customerList = new List<IDNamePair>();

            try
            {
                lr.success = true;
                LoginDA loginAttempt = new DataAccess.LoginDA();

                int userID = loginAttempt.GetUserID(username);
                if (userID == 0)
                {
                    lr.errormsg = "Invalid Login Request";
                    lr.userSessionGuid = returnGuid;
                }
                else 
                {
                    returnGuid = loginAttempt.Login(userID, username, password);
                    lr.userSessionGuid = returnGuid;

                    var rolelist = loginAttempt.GetUserRoleList(userID);
                    foreach (Role role in rolelist)
                    {
                        lr.roleList.Add(new IDNamePair(role.role_id, role.role_name));
                    }

                    var companyList = loginAttempt.GetUserCompanyList(userID);
                    foreach (Company company in companyList)
                    {
                        lr.companyList.Add(new IDNamePair(company.company_id, company.company_name));
                    }

                    var customerList = loginAttempt.GetUserCustomerList(userID);
                    foreach (Customer customer in customerList)
                    {
                        lr.customerList.Add(new IDNamePair(customer.customer_id, customer.customer_name));
                    }
                }
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
                        , "LoginController.Get " + callGuid
                        , 0
                        , 1);

                lr.success = false;
                lr.errormsg = "An exception occurred, " + ex.Message;
            }
            return lr;
        }
    }

    public class LoginResponse
    {
        public bool success { get; set; }
        public string errormsg { get; set; }
        public Guid userSessionGuid { get; set; }
        public List<IDNamePair> roleList { get; set; }
        public List<IDNamePair> companyList { get; set; }
        public List<IDNamePair> customerList { get; set; }
    }

    public class IDNamePair
    {
        public int id { get; set; }
        public string name { get; set; }

        public IDNamePair(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
