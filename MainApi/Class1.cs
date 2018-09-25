namespace MainApi
{
    using Quartz;
    using Quartz.Impl;
    using System.Linq;
    using Castle.Windsor;
    using System;
    using ApiAdditional;
    using System.IO;
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using MainApi.Interfaces;

    public class MainApiClass
    {
        public void Start(IWindsorContainer container)
        {
            RegisterPlugins(container);
            RegisterReports(container);
        }

        //public async void Start2()
        //{
        //    var container = new WindsorContainer();
        //    RegisterPlugins(container);
        //    RegisterReports(container);

        //    var scheduler = await StdSchedulerFactory.GetDefaultScheduler();

        //    await scheduler.Start();

        //    var job = JobBuilder.Create<JobScheduler>().Build();

        //    job.JobDataMap.Add("Plugins", container.ResolveAll<IPlugin>());
        //    job.JobDataMap.Add("Repository", container.Resolve<IRepository>("Repository"));
        //    job.JobDataMap.Add("Logger", container.Resolve<ILogger>("Logger"));

        //    var trigger = TriggerBuilder.Create()
        //        .WithIdentity("trigger1", "group1")
        //        .StartNow()
        //        .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
        //        .Build();

        //    await scheduler.ScheduleJob(job, trigger);
        //}

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
                        container.Register(Component.For<IPlugin>().Instance(instance));
                    }
                }
            }

            container.Register(Component.For<IRepository>().ImplementedBy<Repository>());
        }

        private void RegisterReports(IWindsorContainer container)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            var types = assembly.GetTypes();

            var reportType = typeof(IGenerateReport);

            foreach (var type in types)
            {
                if (type.GetInterface(reportType.FullName) != null)
                {
                    var instance = (IGenerateReport)Activator.CreateInstance(type);
                    container.Register(Component.For<IGenerateReport>().Instance(instance).Named(instance.ReportId));
                }
            }
        }
    }
}