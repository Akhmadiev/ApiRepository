namespace MainApi
{
    using Quartz;
    using Quartz.Impl;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Windsor;
    using System;

    public class MainApiClass
    {
        public async void Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<JobScheduler>().Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public static void RegisterService(IWindsorContainer container)
        {
            
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