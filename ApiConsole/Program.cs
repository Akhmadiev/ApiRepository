namespace ApiConsole
{
    using ApiAdditional;
    using MainApi;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var plugins = GetPlugins();
            var data = plugins[0].Do<Country>();

            Start(plugins);

            while (true) { }
        }

        private static void Start(List<IPlugin> plugins)
        {
            object objlock = new object();

            var countries = new List<Country>(); 

            var result = Parallel.ForEach(plugins, (IPlugin plug) =>
            {
                try
                {
                    var task = plug.Do<Country>();
                    task.Wait();

                    lock (objlock)
                    {
                        countries.AddRange(task.Result);
                    }
                }
                catch(Exception e)
                {
                    var path = $"{Path.GetTempPath()}\\api_error_{DateTime.Now.ToString("dd.MM.yyyy hh.mm.ss")}.txt";

                    using (var sw = File.CreateText(path))
                    {
                        sw.WriteLine(e.StackTrace);
                    }
                }
            });
        }
    }
}
