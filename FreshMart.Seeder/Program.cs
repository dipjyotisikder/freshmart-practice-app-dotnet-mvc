using FreshMart.Seeder;
using System;
using System.Threading.Tasks;

namespace FreshMart.Init
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello from Seeder!");
            SeederBase seeder = new SeederBase();
            await seeder.Seed();
        }
    }
}
