namespace MainApi.Reports
{
    using MainApi.Entities;
    using MainApi.Interfaces;
    using System.Linq;

    public class MaxReport : BaseReport, IGenerateReport
    {
        public string ReportId => ReportIds.Max;

        public void Execute()
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

            var report = Repository.GetAll<Report>()
                .OrderBy(x => x.StartDate)
                .First();

            var entity = new Entities.MaxReport
            {
                Max = summary.Max,
                Min = summary.Min,
                Avg = summary.Avg,
                Report = report
            };

            Repository.Save(entity);

            report.ReportStatus = Enums.ReportStatus.Finished;
            Repository.Update(entity);
        }
    }
}
