using FreshMart.Database;
using FreshMart.Models;
using FreshMart.Seeder.SeedData.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMart.Seeder.SeedData.Extension;

namespace Imgloo.seeder
{
    public class SeederConcrete
    {
        private readonly AppDbContext _context;
        public SeederConcrete(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> SeedCategory()
        {
            var divisionPath = GetFilePath<CategoriesSeedModel>("json");

            var categories = JsonConvert.DeserializeObject<List<CategoriesSeedModel>>(File.ReadAllText(divisionPath))
                .Select(x => new Category
                {
                    Id = x.Id,
                    Name = x.Name
                });

            _context.AddOrUpdateRange<Category>(categories);

            try
            {
                var res = await _context.SaveChangesAsync();
                if (res >= 0)
                {
                    return "done!";
                }
            }
            catch (Exception e)
            {
                throw;
            }


            return "error!";
        }


        public async Task<string> SeedDistricts()
        {
            var divisionPath = GetFilePath<DivisionsSeedModel>("json");
            var divList = JsonConvert.DeserializeObject<List<DivisionsSeedModel>>(File.ReadAllText(divisionPath))
                .Select(x => new Division
                {
                    Id = x.Id,
                    Name = x.Name
                });
            _context.AddOrUpdateRange<Division>(divList);
            var divRes = await _context.SaveChangesAsync();


            var districtPath = GetFilePath<DistrictsSeedModel>("json");
            var objList = JsonConvert.DeserializeObject<List<DistrictsSeedModel>>(File.ReadAllText(districtPath))
                .Select(x => new District
                {
                    Id = x.Id,
                    Name = x.Name,
                    DivisionId = x.DivisionId
                });
            _context.AddOrUpdateRange<District>(objList);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                return "done!";
            }
            return "error!";
        }



        public async Task<string> SeedProduct()
        {

            var districtPath = GetFilePath<DistrictsSeedModel>("json");
            var objList = JsonConvert.DeserializeObject<List<DistrictsSeedModel>>(File.ReadAllText(districtPath))
                .Select(x => new District
                {
                    Id = x.Id,
                    Name = x.Name,
                    DivisionId = x.DivisionId
                });
            await _context.Districts.AddRangeAsync(objList);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                return "done!";
            }
            return "error!";
        }


        string GetFilePath<T>(string fileExtension)
        {
            var root = @"C:\Users\dip\source\repos\freshmart\FreshMart.Seeder\SeedData\Data\";
            return root + $"{ typeof(T).Name}.{ fileExtension}";
        }

    }

}
