namespace MainApi.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Castle.Windsor;
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

        public int Generate(string templateId)
        {
            var reportId = Repository.GetAll<Entities.Report>()
                .Max(x => (int?)x.ReportId);

            var report = new Entities.Report
            {
                TemplateId = templateId,
                StartDate = DateTime.Now,
                ReportStatus = Enums.ReportStatus.Queue,
                ReportId = !reportId.HasValue ? 1 : reportId.Value + 1
            };
            Repository.Save(report);

            _reportQueue.Enqueue(report);

            _evt.Set();

            return report.ReportId;
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
                        .First(x => x.ReportId == report.TemplateId);

                    generateReport.Generate(report);

                    report.ReportStatus = Enums.ReportStatus.Finished;
                    repository.Update(report);
                }
            }
        }

        public string GetReport(int id)
        {
            var report = Repository.GetAll<Entities.Report>()
                .Where(x => x.ReportId == id)
                .FirstOrDefault();

            if (report == null)
            {
                return "Report not found. Please create a new one";
            }

            if (report.ReportStatus != Enums.ReportStatus.Finished)
            {
                return "Your report don't ready";
            }

            var generateReport = GenerateReports
                .First(x => x.ReportId == report.TemplateId);

            return generateReport.GetReport(report);
        }
    }
}
