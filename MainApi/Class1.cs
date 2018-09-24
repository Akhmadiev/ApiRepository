namespace MainApi
{
    using Quartz;
    using Quartz.Impl;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Windsor;
    using System;
    using ApiAdditional;
    using System.IO;
    using System.Reflection;
    using Castle.MicroKernel.Registration;

    public class MainApiClass
    {
        public async void Start()
        {
            var container = new WindsorContainer();
            RegisterPlugins(container);

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<JobScheduler>().Build();
            job.JobDataMap.Add("Container", container);

            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// Get dll files and return list of IPlugin
        /// </summary>
        private void RegisterPlugins(IWindsorContainer container)
        {
            var files = Directory.GetFiles($"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}\\dll");

            var pluginType = typeof(IPlugin);

            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(file);
                var types = assembly.GetTypes();

                foreach (var type in types.Where(x => !x.IsInterface && !x.IsAbstract))
                {
                    if (type.GetInterface(pluginType.FullName) != null)
                    {
                        var instance = (IPlugin)Activator.CreateInstance(type);

                        container.Register(Component.For<IPlugin>().ImplementedBy(instance.GetType()).Named(instance.Name));
                    }
                }
            }
        }

        /// <summary>
        /// Get all countries
        /// </summary>
        public IQueryable<Country> GetAll()
        {
            var countries = new List<Country>();

            var context = new ApiContext();

            return context.Countries;
        }

        /// <summary>
        /// Get all countries by date
        /// </summary>
        public IQueryable<Country> GetAll(DateTime dateTime)
        {
            var countries = new List<Country>();

            var context = new ApiContext();

            return context.Countries
                .Where(x => x.StartDate.Date == dateTime.Date);
        }
    }
}