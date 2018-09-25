namespace MainApi.Interfaces
{
    public interface IGenerateReport
    {
        /// <summary>
        /// Generate report
        /// </summary>
        void Execute();

        /// <summary>
        /// Report ID
        /// </summary>
        string ReportId { get; }
    }
}
