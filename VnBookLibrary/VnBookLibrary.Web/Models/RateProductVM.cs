using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VnBookLibrary.Web.Models
{
    public class RateProductVM
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Point { get; set; }
    }
}