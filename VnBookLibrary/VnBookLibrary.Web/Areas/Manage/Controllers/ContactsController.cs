﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VnBookLibrary.Model.DAL;
using VnBookLibrary.Model.Entities;
using VnBookLibrary.Web.Areas.Manage.Customizes;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    public class ContactsController : Controller
    {
        private VnBookLibraryDbContext db = new VnBookLibraryDbContext();

        [HasRole(RoleCode = "VIEW_CONTACT")]
        public ActionResult Index()
        {
            var contacts = db.Contacts.Include(c => c.Customer).Include(c => c.Employee);
            return View(contacts.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
