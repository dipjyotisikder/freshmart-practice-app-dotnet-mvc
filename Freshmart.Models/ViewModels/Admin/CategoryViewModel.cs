using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Models;

namespace FreshMart.ViewModels
{
    public class CategoryViewModel

    {
        public IEnumerable<Category> Categories { get; set; }
        public Category Category { get; set; }
        public Category Parent { get; set; }
    }


}
