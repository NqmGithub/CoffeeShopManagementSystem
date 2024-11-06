using CoffeeShopManagement.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.Helpers
{
    public class ProductHelper
    {
        internal static string ConvertToStatusString(int status)
        {
            switch (status)
            {
                case 0:
                    return "InActive";
                case 1:
                    return "Active";
            }
            return "Unknown";
        }

        internal static int ConvertToStatusInt(string status)
        {
            if (status.Equals("Active"))
            {
                return 1;
            }
            else if (status.Equals("InActive"))
            {
                return 0;
            }
            return -1;
        }
    }
}