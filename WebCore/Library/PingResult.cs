using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Library
{
    public class PingResult
    {
        public bool IsDebug { get; set; }
        public string Mode { get; set; }
        public string Machine { get; set; }
        public DateTime ServerDateTime { get; set; }
        public decimal Amount { get; set; }
        public string IPAddress { get; set; }

        public PingResult(bool isDebug, string mode, string machine, DateTime serverDateTime, decimal amount, string ipAddress)
        {
            this.IsDebug = isDebug;
            this.Mode = mode;
            this.Machine = machine;
            this.ServerDateTime = serverDateTime;
            this.Amount = amount;
            this.IPAddress = ipAddress;
        }
    }
}