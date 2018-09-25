namespace MainApi.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using MainApi.Interfaces;
    using Quartz;
    using Quartz.Impl;

    public class Report
    {
        private static AutoResetEvent evt;

        private static IGenerateReport GenerateReport;

        public async void Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<ReportJobScheduler>().Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public static void Generate(IGenerateReport generateReport)
        {
            GenerateReport = generateReport;
            evt.Set();
        }

        public class ReportJobScheduler : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                evt.WaitOne();

                GenerateReport.Execute();

                return null;
            }
        }
    }
}
