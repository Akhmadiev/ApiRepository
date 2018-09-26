namespace MainApi.Reports
{
    using MainApi.Entities;
    using MainApi.Interfaces;
    using System;
    using System.Linq;

    public class MaxReport : IGenerateReport
    {
        public string ReportId => ReportIds.Max;

        public IRepository Repository { get; set; }

        public Action<string> Action { get; set; }

        public void Generate(Entities.Report report)
        {
            var summary = Repository.GetAll<Product>()
                .GroupBy(x => 1)
                .Select(x => new
                {
                    Max = x.Max(y => y.Price),
                    Min = x.Min(y => y.Price),
                    Avg = x.Average(y => y.Price)
                })
                .First();

            var entity = new Entities.MaxReport
            {
                Max = summary.Max,
                Min = summary.Min,
                Avg = summary.Avg,
                Report = report.Id
            };
            Repository.Save(entity);
        }

        public string GetReport(Entities.Report report)
        {
            var maxReport = Repository.GetAll<Entities.MaxReport>()
                .Where(x => x.Report == report.Id)
                .FirstOrDefault();

            if (maxReport == null)
            {
                return "Report not found. Please create a new one";
            }

            return $"Max: {maxReport.Max}\nMin: {maxReport.Min}\nAvg: {maxReport.Avg}";
        }
    }
}
