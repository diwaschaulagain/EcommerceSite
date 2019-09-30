using MVCLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCLogin
{
    public class MyMenu
    {
        public static List<tblCategory> GetMenus()
        {
            using (var context = new BhaiDBEntities())
            {
                return context.tblCategories.ToList();
            }
        }

    }
}