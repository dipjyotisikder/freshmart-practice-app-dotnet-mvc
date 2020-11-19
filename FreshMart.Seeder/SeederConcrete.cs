using FreshMart.Data;
using FreshMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imgloo.seeder
{
    public class SeederConcrete
    {
        private readonly ApplicationDbContext _context;
        public SeederConcrete(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GoCat()
        {
            var categories = new List<Category>
            {
                new Category{Name="Good"},
                new Category{Name="Better"},
                new Category{Name="Best"}
            };
            var cats = _context.Categories.ToList();
            var selected = categories.Where(x => !cats.Select(y => y.Name).Contains(x.Name)).ToList();
            _context.AddRangeAsync(selected);
            var res = _context.SaveChangesAsync();
            if (res.Result > 0)
            {
                return "done!";
            }
            return "error!";
        }


    }
}
