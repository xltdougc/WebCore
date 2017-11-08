using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebCore.Library;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace WebCore.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet("{echo}")]
        public string Get(string echo)
        {
            return echo;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return id.ToString();
        }

        // GET api/values/5
        [HttpGet]
        public string Get()
        {
            var json = "";
            Boolean isDebugMode = true;
            string currentMode = string.Empty;
            if (isDebugMode)
            {
                currentMode = "DEBUG";
            }
            else
            {
                currentMode = "RELEASE";
            }

            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            decimal amount = 123.45m;

            System.Collections.Generic.List<PingResult> results = new List<PingResult>();

            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            results.Add(new PingResult(isDebugMode, currentMode, Environment.MachineName, DateTime.Now, amount, remoteIpAddress.ToString()));
            results.Add(new PingResult(false, "RELEASE", Environment.MachineName, DateTime.Now, 7890.27m, remoteIpAddress.ToString()));

            if (results.Any())
            {
                foreach (var PingResult in results)
                {
                    json += JsonConvert.SerializeObject(PingResult);
                }
                json += "{status:1}";
            }
            else
            {
                json += "{status:0}";
            }
            return json;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]dug dvalue)
        {
            return string.Format("{0}, {1}, {2}, {3}",
                                        dvalue.id,
                                        dvalue.msg,
                                        dvalue.amt,
                                        System.DateTime.Now.ToString());
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //[HttpGet]
        //public string Ping2()
        //{
        //    var json = "";

        //    Boolean isDebugMode = true;
        //    string currentMode = string.Empty;
        //    if (isDebugMode)
        //    {
        //        currentMode = "DEBUG";
        //    }
        //    else
        //    {
        //        currentMode = "RELEASE";
        //    }

        //    String strHostName = string.Empty;
        //    strHostName = Dns.GetHostName();
        //    decimal amount = 123.45m;

        //    System.Collections.Generic.List<PingResult> results = new List<PingResult>();

        //    results.Add(new PingResult(isDebugMode, currentMode, Environment.MachineName, DateTime.Now, amount));

        //    if (results.Any())
        //    {
        //        foreach (var PingResult in results)
        //        {
        //            json += JsonConvert.SerializeObject(PingResult);
        //        }
        //        json += "{status:1}";
        //    }
        //    else
        //    {
        //        json += "{status:0}";
        //    }
        //    return json;
        //}

        //// GET api/values
        //[HttpGet]
        //public string Ping()
        //{
        //    ///https://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome
        //    Boolean isDebugMode = true;

        //    string currentMode = string.Empty;
        //    if (isDebugMode)
        //    {
        //        currentMode = "DEBUG";
        //    }
        //    else
        //    {
        //        currentMode = "RELEASE";
        //    }

        //    String strHostName = string.Empty;
        //    strHostName = Dns.GetHostName();
        //    decimal amount = 123.45m;

        //    System.Collections.Generic.List<PingResult> results = new List<PingResult>();

        //    results.Add(new PingResult(isDebugMode, currentMode, Environment.MachineName, DateTime.Now, amount));

        //    string json = results.ToString();
        //    return json;
        //}

    }

    public class dug
    {
        public int id { get; set; }
        public string msg { get; set; }
        public decimal amt { get; set; }
    }
}
