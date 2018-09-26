namespace MainApi.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MainApi.Interfaces;

    public class Report
    {
        public IRepository Repository { get; set; }

        public IGenerateReport[] GenerateReports { get; set; }

        private static Queue<Entities.Report> _reportQueue;

        private static AutoResetEvent _evt;

        public void Start()
        {
            _reportQueue = new Queue<Entities.Report>();
            _evt = new AutoResetEvent(false);

            var task = new Task(() => Generate(Repository, GenerateReports));
            task.Start();
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

            _evt.Set();

            return report.Id;
        }

        private static void Generate(IRepository repository, IGenerateReport[] generateReports)
        {
            while (true)
            {
                _evt.WaitOne();

                while (_reportQueue.Any())
                {
                    var report = _reportQueue.Dequeue();

                    report.ReportStatus = Enums.ReportStatus.Started;
                    repository.Update(report);

                    var generateReport = generateReports
                        .First(x => x.ReportId == report.ReportId);

                    generateReport.Repository = repository;
                    generateReport.Generate(report);

                    report.ReportStatus = Enums.ReportStatus.Finished;
                    repository.Update(report);
                }
            }
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
    }
}
