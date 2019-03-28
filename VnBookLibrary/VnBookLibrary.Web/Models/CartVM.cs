using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VnBookLibrary.Model.Entities;

namespace VnBookLibrary.Web.Models
{
    public class CartVM
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}