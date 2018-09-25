namespace MainApi
{
    using ApiAdditional;
    using Castle.Windsor;
    using MainApi.Interfaces;
    using Quartz;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class JobScheduler : IJob
    {
        public IPlugin[] Plugins { get; set; }

        public IRepository Repository { get; set; }

        public ILogger Logger { get; set; }

        private string logPath;

        public Task Execute(IJobExecutionContext context)
        {
            logPath = $"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}\\logs\\api_error.txt";

            var countries = GetCountries();

            SaveData(countries);

            return null;
        }

        /// <summary>
        /// Save to database
        /// </summary>
        private void SaveData(List<Country> countries)
        {
            var currentCountries = Repository.GetAll<Country>().Select(x => x.Name);

            countries = countries.Distinct().Where(x => !currentCountries.Contains(x.Name)).ToList();

            Repository.Save<Country>(countries);
        }

        /// <summary>
        /// Get data from api
        /// </summary>
        private List<Country> GetCountries()
        {
            var objlockAdd = new object();
            var objlockExc = new object();

            var countries = new List<Country>();

            var result = Parallel.ForEach(Plugins, x =>
            {
                try
                {
                    var task = x.Do();
                    task.Wait();

                    lock (objlockAdd)
                    {
                        countries.AddRange(task.Result);
                    }
                }
                catch (Exception e)
                {
                    lock (objlockExc)
                    {
                        Logger.LogError($"{DateTime.Now.ToString()}\n{e.ToString()}");
                    }
                }
            });

            return countries;
        }
    }
}
