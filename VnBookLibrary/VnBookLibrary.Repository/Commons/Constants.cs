using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnBookLibrary.Repository.Commons
{
    public  static class Constants
    {
        public static String MANAGE_SESSION = "MANAGE_SESSION";        
        public static String CUSTOMER_SESSION = "CUSTOMER_SESSION";
        public static String CART_SESSION = "CART_SESSION";
        public static int STATTUS_BILL_NEW = 1;
        public static int STATTUS_BILL_DELIVERING = 2;
        public static int STATTUS_BILL_PAID = 3;
        public static int STATTUS_BILL_RETURNED = 4;
    }
}
