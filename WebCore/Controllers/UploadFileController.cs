using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using System.IO;
using DataAccess;

namespace WebCore.Controllers
{
    [Produces("application/json")]
    [Route("api/UploadFile")]
    public class UploadFileController : Controller
    {
        private IHostingEnvironment hostingEnv;

        public UploadFileController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        [HttpPost]
        public string UploadFile(int invoiceID, int userid, IList<IFormFile> files)
        {
            string callGuid = "(" + Guid.NewGuid().ToString() + ")";

            string returnMessage = string.Empty;

            try
            {
                string clientIP = ("(ClientIP:" + HttpContext.Connection.RemoteIpAddress.ToString() ?? "Unknown") + ")";
                EventLogDA.WriteEventLog(1, null, "UploadFileController.UploadFile " + callGuid + ", " + clientIP, 1);

                long size = 0;
                foreach (var file in files)
                {
                    var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                    string timeStamp = string.Format("{0:yyyyMMdd_HHmmssfff}", DateTime.Now);
                     filename = timeStamp + "_" + filename;

                    string workingfolder = hostingEnv.WebRootPath;
                    ///string workingfolder = hostingEnv.WebRootPath + "\\Uploads";
                    ///string workingfolder = "c:\\PayRecs\\FileUploads";
                    if (!System.IO.Directory.Exists(workingfolder))
                    {
                        System.IO.Directory.CreateDirectory(workingfolder);
                    }

                    string server_filename = System.IO.Path.Combine(workingfolder, filename);
                    size += file.Length;

/*
                    using (FileStream fs = System.IO.File.Create(server_filename))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
*/

                    InvoiceAttachmentDA iada = new InvoiceAttachmentDA();
                    InvoiceAttachmentAddItem iaai = new InvoiceAttachmentAddItem();
                    iaai.file_name = filename;
                    iaai.create_user = userid;
                    iaai.path = System.Environment.MachineName.ToString() + ":" + workingfolder;
                    iaai.invoice_id = invoiceID;
                    int newid = iada.CreateInvoiceAttachment(iaai, out string errormsg);
                    returnMessage = returnMessage + $"{files.Count} file(s) {size} bytes uploaded successfully!";
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
                            , "UploadFileController.UploadFile " + callGuid
                            , 0
                            , 1);

            }
            return returnMessage;
        }
    }
}
