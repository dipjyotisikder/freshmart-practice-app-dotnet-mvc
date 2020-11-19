using System;
using System.Collections.Generic;
using System.Text;
using Imgloo.seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using FreshMart.Data;

namespace Imgloo.Seeder
{
    public class SeederBase
    {
        private string defaultConnection = "";
        private DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();


        public void Seed()
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
                               //.AddEnvironmentVariables()
                               .Build();

            defaultConnection = configuration.GetConnectionString("DefaultConnection");

            //if (env == "1")
            //{
            //    defaultConnection = Constants.CONNECTION_STRING.LocalValue;
            //}
            //else if (env == "2")
            //{
            //    defaultConnection = Constants.CONNECTION_STRING.ProductionValue;
            //}



            builder.UseSqlServer(defaultConnection);

            using (var context = new ApplicationDbContext(builder.Options))
            {
                Console.WriteLine("Choose from below:");
                Console.WriteLine("1. Category");
                Console.WriteLine("00. Seed All");
                //Console.WriteLine("2. Tag");
                //Console.WriteLine("3. DocumentType");
                //Console.WriteLine("4. Privacy");
                //Console.WriteLine("5. Genders");

                var command = Console.ReadLine();
                var startSeed = new SeederConcrete(context);

                Console.WriteLine("You choosed: " + command);

                if (command == "1")
                {
                    var res = startSeed.GoCat();
                    Console.WriteLine(res);
                }
                else if (command == "00")
                {
                    //add all seeder here
                    startSeed.GoCat();

                }
                //else if (command == "3")
                //{
                //    var res = startSeed.SeedDocumentType();
                //    Console.WriteLine(res);
                //}
                //else if (command == "4")
                //{
                //    var res = startSeed.SeedPrivacy();
                //    Console.WriteLine(res);
                //}
                //else if (command == "5")
                //{
                //    var res = startSeed.SeedGenders();
                //    Console.WriteLine(res);
                //}

                Console.WriteLine("Press 1 for again: ");
                var again = Console.ReadLine();

                if (again == "1")
                {
                    Console.Clear();
                    Seed();
                }
            }
        }
    }
}
