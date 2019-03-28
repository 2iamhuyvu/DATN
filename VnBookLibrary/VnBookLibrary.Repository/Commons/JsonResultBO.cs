using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnBookLibrary.Repository.Commons
{
    public class JsonResultBO
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public int TypeReload { get; set; }
        public object Param { get; set; }
        public JsonResultBO()
        {
        }
        public JsonResultBO(bool st)
        {
            Status = st;
        }
        public void MessageFail(string mss)
        {
            Status = false;
            Message = mss;
        }
    }
}
