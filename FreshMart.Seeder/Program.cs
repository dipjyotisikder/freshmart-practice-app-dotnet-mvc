using Imgloo.Seeder;
using System;

namespace Imgloo.Init
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello from Seeder!");
            SeederBase seeder = new SeederBase();
            seeder.Seed();
        }
    }
}
