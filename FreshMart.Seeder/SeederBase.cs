using System;
using System.Collections.Generic;
using System.Text;
using FreshMart.seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using FreshMart.Database;
using System.Threading.Tasks;

namespace FreshMart.Seeder
{
    public class SeederBase
    {
        private string defaultConnection = "";
        private DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>();


        public async Task Seed()
        {
            Console.WriteLine("Choose environment:");
            Console.WriteLine("1. Local environment:");
            Console.WriteLine("2. Production environment:");

            var envName = "dev";

            var env = Int64.Parse(Console.ReadLine());
            if (env == 2)
            {
                envName = "prod";
            }


            var configuration = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                               .AddJsonFile($"appsettings.{envName}.json", optional: true)
                               .AddEnvironmentVariables()
                               .Build();


            defaultConnection = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(defaultConnection);


            using (var context = new AppDbContext(builder.Options))
            {
                Console.WriteLine("Choose from below:");
                Console.WriteLine("1. Category");
                Console.WriteLine("2. Seed Districts");
                Console.WriteLine("00. Seed All");


                var command = Console.ReadLine();
                var startSeed = new SeederConcrete(context);

                Console.WriteLine("You choosed: " + command);

                if (command == "1")
                {
                    var res = await startSeed.SeedCategory();
                    Console.WriteLine(res);
                }
                else if (command == "2")
                {
                    var res = await startSeed.SeedDistricts();
                    Console.WriteLine(res);
                }
                else if (command == "00")
                {
                    //add all seeder here
                    await startSeed.SeedCategory();
                    await startSeed.SeedDistricts();
                }

                Console.WriteLine("Press 1 for again: ");
                var again = Console.ReadLine();
                if (again == "1")
                {
                    Console.Clear();
                    await Seed();
                }
            }
        }
    }
}
