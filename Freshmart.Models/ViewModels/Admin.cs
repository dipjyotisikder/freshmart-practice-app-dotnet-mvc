using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Models.ViewModels
{
    public class Admin
    {
        public string Name { get; set; }
        public string Password { get; set; }


        public void SetValue(Admin admin)
        {
            Name = admin.Name;
            Password = admin.Password;

        }
    }
}
