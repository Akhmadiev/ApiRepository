namespace ApiConsole
{
    using MainApi;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var main = new MainApiClass();
            main.Start();

            try
            {
                while (true)
                {
                    //Console.WriteLine($"Start date: {DateTime.Now}");

                    //var data = main.GetAll().ToList();
                    //var json = JsonConvert.SerializeObject(data);
                    //Console.WriteLine(json);

                    //Console.WriteLine();
                    //Console.WriteLine();

                    //Thread.Sleep(2000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
            
        }
    }
}