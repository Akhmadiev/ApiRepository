namespace MainApi.Reports
{
    using System;
    using System.Collections.Generic;
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

        private static Queue<Entities.Report> _reportQueue;

        public async void Start()
        {
            _reportQueue = new Queue<Entities.Report>();

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<ReportJobScheduler>().Build();
            job.JobDataMap.Add("Repository", Repository);
            job.JobDataMap.Add("GenerateReports", GenerateReports);

            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(2).RepeatForever())
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

            _reportQueue.Enqueue(report);

            return report.Id;
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
                while (!_reportQueue.Any()) { }

                var report = _reportQueue.Dequeue();

                report.ReportStatus = Enums.ReportStatus.Started;
                Repository.Update(report);

                var generateReport = GenerateReports
                    .First(x => x.ReportId == report.ReportId);

                generateReport.Repository = Repository;
                generateReport.Generate(report);

                report.ReportStatus = Enums.ReportStatus.Finished;
                Repository.Update(report);

                return null;
            }
        }
    }
}
