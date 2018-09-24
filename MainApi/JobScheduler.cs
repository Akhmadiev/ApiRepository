namespace MainApi
{
    using ApiAdditional;
    using Castle.Windsor;
    using Quartz;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class JobScheduler : IJob
    {
        private IWindsorContainer container;

        private string logPath;

        public Task Execute(IJobExecutionContext context)
        {
            logPath = $"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}\\logs\\api_error.txt";
            container = context.JobDetail.JobDataMap.Get("Container") as IWindsorContainer;

            if (container == null)
            {
                using (var sw = new StreamWriter(logPath, true))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine("Can't find IWindsorContainer");
                }

                return null;
            }

            var countries = GetCountries();

            SaveData(countries);

            return null;
        }

        /// <summary>
        /// Save to database
        /// </summary>
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

        /// <summary>
        /// Get data from api
        /// </summary>
        private List<Country> GetCountries()
        {
            var plugins = container.ResolveAll<IPlugin>();

            var objlockAdd = new object();
            var objlockExc = new object();

            var countries = new List<Country>();

            var result = Parallel.ForEach(plugins, x =>
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
                        using (var sw = new StreamWriter(logPath, true))
                        {
                            sw.WriteLine(DateTime.Now.ToString());
                            sw.WriteLine(e.ToString());
                        }
                    }
                }
            });

            return countries;
        }
    }
}
