using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VnBookLibrary.Model.Entities;
using VnBookLibrary.Repository.Commons;
using VnBookLibrary.Web.Areas.Manage.Models;

namespace VnBookLibrary.Web.Areas.Manage.Customizes
{
    public class ManageSession
    {        
        public static void SetSession(string Key, object Value)
        {
            HttpContext context = HttpContext.Current;
            context.Session[Key] = Value;
        }

        public static object GetSession(string Key)
        {
            HttpContext context = HttpContext.Current;
            return context.Session[Key];
        }

        public static void RemoveSession(string Key)
        {
            HttpContext context = HttpContext.Current;
            context.Session.Remove(Key);
        }

        public static void ClearSession()
        {
            HttpContext context = HttpContext.Current;
            context.Session.RemoveAll();
        }

        public static bool HasSession(string Key)
        {
            HttpContext context = HttpContext.Current;
            return context.Session[Key] != null;
        }        
        public static List<string> GetListRole()
        {
            HttpContext context = HttpContext.Current;
            return ((ManageSessionModel)context.Session[Constants.MANAGE_SESSION]).RoleCodes;
        }
        public static bool HasRole(string roleCode)
        {            
            HttpContext context = HttpContext.Current;            
            var session = context.Session[Constants.MANAGE_SESSION];
            if (session == null) return false;            
            var listRole = ((ManageSessionModel)session).RoleCodes;
            if (listRole == null)
            {
                return false;
            }            
            if (listRole.Contains(roleCode))
            {
                return true;
            }            
            return false;
        }        
    }
}