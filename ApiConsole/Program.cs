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
        }
    }
}