using FreshMart.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshMart.Seeder.SeedData.Extension
{
    public static class SeederExtension
    {
        public static void AddOrUpdateRange<T>(this DbContext context, IEnumerable<T> enumerable) where T : BaseEntity
        {
            if (enumerable != null && enumerable.Count() > 0)
            {
                var repo = context.Set<T>();
                var allData = enumerable.ToList();

                allData.ForEach(e =>
                 {
                     var entity = repo.Where(x => x.Id == e.Id).FirstOrDefault();
                     if (entity == null)
                     {
                         // create new one
                         context.Add(e);
                     }
                     else
                     {
                         // update existing one
                         context.Entry(entity).CurrentValues.SetValues(e);
                     }
                 });
            }
        }
    }
}
