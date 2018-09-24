namespace ApiConsole
{
    using ApiAdditional;
    using MainApi;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generate report - 1");
            Console.WriteLine("Get report - 2");
            var input = Console.ReadLine();

            if (input == "1")
            {

            }
            else if (input == "2")
            {
                Console.WriteLine("Write ID:");
                var id = Convert.ToInt32(Console.ReadLine());
            }

            var main = new MainApiClass();
            main.Start();
        }
    }
}