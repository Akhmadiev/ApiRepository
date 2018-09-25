namespace MainApi.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MainApi.Interfaces;
    using Quartz;
    using Quartz.Impl;

    public class Report
    {
        public IRepository Repository { get; set; }

        public IGenerateReport[] GenerateReports { get; set; }

        private static AutoResetEvent _autoResetEvent;

        private static Entities.Report _reportEntity;

        public async void Start()
        {
            _autoResetEvent = new AutoResetEvent(false);

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<ReportJobScheduler>().Build();
            job.JobDataMap.Add("Repository", Repository);
            job.JobDataMap.Add("GenerateReports", GenerateReports);

            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public int Generate(string reportId)
        {
            var report = new Entities.Report
            {
                ReportId = reportId,
                StartDate = DateTime.Now,
                ReportStatus = Enums.ReportStatus.Queue
            };
            Repository.Save(report);

            _reportEntity = report;

            _autoResetEvent.Set();

            return _reportEntity.Id;
        }

        public string GetReport(int id)
        {
            var report = Repository.Get<Entities.Report>(id);

            if (report == null)
            {
                return "Report not found. Please create a new one";
            }

            if (report.ReportStatus != Enums.ReportStatus.Finished)
            {
                return "Your report don't ready";
            }

            var generateReport = GenerateReports
                .First(x => x.ReportId == report.ReportId);

            generateReport.Repository = Repository;
            return generateReport.GetReport(report);
        }

        public class ReportJobScheduler : IJob
        {
            public IRepository Repository { get; set; }

            public IGenerateReport[] GenerateReports { get; set; }

            public Task Execute(IJobExecutionContext context)
            {
                _autoResetEvent.WaitOne();

                    _reportEntity.ReportStatus = Enums.ReportStatus.Started;
                Repository.Update(_reportEntity);

                var generateReport = GenerateReports
                    .First(x => x.ReportId == _reportEntity.ReportId);

                generateReport.Repository = Repository;
                generateReport.Generate(_reportEntity);

                _reportEntity.ReportStatus = Enums.ReportStatus.Finished;
                Repository.Update(_reportEntity);

                return null;
            }
        }
    }
}
