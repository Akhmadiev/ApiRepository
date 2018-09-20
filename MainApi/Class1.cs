namespace MainApi
{
    using ApiAdditional;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class MainApiClass
    {
        private void Start()
        {
            var plugins = GetPlugins();

            var countries = GetCountries(plugins);

            SaveData(countries);
        }

        private void SaveData(List<Country> countries)
        {
            using (var context = new ApiContext())
            {
                var currentCountries = context.Countries.Select(x => x.Name);

                countries = countries.Distinct().Where(x => !currentCountries.Contains(x.Name)).ToList();

                foreach (var country in countries)
                {
                    context.Countries.Add(country);
                }

                context.SaveChanges();
            }
        }

        private List<Country> GetCountries(List<IPlugin> plugins)
        {
            var logPath = $"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}\\logs";

            var objlockAdd = new object();
            var objlockExc = new object();

            var countries = new List<Country>();

            var result = Parallel.ForEach(plugins, x =>
            {
                try
                {
                    var task = x.Do<Country>();
                    task.Wait();

                    lock (objlockAdd)
                    {
                        countries.AddRange(task.Result);
                    }
                }
                catch (Exception e)
                {
                    lock(objlockExc)
                    {
                        var path = $"{logPath}\\api_error_{DateTime.Now.ToString("dd.MM.yyyy hh.mm.ss")}.txt";

                        using (var sw = File.CreateText(path))
                        {
                            sw.WriteLine(e.StackTrace);
                        }
                    }
                }
            });

            return countries;
        }

        private List<IPlugin> GetPlugins()
        {
            var files = Directory.GetFiles($"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}\\dll");

            var pluginType = typeof(IPlugin);

            var plugins = new List<IPlugin>();

            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(file);
                var types = assembly.GetTypes();

                foreach (var type in types.Where(x => !x.IsInterface && !x.IsAbstract))
                {
                    if (type.GetInterface(pluginType.FullName) != null)
                    {
                        var instance = (IPlugin)Activator.CreateInstance(type);

                        plugins.Add(instance);
                    }
                }
            }

            return plugins;
        }
    }
}
