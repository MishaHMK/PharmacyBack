using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pharmacy
{
    public class Roles
    {
        public static string Admin = "Admin";
        public static string Customer = "Customer";
        public static string Pharmacist = "Pharmacist";

        public static List<SelectListItem> GetRolesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = Roles.Admin, Text = Roles.Admin },
                new SelectListItem { Value = Roles.Customer, Text = Roles.Customer },
                new SelectListItem { Value = Roles.Pharmacist, Text = Roles.Pharmacist }
            };
        }
    }
}