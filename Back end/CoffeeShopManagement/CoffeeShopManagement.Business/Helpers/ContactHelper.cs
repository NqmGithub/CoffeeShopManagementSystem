using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.Helpers
{
    public class ContactHelper
    {
        internal static string ConvertToStatusString(int status)
        {
            switch (status)
            {
                case 0:
                    return "Pending";
                case 1:
                    return "In Processing";
                case 2:
                    return "Done";
            }
            return "Unknown";
        }

        internal static int ConvertToStatusInt(string status)
        {
            if (status.Equals("Pending"))
            {
                return 0;
            }
            else if (status.Equals("In Processing"))
            {
                return 1;
            } else if (status.Equals("Done"))
            {
                return 2;
            }
            return -1;
        }
    }
}
