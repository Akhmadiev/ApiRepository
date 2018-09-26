namespace ApiWindowsService
{
    using ApiAdditional;
    using Castle.Windsor;
    using MainApi;
    using MainApi.Interfaces;
    using Newtonsoft.Json;
    using Quartz;
    using Quartz.Impl;
    using System;
    using System.IO;
    using System.Linq;
    using System.ServiceProcess;

    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var container = new WindsorContainer();

            var main = new MainApiClass();
            main.Start(container);

            Start(container);
        }

        private async void Start(IWindsorContainer container)
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            await scheduler.Start();

            var job = JobBuilder.Create<JobScheduler>().Build();

            job.JobDataMap.Add("Plugins", container.ResolveAll<IPlugin>());
            job.JobDataMap.Add("Repository", container.Resolve<IRepository>());
            job.JobDataMap.Add("Logger", container.Resolve<ILogger>());

            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(2).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStop()
        {
        }
    }
}
