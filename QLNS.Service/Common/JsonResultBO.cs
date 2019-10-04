using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.Common
{
    public class JsonResultBO
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Param { get; set; }
        public JsonResultBO(bool st)
        {
            Status = st;
        }
        public JsonResultBO(bool st, string message)
        {
            Status = st;
            Message = message;
        }
        public void MessageFail(string mss)
        {
            Status = false;
            Message = mss;
        }
    }
}
