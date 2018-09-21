namespace MainApi
{
    using Quartz;
    using Quartz.Impl;
    using System.Collections.Generic;
    using System.Linq;

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

        public IQueryable<Country> GetAll()
        {
            var countries = new List<Country>();

            var context = new ApiContext();

            return context.Countries;
        }
    }
}