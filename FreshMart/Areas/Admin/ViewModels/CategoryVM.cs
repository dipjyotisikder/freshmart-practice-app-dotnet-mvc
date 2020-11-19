using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Models;

namespace FreshMart.Areas.Admin.ViewModels
{
    public class CategoryViewModel

    {
        public CategoryDomain CategoryDomain { get; set; }
        public IEnumerable<CategoryDomain> CategoryDomains { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public Category Category { get; set; }

    }


}
