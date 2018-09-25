namespace MainApi.Interfaces
{
    using MainApi.Entities;
    using System;

    public interface IGenerateReport
    {
        /// <summary>
        /// Repository
        /// </summary>
        IRepository Repository { get;  set; }

        /// <summary>
        /// Get report action
        /// </summary>
        Action<string> Action { get; set; }

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
