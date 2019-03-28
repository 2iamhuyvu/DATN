using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VnBookLibrary.Model.Entities;

namespace VnBookLibrary.Web.Areas.Manage.Models
{
    public class ManageSessionModel
    {
        public Employee SessionEmployee { get; set; }
        public List<string> RoleCodes { get; set; }         
    }
}