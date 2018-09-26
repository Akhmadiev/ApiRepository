namespace MainApi.Interfaces
{
    using MainApi.Entities;

    public interface IGenerateReport
    {
        /// <summary>
        /// Generate report
        /// </summary>
        void Generate(Report report);

        /// <summary>
        /// Get report
        /// </summary>
        string GetReport(Report report);

        /// <summary>
        /// Report ID
        /// </summary>
        string ReportId { get; }
    }
}
